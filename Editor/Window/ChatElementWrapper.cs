using System;
using JetBrains.Annotations;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Common.Utils;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    [UsedImplicitly]
    class ChatElementWrapper : AdaptiveListViewEntry
    {
        VisualElement m_Root;
        Text m_IndexDebugElement;

        ChatElementBase m_ChatElement;

        protected override void InitializeView(TemplateContainer view)
        {
            m_Root = view.Q<VisualElement>("wrapperRoot");
            m_IndexDebugElement = view.Q<Text>("indexDebugText");
        }

        public override void SetData(int index, object data, bool isSelected = false)
        {
            base.SetData(index, data);

            var message = (MuseMessage)data;
            if (UserSessionState.instance.DebugUIModeEnabled)
            {
                m_IndexDebugElement.style.display = DisplayStyle.Flex;
                m_IndexDebugElement.text = $"Index: {index}";
            }

            switch (message.Role)
            {
                case MuseEditorDriver.k_UserRole:
                    SetupChatElement(ref m_ChatElement, message);
                    break;
                case MuseEditorDriver.k_AssistantRole:
                    SetupChatElement(ref m_ChatElement, message, true);
                    break;
                case MuseEditorDriver.k_SystemRole:
                    SetupChatElement(ref m_ChatElement, message, true);
                    break;
            }
        }

        void SetupChatElement(ref ChatElementBase element, MuseMessage message, bool hideIfEmpty = false)
        {
            if (element == null)
            {
                switch (message.Role)
                {
                    case MuseEditorDriver.k_UserRole:
                        element = new ChatElementUser { EditEnabled = true };
                        break;
                    case MuseEditorDriver.k_SystemRole:
                        element = new ChatElementSystem();
                        break;
                    case MuseEditorDriver.k_AssistantRole:
                        element = new ChatElementResponse();
                        break;
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
