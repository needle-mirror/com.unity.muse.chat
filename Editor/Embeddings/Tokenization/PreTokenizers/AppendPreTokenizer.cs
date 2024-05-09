using System;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization.PreTokenizers
{
    /// <summary>
    ///     Adds a suffix to the input string.
    /// </summary>
    class AppendPreTokenizer : PreTokenizerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PrependPreTokenizer" /> type.
        /// </summary>
        /// <param name="suffix">
        ///     The suffix to add to the input string when passed to
        ///     <see cref="IPreTokenizer.PreTokenize" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="suffix" /> cannot be <c>null</c> or empty.
        /// </exception>
        public AppendPreTokenizer(
            [NotNull] string suffix)
        {
            if (string.IsNullOrEmpty(suffix))
                throw new ArgumentNullException(nameof(suffix));

            Suffix = suffix;
        }

        /// <summary>
        ///     The prefix to add to the input string when passed to
        ///     <see cref="IPreTokenizer.PreTokenize" />.
        /// </summary>
        public string Suffix { get; }

        protected override void PreTokenizeInternal(SubString input, IOutput<SubString> output)
        {
            output.Write($"{input}{Suffix}");
        }
    }
}
