using System.Collections.Generic;

namespace Unity.Muse.Chat.Tokenization.PostProcessors
{
    /// <summary>
    ///     Interlaces the primary and secondary sequences of tokens.
    /// </summary>
    class DefaultPostProcessor : PostProcessorBase
    {
        protected override void PostProcessInternal(
            IEnumerable<IEnumerable<int>> sequenceA,
            IEnumerable<IEnumerable<int>> sequenceB,
            bool _,
            IOutput<IEnumerable<int>> output)
        {
            using var enumA = sequenceA.GetEnumerator();
            using var enumB = sequenceB?.GetEnumerator();

            using var resultHandle = PoolUtility.GetListOfIntPool().Get(out var result);

            var (nextA, nextB) = (enumA.MoveNext(), enumB?.MoveNext() ?? false);

            while (nextA || nextB)
            {
                if (nextA)
                    result.AddRange(enumA.Current!);
                if (nextB)
                    result.AddRange(enumB!.Current!);

                output.Write(result);
                result.Clear();

                (nextA, nextB) = (enumA.MoveNext(), enumB?.MoveNext() ?? false);
            }
        }

        public override int GetNumAddedTokens(bool _)
        {
            return 0;
        }
    }
}
