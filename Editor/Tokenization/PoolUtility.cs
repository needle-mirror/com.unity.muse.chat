using System.Collections.Generic;
using System.Text;

namespace Unity.Muse.Chat.Tokenization
{
    static class PoolUtility
    {
        static Pool<StringBuilder> s_StringBuilderPool;

        static Pool<List<SubString>> s_SubStringPool;

        static Pool<List<int>> s_ListOfIntPool;

        static Pool<List<byte>> s_ListOfBytePool;

        static Pool<List<string>> s_ListOfStringPool;

        static Pool<Output<SubString>> s_OutputOfSubStringPool;

        static Pool<Output<string>> s_OutputOfStringPool;

        public static Pool<StringBuilder> GetStringBuilderPool()
        {
            return s_StringBuilderPool ??= new Pool<StringBuilder>(
                () => new StringBuilder(),
                sb => sb.Clear());
        }

        public static Pool<List<SubString>> GetListOfSubStringPool()
        {
            return s_SubStringPool ??= new Pool<List<SubString>>(
                () => new List<SubString>(),
                l => l.Clear());
        }

        public static Pool<List<int>> GetListOfIntPool()
        {
            return s_ListOfIntPool ??= new Pool<List<int>>(
                () => new List<int>(),
                l => l.Clear());
        }

        public static Pool<List<byte>> GetListOfBytePool()
        {
            return s_ListOfBytePool ??= new Pool<List<byte>>(
                () => new List<byte>(),
                l => l.Clear());
        }

        public static Pool<List<string>> GetListOfStringPool()
        {
            return s_ListOfStringPool ??= new Pool<List<string>>(
                () => new List<string>(),
                l => l.Clear());
        }

        public static Pool<Output<SubString>> GetOutputOfSubStringPool()
        {
            return s_OutputOfSubStringPool ??= new Pool<Output<SubString>>(
                () => new Output<SubString>(),
                o => o.Reset());
        }

        public static Pool<Output<string>> GetOutputOfStringPool()
        {
            return s_OutputOfStringPool ??= new Pool<Output<string>>(
                () => new Output<string>(),
                o => o.Reset());
        }
    }
}
