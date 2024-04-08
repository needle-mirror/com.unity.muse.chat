using System;

namespace Unity.Muse.Chat.Processing
{
    /// <summary>
    ///     Base interface for data processor.
    /// </summary>
    interface IDataProcessor : IDisposable
    {}

    /// <summary>
    ///     Generic interface for data processor.
    ///     Defines an asynchronous <see cref="Process"/> method that processes a
    ///     <typeparamref name="TInput"/> input and produces a
    ///     <typeparamref name="TOutput"/> output.
    /// </summary>
    /// <typeparam name="TInput">
    ///     The type of the process input.
    /// </typeparam>
    /// <typeparam name="TOutput">
    ///     The type of the process output.
    /// </typeparam>
    interface IDataProcessor<in TInput, TOutput> : IDataProcessor
    {
        IDataProcess<TInput, TOutput> CreateProcess();
    }
}
