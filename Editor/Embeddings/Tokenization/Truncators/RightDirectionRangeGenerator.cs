using System;
using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization.Truncators
{
    /// <summary>
    ///     Generates a sequence of <see cref="Range" /> starting from the right (the upper bound of
    ///     the source).
    /// </summary>
    class RightDirectionRangeGenerator : RangeGeneratorBase
    {
        protected override IEnumerable<Range> GetRangesInternal(int length, int rangeMaxLength,
            int stride)
        {
            var offset = rangeMaxLength - stride;

            for (var from = 0; from < length; from += offset)
            {
                var to = Math.Min(from + rangeMaxLength, length);
                yield return new Range(from, to - from);
                if (to == length)
                    yield break;
            }
        }
    }
}
