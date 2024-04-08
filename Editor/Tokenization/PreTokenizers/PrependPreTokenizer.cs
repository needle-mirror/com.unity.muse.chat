using System;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization.PreTokenizers
{
    /// <summary>
    ///     Adds a prefix to the input string.
    /// </summary>
    class PrependPreTokenizer : PreTokenizerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PrependPreTokenizer" /> type.
        /// </summary>
        /// <param name="prefix">
        ///     The prefix to add to the input string when passed to
        ///     <see cref="IPreTokenizer.PreTokenize" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="prefix" /> cannot be <c>null</c> or empty.
        /// </exception>
        public PrependPreTokenizer([NotNull] string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentNullException(nameof(prefix));

            Prefix = prefix;
        }

        /// <summary>
        ///     The prefix to add to the input string when passed to
        ///     <see cref="IPreTokenizer.PreTokenize" />.
        /// </summary>
        public string Prefix { get; }

        /// <inheritdoc />
        protected override void PreTokenizeInternal(SubString input, IOutput<SubString> output)
        {
            var prepended = $"{Prefix}{input}";
            output.Write(prepended);
        }
    }
}
