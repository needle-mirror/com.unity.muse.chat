using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization.Tokenizers
{
    /// <summary>
    ///     Turns an input string into a sequence of token ids.
    /// </summary>
    interface ITokenizer
    {
        /// <summary>
        ///     Turns an input string into a sequence of token ids.
        /// </summary>
        /// <param name="input">
        ///     The source string to tokenize.
        /// </param>
        /// <param name="output">
        ///     Target sequence of token ids.
        /// </param>
        void Tokenize(SubString input, [NotNull] ICollection<int> output);

        void DeTokenize([NotNull] IEnumerable<int> input, [NotNull] ICollection<string> output);
    }
}
