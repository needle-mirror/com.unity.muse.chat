#if UNITY_2023_1_OR_NEWER
using UnityEngine;
#else
using System.Threading.Tasks;
#endif

namespace Unity.Muse.Chat.Processing
{
    /// <summary>
    ///     It defines a <see cref="GetProcessor"/> method returning an instance of
    ///     <see cref="IDataProcessor"/>.
    /// </summary>
    /// <typeparam name="TInput">
    ///     The type of the input of the provided agent processor.
    /// </typeparam>
    /// <typeparam name="TOutput">
    ///     The type of the output produced by the provided agent processor.
    /// </typeparam>
    interface IDataProcessorProvider<TInput, TOutput>
    {
#if UNITY_2023_1_OR_NEWER
        Awaitable<IDataProcessor<TInput, TOutput>> GetProcessor();
#else
        Task<IDataProcessor<TInput, TOutput>> GetProcessor();
#endif
    }
}
