using System;
using Unity.Muse.AppUI.UI;
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
            view.SetupButton("copyButton", OnCopyCodeClicked);
        }

        private void OnCopyCodeClicked(PointerUpEvent evt)
        {
            string disclaimerHeader = string.Format(MuseChatConstants.DisclaimerText, DateTime.Now.ToShortDateString());
            GUIUtility.systemCopyBuffer = string.Concat(disclaimerHeader, m_CodeText);

            MuseChatView.ShowNotification("Copied to clipboard", PopNotificationIconType.Info);
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
    }
}
