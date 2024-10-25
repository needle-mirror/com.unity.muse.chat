using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Muse.Chat.BackendApi.Model;
using Unity.Muse.Chat.FunctionCalling;

namespace Unity.Muse.Chat.Plugins
{
    internal partial class PluginToolbox
    {
        internal class PluginSelector : IFunctionToolboxSelector
        {
            public FunctionDefinition FunctionDefinition { get; }

            public string Name => FunctionDefinition.Name;
            public List<ParameterDefinition> Parameters => FunctionDefinition.Parameters;

            readonly Action<object[]> m_Plugin;

            public PluginSelector(string description,
                List<ParameterDefinition> parameters,
                Action<object[]> plugin,
                MethodInfo method)
            {
                // TODO: We are not including the fully qualified name as the method.Name for the LLMs sake. Eventually we should check for namespace clashes though and add just enough qualification to avoid collisions.
                FunctionDefinition = new FunctionDefinition(
                    method.Name,
                    description,
                    parameters);

                m_Plugin = plugin;
            }

            public void Run(object[] args) => m_Plugin.Invoke(args);
        }


    }
}
