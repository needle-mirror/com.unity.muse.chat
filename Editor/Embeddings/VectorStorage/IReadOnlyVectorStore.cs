using System.Collections.Generic;
using JetBrains.Annotations;

#if UNITY_2023_1_OR_NEWER
using UnityEngine;
#else
using System.Threading.Tasks;
#endif

namespace Unity.Muse.Chat.VectorStorage
{
    interface IReadOnlyVectorStore
    {
        int Count { get; }

#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<(string key, float priority)>>
#else
        Task<ICollection<(string key, float priority)>>
#endif
            Query([NotNull] string request, int topK);

#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<(string key, float priority)>>
#else
        Task<ICollection<(string key, float priority)>>
#endif
            Query([NotNull] string request, float minScore);

#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<(string key, float priority)>>
#else
        Task<ICollection<(string key, float priority)>>
#endif
            Query([NotNull] string request, int topK, float minScore);
    }
}
