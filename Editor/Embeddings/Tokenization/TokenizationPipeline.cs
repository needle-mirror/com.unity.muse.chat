using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Muse.Chat.Embeddings.Tokenization.Decoders;
using Unity.Muse.Chat.Embeddings.Tokenization.PaddingProcessors;
using Unity.Muse.Chat.Embeddings.Tokenization.PostProcessors;
using Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers;
using Unity.Muse.Chat.Embeddings.Tokenization.Tokenizers;
using Unity.Muse.Chat.Embeddings.Tokenization.Truncators;

namespace Unity.Muse.Chat.Embeddings.Tokenization
{
    /// <summary>
    ///     <para>
    ///         This type is the entry point of the tokenization/detokenization pipeline.
    ///     </para>
    ///     <para>
    ///         The pipeline is composed of six steps, and turns an input string into an
    ///         <see cref="IEncoding" /> chain:
    ///     </para>
    ///     <list type="number">
    ///         <item>
    ///             <term>Pretokenization</term>
    ///             <description>
    ///                 Splits the result of the normalization step into small pieces (example:
    ///                 split by whitespace).
    ///                 See <see cref="IPreTokenizer" /> for more details.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Encoding</term>
    ///             <description>
    ///                 Central step of the tokenization, this one turns each piece from the
    ///                 pretokenizaztion process into sequence of <see cref="int" /> ids.
    ///                 See <see cref="ITokenizer" /> for more details.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Truncation</term>
    ///             <description>
    ///                 Splits the sequence of ids from the encoding step into smaller subsequences.
    ///                 The most frequent truncation rule in "max length".
    ///                 See <see cref="ITruncator" /> for more details.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Postprocessing</term>
    ///             <description>
    ///                 Transforms each subsequences of generated from the truncation.
    ///                 The most common transformation is adding <c>[CLS]</c> and <c>[SEP]</c>
    ///                 tokens before and after the sequence.
    ///                 See <see cref="IPostProcessor" /> for more details.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Padding</term>
    ///             <description>
    ///                 Pads each subsequence from the postprocessing to match the expected sequence
    ///                 size.
    ///                 See <see cref="IPaddingProcessor" /> for more details.
    ///             </description>
    ///         </item>
    ///     </list>
    /// </summary>
    partial class TokenizationPipeline : ITokenizationPipeline
    {
        readonly IDecoder m_Decoder;
        readonly Pool<List<(int, int)>> m_ListOfIntIntPool;

        readonly Pool<List<int>> m_ListOfIntPool;
        readonly Pool<List<List<(int, int)>>> m_ListOfListOfIntIntPool;

        readonly Pool<List<List<int>>> m_ListOfListOfIntPool;
        readonly Pool<List<string>> m_ListOfStringPool;
        readonly Pool<OutputCollection<(int, int)>> m_OutputCollectionOfIntIntPool;

