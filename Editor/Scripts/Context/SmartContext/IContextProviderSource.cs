using System.Reflection;

namespace Unity.Muse.Chat.Context.SmartContext
{
    interface IContextProviderSource
    {
        Info[] GetMethods();

        public class Info
        {
            public string Description;
            public MethodInfo Method;
        }
    }
}
