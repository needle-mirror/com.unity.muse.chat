using System;

namespace Unity.Muse.Chat
{
    static class TypeDef<T>
    {
        public static readonly Type Value = typeof(T);
        public static readonly string Name = Value.Name;
    }
}
