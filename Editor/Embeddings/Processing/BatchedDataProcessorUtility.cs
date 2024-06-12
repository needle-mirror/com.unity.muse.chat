using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Embeddings.Processing
{
    static class BatchedDataProcessorUtility
    {
        public static IEnumerable<TOutput> ProcessBatch<TInput, TOutput>(
            [NotNull] this IBatchedDataProcessor<TInput, TOutput> @this, [NotNull] IEnumerable<TInput> input)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            using var process = @this.CreateBatchProcess();
            return process.Update(input);
        }
    }
}
