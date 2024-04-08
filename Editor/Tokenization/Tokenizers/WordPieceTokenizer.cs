using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization.Tokenizers
{
    /// <summary>
    ///     Turns an input string into a sequence of token ids using the Word Piece strategy.
    /// </summary>
    class WordPieceTokenizer : TokenizerBase
    {
        IConverter<IEnumerable<int>, IEnumerable<string>> m_DeTokenizer;
        Pool<List<int>> m_TokenListPool;

        IVocabulary m_Vocabulary;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WordPieceTokenizer" /> type.
        /// </summary>
        /// <param name="vocabulary">
        ///     The value->ids map for token definitions.
        /// </param>
        /// <param name="unknownToken">
        ///     The value of the unknown token.
        /// </param>
        /// <param name="continuingSubWordPrefix">
        ///     The prefix to add to inner subwords (not at the beginning of a word).
        /// </param>
        /// <param name="maxInputCharsPerWord">
        ///     Maximum length of a tokenizable word.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="maxInputCharsPerWord" /> is negative or <c>0</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="vocabulary" /> cannot be <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="unknownToken" /> not found in the vocabulary.
        /// </exception>
        public WordPieceTokenizer(
            [NotNull] IVocabulary vocabulary,
            string unknownToken = "[UNK]",
            string continuingSubWordPrefix = "##",
            int maxInputCharsPerWord = 100)
        {
            if (maxInputCharsPerWord <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxInputCharsPerWord),
                    maxInputCharsPerWord, null);

            if (vocabulary is null)
                throw new ArgumentNullException(nameof(vocabulary));

            if (!vocabulary.TryGetToken(unknownToken, out var unk, true))
                throw new ArgumentException(
                    $"Cannot find the unknown token {unknownToken} in the vocabulary",
                    nameof(unknownToken));

            Init(PoolUtility.GetListOfIntPool(), vocabulary, unk, continuingSubWordPrefix,
                maxInputCharsPerWord);
        }

        internal WordPieceTokenizer(
            Pool<List<int>> listOfIntPool,
            IVocabulary vocabulary,
            ITokenDefinition unknownToken,
            string continuingSubWordPrefix,
            int maxInputCharsPerWord)
        {
            Init(listOfIntPool, vocabulary, unknownToken, continuingSubWordPrefix,
                maxInputCharsPerWord);
        }

        /// <summary>
        ///     The definition to use for unknown token.
        /// </summary>
        public ITokenDefinition UnknownToken { get; private set; }

        /// <summary>
        ///     The prefix to add to inner subwords (not at the beginning of a word).
        /// </summary>
        public string ContinuingSubWordPrefix { get; private set; }

        /// <summary>
        ///     Maximum length of a tokenizable word.
        /// </summary>
        public int MaxInputCharsPerWord { get; private set; }

        void Init(Pool<List<int>> listOfIntPool,
            IVocabulary vocabulary,
            ITokenDefinition unknownToken,
            string continuingSubWordPrefix,
            int maxInputCharsPerWord = 100)
        {
            m_TokenListPool = listOfIntPool;
            m_Vocabulary = vocabulary;

            ContinuingSubWordPrefix = continuingSubWordPrefix;
            MaxInputCharsPerWord = maxInputCharsPerWord;

            m_DeTokenizer = new TokenToStringConverter(vocabulary);
            UnknownToken = unknownToken;
        }

        protected override void TokenizeInternal(SubString input, ICollection<int> output)
        {
            if (input.Length > MaxInputCharsPerWord)
            {
                foreach (var id in UnknownToken.Ids)
                    output.Add(id);
                return;
            }

            using var _ = m_TokenListPool.Get(out var tokens);

            var searchInput = input;
            var @continue = false;
            while (searchInput.Length > 0)
            {
                var found = m_Vocabulary.Find(
                    searchInput,
                    out var result,
                    @continue ? ContinuingSubWordPrefix : null);

                if (!found)
                {
                    foreach (var id in UnknownToken.Ids)
                        output.Add(id);
                    return;
                }

                tokens.AddRange(result.definition.Ids);

                if (searchInput.Length - result.length == 0)
                    break;
                searchInput = searchInput.Sub(result.length);

                @continue = true;
            }

            foreach (var token in tokens)
                output.Add(token);
        }

        protected override void DeTokenizeInternal(IEnumerable<int> input,
            ICollection<string> output)
        {
            var strings = m_DeTokenizer.Convert(input);
            foreach (var s in strings)
                output.Add(s);
        }
    }
}
