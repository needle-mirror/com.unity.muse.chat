using System;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization.PreTokenizers
{
    /// <summary>
    ///     Replaces a specified pattern by another string.
    /// </summary>
    class ReplacePreTokenizer : PreTokenizerBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ReplacePreTokenizer" /> type.
        /// </summary>
        /// <param name="pattern">
        ///     The pattern to look for in the input string.
        /// </param>
        /// <param name="replacement">
        ///     The string to replace the pattern with.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The <paramref name="pattern" /> cannot be null or empty.
        /// </exception>
        public ReplacePreTokenizer(
            [NotNull] string pattern,
            [CanBeNull] string replacement)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentNullException(nameof(pattern));

            Pattern = pattern;
            Replacement = replacement ?? string.Empty;
        }

        /// <summary>
        ///     The pattern to look for in the input string.
        /// </summary>
        public string Pattern { get; }

        /// <summary>
        ///     The string to replace the pattern with.
        /// </summary>
        public string Replacement { get; }

        /// <inheritdoc />
        protected override void PreTokenizeInternal(SubString input, IOutput<SubString> output)
        {
            var replaced = input.ToString().Replace(Pattern, Replacement);
            output.Write(replaced);
        }
    }
}
