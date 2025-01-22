using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Unity.Muse.Agent.Dynamic;
using Object = UnityEngine.Object;

namespace Unity.Muse.Chat
{
    class AgentRunCommand
    {
        static readonly string[] k_UnauthorizedNamespaces = { "System.Net", "System.Diagnostics" };

        IRunCommand m_CommandInstance;

        string m_Description;

        List<ClassCodeTextDefinition> m_RequiredMonoBehaviours;

        public string Script { get; set; }
        public string CompilationLogs { get; set; }

        public bool PreviewIsDone { get; set; }

        public bool Unsafe { get; set; }

        public string Description => m_Description;

        public IRunCommand Instance => m_CommandInstance;

        public bool CompilationSuccess => m_CommandInstance != null;

        public IEnumerable<ClassCodeTextDefinition> RequiredMonoBehaviours => m_RequiredMonoBehaviours;

        public List<Object> CommandAttachments;

        public AgentRunCommand()
        {
            CommandAttachments = new List<Object>(Assistant.instance.GetValidAttachment(Assistant.instance.k_ObjectAttachments));
        }

        public void SetInstance(IRunCommand instance, string description)
        {
            m_CommandInstance = instance;
            m_Description = description;
        }

        public bool Execute(out ExecutionResult executionResult)
        {
            executionResult = new ExecutionResult(m_Description);

            if (m_CommandInstance == null)
                return false;

            executionResult.Start();

            try
            {
                m_CommandInstance.Execute(executionResult);
            }
            catch (Exception e)
            {
                executionResult.LogError(e.ToString());
            }

            executionResult.End();

            return true;
        }

        public void BuildPreview(out PreviewBuilder builder)
        {
            builder = new PreviewBuilder();

            if (m_CommandInstance == null)
                return;

            PreviewIsDone = true;
            m_CommandInstance.BuildPreview(builder);
        }

        public void SetRequiredMonoBehaviours(List<ClassCodeTextDefinition> requiredMonoBehaviours)
        {
            m_RequiredMonoBehaviours = requiredMonoBehaviours;
        }

        public bool HasUnauthorizedNamespaceUsage()
        {
            var tree = SyntaxFactory.ParseSyntaxTree(Script);
            return tree.ContainsNamespaces(k_UnauthorizedNamespaces);
        }

        public bool IsUsingDeprecatedStructure()
        {
            var tree = SyntaxFactory.ParseSyntaxTree(Script);
            return tree.ContainsInterface("IAgentAction");
        }

        public IEnumerable<Object> GetAttachments(Type type)
        {
            return CommandAttachments.Where(o => o.GetType() == type);
        }

        public Object GetAttachmentByNameOrFirstCompatible(string objectName, Type type)
        {
            var filtered = GetAttachments(type);
            if (string.IsNullOrEmpty(objectName))
                return filtered.FirstOrDefault();

            var objectByName = filtered.FirstOrDefault(a => a.name == objectName);
            return objectByName;
        }
    }
}
