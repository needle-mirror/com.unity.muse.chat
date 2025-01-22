using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Unity.Muse.Agent.Dynamic;
using Unity.Muse.Chat.BackendApi.Model;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.Muse.Chat
{
    class RunCommandInterpreter
    {
        internal const string k_DynamicAssemblyName = "Unity.Muse.Agent.Dynamic";
        internal const string k_DynamicActionNamespace = "Unity.Muse.Agent.Dynamic";
        internal const string k_DynamicActionClassName = "CommandScript";

        internal static readonly Regex k_CsxMarkupRegex = new("```csx(.*?)```", RegexOptions.Compiled | RegexOptions.Singleline);

        const string k_DynamicActionFullClassName = k_DynamicActionNamespace + "." + k_DynamicActionClassName;

        const string k_DummyCommandScript =
            "\nusing UnityEngine;\nusing UnityEditor;\n\ninternal class CommandScript : IRunCommand\n{\n    public void Execute(ExecutionResult result) {}\n    public void Preview(PreviewBuilder builder) {}\n}";


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

        public RunCommandInterpreter()
        {
            Task.Run(InitCacheWithDummyCompilation);
        }

        void InitCacheWithDummyCompilation()
        {
            // To enable the internal assembly cache we start a compilation of an empty action
            m_Builder.Compile(k_DummyCommandScript, out _);
        }

        public AgentRunCommand BuildCommand(string runCommandScript)
        {
            // Remove class embedded MonoBehaviours that already exist in the project
            var embeddedMonoBehaviours = ExtractRequiredMonoBehaviours(ref runCommandScript);

            var runCommandAssembly = m_Builder.CompileAndLoadAssembly(runCommandScript, out var compilationLogs, out var updatedScript);
            var runCommand = new AgentRunCommand { CompilationLogs = compilationLogs, Script = updatedScript };

            if (runCommandAssembly != null)
            {
                var runInstance = CreateCommand(runCommandAssembly, out var runCommandDescription);
                runCommand.SetInstance(runInstance, runCommandDescription);

                if (runInstance != null)
                {
                    InitializeFieldWithLookup(runInstance, runCommand);

                    CheckForUnsafeCalls(updatedScript, runCommand);

                    // Save embedded MonoBehaviours list
                    runCommand.SetRequiredMonoBehaviours(embeddedMonoBehaviours);
                }
                else
                {
                    InternalLog.LogWarning($"Unable to find a type CommandScript in the assembly");
                }
            }
            else
            {
                InternalLog.LogWarning($"Unable to compile the action:\n{compilationLogs}");
            }

            return runCommand;
        }

        static List<ClassCodeTextDefinition> ExtractRequiredMonoBehaviours(ref string commandScript)
        {
            var tree = SyntaxFactory.ParseSyntaxTree(commandScript);
            var embeddedMonoBehaviours = tree.ExtractTypesByInheritance<MonoBehaviour>();
            for (var i = embeddedMonoBehaviours.Count - 1; i >= 0; i--)
            {
                var monoBehaviour = embeddedMonoBehaviours[i];
                if (UserAssemblyContainsType(monoBehaviour.ClassName))
                {
                    commandScript = tree.RemoveType(monoBehaviour.ClassName).GetText().ToString();
                    embeddedMonoBehaviours.RemoveAt(i);
                }
            }

            return embeddedMonoBehaviours;
        }

        void CheckForUnsafeCalls(string script, AgentRunCommand runCommand)
        {
            var compilation = m_Builder.Compile(script, out var tree);
            var model = compilation.GetSemanticModel(tree);

            var root = tree.GetCompilationUnitRoot();
            var walker = new PublicMethodCallWalker(model);
            walker.Visit(root);

            foreach (var methodCall in walker.PublicMethodCalls)
            {
                if (k_UnsafeMethods.Contains(methodCall))
                {
                    runCommand.Unsafe = true;
                    break;
                }
            }
        }

        IRunCommand CreateCommand(Assembly dynamicAssembly, out string actionDescription)
        {
            var type = dynamicAssembly.GetType(k_DynamicActionFullClassName);
            if (type == null)
            {
                actionDescription = null;
                return null;
            }

            var attribute = type.GetCustomAttribute<CommandDescriptionAttribute>();
            actionDescription = attribute?.Description;

            return Activator.CreateInstance(type) as IRunCommand;
        }

        void InitializeFieldWithLookup(object instance, AgentRunCommand runCommand)
        {
            var type = instance.GetType();

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var fieldInfo in fields)
            {
                var actionParameterAttribute = fieldInfo.GetCustomAttribute<CommandParameterAttribute>();
                if (actionParameterAttribute == null)
                    continue;

                var fieldType = fieldInfo.FieldType;
                if (typeof(Object).IsAssignableFrom(fieldType))
                {
                    Object defaultValue = null;
                    switch (actionParameterAttribute.LookupType)
                    {
                        case LookupType.Attachment:
                            defaultValue = runCommand.GetAttachmentByNameOrFirstCompatible(actionParameterAttribute.LookupName, fieldType);
                            break;
                        case LookupType.Scene:
                            defaultValue = GameObject.Find(actionParameterAttribute.LookupName);
                            break;
                        case LookupType.Asset:
                            defaultValue = AssetDatabase.LoadAssetAtPath(actionParameterAttribute.LookupName, fieldType);
                            break;
                    }

                    fieldInfo.SetValue(instance, defaultValue);
                }
                else if (fieldType.IsGenericType &&
                         fieldType.GetGenericTypeDefinition() == typeof(List<>) &&
                         typeof(Object).IsAssignableFrom(fieldType.GetGenericArguments()[0]))
                {
                    var elementType = fieldType.GetGenericArguments()[0];
                    var listInstance = Activator.CreateInstance(fieldType) as IList;

                    switch (actionParameterAttribute.LookupType)
                    {
                        case LookupType.Attachment:
                            var attachments = runCommand.GetAttachments(elementType);
                            foreach (var attachment in attachments)
                                listInstance?.Add(attachment);
                            break;
                        case LookupType.Asset:
                            listInstance = AssetDatabase.LoadAllAssetsAtPath(actionParameterAttribute.LookupName);
                            break;
                        case LookupType.Scene:
                            Debug.LogWarning("List of GameObject in the scene is not supported.");
                            break;
                    }

                    fieldInfo.SetValue(instance, listInstance);
                }
            }
        }

        public async Task<string> RepairAction(MuseMessageId messageId, int messageIndex, string errorToRepair, string scriptToRepair)
        {
            var repairedMessage = await Assistant.instance.RepairScript(messageId, messageIndex, errorToRepair, scriptToRepair, ScriptType.AgentAction);

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
