using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Chat.Embeddings.Processing;
using Unity.Muse.Chat.Embeddings.Tokenization;
using Unity.Muse.Chat.Embeddings.Tokenization.PaddingProcessors;
using Unity.Muse.Chat.Embeddings.Tokenization.PostProcessors;
using Unity.Muse.Chat.Embeddings.Tokenization.PostProcessors.Templating;
using Unity.Muse.Chat.Embeddings.Tokenization.PreTokenizers;
using Unity.Muse.Chat.Embeddings.Tokenization.Tokenizers;
using Unity.Muse.Chat.Embeddings.Tokenization.Truncators;
using Unity.Sentis;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat.Embeddings.Processors
{
    static class MiniLM_SentisProcessorProvider
    {
        public enum Backend
        {
            Cpu,
            Gpu
        }

        const string k_MiniLM_SentisModelGuid = "7dc3bc5cb146a4d00bcbdf09b2230f29";
        const string k_MiniLM_TokenizerDataGuid = "8806eb8dd84694df29c2538bb2ac1f40";
        const int k_MaxSequenceLength = 128;
        const Backend k_DefaultBackend = Backend.Gpu;

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

        static BackendType GetBackend(Backend value) =>
            value switch
            {
                Backend.Cpu => BackendType.CPU,
                Backend.Gpu => BackendType.GPUCompute,
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };

        public static IDataProcessor<string, float[]> BuildProcessor(Backend backend = k_DefaultBackend)
        {
            var modelPath = AssetDatabase.GUIDToAssetPath(k_MiniLM_SentisModelGuid);
            var model = ModelLoader.Load(modelPath);
            var worker = WorkerFactory.CreateWorker(GetBackend(backend), model);

            var tokenizationPipeline = CreateTokenizer();

            return new MiniLM_SentisProcessor
                (worker, tokenizationPipeline, k_MaxSequenceLength, true);
        }
    }
}
