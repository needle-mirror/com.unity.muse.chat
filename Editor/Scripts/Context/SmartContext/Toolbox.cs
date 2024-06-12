using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Muse.Chat.Model;
using UnityEngine;

namespace Unity.Muse.Chat.Context.SmartContext
{
    internal partial class Toolbox
    {
        private readonly Dictionary<string, ToolSelector> m_toolMethodSelectors = new();

        internal IEnumerable<ToolSelector> Tools  => m_toolMethodSelectors.Values;

        /// <summary>
        ///     Create a toolbox.
        ///     The Toolbox will use mthods returned by the contextProviderSource to build a list of available tools.
        /// </summary>
        /// <param name="contextProviderSource">Provides context methods</param>
        public Toolbox(IContextProviderSource contextProviderSource)
        {
            // Build list of available tool methods:
            m_toolMethodSelectors.Clear();

            var methods = contextProviderSource.GetMethods();
            foreach (var toolMethodInfo in methods)
            {
                var parameters = toolMethodInfo.Method.GetParameters();

                bool valid = true;

                // Create parameter info list:
                var toolParameters = new List<ParameterDefinition>(parameters.Length);
                for (var parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                {
                    var parameter = parameters[parameterIndex];
                    var parameterInfo = GetToolParameterInfo(parameter, toolMethodInfo.Method);
                    if (parameterInfo == null)
                    {
                        valid = false;
                        break;
                    }

                    toolParameters.Add(parameterInfo);
                }

                if (!valid)
                {
                    continue;
                }

                var selector = new ToolSelector(
                    toolMethodInfo.Description,
                    toolParameters,
                    args => (string) toolMethodInfo.Method.Invoke(null, args),
                    toolMethodInfo.Method);

                m_toolMethodSelectors.Add(selector.Name, selector);
            }
        }

        ParameterDefinition GetToolParameterInfo(ParameterInfo parameter, MethodInfo toolMethod)
        {
            var parameterAttribute = parameter.GetCustomAttribute<ParameterAttribute>();
            if (parameterAttribute == null)
            {
                Debug.LogWarning(
                    $"Method \"{toolMethod.Name}\" in \"{toolMethod.DeclaringType?.FullName}\" is marked with the {nameof(ContextProviderAttribute)} attribute but the parameter \"{parameter.Name}\" is not marked with the {nameof(ParameterAttribute)} attribute. This method will be ignored.");
                return null;
            }

            var parameterType = parameter.ParameterType.Name;

            if (parameterType.EndsWith("[]"))
            {
                var elementType = parameterType.Substring(0, parameterType.Length - 2); // Remove "[]"
                parameterType = $"List[{elementType}]"; // Convert to Python list
            }

            return new ParameterDefinition(parameter.Name, parameterType, parameterAttribute.Description);
        }

        Func<string, object> GetConverter(string csharpType)
        {
            // Get parser for array types, format should be: List[<type>]:
            if (csharpType.StartsWith("List["))
            {
                var elementType = csharpType.Substring(5, csharpType.Length - 6);
                var elementConverter = GetConverter(elementType);

                // Make converter that parses an array:
                return ConverterFactory(s =>
                {
                    // s is a comma separated string:
                    var elements = s.Replace("[","").Replace("]","").Replace("'","").Split(',');

                    // We cannot just return an array of type object[], we need to create an array of the correct type:
                    var result = Array.CreateInstance(GetArrayType(), elements.Length);
                    for (var i = 0; i < elements.Length; i++)
                    {
                        result.SetValue(elementConverter(elements[i]), i);
                    }

                    return result;

                    Type GetArrayType()
                    {
                        return elementType switch
                        {
                            "String" => typeof(string),
                            "Int32" => typeof(int),
                            "Int64" => typeof(long),
                            "Single" => typeof(float),
                            "Boolean" => typeof(bool),
                            _ => null
                        };
                    }
                });
            }

            return csharpType switch
            {
                "String" => (string s) => s,
                "Int32" => ConverterFactory(int.Parse),
                "Int64" => ConverterFactory(long.Parse),
                "Single" => ConverterFactory(float.Parse),
                "Boolean" => ConverterFactory(bool.Parse),
                _ => s => null
            };

            Func<string, object> ConverterFactory<T>(Func<string, T> converter)
            {
                return s =>
                {
                    try
                    {
                        return converter(s);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                };
            }
        }

        /// <summary>
        /// Executes a context retrieval tool by name with the given arguments.
        /// </summary>
        /// <param name="name">Name of the tool function.</param>
        /// <param name="args">Arguments to pass to the tool function.</param>
        public bool TryRunToolByName(string name, string[] args, out IContextSelection output)
        {
            if (m_toolMethodSelectors.TryGetValue(name, out var toolMethodSelector))
            {
                try
                {
                    if (args.Length != toolMethodSelector.Parameters.Count)
                        throw new ArgumentException("The incorrect number of args were provided");

                    object[] convertedArgs = new object[toolMethodSelector.Parameters.Count];

                    string[] argNames = toolMethodSelector
                        .Parameters
                        .Select(param => param.Name)
                        .ToArray();

                    Func<string, object>[] converters = toolMethodSelector
                        .Parameters
                        .Select(param => GetConverter(param.Type))
                        .ToArray();

                    for (int i = 0; i < args.Length; i++)
                    {
                        string arg = args[i];

                        if (!arg.Contains(":"))
                        {
                            Debug.LogWarning($"SmartContextError: The LLM did not return an arg as a named arg. Assuming it is a positional arg");

                            try
                            {
                                convertedArgs[i] = converters[i](arg);
                                continue;
                            }
                            catch (Exception)
                            {
                                Debug.LogError($"SmartContextError: The LLM did not return an arg that was a valid positional arg");
                            }
                        }

                        string[] argParts = arg.Split(':');

                        if (argParts.Length != 2)
                        {
                            Debug.LogError($"SmartContextError: The LLM did not return an arg that was a valid named arg. It should be in the format 'name:value'.");
                            continue;
                        }

                        int namedindex = Array.IndexOf(argNames, argParts[0]);

                        if (namedindex == -1)
                        {
                            Debug.LogError($"SmartContextError: The LLM returned an arg with a name that does not match any of the expected names.");
                            continue;
                        }

                        try
                        {
                            convertedArgs[namedindex] = converters[namedindex](argParts[1]);
                        }
                        catch (Exception)
                        {
                            Debug.LogError($"SmartContextError: The LLM did not return an arg that was a valid named arg");
                        }
                    }

                    output = new ToolBoxContextSelection(
                        toolMethodSelector,
                        toolMethodSelector.Result(convertedArgs));

                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            output = default;
            return false;
        }

        /// <summary>
        /// Returns the names and descriptions of all available tools.
        /// </summary>
        public List<FunctionDefinition> GetToolDescriptions()
        {
            List<FunctionDefinition> functionDefinitions = m_toolMethodSelectors
                .Values
                .Select(selector => selector.FunctionDefinition)
                .ToList();

            return functionDefinitions;
        }
    }
}
