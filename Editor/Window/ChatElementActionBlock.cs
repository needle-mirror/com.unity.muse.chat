using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Common.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    class ChatElementActionBlock : ManagedTemplate
    {
        public static Action<string, string> OnDevToolClicked;

        string m_CodeContent;

        Text m_Title;
        Button m_ExecuteButton;
        Button m_DebugButton;
        Text m_CodePreview;
        Accordion m_CodePreviewFoldout;
        VisualElement m_PreviewContainer;
        AccordionItem m_CodePreviewTitle;
        VisualElement m_WarningContainer;
        Text m_WarningText;

        AgentAction m_Action;

        MuseMessage m_ParentMessage;

        public ChatElementActionBlock()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Title = view.Q<Text>("actionTitle");
            m_Title.text = "New command";

            m_WarningContainer = view.Q<VisualElement>("warningContainer");
            m_WarningContainer.SetDisplay(false);
            m_WarningText = view.Q<Text>("warningText");

            m_ExecuteButton = view.SetupButton("executeButton", OnExecuteCodeClicked);
            m_ExecuteButton.SetEnabled(false);

            var devToolButton = view.Q<Button>("devToolButton");
            devToolButton.SetDisplay(false);

            if (OnDevToolClicked != null)
                InitializeDevTool(devToolButton);

            var overviewFoldout = view.Q<Accordion>("overviewFoldout");
            overviewFoldout.RealignFoldoutIcon();

            m_CodePreviewFoldout = view.Q<Accordion>("codePreviewFoldout");
            m_CodePreviewFoldout.RealignFoldoutIcon();
            m_CodePreviewFoldout.SetDisplay(false);

            m_CodePreviewTitle = view.Q<AccordionItem>("codePreviewTitle");

            m_CodePreview = view.Q<Text>("actionCode");

            m_PreviewContainer = view.Q<VisualElement>("previewContainer");
        }

        void InitializeDevTool(Button devToolButton)
        {
            devToolButton.SetDisplay(true);
            devToolButton.clicked += () =>
            {
                string userQuery = string.Empty;
                var conversation = MuseEditorDriver.instance.GetActiveConversation();
                for (var i = conversation.Messages.Count - 1; i >= 1; i--)
                {
                    var message = conversation.Messages[i];
                    if (message.Id == m_ParentMessage.Id)
                    {
                        userQuery = conversation.Messages[i - 1].Content;
                        break;
                    }
                }

                OnDevToolClicked.Invoke(userQuery, m_CodeContent);
            };
        }

        public void SetData(string data)
        {
            m_CodeContent = data;
        }

        public void SetMessage(MuseMessage message)
        {
            m_ParentMessage = message;
            // Set the conversation id to driver's active conversation id, in case SetData() has not received the conversation id yet.
            if (string.IsNullOrEmpty(message.Id.ConversationId.Value))
                m_ParentMessage.Id = new MuseMessageId(MuseEditorDriver.instance.GetActiveConversation().Id, message.Id.FragmentId, message.Id.Type);
        }

        internal async Task SetupAction()
        {
            m_Action = MuseEditorDriver.instance.Agent.BuildAction(m_CodeContent);

            if (m_Action.CompilationSuccess)
            {
                GeneratePreview();
            }
            else
            {
                // Check if the code is already under repair, do not send another repair request.
                if (!MuseEditorDriver.instance.IsUnderRepair(m_ParentMessage.Id))
                {
                    // Check if the last llm response has not been finished, wait for it to finish before sending repair request.
                    if (MuseEditorDriver.instance.MessageUpdatersNum != 0)
                    {
                        var currentUpdater = MuseEditorDriver.instance.GetUpdaterForConversation(m_ParentMessage.Id.ConversationId);
                        if (currentUpdater is { IsInProgress: true })
                        {
                            await currentUpdater.MessageTaskCompletionSource.Task;
                        }
                    }

                    m_CodeContent = await MuseEditorDriver.instance.Agent.RepairAction(m_ParentMessage.Id, m_ParentMessage.MessageIndex, m_Action.CompilationLogs, m_Action.Script);
                    if (m_CodeContent == null)
                    {
                        DisplayCompilationWarning();
                        return;
                    }

                    m_Action = MuseEditorDriver.instance.Agent.BuildAction(m_CodeContent);

                    if (m_Action.CompilationSuccess)
                        GeneratePreview();
                    else
                        DisplayCompilationWarning();
                }
                else
                {
                    DisplayCompilationWarning();
                }
            }
        }

        void DisplayCompilationWarning()
        {
            //Still display the code
            m_CodePreviewTitle.title = "Failed command attempt";
            m_CodePreviewFoldout.SetDisplay(true);
            m_CodePreview.text = CodeSyntaxHighlight.Highlight(CodeExportUtils.Format(m_Action.Script));

            m_WarningContainer.SetDisplay(true);

            if (m_Action.HasUnauthorizedNamespaceUsage())
                m_WarningText.text =  "<b>Sorry, I can’t help with that.</b>\nA script was generated that does triggers an unauthorized API. As a safety precaution, this is not permitted.";
            else
                m_WarningText.text =  "<b>Can we try that again?</b>\nIt helps to be detailed and add an attachment to add context to your request. If you keep getting this message, try to ask something else.";

            m_WarningText.tooltip = $"Unable to compile the action:\n {m_Action.CompilationLogs}";
        }

        void DisplayUnsafeWarning()
        {
            m_WarningContainer.SetDisplay(true);
            m_WarningText.text =  "This action is performing operations that cannot be undone.";
        }

        void GeneratePreview()
        {
            if (m_Action.Unsafe)
                DisplayUnsafeWarning();

            m_Title.text = m_Action.Description;

            // Update Code preview text with latest code
            m_CodePreviewFoldout.SetDisplay(true);
            m_CodePreview.text = FormatDisplayScript();

            if (!m_Action.PreviewIsDone)
            {
                // Update preview content with DryRun
                m_Action.BuildPreview(out var previewBuilder);

                if (m_Action.RequiredMonoBehaviours.Any())
                {
                    foreach (var requiredComponent in m_Action.RequiredMonoBehaviours)
                    {
                        var actionBlock = new ChatElementActionEntry( $"A new C# component {requiredComponent.ClassName} is required to perform this action.", m_Action);
                        actionBlock.Initialize(false);
                        actionBlock.RegisterAction(() =>
                        {
                            string file = EditorUtility.SaveFilePanel("Save new Component", Application.dataPath, requiredComponent.ClassName, "cs");
                            if (!string.IsNullOrEmpty(file))
                            {
                                File.WriteAllText(file, requiredComponent.Code);
                                EditorUtility.DisplayProgressBar("New components", "Recompiling assembly", 0);
                                AssetDatabase.Refresh();
                                EditorUtility.RequestScriptReload();
                            }
                        });

                        m_PreviewContainer.Add(actionBlock);
                    }

                }

                foreach (var previewLine in previewBuilder.Preview)
                {
                    var actionBlock = new ChatElementActionEntry(previewLine, m_Action);
                    actionBlock.Initialize(false);

                    m_PreviewContainer.Add(actionBlock);
                }

                if (!EditorApplication.isPlaying && !m_Action.RequiredMonoBehaviours.Any())
                    m_ExecuteButton.SetEnabled(true);
            }
        }

        string FormatDisplayScript()
        {
            // Update Code preview with Muse AI disclaimer
            var actionScriptDisclaimer = string.Format(MuseChatConstants.DisclaimerText, DateTime.Now.ToShortDateString());

            // Remove namespaces from display
            var tree = SyntaxFactory.ParseSyntaxTree(actionScriptDisclaimer + m_Action.Script);
            tree = tree.RemoveNamespaces();

            return CodeSyntaxHighlight.Highlight(tree.GetText().ToString());
        }

        void OnExecuteCodeClicked(PointerUpEvent evt)
        {
            ExecuteAction();
        }

        void ExecuteAction()
        {
            m_Action.Execute(out var executionResult);

            var agent = MuseEditorDriver.instance.Agent;
            agent.StoreExecution(executionResult);

            MuseEditorDriver.instance.AddInternalMessage($"```{ChatElementRunExecutionBlock.FencedBlockTag}\n{executionResult.Id}\n```", MuseEditorDriver.k_SystemRole, false);
        }
    }
}
