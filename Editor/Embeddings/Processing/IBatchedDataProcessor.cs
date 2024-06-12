namespace Unity.Muse.Chat.Embeddings.Processing
{
    /// <summary>
    ///     Generic interface for agent processor which supports batching process.
    ///     The <see cref="ProcessBatch"/> method takes a sequence of input of type
    ///     <typeparamref name="TInput"/> an produces a sequence of
    ///     <typeparamref name="TOutput"/> output.
    /// </summary>
    /// <typeparam name="TInput">
    ///     The type of the input element to process.
    /// </typeparam>
    /// <typeparam name="TOutput">
    ///     The type of the produced output.
    /// </typeparam>
    interface IBatchedDataProcessor<in TInput, TOutput> : IDataProcessor<TInput, TOutput>
    {
        IBatchedDataProcess<TInput, TOutput> CreateBatchProcess();
    }
}
