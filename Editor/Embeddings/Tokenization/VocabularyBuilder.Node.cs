using System.Collections.Generic;

namespace Unity.Muse.Chat.Tokenization
{
    partial class VocabularyBuilder
    {
        class Node<TKey, TValue>
        {
            public Dictionary<TKey, Node<TKey, TValue>> children;
            public Node<TKey, TValue> parent;
            public TValue value;
        }
    }
}
