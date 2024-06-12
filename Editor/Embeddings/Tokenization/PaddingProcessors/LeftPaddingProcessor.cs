using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Embeddings.Tokenization.PaddingProcessors
{
    /// <summary>
    ///     Pads the sequences of tokens by adding tokens to the left.
    /// </summary>
    class LeftPaddingProcessor : DirectionalPaddingProcessorBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LeftPaddingProcessor" /> type.
        /// </summary>
        /// <param name="paddingSizeProvider">
        ///     Computes the target length of the padded sequences.
        /// </param>
        /// <param name="padToken">
        ///     The token to use to pad a sequence of token.
        /// </param>
        public LeftPaddingProcessor(
            [NotNull] IPaddingSizeProvider paddingSizeProvider,
            ITokenDefinition padToken)
            : base(paddingSizeProvider, padToken)
        {
        }

        protected override IEnumerable<(int id, int attention)> Pad(IReadOnlyCollection<int> input,
            int padSize)
        {
            for (int i = 0, limit = padSize - input.Count; i < limit; i++)
                foreach (var id in PadToken.Ids)
                    yield return (id, 0);

            foreach (var token in input)
                yield return (token, 1);
        }
    }
}
