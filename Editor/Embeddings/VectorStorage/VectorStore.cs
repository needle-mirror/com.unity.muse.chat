using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Muse.Chat.Embeddings.Processing;
using Unity.Jobs;

#if UNITY_2023_1_OR_NEWER
using UnityEngine;
#else
using System.Threading.Tasks;
#endif

namespace Unity.Muse.Chat.Embeddings.VectorStorage
{
    partial class VectorStore : IVectorStore, IDisposable
    {
        struct LockHandle : IDisposable
        {
            readonly VectorStore m_Store;

            public LockHandle(VectorStore store)
            {
                m_Store = store;
                m_Store.m_Locked = true;
            }

            public void Dispose() => m_Store.m_Locked = false;
        }

        bool m_Disposed;

        readonly IDataProcessor<string, float[]> m_EmbeddingProcessor;

        readonly int m_VectorLength;

        NativeArray<IntPtr> m_Vectors;
        readonly List<string> m_Keys = new();

        readonly Base64KeyGenerator m_KeyGenerator = new();

        bool m_Locked;

        public VectorStore(IDataProcessor<string, float[]> embeddingProcessor, int vectorLength, int initialSize = 10000)
        {
            m_EmbeddingProcessor = embeddingProcessor;
            m_VectorLength = vectorLength;

            m_Vectors = new NativeArray<IntPtr>(initialSize, Allocator.Persistent);
        }

        ~VectorStore() => DisposeObject();

        public int Count => m_Keys.Count;

        public void Dispose()
        {
            DisposeObject();
            GC.SuppressFinalize(this);
        }

        public async
#if UNITY_2023_1_OR_NEWER
        Awaitable<string>
#else
        Task<string>
#endif
        Insert([NotNull] string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            using var _ = await LockStore();
            return await InsertInternal(key);
        }

        public async
#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<string>>
#else
        Task<ICollection<string>>
#endif
        Insert([NotNull] IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            using var _ = await LockStore();

            return await InsertInternal(keys);
        }

        public async
#if UNITY_2023_1_OR_NEWER
        Awaitable<bool>
#else
        Task<bool>
#endif
        Remove([NotNull] string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            using var _ = await LockStore();

            var index = m_Keys.IndexOf(key);
            if (index < 0)
                return false;

            var vector = m_Vectors[index];
            Marshal.FreeHGlobal(vector);

            {
                var target = m_Vectors.GetSubArray(index, m_Keys.Count - index);

                var first = index + 1;
                var count = m_Keys.Count - first;
                var source = m_Vectors.GetSubArray(first, count);

                source.CopyTo(target);
            }

            m_Keys.RemoveAt(index);

            return true;
        }

        public async
#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<(string key, float priority)>>
#else
        Task<ICollection<(string key, float priority)>>
#endif
        Query([NotNull] string request, int topK)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (topK <= 0)
                throw new ArgumentOutOfRangeException(nameof(topK), topK, "Must be positive.");

            var vector = m_EmbeddingProcessor.Process(request);

            using var _ = await LockStore();

            using var scores = ComputeScores(vector);

            var ordered = scores
                .Select((score, index) => (index, score))
                .OrderByDescending(t => t.score)
                .Take(topK)
                .Select(t => (m_Keys[t.index], t.score))
                .ToArray();

            return ordered;
        }

        public async
#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<(string key, float priority)>>
#else
        Task<ICollection<(string key, float priority)>>
#endif
        Query([NotNull] string request, float minScore)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (minScore is <= 0 or > 1f)
                throw new ArgumentOutOfRangeException(nameof(minScore), minScore,
                    "Must be between 0 exclusive and 1.");

            var vector = m_EmbeddingProcessor.Process(request);

            using var _ = await LockStore();

            using var scores = ComputeScores(vector);

            var ordered = scores
                .Select((score, index) => (key: m_Keys[index], score))
                .OrderByDescending(t => t.score)
                .Where(t => t.score >= minScore)
                .ToArray();

            return ordered;
        }

        public async
#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<(string key, float priority)>>
#else
        Task<ICollection<(string key, float priority)>>
