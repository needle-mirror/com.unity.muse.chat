using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Unity.Muse.Agent.Dynamic;
using Unity.Muse.Chat.BackendApi.Model;
using UnityEngine;

namespace Unity.Muse.Chat
{
    class MuseAgent
    {
        internal const string k_DynamicAssemblyName = "Unity.Muse.Agent.Dynamic";
        internal const string k_DynamicActionNamespace = "Unity.Muse.Agent.Dynamic";
        internal const string k_DynamicActionClassName = "ActionScript";

        internal static readonly Regex k_CsxMarkupRegex = new("```csx(.*?)```", RegexOptions.Compiled | RegexOptions.Singleline);

        const string k_DynamicActionFullClassName = k_DynamicActionNamespace + "." + k_DynamicActionClassName;

        const string k_DummyActionScript =
            "\nusing UnityEngine;\nusing UnityEditor;\n\ninternal class ActionScript : IAgentAction\n{\n    public void Execute(ActionAttachment attachments, ExecutionResult result) {}\n    public void Preview(PreviewBuilder builder) {}\n}";


        readonly DynamicAssemblyBuilder m_Builder = new(k_DynamicAssemblyName, k_DynamicActionNamespace);
        Dictionary<int, ExecutionResult> m_ActionExecutions = new();

        static string[] k_UnsafeMethods = new[]
        {
            "UnityEditor.AssetDatabase.DeleteAsset",
            "UnityEditor.FileUtil.DeleteFileOrDirectory",
            "System.IO.File.Delete",
            "System.IO.Directory.Delete",
            "System.IO.File.Move",
            "System.IO.Directory.Move"
        };

        public MuseAgent()
        {
            Task.Run(InitCacheWithDummyCompilation);
        }

        void InitCacheWithDummyCompilation()
        {
            // To enable the internal assembly cache we start a compilation of an empty action
            m_Builder.Compile(k_DummyActionScript, out _);
        }

        public AgentAction BuildAction(string actionScript)
        {
            // Remove action embedded MonoBehaviours that already exist in the project
            var embeddedMonoBehaviours = ExtractRequiredMonoBehaviours(ref actionScript);

            var agentAssembly = m_Builder.CompileAndLoadAssembly(actionScript, out var compilationLogs, out var updatedScript);
            var action = new AgentAction { CompilationLogs = compilationLogs, Script = updatedScript };

            if (agentAssembly != null)
            {
                var agentScript = CreateAction(agentAssembly, out var actionDescription);

                action.SetInstance(agentScript, actionDescription);

                if (agentScript != null)
                {
                    CheckForUnsafeCalls(updatedScript, action);

                    // Save embedded MonoBehaviours list
                    action.SetRequiredMonoBehaviours(embeddedMonoBehaviours);
                }
                else
                {
                    Debug.LogWarning($"Unable to find a type ActionScript in the assembly");
                }
            }
            else
            {
                Debug.LogWarning($"Unable to compile the action:\n{compilationLogs}");
            }

            return action;
        }

        static List<ClassCodeTextDefinition> ExtractRequiredMonoBehaviours(ref string actionScript)
        {
            var tree = SyntaxFactory.ParseSyntaxTree(actionScript);
            var embeddedMonoBehaviours = tree.ExtractTypesByInheritance<MonoBehaviour>();
            for (var i = embeddedMonoBehaviours.Count - 1; i >= 0; i--)
            {
                var monoBehaviour = embeddedMonoBehaviours[i];
                if (UserAssemblyContainsType(monoBehaviour.ClassName))
                {
                    actionScript = tree.RemoveType(monoBehaviour.ClassName).GetText().ToString();
                    embeddedMonoBehaviours.RemoveAt(i);
                }
            }

            return embeddedMonoBehaviours;
        }

        void CheckForUnsafeCalls(string actionScript, AgentAction action)
        {
            var compilation = m_Builder.Compile(actionScript, out var tree);
            var model = compilation.GetSemanticModel(tree);

            var root = tree.GetCompilationUnitRoot();
            var walker = new PublicMethodCallWalker(model);
            walker.Visit(root);

            foreach (var methodCall in walker.PublicMethodCalls)
            {
                if (k_UnsafeMethods.Contains(methodCall))
                {
                    action.Unsafe = true;
                    break;
                }
            }
        }

        IAgentAction CreateAction(Assembly dynamicAssembly, out string actionDescription)
        {
            var type = dynamicAssembly.GetType(k_DynamicActionFullClassName);
            if (type == null)
            {
                actionDescription = null;
                return null;
            }

            var attribute = type.GetCustomAttribute<ActionDescriptionAttribute>();
            actionDescription = attribute?.Description;

            return Activator.CreateInstance(type) as IAgentAction;
        }

        public async Task<string> RepairAction(MuseMessageId messageId, int messageIndex, string errorToRepair, string scriptToRepair)
        {
            var repairedMessage = await MuseEditorDriver.instance.RepairScript(messageId, messageIndex, errorToRepair, scriptToRepair, ScriptType.AgentAction);

            if (string.IsNullOrEmpty(repairedMessage))
                return null;

            var match = k_CsxMarkupRegex.Match(repairedMessage);
            if (match.Success)
            {
                var code = match.Groups[1].Value;
                return code;
            }

            return null;
        }

        public void StoreExecution(ExecutionResult executionResult)
        {
            m_ActionExecutions.Add(executionResult.Id, executionResult);
        }

        public ExecutionResult RetrieveExecution(int id)
        {
            return m_ActionExecutions.GetValueOrDefault(id);
        }

        static bool UserAssemblyContainsType(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblyCSharp = assemblies.FirstOrDefault(a => a.GetName().Name == "Assembly-CSharp");

            if (assemblyCSharp != null)
            {
                var type = assemblyCSharp.GetType(typeName);
                return type != null;
            }

            return false;
        }
    }
}
