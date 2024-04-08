using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Chat.Processors;
using Unity.Muse.Chat.VectorStorage;
using UnityEditor;
using UnityEngine;

#if UNITY_2023_1_OR_NEWER
using UnityEngine;
#else
using System.Threading.Tasks;
#endif

namespace Unity.Muse.Chat
{
    class ContextRetrieval : IDisposable
    {
        static ContextRetrieval s_Instance;

        static async
#if UNITY_2023_1_OR_NEWER
        Awaitable
#else
        Task
#endif
        Init()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;

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

            s_Instance = new()
            {
                m_VectorStore = vectorStore,
                m_ContextSelections = contextSelections,
                m_Classifiers = classifierLut
            };
        }

        static void OnBeforeAssemblyReload()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;

            s_Instance?.Dispose();
            s_Instance = null;
        }

        public static async
#if UNITY_2023_1_OR_NEWER
        Awaitable<(string classifier, float priority)[]>
#else
            Task<(string classifier, float priority)[]>
#endif
            GetClassifiers(string question, int topK, float minScore=0.01f)
        {
            if (s_Instance is null)
                await Init();

            return (await s_Instance!.m_VectorStore.Query(question, topK, minScore))
                .Select(t => (s_Instance.m_Classifiers[t.key], t.priority))
                .ToArray();
        }


        public static async
#if UNITY_2023_1_OR_NEWER
        Awaitable<IContextSelection[]>
#else
            Task<IContextSelection[]>
#endif
            GetContext(params string[] classifiers)
        {
            if (s_Instance is null)
                await Init();

            return classifiers
                .SelectMany(classifier => s_Instance.m_ContextSelections[classifier])
                .ToArray();
        }

        Dictionary<string, string> m_Classifiers;
        Dictionary<string, IContextSelection[]> m_ContextSelections;
        IVectorStore m_VectorStore;

        ContextRetrieval()
        {}

        ~ContextRetrieval() => DisposeObject();

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
