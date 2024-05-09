using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization.Tokenizers
{
    /// <summary>
    ///     Base type for builtin implementations of <see cref="ITokenizer" />.
    ///     The <see cref="Tokenize" /> methods do the necessary parameter validations before calling
    ///     a protected <see cref="TokenizeInternal" /> method.
    /// </summary>
    abstract class TokenizerBase : ITokenizer
    {
        void ITokenizer.Tokenize(SubString input, ICollection<int> output)
        {
            Tokenize(input, output);
        }

        void ITokenizer.DeTokenize(IEnumerable<int> input, ICollection<string> output)
        {
            DeTokenize(input, output);
        }

        /// <inheritdoc cref="ITokenizer.Tokenize" />
        public void Tokenize(SubString input, [NotNull] ICollection<int> output)
        {
            if (output == null)
                throw new ArgumentNullException(nameof(output));

            TokenizeInternal(input, output);
        }

        public void DeTokenize([NotNull] IEnumerable<int> input,
            [NotNull] ICollection<string> output)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            DeTokenizeInternal(input, output);
        }

        /// <inheritdoc cref="ITokenizer.Tokenize" />
        protected abstract void TokenizeInternal(SubString input, ICollection<int> output);

        protected abstract void DeTokenizeInternal(IEnumerable<int> input,
            ICollection<string> output);
    }
}
