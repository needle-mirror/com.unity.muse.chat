using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Muse.Common.Editor.Integration;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat.FunctionCalling
{
    class AttributeBasedFunctionSource : IFunctionSource
    {
        /// <summary>
        ///     Returns all methods marked with the <see cref="ContextProviderAttribute"/> or the
        ///     <see cref="PluginAttribute"/> that meet the requirements for being a context provider or a plugin.
        /// </summary>
        public CachedFunction[] GetFunctions()
        {
            List<CachedFunction> methods = new();

            PopulateListFromAttribute<ContextProviderAttribute>(
                method => method.IsStatic && method.ReturnType == typeof(string),
                attribute => attribute.Description
            );

            PopulateListFromAttribute<PluginAttribute>(
                method => method.IsStatic || method.ReturnType == typeof(void),
                attribute => attribute.Description
            );

            return methods.ToArray();


            void PopulateListFromAttribute<T>(Predicate<MethodInfo> validator, Func<T, string> descriptionExtractor)
                where T : Attribute
            {
                // Get all methods marked with the ContextProviderAttribute attribute.
                methods.AddRange(
                    TypeCache.GetMethodsWithAttribute<T>()
                        .Where(methodInfo =>
                        {
                            if (!validator(methodInfo))
                            {
                                Debug.LogWarning(
                                    $"Method \"{methodInfo.Name}\" in \"{methodInfo.DeclaringType?.FullName}\" failed" +
                                    $"validation. This means it does not have the appropriate function signature for" +
                                    $"the given attribute {typeof(T).Name}");
                                return false;
                            }

                            return true;
                        })
                        .Where(method => method.GetCustomAttribute<T>() != null)
                        .Select(method =>
                        {
                            var att = method.GetCustomAttribute<T>();

                            return new CachedFunction
                            {
                                Method = method,
                                FunctionDefinition = FunctionCallingUtilities.GetFunctionDefinition(
                                    method,
                                    descriptionExtractor(att),
                                    FunctionCallingUtilities.GetTagForAttribute(att))
                            };
                        })
                );
            }
        }
    }
}
