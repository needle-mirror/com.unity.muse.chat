using System.Reflection;
using Unity.Muse.Chat.Model;

namespace Unity.Muse.Chat.FunctionCalling
{
    class CachedFunction
    {
        public MethodInfo Method;
        public FunctionDefinition FunctionDefinition;
    }
}
