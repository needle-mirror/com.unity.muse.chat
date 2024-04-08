using System;
using System.Collections.Generic;
using JetBrains.Annotations;

#if UNITY_2023_1_OR_NEWER
using UnityEngine;
#else
using System.Threading.Tasks;
#endif

namespace Unity.Muse.Chat.VectorStorage
{
    interface IVectorStore : IReadOnlyVectorStore, IDisposable
    {
#if UNITY_2023_1_OR_NEWER
        Awaitable<string>
#else
        Task<string>
#endif
            Insert([NotNull] string value);

#if UNITY_2023_1_OR_NEWER
        Awaitable<ICollection<string>>
#else
        Task<ICollection<string>>
#endif
            Insert([NotNull] IEnumerable<string> values);

#if UNITY_2023_1_OR_NEWER
        Awaitable<bool>
#else
        Task<bool>
#endif
            Remove([NotNull] string key);
    }
}
