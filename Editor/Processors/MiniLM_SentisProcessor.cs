using System;
using JetBrains.Annotations;
using Unity.Muse.Chat.Processing;
using Unity.Muse.Chat.Tokenization;
using Unity.Sentis;
using UnityEngine;

namespace Unity.Muse.Chat.Processors
{
    /// <summary>
    ///     Computes an embedding (<see cref="float"/> array) from an input <see cref="string"/>.
    ///     Supports batch computation.
    /// </summary>
    partial class MiniLM_SentisProcessor : IBatchedDataProcessor<string, float[]>
    {
        // Used in various Ops calls.
        static readonly int[] k_AxisOne = {1};

        readonly IWorker m_EmbedderWorker;
        readonly ITensorAllocator m_Allocator;
        readonly Ops m_Ops;
        readonly ITokenizationPipeline m_TokenizationPipeline;

        readonly bool m_DisposeWorker;

        bool m_Disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MiniLM_SentisProcessor"/> type.
        /// </summary>
        /// <param name="embedWorker">
        ///     The worker which computes the embedding.
        /// </param>
        /// <param name="tokenizationPipeline">
        ///     The string to token converter.
        /// </param>
        /// <param name="maxSequenceLength">
        ///     The maximum size of an input.
        /// </param>
        /// <param name="disposeWorker">
        ///     Tells whether the worker is disposed when this processor is disposed.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="embedWorker"/> and <paramref name="tokenizationPipeline"/> cannot
        ///     be <see langword="null"/>.
        /// </exception>
        public MiniLM_SentisProcessor(
            [NotNull] IWorker embedWorker,
            [NotNull] ITokenizationPipeline tokenizationPipeline,
            int maxSequenceLength = 128,
            bool disposeWorker = false)
        {
            m_EmbedderWorker = embedWorker ?? throw new ArgumentNullException(nameof(embedWorker));
            m_TokenizationPipeline = tokenizationPipeline ?? throw new ArgumentNullException(nameof(tokenizationPipeline));

            m_DisposeWorker = disposeWorker;
            MaxSequenceLength = maxSequenceLength;

            m_Allocator = new TensorCachingAllocator();
            m_Ops = new GPUComputeOps();
        }

        ~MiniLM_SentisProcessor() => DisposeObject();

        /// <summary>
        ///     The maximum number of tokens to process into an embedding.
        /// </summary>
        public int MaxSequenceLength { get; }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            DisposeObject();
            GC.SuppressFinalize(this);
        }

        public IDataProcess<string, float[]> CreateProcess() => new Process(this);

        void DisposeObject()
        {
            if (m_Disposed)
                return;

            m_Allocator?.Dispose();
            m_Ops?.Dispose();

            if(m_DisposeWorker)
                m_EmbedderWorker.Dispose();

            m_Disposed = true;
        }

        void IDisposable.Dispose() => Dispose();

        IDataProcess<string, float[]> IDataProcessor<string, float[]>.CreateProcess()
            => CreateProcess();

        IBatchedDataProcess<string, float[]> IBatchedDataProcessor<string, float[]>.CreateBatchProcess()
            => CreateProcess() as IBatchedDataProcess<string, float[]>;
    }
}
