using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat.Context.SmartContext
{
    class AttributeBasedContextProvider : IContextProviderSource
    {
        /// <summary>
        ///     Returns all methods marked with the <see cref="ContextProviderAttribute"/> that are static and return a string.
        /// </summary>
        public IContextProviderSource.Info[] GetMethods()
        {
            var methodsWithAttribute = TypeCache.GetMethodsWithAttribute<ContextProviderAttribute>().ToArray();

            return methodsWithAttribute
                .Where(methodInfo =>
                {
                    if (!methodInfo.IsStatic || methodInfo.ReturnType != typeof(string))
                    {
                        Debug.LogWarning(
                            $"Method \"{methodInfo.Name}\" in \"{methodInfo.DeclaringType?.FullName}\" is " +
                            $"marked with the {nameof(ContextProviderAttribute)} attribute but is not static or " +
                            "does not return a string. This method will be ignored.");
                        return false;
                    }

                    return true;
                })
                .Select(method => new IContextProviderSource.Info
                {
                    Description = method.GetCustomAttribute<ContextProviderAttribute>().Description,
                    Method = method
                })
                .ToArray();
        }
    }
}
