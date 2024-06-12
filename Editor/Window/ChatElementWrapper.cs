using JetBrains.Annotations;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Common.Utils;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    [UsedImplicitly]
    internal class ChatElementWrapper : AdaptiveListViewEntry
    {
        private VisualElement m_Root;
        private Text m_IndexDebugElement;

        private ChatElementBase m_UserChatElement;
        private ChatElementBase m_ResponseChatElement;

        protected override void InitializeView(TemplateContainer view)
        {
            m_Root = view.Q<VisualElement>("wrapperRoot");
            m_IndexDebugElement = view.Q<Text>("indexDebugText");
        }

        public override void SetData(int index, object data, bool isSelected = false)
        {
            base.SetData(index, data);

            var message = (MuseMessage)data;
            if (UserSessionState.instance.DebugModeEnabled)
            {
                m_IndexDebugElement.style.display = DisplayStyle.Flex;
                m_IndexDebugElement.text = $"Index: {index}";
            }

            if(message.Role == MuseEditorDriver.k_UserRole)
            {
                m_ResponseChatElement?.SetDisplay(false);
                SetupChatElement(ref m_UserChatElement, message);
            }
            else
            {
                m_UserChatElement?.SetDisplay(false);
                SetupChatElement(ref m_ResponseChatElement, message, true);
            }
        }

        void SetupChatElement(ref ChatElementBase element, MuseMessage message, bool hideIfEmpty = false)
        {
            if (element == null)
            {
                if (message.Role == MuseEditorDriver.k_UserRole)
                {
                    element = new ChatElementUser { EditEnabled = true };
                }
                else
                {
                    element = new ChatElementResponse();
                }

                element.Initialize();
                m_Root.Add(element);
            }
            else
            {
                element.SetDisplay(true);
            }

            if (hideIfEmpty && string.IsNullOrEmpty(message.Content))
            {
                element.SetDisplay(false);
            }
            else
            {
                element.SetDisplay(true);
            }

            if (element.Message.Content == message.Content &&
                element.Message.IsComplete == message.IsComplete)   // complete flag removes last word when false.
            {
                // No change to content, no need to update
                return;
            }

            element.SetData(message);
        }
    }
}
