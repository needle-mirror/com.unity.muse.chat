using System;

namespace Unity.Muse.Chat
{
    partial class AttributeBasedContextRetrievalBuilder
    {
        class Selector : IContextSelection
        {
            readonly Func<string> m_Func;

            public Selector(string classifier, string description, Func<string> func)
            {
                Classifier = classifier;
                Description = description;
                m_Func = func;
            }

            public bool Equals(IContextSelection other) =>
                other is Selector selector
                && ReferenceEquals(m_Func, selector.m_Func);

            public string Classifier { get; }

            public string Description { get; }

            public string Payload => m_Func.Invoke();
        }
    }
}
