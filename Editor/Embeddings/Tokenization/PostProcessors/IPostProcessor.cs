using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Muse.Chat.Embeddings.Tokenization.Tokenizers;

namespace Unity.Muse.Chat.Embeddings.Tokenization.PostProcessors
{
    /// <summary>
    ///     Transforms the sequences of tokens from the truncated output of <see cref="ITokenizer" />
    ///     and merges it into a single sequence.
    /// </summary>
    interface IPostProcessor
    {
        /// <summary>
        ///     Determines the number of tokens that this <see cref="IPostProcessor" /> will add to
        ///     the sequence of tokens.
        /// </summary>
        /// <param name="isPair">
        ///     Tells if we want the number of added tokens for a pair of sequences of tokens
        ///     (<see langword="true" />), of a single sequence (<see langword="false" />).
        /// </param>
        /// <returns>
        ///     number of tokens that this <see cref="IPostProcessor" /> will add to
        ///     the sequence of tokens
        /// </returns>
        int GetNumAddedTokens(bool isPair);

        /// <summary>
        ///     Transforms the sequence of tokens.
        /// </summary>
        /// <param name="sequenceA">
        ///     The primary sequence of tokens (mandatory).
        /// </param>
        /// <param name="sequenceB">
        ///     An optional sequence of tokens.
        /// </param>
        /// <param name="addSpecialTokens">
        ///     Tells whether to add the special tokens when transforming.
        /// </param>
        /// <param name="output">
        ///     The target container to receive the processed sequence.
        /// </param>
        void PostProcess(
            [NotNull] IEnumerable<IEnumerable<int>> sequenceA,
            [CanBeNull] IEnumerable<IEnumerable<int>> sequenceB,
            bool addSpecialTokens,
            IOutput<IEnumerable<int>> output);
    }
}
