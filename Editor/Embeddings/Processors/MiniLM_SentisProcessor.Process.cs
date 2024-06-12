using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Chat.Embeddings.Processing;
using Unity.Sentis;
namespace Unity.Muse.Chat.Embeddings.Processors
{
    partial class MiniLM_SentisProcessor
    {
        class Process : IDataProcess<string, float[]>, IBatchedDataProcess<string, float[]>
        {
            readonly MiniLM_SentisProcessor m_Owner;

            bool m_Disposed;

            public Process(MiniLM_SentisProcessor owner) => m_Owner = owner;

            ~Process() => DisposeObject();

            public void Dispose()
            {
                DisposeObject();
                GC.SuppressFinalize(this);
            }

            /// <summary>
            ///     Takes a string, key, and creates an embedding using the embedding model.
            /// </summary>
            /// <param name="key">
            ///     The string to create an embedding for.
            /// </param>
            /// <returns>
            ///     A <see cref="float"/> array.
            /// </returns>
            public float[] Embed(string key)
            {
                var encoding = m_Owner.m_TokenizationPipeline.Encode(key);
                var ids = encoding.Ids.ToArray();
                var attention = encoding.Attention.ToArray();
                var typeIDs = encoding.TypeIds.ToArray();

                // create model inputs as tensors
                var shape = new TensorShape(1, ids.Length);
                using var inputIDs = new TensorInt(shape, ids);
                using var attentionMask = new TensorInt(shape, attention);
                using var tokenTypeIDs = new TensorInt(shape, typeIDs);

                // package up all input tensors
                var inputTensors = new Dictionary<string, Tensor>
                {
                    {"input_ids", inputIDs},
                    {"attention_mask", attentionMask},
                    {"token_type_ids", tokenTypeIDs},
                };

                // run inference
                m_Owner.m_EmbedderWorker.Execute(inputTensors);

                // grab output
                using var output = m_Owner.m_EmbedderWorker.PeekOutput("output") as TensorFloat;

                // mean pooling
                TensorFloat pooled;
                {
                    attentionMask.Reshape(new TensorShape(1, m_Owner.MaxSequenceLength, 1));

                    using var expanded = TensorInt.AllocNoData(output!.shape);
                    m_Owner.m_Backend.Expand(attentionMask, expanded);

                    using var attentionMaskExpanded = TensorFloat.AllocNoData(expanded.shape);
                    m_Owner.m_Backend.Cast(expanded, attentionMaskExpanded);

                    using var outputMul = TensorFloat.AllocNoData(output.shape);
                    m_Owner.m_Backend.Mul(output, attentionMaskExpanded, outputMul);

                    using var reduced = TensorFloat.AllocNoData(outputMul.shape.Reduce(k_AxisOne, false));
                    m_Owner.m_Backend.ReduceSum(outputMul, reduced, k_AxisOne, false);

                    using var sumMask = TensorFloat.AllocNoData(attentionMaskExpanded.shape.Reduce(k_AxisOne, false));
                    m_Owner.m_Backend.ReduceSum(attentionMaskExpanded, sumMask, k_AxisOne, false);

                    using var clipped = TensorFloat.AllocNoData(sumMask.shape);
                    m_Owner.m_Backend.Clip(sumMask, clipped, 1e-9f, 1e9f);

                    pooled = TensorFloat.AllocNoData(reduced.shape);
                    m_Owner.m_Backend.Div(reduced, clipped, pooled);
                }

                // normalization
                TensorFloat embeddings;
                {
                    using var reduced = TensorFloat.AllocNoData(pooled.shape.Reduce(k_AxisOne, false));
                    m_Owner.m_Backend.ReduceL2(pooled, reduced, k_AxisOne, false);

                    using var norm = TensorFloat.AllocNoData(reduced.shape);
                    m_Owner.m_Backend.Clip(reduced, norm, 1e-12f, 1e12f);

                    embeddings = TensorFloat.AllocNoData(pooled.shape);
                    m_Owner.m_Backend.Div(pooled, norm, embeddings);
                }

                // force the AsyncGPUReadback request to complete and grab embeddings
                embeddings.CompleteOperationsAndDownload();
                var result = embeddings.ToReadOnlyArray();

                // dispose of tensors
                pooled?.Dispose();
                embeddings.Dispose();

                return result;
            }

