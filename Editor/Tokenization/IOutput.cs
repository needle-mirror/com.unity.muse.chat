using System.Collections.Generic;

namespace Unity.Muse.Chat.Tokenization
{
    interface IOutput<in T>
    {
        void Write(T value);
        void Write(IEnumerable<T> values);
    }
}
