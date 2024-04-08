using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Unity.Muse.Chat
{
    partial class AttributeBasedContextRetrievalBuilder : IContextRetrievalBuilder
    {
        [ContextRetrievalBuilder]
        static IContextRetrievalBuilder GetInstance() => new AttributeBasedContextRetrievalBuilder();

        public IEnumerable<IContextSelection> GetSelectors()
        {
            var methods = TypeCache
                .GetMethodsWithAttribute<MuseChatContextAttribute>()
                .Where(methodInfo => methodInfo.IsStatic)
                .Select(methodInfo => (methodInfo, attr: (MuseChatContextAttribute)methodInfo.GetCustomAttributes(false).FirstOrDefault(attr => attr is MuseChatContextAttribute)))
                .Where(t => t.methodInfo.ReturnType == typeof(string))
                .Where(t => t.methodInfo.GetParameters().Length == 0)
                .ToArray();

            // Not used at the moment
            //
            // var alwaysIncludedMethods = methods
            //     .Where(t => t.attr.AlwaysInclude)
            //     .Select(t =>
            //         new Func<string>(() => (string)t.methodInfo.Invoke(null, null)))
            //     .ToArray();
            //
            // var optionalMethods = methods
            //     .Where(t => !t.attr.AlwaysInclude)
            //     .GroupBy(t => t.attr.Classifier)
            //     .Select(g => (
            //         classifier: g.Key,
            //         methods: g
            //             .Select(t => t.methodInfo)
            //             .Select(methodInfo => new Func<string>( () => (string)methodInfo.Invoke(null, null)))))
            //     .ToDictionary(o => o.classifier, o => o.methods.ToArray());

            foreach (var (methodInfo, attr) in methods)
            {
                yield return new Selector(
                    attr.Classifier,
                    attr.Description,
                    () => (string)methodInfo.Invoke(null, null));
            }
        }
    }
}
