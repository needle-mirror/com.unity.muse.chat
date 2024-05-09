using System.Collections.Generic;
using System.Linq;
using SEncoding = System.Text.Encoding;

namespace Unity.Muse.Chat.Tokenization
{
    /// <inheritdoc />
    class CharToByteConverter : IConverter<char, IReadOnlyCollection<byte>>
    {
        /// <summary>
        ///     Instance of <see cref="Encoding" /> used to generate the byte array
        ///     representation.
        /// </summary>
        readonly SEncoding m_Encoder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CharToByteConverter" /> type.
        /// </summary>
        public CharToByteConverter()
            : this(SEncoding.Unicode)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CharToByteConverter" /> type.
        /// </summary>
        /// <param name="encoder">
        ///     The encoding to use to generate the byte array representation of a character.
        /// </param>
        internal CharToByteConverter(SEncoding encoder)
        {
            m_Encoder = encoder;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<byte> Convert(char character)
        {
            return m_Encoder.GetBytes(character.ToString()).ToList().AsReadOnly();
        }
    }
}
