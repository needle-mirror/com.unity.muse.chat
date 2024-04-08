using System;
using System.Collections.Generic;
using JetBrains.Annotations;

#if UNITY_2023_1_OR_NEWER
using UnityEngine;
#endif

namespace Unity.Muse.Chat.Processing
{
    interface IBatchedDataProcess<in TInput, TOutput> : IDisposable
    {
        /// <summary>
        ///     Synchronously gets the processor of type
        ///     <see cref="IDataProcessor{TInput,TOutput}"/>.
        /// </summary>
        /// <returns>
        ///     The agent processor.
        /// </returns>
#if UNITY_2023_1_OR_NEWER
        IEnumerable<TOutput> Update([NotNull] IEnumerable<TInput> input);
#else
        IEnumerable<TOutput> Update([NotNull] IEnumerable<TInput> input);
#endif

        void Reset();
    }
}
