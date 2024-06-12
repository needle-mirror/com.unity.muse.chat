using System;

namespace Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers
{
    /// <summary>
    ///     Base implementation for the builtin <see cref="IPreTokenizer" />s.
    ///     Runs the necessary parameter validation before calling
    ///     <see cref="PreTokenizeInternal" />.
    /// </summary>
    abstract class PreTokenizerBase : IPreTokenizer
    {
        void IPreTokenizer.PreTokenize(SubString input, IOutput<SubString> output)
        {
            PreTokenize(input, output);
        }

        /// <inheritdoc cref="IPreTokenizer.PreTokenize" />
        public void PreTokenize(SubString input, IOutput<SubString> output)
        {
            if (input.IsNull)
                throw new ArgumentNullException(nameof(input));

            PreTokenizeInternal(input, output);
        }

        /// <inheritdoc cref="PreTokenize" />
        protected abstract void PreTokenizeInternal(SubString input, IOutput<SubString> output);
    }
}
