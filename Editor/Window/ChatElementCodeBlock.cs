using System;
using System.IO;
using System.Threading.Tasks;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Common.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal class ChatElementCodeBlock : ManagedTemplate
    {
        const string k_OldFailMessage = "Something went wrong.\nThe generated script was not able to compile in your project.";
        const string k_NewFailMessage = "Something went wrong.\nThe generated script was not able to compile in your project. Try to correct any errors, or generate it again.";
        Text m_Text;
        AppUI.UI.Button m_SaveButton;
        AppUI.UI.Button m_CopyButton;
        VisualElement m_WarningContainer;
        Text m_WarningText;

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

            m_WarningContainer = view.Q<VisualElement>("warningContainer");
            m_WarningContainer.SetDisplay(false);
            m_WarningContainer.style.marginBottom = 10;

            m_WarningText = view.Q<Text>("warningText");
        }

        public void SetData(string rawCode, bool validate)
        {
            m_rawCode = rawCode;
            if (!validate)
            {
                m_ValidatedCode = m_rawCode;

                UpdateTextWithValidatedCode();
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

                UpdateTextWithValidatedCode();
            }
            else
            {
                // Not valid code and an old message? Just display the error logs
                // Current message and invalid code? Try the repair step
                if (!MuseEditorDriver.instance.ValidRepairTarget(m_ParentMessage.Id))
                {
                    // Fail message (warning, not compatible with current codebase)
                    FailCode(k_OldFailMessage, logs);

                    m_ValidatedCode = localRepairedCode;
                }

                if (!MuseEditorDriver.instance.IsUnderRepair(m_ParentMessage.Id))
                {
                    // TODO replace it with Musing widget like Agent
                    m_Text.text = "Hmm, seems like I had some trouble with this script. Let me try again.".RichColor("#FF85AB");

                    var remoteRepairedCode = await MuseEditorDriver.instance.CodeBlockValidator.Repair(m_ParentMessage.Id, m_ParentMessage.MessageIndex, logs, localRepairedCode);
                    if (remoteRepairedCode != null)
                    {
                        m_ValidCode = MuseEditorDriver.instance.CodeBlockValidator.ValidateCode(remoteRepairedCode, out localRepairedCode, out logs);
                        if (!m_ValidCode)
                            FailCode(k_NewFailMessage, logs);
                    }
                    else
                    {
                        FailCode(k_NewFailMessage, logs);
                    }

                    m_ValidatedCode = localRepairedCode;
                }

                UpdateTextWithValidatedCode();
            }
        }

        void FailCode(string message, string errorLog)
        {
            m_WarningContainer.SetDisplay(true);
            m_WarningText.text = message;

            Debug.Log($"Unable to compile script: {errorLog}");
        }

        void UpdateTextWithValidatedCode()
        {
            // Update Code preview text with latest code
            string disclaimerHeader = string.Format(MuseChatConstants.DisclaimerText, DateTime.Now.ToShortDateString());
            m_Text.text = CodeSyntaxHighlight.Highlight(string.Concat(disclaimerHeader, m_ValidatedCode));

            m_SaveButton.SetEnabled(true);
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
