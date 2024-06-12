using Unity.Muse.Chat.Embeddings.Tokenization.Tokenizers;

namespace Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers
{
    /// <summary>
    ///     Pre-cuts the input <see cref="string" /> into smaller parts.
    ///     Those parts will be passed to the <see cref="ITokenizer" /> for tokenization.
    /// </summary>
    interface IPreTokenizer
    {
        /// <summary>
        ///     Pre-cuts the <paramref name="input" /> into smaller parts.
        /// </summary>
        /// <param name="input">
        ///     The source to pre-cut.
        /// </param>
        /// <param name="output">
        ///     Target collection of generated pretokenized strings.
        /// </param>
        void PreTokenize(SubString input, IOutput<SubString> output);
    }
}
