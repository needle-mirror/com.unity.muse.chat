using SEncoding = System.Text.Encoding;
using Bytes = System.Collections.ObjectModel.ReadOnlyCollection<byte>;
using IBytes = System.Collections.Generic.IReadOnlyCollection<byte>;

namespace Unity.Muse.Chat.Tokenization
{
    /// <summary>
    ///     Convert <see cref="SubString" /> input to a read only sequence of <see cref="byte" />s.
    /// </summary>
    class SubStringToByteConverter : IConverter<SubString, IBytes>
    {
        /// <summary>
        ///     Encodes the input <see cref="SubString" /> into a sequence of <see cref="byte" />s.
        /// </summary>
        readonly SEncoding m_Encoding;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SubStringToByteConverter" /> type.
        /// </summary>
        public SubStringToByteConverter()
        {
            m_Encoding = SEncoding.UTF8;
        }

        /// <summary>
        ///     Gets a singleton instance of the <see cref="SubStringToByteConverter" /> type.
        /// </summary>
        public static SubStringToByteConverter Instance { get; } = new();

        IBytes IConverter<SubString, IBytes>.Convert(SubString input)
        {
            return Convert(input);
        }

        /// <summary>
        ///     .
        /// </summary>
        /// <param name="input">
        ///     .
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        public Bytes Convert(SubString input)
        {
            var (source, from, length) = input;
            var bytes = m_Encoding.GetBytes(source, from, length);
            var converted = new Bytes(bytes);
            return converted;
        }
    }
}
