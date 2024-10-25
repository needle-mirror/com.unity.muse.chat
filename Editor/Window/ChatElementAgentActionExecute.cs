using System.Text;
using Unity.Muse.Agent.Dynamic;
using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    class ChatElementAgentActionExecute : ManagedTemplate
    {
        const string k_ActionCursorClassName = "mui-action-cursor";

        Button m_UndoButton;
        Text m_Text;
        Text m_Title;

        public ChatElementAgentActionExecute()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Title = view.Q<Text>("actionTitle");
            m_Text = view.Q<Text>("resultText");

            m_UndoButton = view.SetupButton("undoButton", _ => UndoHistoryUtils.OpenHistory());
            m_UndoButton.SetEnabled(false);

            m_Text.RegisterCallback<PointerUpLinkTagEvent>(ClickOnObjectLink);
            m_Text.RegisterCallback<PointerOverLinkTagEvent>(OnLinkOver);
            m_Text.RegisterCallback<PointerOutLinkTagEvent>(OnLinkOut);
        }

        void OnLinkOut(PointerOutLinkTagEvent evt)
        {
            if (evt.target is Text text)
                text.RemoveFromClassList(k_ActionCursorClassName);
        }

        void OnLinkOver(PointerOverLinkTagEvent evt)
        {
            if (evt.target is Text text)
                text.AddToClassList(k_ActionCursorClassName);
        }

        void ClickOnObjectLink(PointerUpLinkTagEvent evt)
        {
            if (!int.TryParse(evt.linkID, out int gameObjectInstanceId))
                return;

            var obj = EditorUtility.InstanceIDToObject(gameObjectInstanceId);
            EditorGUIUtility.PingObject(obj);
        }

        public void SetData(string data)
        {
            if (int.TryParse(data, out var executionId))
            {
                var execution = MuseEditorDriver.instance.Agent.RetrieveExecution(executionId);

                m_Text.text = execution.SuccessfullyStarted ? FormatExecutionResult(execution) : $"<color={ExecutionResult.WarningTextColor}>{execution.ConsoleLogs}</color>";
                m_Title.text = execution.ActionName ?? "Command completed";

                m_UndoButton.SetEnabled(execution.SuccessfullyStarted);
            }
        }

        internal static string FormatExecutionResult(ExecutionResult executionResult)
        {
            const int maxPreviewLine = 10;
            var executionResults = executionResult.Result;

            var stringBuilder = new StringBuilder();

            if (executionResults.Count == 0)
                stringBuilder.AppendLine($"Executed without logs");

            for (var i = 0; i < executionResults.Count; i++)
            {
                if (i > 0)
                    stringBuilder.AppendLine();

                if (i > maxPreviewLine)
                {
                    stringBuilder.Append("...");
                    break;
                }

                var line = executionResults[i];
                stringBuilder.Append(line);
            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