        readonly Pool<OutputCollection<int>> m_OutputCollectionOfIntPool;
        readonly IPaddingProcessor m_PaddingProcessor;
        readonly IPostProcessor m_PostProcessor;
        readonly IPreTokenizer m_PreTokenizer;
        readonly ITokenizer m_Tokenizer;
        readonly ITruncator m_Truncator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Agents.Tokenizer" /> type.
        /// </summary>
        /// <param name="tokenizer">
        ///     The <see cref="ITokenizer" /> encoding to use to turn the strings into tokens.
        /// </param>
        /// <param name="preTokenizer">
        ///     The pretokenization rules.
        ///     See <see cref="IPreTokenizer" />.
        /// </param>
        /// <param name="postProcessor">
        ///     The post processing of the token sequence.
        ///     See <see cref="IPostProcessor" />.
        /// </param>
        /// <param name="truncator">
        ///     The truncation rules.
        ///     See <see cref="ITruncator" />.
        /// </param>
        /// <param name="paddingProcessor">
        ///     The padding rules.
        ///     See <see cref="IPaddingProcessor" />.
        /// </param>
        /// <param name="cacheTokenizerResult">
        ///     If <see langword="true" />, caches the result of the <see cref="ITokenizer.Tokenize" />
        ///     calls in order to optimize the encoding process.
        ///     Some tokenization strategies can be resource consuming (like
        ///     <see cref="BpeTokenizer" />) and the probability of tokenizing the same words multiple
        ///     times is really high.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="tokenizer" /> cannot be <see langword="null" />.
        /// </exception>
        public TokenizationPipeline(
            [NotNull] ITokenizer tokenizer,
            [CanBeNull] IPreTokenizer preTokenizer = null,
            [CanBeNull] IPostProcessor postProcessor = null,
            [CanBeNull] ITruncator truncator = null,
            [CanBeNull] IPaddingProcessor paddingProcessor = null,
            [CanBeNull] IDecoder decoder = null,
            bool cacheTokenizerResult = true)
        {
            if (tokenizer is null)
                throw new ArgumentNullException(nameof(tokenizer));

            m_Tokenizer = cacheTokenizerResult ? new TokenizationCache(tokenizer) : tokenizer;

            m_PreTokenizer = preTokenizer ?? new DefaultPreTokenizer();
            m_PostProcessor = postProcessor ?? new DefaultPostProcessor();
            m_Truncator = truncator ?? new DefaultTruncator();
            m_PaddingProcessor = paddingProcessor ?? new DefaultPaddingProcessor();
            m_Decoder = decoder ?? new DefaultDecoder();

            // Pools initialization
            {
                m_ListOfIntPool = PoolUtility.GetListOfIntPool();
                m_ListOfStringPool = PoolUtility.GetListOfStringPool();

                m_ListOfIntIntPool = new Pool<List<(int, int)>>(
                    () => new List<(int, int)>(),
                    list => list.Clear());

                m_ListOfListOfIntPool = new Pool<List<List<int>>>(
                    () => new List<List<int>>(),
                    list => list.Clear());

                m_ListOfListOfIntIntPool =
                    new Pool<List<List<(int, int)>>>(
                        () => new List<List<(int, int)>>(),
                        list => list.Clear());

                m_OutputCollectionOfIntPool = new Pool<OutputCollection<int>>(
                    () => new OutputCollection<int>(),
                    o => o.Reset());

                m_OutputCollectionOfIntIntPool = new Pool<OutputCollection<(int, int)>>(
                    () => new OutputCollection<(int, int)>(),
                    o => o.Reset());
            }
        }

        /// <inheritdoc />
        IEncoding ITokenizationPipeline.Encode
            (string inputA, string inputB, bool addSpecialTokens)
        {
            return Encode(inputA, inputB, addSpecialTokens);
        }

        /// <inheritdoc />
        string ITokenizationPipeline.Decode(IEnumerable<int> input)
        {
            return Decode(input);
        }

        /// <inheritdoc cref="ITokenizationPipeline.Encode" />
        public Encoding Encode
        ([NotNull] string inputA, [CanBeNull] string inputB = default,
            bool addSpecialTokens = true)
        {
            if (inputA is null)
                throw new ArgumentNullException(nameof(inputA));

            return EncodeInternal(inputA, inputB, addSpecialTokens);
        }

