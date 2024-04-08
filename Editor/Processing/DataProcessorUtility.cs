using System;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Processing
{
    static class DataProcessorUtility
    {
        public static TOutput Process<TInput, TOutput>(
            [NotNull] this IDataProcessor<TInput, TOutput> @this, [NotNull] TInput input)
        {
            if (@this == null)
                throw new ArgumentNullException(nameof(@this));

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            using var process = @this.CreateProcess();
            return process.Update(input);
        }

    }
}
