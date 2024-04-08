using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Chat.Processing;
using Unity.Muse.Chat.Tokenization;
using Unity.Muse.Chat.Tokenization.PaddingProcessors;
using Unity.Muse.Chat.Tokenization.PostProcessors;
using Unity.Muse.Chat.Tokenization.PostProcessors.Templating;
using Unity.Muse.Chat.Tokenization.PreTokenizers;
using Unity.Muse.Chat.Tokenization.Tokenizers;
using Unity.Muse.Chat.Tokenization.Truncators;
using Unity.Sentis;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat.Processors
{
    static class MiniLM_SentisProcessorProvider
    {
        const string k_MiniLM_SentisModelGuid = "7dc3bc5cb146a4d00bcbdf09b2230f29";
        const string k_MiniLM_TokenizerDataGuid = "8806eb8dd84694df29c2538bb2ac1f40";
        const int k_MaxSequenceLength = 128;
        const BackendType k_BackendType = BackendType.GPUCompute;

        static TokenizerUtility.TokenizerData GetTokenizerData()
        {
            var dataPath = AssetDatabase.GUIDToAssetPath(k_MiniLM_TokenizerDataGuid);
            var tokenizerData = AssetDatabase.LoadAssetAtPath<TextAsset>(dataPath);
            return TokenizerUtility.Load(tokenizerData.text);
        }

        static TokenizationPipeline CreateTokenizer()
        {
            var tokenizerData = GetTokenizerData();

            var specialTokenValues = new[] { "[PAD]", "[CLS]", "[SEP]", "[UNK]", "[MASK]"};

            var definitions = tokenizerData.Vocab
                .Select(entry => (
                    value: entry.Key,
                    ids: new[] {entry.Value} as IEnumerable<int>,
                    special: specialTokenValues.Contains(entry.Key)));

            var builder = new VocabularyBuilder();
            foreach (var (value, ids, special) in definitions)
                builder.Add(value, ids, special);

            var vocabulary = builder.Build();

            vocabulary.TryGetToken("[PAD]", out var pad, special:true);
            vocabulary.TryGetToken("[CLS]", out var cls, special:true);
            vocabulary.TryGetToken("[SEP]", out var sep, special:true);
            vocabulary.TryGetToken("[UNK]", out var unk, special:true);

            var t = new TokenizationPipeline(
                preTokenizer: new SequencePreTokenizer(
                    new BertNormalizer(),
                    new BertPreTokenizer()),
                tokenizer: new WordPieceTokenizer(vocabulary, unknownToken: unk.Value),
                postProcessor: new TemplatePostProcessor(
                    new Template("[CLS] $A [SEP]"),
                    new Template("[CLS] $A [SEP] $B:1 [SEP]:1"),
                    new[] {cls, sep}),
                truncator: new LongestFirstTruncator(
                    new RightDirectionRangeGenerator(),
                    128,
                    0),
                paddingProcessor: new RightPaddingProcessor(
                    new FixedPaddingSizeProvider(128),
                    pad));

            return t;
        }

        public static IDataProcessor<string, float[]> BuildProcessor(BackendType backendType = k_BackendType)
        {
            var modelPath = AssetDatabase.GUIDToAssetPath(k_MiniLM_SentisModelGuid);
            var model = ModelLoader.Load(modelPath);
            var worker = WorkerFactory.CreateWorker(backendType, model);

            var tokenizationPipeline = CreateTokenizer();

            return new MiniLM_SentisProcessor
                (worker, tokenizationPipeline, k_MaxSequenceLength, true);
        }
    }
}
