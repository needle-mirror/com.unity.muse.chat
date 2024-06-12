using System;
using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization.Truncators
{
    /// <summary>
    ///     Placeholder implementation of the truncation.
    ///     Does not truncate, only concatenates the primary and secondary sequence of tokens.
    /// </summary>
    class DefaultTruncator : ITruncator
    {
        public void Truncate(
            IEnumerable<int> inputA,
            IEnumerable<int> inputB,
            int numAddedTokens,
            IOutput<IEnumerable<int>> outputA,
            IOutput<IEnumerable<int>> outputB)
        {
            if (inputA == null)
                throw new ArgumentNullException(nameof(inputA));

            if (outputA == null)
                throw new ArgumentNullException(nameof(outputA));

            if (inputB is not null && outputB is null)
                throw new ArgumentNullException(nameof(outputB));

            outputA.Write(inputA);

            if (inputB is not null)
                outputB.Write(inputB);
        }
    }
}
