using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Chat.Processing;
using Unity.Sentis;
namespace Unity.Muse.Chat.Processors
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
                using var output = m_Owner.m_EmbedderWorker.PeekOutput("last_hidden_state") as TensorFloat;

                // mean pooling
                TensorFloat pooled;
                {
                    using var reshaped = m_Owner.m_Ops.Reshape(attentionMask, new TensorShape(1, m_Owner.MaxSequenceLength, 1));

                    using var expanded = m_Owner.m_Ops.Expand(reshaped, output!.shape);
                    using var attentionMaskExpanded = m_Owner.m_Ops.Cast(expanded, DataType.Float) as TensorFloat;
                    using var outputMul = m_Owner.m_Ops.Mul(output, attentionMaskExpanded);
                    using var reduced = m_Owner.m_Ops.ReduceSum(outputMul, k_AxisOne, false);
                    using var sumMask = m_Owner.m_Ops.ReduceSum(attentionMaskExpanded, k_AxisOne, false);
                    using var clipped = m_Owner.m_Ops.Clip(sumMask, 1e-9f, 1e9f);
                    pooled = m_Owner.m_Ops.Div(reduced, clipped);
                }

                // normalization
                TensorFloat embeddings;
                {
                    using var reduced = m_Owner.m_Ops.ReduceL2(pooled, k_AxisOne, false);
                    using var norm = m_Owner.m_Ops.Clip(reduced, 1e-12f, 1e12f);
                    embeddings = m_Owner.m_Ops.Div(pooled, norm);
                }

                // force the AsyncGPUReadback request to complete and grab embeddings
                embeddings.MakeReadable();
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
                using var output = m_Owner.m_EmbedderWorker.PeekOutput("last_hidden_state") as TensorFloat;

                var vectorLength = output!.shape[2];

                // mean pooling
                TensorFloat pooled;
                {
                    using var reshaped = m_Owner.m_Ops.Reshape(
                        attentionMask,
                        new TensorShape(encodings.Length, m_Owner.MaxSequenceLength, 1));

                    using var expanded = m_Owner.m_Ops.Expand(reshaped, output.shape);
                    using var attentionMaskExpanded = m_Owner.m_Ops.Cast(expanded, DataType.Float) as TensorFloat;
                    using var outputMul = m_Owner.m_Ops.Mul(output, attentionMaskExpanded);
                    using var reduced = m_Owner.m_Ops.ReduceSum(outputMul, k_AxisOne, false);
                    using var sumMask = m_Owner.m_Ops.ReduceSum(attentionMaskExpanded, k_AxisOne, false);
                    using var clipped = m_Owner.m_Ops.Clip(sumMask, 1e-9f, 1e9f);
                    pooled = m_Owner.m_Ops.Div(reduced, clipped);
                }

                // normalization
                TensorFloat embeddings;
                {
                    using var reduced = m_Owner.m_Ops.ReduceL2(pooled, k_AxisOne, false);
                    using var norm = m_Owner.m_Ops.Clip(reduced, 1e-12f, 1e12f);

                    using var reshaped = m_Owner.m_Ops.Reshape(norm, new TensorShape(encodings.Length, 1));
                    embeddings = m_Owner.m_Ops.Div(pooled, reshaped);
                }

                // force the AsyncGPUReadback requests to complete
                embeddings.MakeReadable();

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
