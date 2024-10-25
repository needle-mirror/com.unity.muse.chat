using System;
using System.IO;
using System.Threading.Tasks;
using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal class ChatElementCodeBlock : ManagedTemplate
    {
        const string k_OldFailMessage = "This script is not compiling in the current project.";
        const string k_NewFailMessage = "This script failed to compile.";
        Text m_Text;
        AppUI.UI.Button m_SaveButton;
        AppUI.UI.Button m_CopyButton;

        MuseMessage m_ParentMessage;


        string m_rawCode;
        string m_ValidatedCode;
        bool m_ValidCode = true;
        bool m_FormatCode = true;

        public ChatElementCodeBlock()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Text = view.Q<Text>("codeText");
            m_SaveButton = view.SetupButton("saveButton", OnSaveCodeClicked);
            m_CopyButton = view.SetupButton("copyButton", OnCopyCodeClicked);
        }

        public void SetData(string rawCode, bool validate)
        {
            m_rawCode = rawCode;
            if (!validate)
            {
                m_ValidatedCode = m_rawCode;
                m_Text.text = CodeSyntaxHighlight.Highlight(m_ValidatedCode);
                m_SaveButton.SetEnabled(true);
                m_CopyButton.SetEnabled(true);
            }
            else
            {
                // Until code is validated it is in progress.
                m_FormatCode = false;
                m_Text.text = "Musing...";
                m_SaveButton.SetEnabled(false);
                m_CopyButton.SetEnabled(false);
            }
        }
        public void SetMessage(MuseMessage message)
        {
            m_ParentMessage = message;
        }

        internal async Task ValidateCode()
        {
            // Check if the last llm response has not been finished, wait for it to finish before sending repair request.
            if (MuseEditorDriver.instance.MessageUpdatersNum != 0)
            {
                var currentUpdater = MuseEditorDriver.instance.GetUpdaterForConversation(m_ParentMessage.Id.ConversationId);

                // We also check the active conversation updater status, as this including writing out messages that occur after code blocks.
                if (currentUpdater != null && currentUpdater.IsInProgress)
                {
                    await currentUpdater.MessageTaskCompletionSource.Task;
                }
            }

            m_ValidCode = MuseEditorDriver.instance.CodeBlockValidator.ValidateCode(m_rawCode, out var localRepairedCode, out var logs);

            if (m_ValidCode)
            {
                m_ValidatedCode = localRepairedCode;
                m_Text.text = CodeSyntaxHighlight.Highlight(localRepairedCode);
                m_SaveButton.SetEnabled(true);
                m_CopyButton.SetEnabled(true);
            }
            else
            {
                // Not valid code and an old message? Just display the error logs
                // Current message and invalid code? Try the repair step
                if (!MuseEditorDriver.instance.ValidRepairTarget(m_ParentMessage.Id))
                {
                    // Fail message (warning, not compatible with current codebase)
                    FailCode(k_OldFailMessage, logs);
                    return;
                }
                if (!MuseEditorDriver.instance.IsUnderRepair(m_ParentMessage.Id))
                {
                    m_Text.text = "Hmm, seems like I had some trouble with this script. Let me try again.".RichColor("#FF85AB");

                    var remoteRepairedCode = await MuseEditorDriver.instance.CodeBlockValidator.Repair(m_ParentMessage.Id, m_ParentMessage.MessageIndex, logs, localRepairedCode);
                    if (remoteRepairedCode == null)
                    {
                        FailCode(k_NewFailMessage, logs);
                        return;
                    }

                    m_ValidCode = MuseEditorDriver.instance.CodeBlockValidator.ValidateCode(remoteRepairedCode, out localRepairedCode, out logs);
                    if (!m_ValidCode)
                    {
                        FailCode(k_NewFailMessage, logs);
                        return;
                    }
                    m_ValidatedCode = localRepairedCode;

                    // Update Code preview text with latest code
                    m_Text.text = remoteRepairedCode;
                    m_SaveButton.SetEnabled(true);
                    m_CopyButton.SetEnabled(true);
                }
            }
        }

        void FailCode(string message, string errorLog)
        {
            Debug.Log($"Unable to compile script: {errorLog}");
            m_Text.text = message.RichColor("#FF85AB") + errorLog.RichColor("#FFFF00");
            // We allow copying of failed code but not saving
            m_CopyButton.SetEnabled(true);
        }

        public void SetSelectable(bool selectable)
        {
            m_Text.selection.isSelectable = selectable;
        }

        private void OnCopyCodeClicked(PointerUpEvent evt)
        {
            string disclaimerHeader = string.Format(MuseChatConstants.DisclaimerText, DateTime.Now.ToShortDateString());
            GUIUtility.systemCopyBuffer = string.Concat(disclaimerHeader, m_ValidatedCode);

            MuseChatView.ShowNotification("Copied to clipboard", PopNotificationIconType.Info);
        }

        private void OnSaveCodeClicked(PointerUpEvent evt)
        {
            string file = EditorUtility.SaveFilePanel("Save Code", Application.dataPath, "code", "cs");
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            EditorUtility.DisplayProgressBar("Saving Code", "Saving code to file", 0.5f);

            try
            {
                string formattedCode = m_FormatCode ? CodeExportUtils.Format(m_ValidatedCode, Path.GetFileNameWithoutExtension(file)) : CodeExportUtils.AddDisclaimer(m_ValidatedCode);
                File.WriteAllText(file, formattedCode);
            }
            catch (Exception)
            {
                MuseChatView.ShowNotification("Failed to save code to file", PopNotificationIconType.Error);
                EditorUtility.ClearProgressBar();
                return;
            }

            MuseChatView.ShowNotification("File Saved", PopNotificationIconType.Info);
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
        }
    }
}
