using System;
using System.IO;
using System.Text;
using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal class ChatElementCodeBlock : ManagedTemplate
    {
        Text m_Text;

        string m_CodeText;

        public ChatElementCodeBlock()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Text = view.Q<Text>("codeText");
            view.SetupButton("saveButton", OnSaveCodeClicked);
            view.SetupButton("copyButton", OnCopyCodeClicked);
        }

        public void SetData(string rawCode)
        {
            m_CodeText = rawCode;

            RefreshUI();
        }

        private void RefreshUI()
        {
            m_Text.text = $"<noparse>{m_CodeText}</noparse>";
        }

        public void SetSelectable(bool selectable)
        {
            m_Text.selection.isSelectable = selectable;
        }

        private void OnCopyCodeClicked(PointerUpEvent evt)
        {
            string disclaimerHeader = string.Format(MuseChatConstants.DisclaimerText, DateTime.Now.ToShortDateString());
            GUIUtility.systemCopyBuffer = string.Concat(disclaimerHeader, m_CodeText);

            MuseChatView.ShowNotification("Copied to clipboard", PopNotificationIconType.Info);
        }


        private void OnSaveCodeClicked(PointerUpEvent evt)
        {
            string file = EditorUtility.SaveFilePanel("Save Code", Application.dataPath, "code", "cs");
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            string formattedCode = CodeExportUtils.Format(m_CodeText, Path.GetFileNameWithoutExtension(file));
            File.WriteAllText(file, formattedCode);
            MuseChatView.ShowNotification("File Saved", PopNotificationIconType.Info);
        }
    }
}