        /// <inheritdoc cref="ITokenizationPipeline.Decode" />
        public string Decode([NotNull] IEnumerable<int> input)
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));

            using var deTokenizedHandle = m_ListOfStringPool.Get(out var deTokenized);
            m_Tokenizer.DeTokenize(input, deTokenized);

            using var finalHandle = m_ListOfStringPool.Get(out var final);

            using (PoolUtility.GetOutputOfStringPool().Get(out var decodeOutput))
            {
                decodeOutput.Target = final;
                m_Decoder.Decode(deTokenized, decodeOutput);
            }

            return string.Concat(final);
        }

        /// <inheritdoc cref="ITokenizationPipeline.Encode" />
        Encoding EncodeInternal
            ([NotNull] string inputA, [CanBeNull] string inputB, bool addSpecialTokens)
        {
            var isPair = inputB is not null;

            // 1. Tokenization
            using var seqAHandle = m_ListOfIntPool.Get(out var sequenceA);
            using var seqBHandle = m_ListOfIntPool.Get(out var sequenceB);

            TokenizeInput(inputA, sequenceA);

            if (isPair)
                TokenizeInput(inputB, sequenceB);

            // 2. Truncation
            using var truncatedAHandle = m_ListOfListOfIntPool.Get(out var truncatedA);
            using var truncatedBHandle = m_ListOfListOfIntPool.Get(out var truncatedB);

            Truncate(
                sequenceA,
                isPair ? sequenceB : null,
                addSpecialTokens,
                truncatedA,
                truncatedB,
                m_ListOfIntPool.Get);

            // 3. Post Processing
            using var tokenHandle = m_ListOfListOfIntPool.Get(out var tokens);

            PostProcess(
                truncatedA,
                isPair ? truncatedB : default,
                addSpecialTokens,
                tokens,
                m_ListOfIntPool.Get);

            // truncatedA content can be released.
            foreach (var list in truncatedA)
                m_ListOfIntPool.Release(list);

            // truncatedB content can be released.
            if (isPair)
                foreach (var list in truncatedB)
                    m_ListOfIntPool.Release(list);

            // 4. Padding
            using var paddedHandle = m_ListOfListOfIntIntPool.Get(out var padded);

            Pad(tokens, padded, m_ListOfIntIntPool.Get);

            // tokens content can be released.
            foreach (var token in tokens)
                m_ListOfIntPool.Release(token);

            using var handleIds = m_ListOfIntPool.Get(out var ids);
            using var handleAttention = m_ListOfIntPool.Get(out var attentions);

            Encoding head = null;
            Encoding parent = null;

            foreach (var sequence in padded)
            {
                foreach (var (id, attention) in sequence)
                {
                    ids.Add(id);
                    attentions.Add(attention);
                }

                var encoding = new Encoding(ids.ToArray(), attentions.ToArray());

                ids.Clear();
                attentions.Clear();

                if (parent is not null)
                    parent.SetOverflow(encoding);
                else
                    head = encoding;

                parent = encoding;
            }

            // padded content can be released.
            foreach (var list in padded)
                m_ListOfIntIntPool.Release(list);

            return head;

            void TokenizeInput(string input, ICollection<int> output)
            {
                // 1.a Pre Tokenization
                using var chunksHandle = PoolUtility.GetListOfSubStringPool().Get(out var chunks);
                using var tempOutputHandle =
                    PoolUtility.GetOutputOfSubStringPool().Get(out var tempOutput);
                tempOutput.Target = chunks;

                m_PreTokenizer.PreTokenize(input, tempOutput);

                // 1.b Tokenization
                foreach (var chunk in chunks)
                    m_Tokenizer.Tokenize(chunk, output);
            }

            void PostProcess(
                IEnumerable<List<int>> inputA,
                IEnumerable<List<int>> inputB,
                bool addSpecialTokens,
                ICollection<List<int>> output,
                Func<List<int>> getList)
            {
                using var tokenOutputHandle = m_OutputCollectionOfIntPool.Get(out var tokenOutput);
                tokenOutput.Init(output, getList);

                m_PostProcessor.PostProcess(
                    inputA,
                    inputB,
                    addSpecialTokens,
                    tokenOutput);
            }

            void Truncate(
                IEnumerable<int> inputA,
                IEnumerable<int> inputB,
                bool addSpecialTokens,
                ICollection<List<int>> outputA,
                ICollection<List<int>> outputB,
                Func<List<int>> getList)
            {
                using var truncationOutputAHandle =
                    m_OutputCollectionOfIntPool.Get(out var truncationOutputA);
                truncationOutputA.Init(outputA, getList);

                using var truncationOutputBHandle =
                    m_OutputCollectionOfIntPool.Get(out var truncationOutputB);
                truncationOutputB.Init(outputB, getList);

                var numAddedTokens = addSpecialTokens
                    ? m_PostProcessor.GetNumAddedTokens(inputB is not null)
                    : 0;
                m_Truncator.Truncate(inputA, inputB, numAddedTokens, truncationOutputA,
                    truncationOutputB);
            }

            void Pad(IEnumerable<List<int>> input, ICollection<List<(int, int)>> target,
                Func<List<(int, int)>> getList)
            {
                using var paddingOutputHandle =
                    m_OutputCollectionOfIntIntPool.Get(out var paddingOutput);
                paddingOutput.Init(target, getList);

                m_PaddingProcessor.Pad(input, paddingOutput);
            }
        }
    }
}
