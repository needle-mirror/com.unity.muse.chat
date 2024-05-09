using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Chat.Processors;
using Unity.Muse.Chat.VectorStorage;
using UnityEditor;

#if UNITY_2023_1_OR_NEWER
using UnityEngine;
#else
using System.Threading.Tasks;
#endif

namespace Unity.Muse.Chat
{
    class ContextRetrieval : IDisposable
    {
        public static async
#if UNITY_2023_1_OR_NEWER
            Awaitable<ContextRetrieval>
#else
            Task<ContextRetrieval>
#endif
            Create()
        {
            var contextSelections = TypeCache

                // I get all the methods tagged with the ContextRetrievalBuilder attribute
                .GetMethodsWithAttribute<ContextRetrievalBuilderAttribute>()

                // I check that the methods have the proper signature
                // - static
                // - returns a IContextRetrievalBuilder object
                // - has no parameter
                .Where(
                    methodInfo => methodInfo.IsStatic
                        && typeof(IContextRetrievalBuilder).IsAssignableFrom(methodInfo.ReturnType)
                        && methodInfo.GetParameters().Length == 0)

                // I invoke the methods, and the IContextRetrievalBuilder objects
                .Select(methodInfo => (IContextRetrievalBuilder)methodInfo.Invoke(null, null))

                // I get all the IContextSelection objects from the builder objects.
                .SelectMany(builder => builder.GetSelectors())

                // I regroup those context selection object by classifier to avoid duplicate classifiers.
                .GroupBy(cs => cs.Classifier)
                .ToDictionary(g => g.Key, g => g.ToArray());

            var classifiers = contextSelections.Keys.ToArray();

            var embeddingProcessor = MiniLM_SentisProcessorProvider.BuildProcessor();
            var vectorStore = new VectorStore(embeddingProcessor, 384, classifiers.Length);

            var keys = await vectorStore.Insert(classifiers);

            var classifierLut = keys
                .Zip(classifiers, (key, classifier) => (key, classifier))
                .ToDictionary(t => t.key, t => t.classifier);

            return new()
            {
                m_VectorStore = vectorStore,
                m_ContextSelections = contextSelections,
                m_Classifiers = classifierLut
            };
        }

        Dictionary<string, string> m_Classifiers;
        Dictionary<string, IContextSelection[]> m_ContextSelections;
        IVectorStore m_VectorStore;

        ContextRetrieval()
        {}

        ~ContextRetrieval() => DisposeObject();

        public async
#if UNITY_2023_1_OR_NEWER
            Awaitable<(string classifier, float priority)[]>
#else
            Task<(string classifier, float priority)[]>
#endif
            GetClassifiers(string question, int topK, float minScore = 0.01f)
        {
            return (await m_VectorStore.Query(question, topK, minScore))
                .Select(t => (m_Classifiers[t.key], t.priority))
                .ToArray();
        }

        public IContextSelection[] GetContext(params string[] classifiers) =>
            classifiers
                .SelectMany(classifier => m_ContextSelections[classifier])
                .ToArray();

        public void Dispose()
        {
            DisposeObject();
            GC.SuppressFinalize(this);
        }

        void DisposeObject()
        {
            m_VectorStore.Dispose();
            m_VectorStore = null;
        }
    }
}
