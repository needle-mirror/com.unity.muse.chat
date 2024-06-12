using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers;

namespace Unity.Muse.Chat.Embeddings.Tokenization.Tokenizers
{
    /// <summary>
    ///     Caches the result of the tokenization of each chunk returned by the pre-tokenizer
    ///     (<see cref="IPreTokenizer" />).
    ///     Gives a chance to save tokenization process from the tokenization model
    ///     (<see cref="ITokenizer" />).
    /// </summary>
    class TokenizationCache : TokenizerBase
    {
        /// <summary>
        ///     Caches the sequence of ids for a given substring.
        /// </summary>
        readonly Dictionary<SubString, int[]> m_Cache = new();

        readonly Pool<List<int>> m_ListOfIntPool = PoolUtility.GetListOfIntPool();

        /// <summary>
        ///     The model used in case the cache doesn't contain any token representation of the
        ///     specified input.
        /// </summary>
        readonly ITokenizer m_Tokenizer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TokenizationCache" /> type.
        /// </summary>
        /// <param name="tokenizer">
        ///     The fallback model to use in case the cache doesn't contain any token representation
        ///     of the specified input.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="tokenizer" /> cannot be null.
        /// </exception>
        public TokenizationCache([NotNull] ITokenizer tokenizer)
        {
            m_Tokenizer = tokenizer ?? throw new ArgumentNullException(nameof(tokenizer));
        }

        /// <inheritdoc />
        protected override void TokenizeInternal(SubString input, ICollection<int> output)
        {
            var found = m_Cache.TryGetValue(input, out var ids);
            if (!found)
            {
                using var tokensHandle = m_ListOfIntPool.Get(out var tokens);
                m_Tokenizer.Tokenize(input, tokens);
                ids = tokens.ToArray();
                m_Cache[input.Apply()] = ids;
            }

            foreach (var id in ids)
                output.Add(id);
        }

        protected override void DeTokenizeInternal(IEnumerable<int> input,
            ICollection<string> output)
        {
            m_Tokenizer.DeTokenize(input, output);
        }
    }
}
