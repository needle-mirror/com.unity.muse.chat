using System;
using JetBrains.Annotations;

#if UNITY_2023_1_OR_NEWER
using System.Threading.Tasks;
using UnityEngine;
#else
using System.Threading.Tasks;
#endif

namespace Unity.Muse.Chat.Embeddings.Processing
{
    interface IDataProcess<in TInput, TOutput> : IDisposable
    {
        /// <summary>
        ///     Synchronously processes the <paramref name="input"/> of type
        ///     <typeparamref name="TInput"/> and produces a <typeparamref name="TOutput"/> output.
        /// </summary>
        /// <param name="input">
        ///     The data to process.
        /// </param>
        /// <returns>
        ///     The output of the process.
        /// </returns>
#if UNITY_2023_1_OR_NEWER
        TOutput Update([NotNull] TInput input);
#else
        TOutput Update([NotNull] TInput input);
#endif

        void Reset();
    }
}
