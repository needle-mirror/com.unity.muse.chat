using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBytes = System.Collections.Generic.IReadOnlyCollection<byte>;

namespace Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers
{
    partial class ByteLevelPreTokenizer : PreTokenizerBase
    {
        IConverter<SubString, IEnumerable<SubString>> m_InputSplitter;
        Pool<StringBuilder> m_StringBuilderPool;
        IConverter<SubString, IBytes> m_SubStringToBytes;
        IConverter<SubString, IEnumerable<SubString>> m_Utf8CharSplitter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ByteLevelPreTokenizer" /> type.
        /// </summary>
        /// <param name="addPrefixSpace">
        ///     Adds a whitespace at the beginning of the input if it doesn't start with one.
        /// </param>
        /// <param name="gpt2Regex">
        ///     Uses the GPT2 regex to split the input into smaller <see cref="SubString" />s.
        /// </param>
        public ByteLevelPreTokenizer(bool addPrefixSpace = true, bool gpt2Regex = true)
        {
            Init(
                gpt2Regex ? new Gpt2Splitter() : new DefaultSplitter(),
                Utf8CharSplitter.Instance,
                new CachedConverter<SubString, IBytes>(SubStringToByteConverter.Instance),
                PoolUtility.GetStringBuilderPool(),
                addPrefixSpace);
        }

        internal ByteLevelPreTokenizer(
            IConverter<SubString, IEnumerable<SubString>> splitter,
            IConverter<SubString, IEnumerable<SubString>> stringToUtf8Chars,
            IConverter<SubString, IBytes> stringToBytes,
            Pool<StringBuilder> stringBuilderPool,
            bool addPrefixSpace)
        {
            Init(
                splitter, stringToUtf8Chars, stringToBytes,
                stringBuilderPool, addPrefixSpace);
        }

        /// <summary>
        ///     Adds a whitespace at the beginning of the input if it doesn't start with one.
        /// </summary>
        public bool AddPrefixSpace { get; private set; }

        void Init(
            IConverter<SubString, IEnumerable<SubString>> inputSplitter,
            IConverter<SubString, IEnumerable<SubString>> utf8CharsSplitter,
            IConverter<SubString, IBytes> subStringToBytes,
            Pool<StringBuilder> stringBuilderPool,
            bool addPrefixSpace)
        {
            m_InputSplitter = inputSplitter;
            m_Utf8CharSplitter = utf8CharsSplitter;
            m_SubStringToBytes = subStringToBytes;
            m_StringBuilderPool = stringBuilderPool;

            AddPrefixSpace = addPrefixSpace;
        }

        protected override void PreTokenizeInternal(SubString input, IOutput<SubString> output)
        {
            if (AddPrefixSpace && !input.StartsWith(" "))
                input = $" {input}";

            var subStrings = m_InputSplitter.Convert(input);

            foreach (var subString in subStrings)
            {
                var characters = m_Utf8CharSplitter
                    .Convert(subString)
                    .SelectMany(utf8Char => m_SubStringToBytes
                        .Convert(utf8Char)
                        .Select(b => ByteLevelHelper.BytesChars[b]));

                output.Write(ToString(characters));
            }
        }

        string ToString(IEnumerable<char> characters)
        {
            using var _ = m_StringBuilderPool.Get(out var builder);
            foreach (var c in characters)
                builder.Append(c);
            return builder.ToString();
        }
    }
}
