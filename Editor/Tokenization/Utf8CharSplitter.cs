using System.Collections.Generic;

namespace Unity.Muse.Chat.Tokenization
{
    /// <summary>
    ///     Separates each UTF-8 character from a <see cref="SubString" /> input.
    /// </summary>
    class Utf8CharSplitter : IConverter<SubString, IEnumerable<SubString>>
    {
        /// <summary>
        ///     Gets the singleton instance of the <see cref="Utf8CharSplitter" /> type.
        /// </summary>
        public static Utf8CharSplitter Instance { get; } = new();

        IEnumerable<SubString> IConverter<SubString, IEnumerable<SubString>>.Convert(
            SubString input)
        {
            return Convert(input);
        }

        /// <inheritdoc cref="IConverter{TFrom,TTo}.Convert" />
        public IEnumerable<SubString> Convert(SubString input)
        {
            var (source, offset, length) = input;
            var to = offset + length;

            while (offset < to)
                if (!char.IsSurrogate(source[offset]))
                {
                    yield return new SubString(source, offset, 1);
                    offset++;
                }

                // Simple character
                else
                {
                    var end = offset + 1;
                    while (end < to && char.IsSurrogate(source[end]))
                        end++;
                    yield return SubString.FromTo(source, offset, end);
                    offset = end;
                }
        }
    }
}
