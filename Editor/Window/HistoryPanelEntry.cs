using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;
using TextField = Unity.Muse.AppUI.UI.TextField;

namespace Unity.Muse.Chat
{
    internal class HistoryPanelEntry : AdaptiveListViewEntry
    {
        private const string k_HeaderClass = "mui-history-panel-header-entry";
        private const string k_SelectedClass = "mui-history-panel-entry-selected";

        const string k_Edit = "Edit";
        const string k_Delete = "Delete";
        MuseConversationInfo m_Data;

        VisualElement m_HeaderRoot;
        Text m_HeaderText;

        VisualElement m_ConversationRoot;
        Icon m_ConversationIcon;
        Text m_ConversationText;
        TextField m_ConversationEditText;
        Button m_ConversationButton;

        bool m_EditModeActive;
        bool m_IsHeader;
        bool m_IsSelected;
        bool m_IsContextClick;

        public MuseConversationInfo Data => m_Data;

        public void SetSelected(bool selected)
        {
            if (m_IsSelected == selected)
            {
                return;
            }

            m_IsSelected = selected;
            EnableInClassList(k_SelectedClass, selected);
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_HeaderRoot = view.Q<VisualElement>("historyPanelHeaderRoot");
            m_HeaderText = view.Q<Text>("historyPanelHeaderText");

            m_ConversationRoot = view.Q<VisualElement>("historyPanelElementConversationRoot");
            m_ConversationRoot.RegisterCallback<PointerEnterEvent>(_ =>
            {
                m_ConversationButton.style.display = m_EditModeActive ? DisplayStyle.None : DisplayStyle.Flex;
            });

            m_ConversationRoot.RegisterCallback<PointerLeaveEvent>(_ => m_ConversationButton.style.display = DisplayStyle.None);
            m_ConversationRoot.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (evt.button == (int)MouseButton.RightMouse)
                {
                    OnButtonClicked(evt);
                }
            });

            m_ConversationRoot.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (evt.button == (int)MouseButton.RightMouse)
                {
                    OnButtonClicked(evt);
                }
            });

            m_ConversationIcon = view.Q<Icon>("historyPanelElementConversationIcon");
            m_ConversationText = view.Q<Text>("historyPanelElementConversationText");
            m_ConversationText.enableRichText = false;
            m_ConversationEditText = view.Q<TextField>("historyPanelElementConversationEditText");
            m_ConversationEditText.RegisterCallback<FocusOutEvent>(OnEditFocusLost);
            m_ConversationEditText.RegisterValueChangedCallback(OnEditComplete);
            m_ConversationButton = view.SetupButton("historyPanelElementConversationButton", OnButtonClicked);

            RegisterCallback<PointerUpEvent>(OnSelectEntry);

            RefreshUI();
        }

        private void OnSelectEntry(PointerUpEvent evt)
        {
            if (m_IsSelected || m_IsHeader || m_IsContextClick)
            {
                m_IsContextClick = false;
                return;
            }

            NotifySelectionChanged();
        }

        void OnButtonClicked(PointerUpEvent evt)
        {
            m_IsContextClick = true;

            // Create the menu and add items to it
            var menu = new GenericMenu();

            // Add menu items
            menu.AddItem(new GUIContent(k_Edit), false, OnEditClicked);
            menu.AddItem(new GUIContent(k_Delete), false, OnDeleteClicked);
            // Add more items here

            // Show the menu at the current mouse position
            menu.ShowAsContext();

            // Use the event
            evt.StopPropagation();
            evt.StopImmediatePropagation();
            Event.current.Use();
        }

        void OnEditClicked()
        {
            m_EditModeActive = true;
            m_ConversationEditText.SetValueWithoutNotify(m_Data.Title);

            RefreshUI();
        }

        void OnDeleteClicked()
        {
            MuseEditorDriver.instance.StartDeleteConversation(m_Data);

            MuseChatView.ShowNotification("Chat deleted", PopNotificationIconType.Info);
        }

        public override void SetData(int index, object newData, bool isSelected = false)
        {
            base.SetData(index, newData);

            if (newData is string headerText)
            {
                SetAsHeader(headerText);
                SetSelected(false);
            }
            else
            {
                var data = (MuseConversationInfo)newData;
                SetAsData(data);
                SetSelected(isSelected);
            }
        }

        void SetAsHeader(string text)
        {
            m_IsHeader = true;

            m_HeaderRoot.style.display = DisplayStyle.Flex;
            m_ConversationRoot.style.display = DisplayStyle.None;
            m_HeaderText.text = text;
            AddToClassList(k_HeaderClass);

            RefreshUI();
        }

        void SetAsData(MuseConversationInfo data)
        {
            m_Data = data;
            m_EditModeActive = false;
            m_IsHeader = false;

            m_HeaderRoot.style.display = DisplayStyle.None;
            m_ConversationRoot.style.display = DisplayStyle.Flex;
            RemoveFromClassList(k_HeaderClass);

            // TODO: Enable if design wants a log to indicate contextual conversations
            // m_ConversationIcon.style.display = data.IsContextAware ? DisplayStyle.Flex : DisplayStyle.None;
            m_ConversationText.text = data.Title.Replace("\n", " ");
            m_ConversationText.tooltip = data.Title;

            RefreshUI();
        }

        void RefreshUI()
        {
            if (m_EditModeActive)
            {
                m_ConversationButton.style.display = DisplayStyle.None;
                m_ConversationEditText.style.display = DisplayStyle.Flex;
                m_ConversationText.style.display = DisplayStyle.None;
                m_ConversationEditText.Focus();
            }
            else
            {
                m_ConversationButton.style.display = DisplayStyle.None;
                m_ConversationEditText.style.display = DisplayStyle.None;
                m_ConversationText.style.display = DisplayStyle.Flex;
            }
        }

        private void OnEditFocusLost(FocusOutEvent evt)
        {
            m_EditModeActive = false;
            RefreshUI();
        }

        private void OnEditComplete(ChangeEvent<string> evt)
        {
            m_EditModeActive = false;
            RefreshUI();

            if (evt.newValue == m_Data.Title)
            {
                return;
            }

            // Set the conversation title directly, we don't wait for the server to respond
            m_Data.Title = evt.newValue;
            m_ConversationText.text = m_Data.Title;

            MuseEditorDriver.instance.StartConversationRename(m_Data.Id, evt.newValue);
        }
    }
}
