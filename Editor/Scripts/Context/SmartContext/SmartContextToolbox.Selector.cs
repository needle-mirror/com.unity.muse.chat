using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Muse.Chat.BackendApi.Model;
using Unity.Muse.Chat.FunctionCalling;

namespace Unity.Muse.Chat.Context.SmartContext
{
    partial class SmartContextToolbox
    {
        internal class Selector : IFunctionToolboxSelector
        {
            public FunctionDefinition FunctionDefinition { get; }

            public string Name => FunctionDefinition.Name;
            public List<ParameterDefinition> Parameters => FunctionDefinition.Parameters;

            readonly Func<object[], string> m_Func;

            public Selector(string description,
                List<ParameterDefinition> parameters,
                Func<object[], string> func,
                MethodInfo method)
            {
                // TODO: We are not including the fully qualified name as the method.Name for the LLMs sake. Eventually we should check for namespace clashes though and add just enough qualification to avoid collisions.
                FunctionDefinition = new FunctionDefinition(
                    method.Name,
                    description,
                    parameters);

                m_Func = func;
            }

            public string Result(object[] args)
            {
                return m_Func.Invoke(args);
            }
        }
    }
}
