using System;
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
        Button m_FavoriteToggle;
        Text m_ConversationText;
        TextField m_ConversationEditText;

        bool m_EditModeActive;
        bool m_IsHeader;
        bool m_IsSelected;
        bool m_IsButtonClick;
        bool m_IsFavorited;

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
            m_ConversationRoot.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (evt.button == (int)MouseButton.RightMouse)
                {
                    OnConversationClicked(evt);
                }
            });

            m_ConversationRoot.RegisterCallback<PointerUpEvent>(evt =>
            {
                if (evt.button == (int)MouseButton.RightMouse)
                {
                    OnConversationClicked(evt);
                }
            });

            m_FavoriteToggle = view.SetupButton("historyPanelFavoriteStateButton", OnToggleFavorite);

            m_ConversationText = view.Q<Text>("historyPanelElementConversationText");
            m_ConversationText.enableRichText = false;

            m_ConversationEditText = view.Q<TextField>("historyPanelElementConversationEditText");
            m_ConversationEditText.RegisterCallback<FocusOutEvent>(OnEditFocusLost);
            m_ConversationEditText.RegisterValueChangedCallback(OnEditComplete);

            RegisterCallback<PointerUpEvent>(OnSelectEntry);

            RefreshUI();
        }

        private void OnSelectEntry(PointerUpEvent evt)
        {
            if (m_IsSelected || m_IsHeader || m_IsButtonClick)
            {
                m_IsButtonClick = false;
                return;
            }

            NotifySelectionChanged();
        }

        void OnToggleFavorite(PointerUpEvent evt)
        {
            m_IsButtonClick = true;

            m_IsFavorited = !m_IsFavorited;

            MuseEditorDriver.instance.StartConversationFavoriteToggle(m_Data.Id, m_IsFavorited);
            RefreshFavoriteDisplay();

            MuseChatHistoryBlackboard.SetFavoriteCache(m_Data.Id, m_IsFavorited);
            MuseChatHistoryBlackboard.HistoryPanelRefreshRequired?.Invoke();
        }

        void OnConversationClicked(PointerUpEvent evt)
        {
            m_IsButtonClick = true;

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

            // This field is fetched via cache, which gets invalidated on a full reload
            // Until then we persist our local state since changing this value can take longer than a conversation refresh
            m_IsFavorited = MuseChatHistoryBlackboard.GetFavoriteCache(data.Id);

            RefreshUI();
        }

        void RefreshFavoriteDisplay()
        {
            m_FavoriteToggle.leadingIcon = m_IsFavorited ? "star-filled-white" : "star-filled-grey";
        }

        void RefreshUI()
        {
            if (m_EditModeActive)
            {
                m_ConversationEditText.style.display = DisplayStyle.Flex;
                m_ConversationText.style.display = DisplayStyle.None;
                m_ConversationEditText.Focus();
            }
            else
            {
                m_ConversationEditText.style.display = DisplayStyle.None;
                m_ConversationText.style.display = DisplayStyle.Flex;
            }

            RefreshFavoriteDisplay();
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
