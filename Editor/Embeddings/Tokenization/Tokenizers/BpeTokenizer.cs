using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Unity.Muse.Chat.Tokenization.Tokenizers
{
    /// <summary>
    ///     Turns a string input into a sequence of <see cref="ITokenDefinition" /> instances using
    ///     the Byte-Pair Encoding strategy.
    /// </summary>
    partial class BpeTokenizer : TokenizerBase
    {
        /// <summary>
        ///     Turns a list of token ids into list of token string values.
        /// </summary>
        IConverter<IEnumerable<int>, IEnumerable<string>> m_DeTokenizer;

        /// <summary>
        ///     The converter in charge of optimizing the output of the <see cref="m_Tokenizer" />
        ///     using the merge rules.
        /// </summary>
        IConverter<IEnumerable<ITokenDefinition>, IEnumerable<ITokenDefinition>> m_Merger;

        /// <summary>
        ///     The converter in charge of turning each character of the string into an instance of
        ///     <see cref="ITokenDefinition" /> using the given vocabulary.
        /// </summary>
        IConverter<SubString, IEnumerable<ITokenDefinition>> m_Tokenizer;

        /// <summary>
        ///     Convert a substring into a sequence of <see cref="ITokenDefinition" /> instances
        ///     using the Byte-Pair Encoding strategy.
        /// </summary>
        /// <param name="vocabulary">
        ///     The map associating token string representation with their ids.
        /// </param>
        /// <param name="merges">
        ///     The list of mergeable token pairs, ordered by priority.
        /// </param>
        /// <param name="unknownConfig">
        ///     The config for unknown tokens.
        ///     <ul>
        ///         <li><c>token</c>: the string representation of the token to use.</li>
        ///         <li><c>fuse</c>: tells whether the sequence of unknown tokens is fused.</li>
        ///         <li>
        ///             <c>byteFallback</c>: tells if tokenized byte must be searched before using
        ///             the unknown token.
        ///         </li>
        ///     </ul>
        /// </param>
        /// <param name="decoratorConfig">
        ///     Decorates token string representation.
        ///     <ul>
        ///         <li>
        ///             <c>subWordPrefix</c>: string prepended to a token value representing a piece
        ///             of a word while not being at the beginning of it.
        ///         </li>
        ///         <li>
        ///             <c>wordSuffix</c>: string appended to a token value representing a piece of
        ///             a word which being at the end of it.
        ///         </li>
        ///     </ul>
        /// </param>
        public BpeTokenizer(
            [NotNull] IVocabulary vocabulary,
            [CanBeNull] IEnumerable<(string a, string b)> merges = null,
            [CanBeNull] (string token, bool fuse, bool byteFallback)? unknownConfig = default,
            [CanBeNull] (string subWordPrefix, string wordSuffix)? decoratorConfig = default)
        {
            if (vocabulary == null) throw new ArgumentNullException(nameof(vocabulary));

            // building the unknown token configuration.

            var unknown = (
                token: default(ITokenDefinition),
                unknownConfig?.fuse ?? false,
                unknownConfig?.byteFallback ?? false);

            if (unknownConfig?.token is not null)
            {
                if (!vocabulary.TryGetToken(unknownConfig.Value.token, out var definition, true))
                    throw new ArgumentOutOfRangeException(
                        nameof(unknownConfig.Value.token),
                        unknownConfig.Value.token, null);

                unknown.token = definition;
            }

            // building the token decorator configuration.

            var decorator = (
                subWordPrefix: string.Empty,
                wordSuffix: string.Empty);

            if (decoratorConfig.HasValue)
            {
                if (decoratorConfig.Value.subWordPrefix != null)
                    decorator.subWordPrefix = decoratorConfig.Value.subWordPrefix;

                if (decoratorConfig.Value.wordSuffix != null)
                    decorator.wordSuffix = decoratorConfig.Value.wordSuffix;
            }

            // creating the default tokenizer
            var stringToTokenSequence = new InternalTokenizer(vocabulary, unknown, decorator);

            // creating the merger
            var merger = BuildMerger(vocabulary, merges, decorator);

            var deTokenizer = new TokenToStringConverter(vocabulary);

            Init(stringToTokenSequence, merger, deTokenizer);
        }

        /// <summary>
        ///     This constructor is used for unit testing purpose.
        /// </summary>
        /// <param name="tokenizer">
        ///     An implementation of the string->token conversion.
        /// </param>
        /// <param name="merger">
        ///     An implementation of the token merging process.
        /// </param>
        /// <param name="deTokenizer">
        ///     An implementation of the token to string process.
        /// </param>
        internal BpeTokenizer(
            IConverter<SubString, IEnumerable<ITokenDefinition>> tokenizer,
            IConverter<IEnumerable<ITokenDefinition>, IEnumerable<ITokenDefinition>> merger,
            IConverter<IEnumerable<int>, IEnumerable<string>> deTokenizer)
        {
            Init(tokenizer, merger, deTokenizer);
        }

        /// <summary>
        ///     Builds the merger.
        /// </summary>
        /// <param name="vocabulary">
        ///     The value->ids map of token definitions.
        /// </param>
        /// <param name="merges">
        ///     The list of mergeable pairs, ordered from the most frequent to the rarest.
        /// </param>
        /// <param name="decorator">
        ///     Configuration for the token string representation.
        ///     <ul>
        ///         <li>
        ///             <c>subWordPrefix</c>: string prepended to a token value representing a piece
        ///             of a word while not being at the beginning of it.
        ///         </li>
        ///         <li>
        ///             <c>wordSuffix</c>: string appended to a token value representing a piece of
        ///             a word which being at the end of it.
        ///         </li>
        ///     </ul>
        /// </param>
        /// <returns>
        ///     The merger instance.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        static IConverter<IEnumerable<ITokenDefinition>, IEnumerable<ITokenDefinition>> BuildMerger(
            IVocabulary vocabulary,
            IEnumerable<(string a, string b)> merges,
            (string subWordPrefix, string wordSuffix) decorator)
        {
            IConverter<IEnumerable<ITokenDefinition>, IEnumerable<ITokenDefinition>> merger;

            // If no merge rules, returning an instance of DefaultMerger, which does nothing.
            if (merges is null)
            {
                merger = new DefaultMerger();
            }
            else
            {
                var mergeDefinitions = merges.Select((t, rank) =>
                {
                    if (!vocabulary.TryGetToken(t.a, out var a))
                        throw new ArgumentException($"Token {t.a} not found in the vocabulary",
                            nameof(merges));

                    if (!vocabulary.TryGetToken(t.b, out var b))
                        throw new ArgumentException($"Token {t.b} not found in the vocabulary",
                            nameof(merges));

                    var mergedWord =
                        string.Concat(a.Value, b.Value[decorator.subWordPrefix.Length..]);
                    if (!vocabulary.TryGetToken(mergedWord, out var mergedToken))
                        throw new ArgumentException(
                            $"Merged word '{mergedWord}' not found in the vocabulary");

                    return (a, b, mergedToken, rank);
                });

                merger = new Merger(mergeDefinitions);
            }

            return merger;
        }

        /// <summary>
        ///     Initializes the <see cref="BpeTokenizer" /> instance.
        /// </summary>
        /// <param name="tokenizer">
        ///     An implementation of the string->token conversion.
        /// </param>
        /// <param name="merger">
        ///     An implementation of the token merging process.
        /// </param>
        /// <param name="deTokenizer">
        ///     An implementation of the token to string process.
        /// </param>
        void Init(
            IConverter<SubString, IEnumerable<ITokenDefinition>> tokenizer,
            IConverter<IEnumerable<ITokenDefinition>, IEnumerable<ITokenDefinition>> merger,
            IConverter<IEnumerable<int>, IEnumerable<string>> deTokenizer)
        {
            m_Tokenizer = tokenizer;
            m_Merger = merger;
            m_DeTokenizer = deTokenizer;
        }

        /// <inheritdoc />
        protected override void TokenizeInternal(SubString input, ICollection<int> output)
        {
            var definitions = m_Tokenizer.Convert(input);
            definitions = m_Merger.Convert(definitions);
            foreach (var definition in definitions)
            foreach (var id in definition.Ids)
                output.Add(id);
        }

        protected override void DeTokenizeInternal(IEnumerable<int> input,
            ICollection<string> output)
        {
            foreach (var s in m_DeTokenizer.Convert(input))
                output.Add(s);
        }
    }
}