            /// <summary>
            ///     Takes a list of strings and creates an embedding using the embedding model with
            ///     batched inference.
            /// </summary>
            /// <param name="keys">
            ///     The string to create an embedding for.
            /// </param>
            /// <returns>
            ///     A list of <see cref="float"/> array.
            /// </returns>
            public IEnumerable<float[]> Embed(IEnumerable<string> keys)
            {
                 var encodings = keys.Select(key => m_Owner.m_TokenizationPipeline.Encode(key))
                    .ToArray();

                var (ids, attention, typeIds) = (
                    encodings.SelectMany(e => e.Ids).ToArray(),
                    encodings.SelectMany(e => e.Attention).ToArray(),
                    encodings.SelectMany(e => e.TypeIds).ToArray());

                // create model inputs as tensors
                var shape = new TensorShape(encodings.Length, m_Owner.MaxSequenceLength);
                using var inputIDs = new TensorInt(shape, ids);
                using var attentionMask = new TensorInt(shape, attention);
                using var tokenTypeIDs = new TensorInt(shape, typeIds);

                // package up all input tensors
                var inputTensors = new Dictionary<string, Tensor>
                {
                    {"input_ids", inputIDs},
                    {"attention_mask", attentionMask},
                    {"token_type_ids", tokenTypeIDs}
                };

                // run inference
                m_Owner.m_EmbedderWorker.Execute(inputTensors);

                // grab output
                using var output = m_Owner.m_EmbedderWorker.TakeOutputOwnership("output") as TensorFloat;

                var vectorLength = output!.shape[2];

                // mean pooling
                TensorFloat pooled;
                {
                    attentionMask.Reshape(new TensorShape(encodings.Length, m_Owner.MaxSequenceLength, 1));

                    using var expanded = TensorInt.AllocNoData(output!.shape);
                    m_Owner.m_Backend.Expand(attentionMask, expanded);

                    using var attentionMaskExpanded = TensorFloat.AllocNoData(expanded.shape);
                    m_Owner.m_Backend.Cast(expanded, attentionMaskExpanded);

                    using var outputMul = TensorFloat.AllocNoData(output.shape);
                    m_Owner.m_Backend.Mul(output, attentionMaskExpanded, outputMul);

                    using var reduced = TensorFloat.AllocNoData(outputMul.shape.Reduce(k_AxisOne, false));
                    m_Owner.m_Backend.ReduceSum(outputMul, reduced, k_AxisOne, false);

                    using var sumMask = TensorFloat.AllocNoData(attentionMaskExpanded.shape.Reduce(k_AxisOne, false));
                    m_Owner.m_Backend.ReduceSum(attentionMaskExpanded, sumMask, k_AxisOne, false);

                    using var clipped = TensorFloat.AllocNoData(sumMask.shape);
                    m_Owner.m_Backend.Clip(sumMask, clipped, 1e-9f, 1e9f);

                    pooled = TensorFloat.AllocNoData(reduced.shape);
                    m_Owner.m_Backend.Div(reduced, clipped, pooled);
                }

                // normalization
                TensorFloat embeddings;
                {
                    using var reduced = TensorFloat.AllocNoData(pooled.shape.Reduce(k_AxisOne, false));
                    m_Owner.m_Backend.ReduceL2(pooled, reduced, k_AxisOne, false);

                    using var norm = TensorFloat.AllocNoData(reduced.shape);
                    m_Owner.m_Backend.Clip(reduced, norm, 1e-12f, 1e12f);

                    using var reshaped = TensorFloat.AllocNoData(new TensorShape(encodings.Length, 1));
                    m_Owner.m_Backend.Reshape(norm, reshaped);

                    embeddings = TensorFloat.AllocNoData(pooled.shape);
                    m_Owner.m_Backend.Div(pooled, reshaped, embeddings);
                }

                // force the AsyncGPUReadback requests to complete
                embeddings.CompleteOperationsAndDownload();

                // grab embedding
                var embeddingsArray = embeddings.ToReadOnlyArray();
                var result = Enumerable.Range(0, encodings.Length)
                    .Select(i => embeddingsArray.Skip(i * vectorLength).Take(vectorLength))
                    .Select(e => e.ToArray());

                // dispose of tensors
                pooled?.Dispose();
                embeddings.Dispose();

                return result;
            }

            void DisposeObject()
            {
                // Do nothing for this one.
            }

            void IDisposable.Dispose() => Dispose();

#if UNITY_2023_1_OR_NEWER
            float[] IDataProcess<string, float[]>.Update(string input)
#else
            float[] IDataProcess<string, float[]>.Update(string input)
#endif
                => Embed(input);

#if UNITY_2023_1_OR_NEWER
            IEnumerable<float[]> IBatchedDataProcess<string, float[]>.Update
#else
            IEnumerable<float[]> IBatchedDataProcess<string, float[]>.Update
#endif
                (IEnumerable<string> input) => Embed(input);

            void IBatchedDataProcess<string, float[]>.Reset()
            {}

            void IDataProcess<string, float[]>.Reset()
            {}
        }
    }
}