#endif
        Query([NotNull] string request, int topK, float minScore)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (topK <= 0)
                throw new ArgumentOutOfRangeException(nameof(topK), topK, "Must be positive.");

            if (minScore is <= 0 or > 1f)
                throw new ArgumentOutOfRangeException(nameof(minScore), minScore,
                    "Must be between 0 exclusive and 1.");

            var vector = m_EmbeddingProcessor.Process(request);

            using var _ = await LockStore();

            using var scores = ComputeScores(vector);

            var ordered = scores
                .Select((score, index) => (key: m_Keys[index], score))
                .OrderByDescending(t => t.score)
                .Where(t => t.score >= minScore)
                .Take(topK)
                .ToArray();

            return ordered;
        }

        unsafe void Add(string key, float[] data)
        {
            var index = m_Keys.Count;
            m_Keys.Add(key);
            var size = data.Length * sizeof(float);
            var d = (float*)Marshal.AllocHGlobal(size);
            fixed (float* pData = data)
            {
                Buffer.MemoryCopy(pData, d, size, size);
            }

            m_Vectors[index] = new IntPtr(d);
        }

        NativeArray<float> ComputeScores(float[] vector)
        {
            using var request = new NativeArray<float>(vector, Allocator.TempJob);
            var results = new NativeArray<float>(m_Keys.Count, Allocator.Persistent);

            var job = new FlatCSParallelJob(request, m_Vectors, results);
            var handle = job.Schedule(m_Keys.Count, 1000);

            handle.Complete();
            return results;
        }

#pragma warning disable CS1998
        async
 #if UNITY_2023_1_OR_NEWER
        Awaitable<string>
#else
        Task<string>
#endif
        InsertInternal([NotNull] string value)
        {
            var data = m_EmbeddingProcessor.Process(value);
            var key = m_KeyGenerator.Generate();

            Add(key, data);
            return key;
        }

        async
#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<string>>
#else
        Task<ICollection<string>>
#endif
        InsertInternal([NotNull] IEnumerable<string> values)
        {
            var embeddingList = new List<string>();

            if (m_EmbeddingProcessor is IBatchedDataProcessor<string, float[]> batchProcessor)
            {
                const int k_BatchSize = 500;

                var valueArray = values.ToArray();

                for (var i = 0; i < valueArray.Length; i += k_BatchSize)
                {
                    var portion = valueArray.Skip(i).Take(k_BatchSize);
                    var vectors = batchProcessor.ProcessBatch(portion);

                    var embeddings = vectors
                        .Select(vector => (key: m_KeyGenerator.Generate(), vector));

                    foreach (var (key, vector) in embeddings)
                    {
                        Add(key, vector);
                        embeddingList.Add(key);
                    }
                }
            }
            else
            {
                foreach (var value in values)
                {
                    var key = await InsertInternal(value);
                    embeddingList.Add(key);
                }
            }

            return embeddingList.ToArray();
        }

        async
#if UNITY_2023_1_OR_NEWER
        Awaitable<LockHandle>
#else
        Task<LockHandle>
#endif
        LockStore()
        {
            while (m_Locked)
#if UNITY_2023_1_OR_NEWER
                await Awaitable.NextFrameAsync();
#else
                await Task.Yield();
#endif

            return new LockHandle(this);
        }

#pragma warning restore CS1998

        void DisposeObject()
        {
            if(m_Disposed) return;

            m_EmbeddingProcessor?.Dispose();

            foreach (var ptr in m_Vectors)
                Marshal.FreeHGlobal(ptr);

            m_Vectors.Dispose();
            m_KeyGenerator?.Dispose();
            m_Disposed = true;
        }

        int IReadOnlyVectorStore.Count => Count;

#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<(string key, float priority)>>
#else
        Task<ICollection<(string key, float priority)>>
#endif
        IReadOnlyVectorStore.Query(string request, int topK) => Query(request, topK);

#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<(string key, float priority)>>
#else
        Task<ICollection<(string key, float priority)>>
#endif
        IReadOnlyVectorStore.Query(string request, float minScore) =>
            Query(request, minScore);

#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<(string key, float priority)>>
#else
        Task<ICollection<(string key, float priority)>>
#endif
        IReadOnlyVectorStore.Query(string request, int topK, float minScore) =>
            Query(request, topK, minScore);

#if UNITY_2023_1_OR_NEWER
        Awaitable<string>
#else
        Task<string>
#endif
        IVectorStore.Insert(string value) => Insert(value);

#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<string>>
#else
        Task<ICollection<string>>
#endif
        IVectorStore.Insert(IEnumerable<string> values) => Insert(values);

#if UNITY_2023_1_OR_NEWER
        Awaitable<bool>
#else
        Task<bool>
#endif
        IVectorStore.Remove(string key) => Remove(key);

    }
}
