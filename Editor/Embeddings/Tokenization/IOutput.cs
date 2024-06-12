using System.Collections.Generic;

namespace Unity.Muse.Chat.Embeddings.Tokenization
{
    interface IOutput<in T>
    {
        void Write(T value);
        void Write(IEnumerable<T> values);
    }
}
