using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine.Assertions;

namespace Unity.Muse.Chat.Embeddings.Tokenization.Tokenizers
{
    /// <summary>
    ///     Converts a string into a sequence of phonemes using the Piper Phonemizer strategy.
    /// </summary>
    class PiperTokenizer : TokenizerBase
    {
        IConverter<IEnumerable<int>, IEnumerable<string>> m_DeTokenizer;
        ITokenDefinition m_EndOfSequence;
        ITokenDefinition m_Separator;
        Pool<List<int>> m_TokenListPool;

        IVocabulary m_Vocabulary;

        /// <summary>
        ///     Creates a new Piper Tokenizer.
        /// </summary>
        /// <param name="vocabulary">
        ///     The map associating phoneme string representations with their ids.
        /// </param>
        /// <param name="separator">
        ///     Optional separator.
        /// </param>
        /// <param name="endOfSequence">
        ///     Optional end of sequence.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public PiperTokenizer
            ([NotNull] IVocabulary vocabulary, string separator = "_", string endOfSequence = "$")
        {
            if (vocabulary is null)
                throw new ArgumentNullException(nameof(vocabulary));

            Assert.IsTrue(vocabulary.TryGetToken(separator, out var separatorToken),
                $"Separator {separator} not found in vocabulary.");
            Assert.IsTrue(vocabulary.TryGetToken(endOfSequence, out var endOfSequenceToken),
                $"End of Sequence {endOfSequence} not found in vocabulary.");

            Init(
                new TokenToStringConverter(vocabulary),
                PoolUtility.GetListOfIntPool(),
                vocabulary,
                separatorToken,
                endOfSequenceToken);
        }

        internal PiperTokenizer(IConverter<IEnumerable<int>, IEnumerable<string>> tokenToString,
            Pool<List<int>> listOfIntPool,
            IVocabulary vocabulary,
            ITokenDefinition separator,
            ITokenDefinition endOfSequence)
        {
            Init(tokenToString, listOfIntPool, vocabulary, separator, endOfSequence);
        }

        void Init(
            IConverter<IEnumerable<int>, IEnumerable<string>> tokenToString,
            Pool<List<int>> listOfIntPool,
            IVocabulary vocabulary,
            ITokenDefinition separator,
            ITokenDefinition endOfSequence)
        {
            m_TokenListPool = listOfIntPool;
            m_DeTokenizer = tokenToString;

            m_Vocabulary = vocabulary;
            m_Separator = separator;
            m_EndOfSequence = endOfSequence;
        }

        protected override void TokenizeInternal(SubString input, ICollection<int> output)
        {
            using var tokenListHandle = m_TokenListPool.Get(out var tokens);

            tokens.Add(1);

            foreach (var phoneme in input)
            {
                var found = m_Vocabulary.TryGetToken(char.ToString(phoneme), out var result);
                if (!found) throw new KeyNotFoundException($"Phoneme: {phoneme} not found.");

                tokens.AddRange(result.Ids);
                tokens.Add(m_Separator.Ids.First());
            }

            tokens.Add(m_EndOfSequence.Ids.First());

            foreach (var token in tokens)
                output.Add(token);
        }

        protected override void DeTokenizeInternal(IEnumerable<int> input,
            ICollection<string> output)
        {
            foreach (var s in m_DeTokenizer.Convert(input))
                output.Add(s);
        }
    }
}
