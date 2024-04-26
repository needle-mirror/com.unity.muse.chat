using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Common;
using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    partial class MuseChatView : ManagedTemplate
    {
        static Texture2D s_NewChatButtonImage;
        static Texture2D s_HistoryButtonImage;

        readonly IDictionary<MuseMessageId, ChatElementBase> k_ActiveChatElements = new Dictionary<MuseMessageId, ChatElementBase>();

        VisualElement m_RootMain;
        VisualElement m_NotificationContainer;

        Button m_NewChatButton;
        Button m_HistoryButton;

        Text m_ConversationName;

        VisualElement m_HistoryPanelRoot;
        ChatElementResponse m_MusingElement;
        ChatElementUser m_LastUserElement;

        VisualElement m_SuggestionRoot;
        VisualElement m_SuggestionContent;

        AccountDropdown m_AccountDropdown;

        HistoryPanel m_HistoryPanel;
        ScrollView m_ConversationRoot;
        VisualElement m_ConversationContent;
        Scroller m_ChatScroller;

        VisualElement m_FooterRoot;
        VisualElement m_WarningStatusRoot;

        VisualElement m_ChatInputRoot;
        MuseTextField m_ChatInput;

        VisualElement m_NotificationBanner;
        Text m_NotificationBannerTitle;
        Text m_NotificationBannerMessage;

        Text m_SelectionLabel;

        VisualElement m_SelectedContextRoot;
        VisualElement m_ExceedingSelectedConsoleMessageLimitRoot;
        int m_SelectedConsoleMessageNum;
        Button m_SelectedConsoleInfoButton;
        Button m_SelectedConsoleWarnButton;
        Button m_SelectedConsoleErrorButton;
        Button m_SelectedGameObjectButton;
        string m_SelectedConsoleMessageContent;
        int m_SelectedGameObjectNum;
        string m_SelectedGameObjectName;

        bool m_MusingInProgress;

        internal VisualElement WarningStatusRoot => m_WarningStatusRoot;

        /// <summary>
        /// Constructor for the MuseChatView.
        /// </summary>
        public MuseChatView()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        /// <summary>
        /// Initialize the view and it's component, called by the managed template
        /// </summary>
        /// <param name="view">the template container of the current element</param>
        protected override void InitializeView(TemplateContainer view)
        {
            m_RootMain = view.Q<VisualElement>("root-main");
            m_RootMain.RegisterCallback<MouseEnterEvent>(UpdateSelectedContextWarning);
            m_NotificationContainer = view.Q<VisualElement>("notificationContainer");

            m_NewChatButton = view.SetupButton("newChatButton", OnNewChatClicked);
            m_HistoryButton = view.SetupButton("historyButton", OnHistoryClicked);

            m_ConversationName = view.Q<Text>("conversationNameLabel");
            m_ConversationName.enableRichText = false;

            m_AccountDropdown = new AccountDropdown();
            view.Q<VisualElement>("museAccountContainer").Add(m_AccountDropdown);

            m_ConversationRoot = view.Q<ScrollView>("conversationRoot");
            m_ConversationRoot.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

            m_ConversationContent = view.Q<VisualElement>("conversationContent");
            m_ConversationContent.RegisterCallback<GeometryChangedEvent>(evt => QueueScrollLock());

            m_ChatScroller = m_ConversationRoot.verticalScroller;
            m_ChatScroller.value = m_ChatScroller.highValue;

            m_HistoryPanelRoot = view.Q<VisualElement>("historyPanelRoot");
            m_HistoryPanel = new HistoryPanel();
            m_HistoryPanel.Initialize();
            m_HistoryPanelRoot.Add(m_HistoryPanel);
            m_HistoryPanelRoot.style.display = MuseChatEnvironment.instance.IsHistoryOpen ? DisplayStyle.Flex : DisplayStyle.None;

            m_MusingElement = new ChatElementResponse();
            m_MusingElement.Initialize();
            m_MusingElement.SetData(new MuseMessage { Id = new MuseMessageId(default, string.Empty, MuseMessageIdType.Internal), IsComplete = true, Content = "Musing...", Role = null });
            m_ConversationRoot.Add(m_MusingElement);

            m_SuggestionRoot = view.Q<VisualElement>("suggestionRoot");
            m_SuggestionContent = view.Q<VisualElement>("suggestionContent");

            PopulateSuggestionTopics(m_SuggestionContent);

            m_FooterRoot = view.Q<VisualElement>("footerRoot");

            m_WarningStatusRoot = view.Q<VisualElement>("warningStatusRoot");

            m_ChatInputRoot = view.Q<VisualElement>("chatTextFieldRoot");
            m_ChatInput = new MuseTextField();
            m_ChatInput.Initialize();
            m_ChatInput.OnSubmit += OnMuseRequestSubmit;
            m_ChatInputRoot.Add(m_ChatInput);

            m_NotificationBanner = view.Q<VisualElement>("notificationBanner");
            m_NotificationBannerTitle = view.Q<Text>("notificationBannerTitle");
            m_NotificationBannerMessage = view.Q<Text>("notificationBannerMessage");
            view.SetupButton("notificationBannerButton", BannerButtonClicked);

            m_SelectedContextRoot = view.Q<VisualElement>("userSelectedContextRoot");
            m_ExceedingSelectedConsoleMessageLimitRoot = view.Q<VisualElement>("userSelectedContextWarningRoot");

            m_SelectedConsoleInfoButton = view.Q<Button>("selectedConsoleInfoButton");
            m_SelectedConsoleWarnButton = view.Q<Button>("selectedConsoleWarnButton");
            m_SelectedConsoleErrorButton = view.Q<Button>("selectedConsoleErrorButton");
            m_SelectedGameObjectButton = view.Q<Button>("selectedGameObjectButton");
            m_SelectedConsoleInfoButton.clicked += OnSelectedConsoleInfoButtonClicked;
            m_SelectedConsoleWarnButton.clicked += OnSelectedConsoleWarnButtonClicked;
            m_SelectedConsoleErrorButton.clicked += OnSelectedConsoleErrorButtonClicked;
            m_SelectedGameObjectButton.clicked += OnSelectedGameObjectButtonClicked;
            UpdateContextButton(m_SelectedConsoleInfoButton, MuseEditorDriver.instance.IsConsoleInfoSelected);
            UpdateContextButton(m_SelectedConsoleWarnButton, MuseEditorDriver.instance.IsConsoleWarningSelected);
            UpdateContextButton(m_SelectedConsoleErrorButton, MuseEditorDriver.instance.IsConsoleErrorSelected);
            UpdateContextButton(m_SelectedGameObjectButton, MuseEditorDriver.instance.IsGameObjectSelected);
            UpdateSelectedContextWarning();

            m_SelectionLabel = view.Q<Text>("selectionLabel");
            m_SelectionLabel.SetEnabled(false);

            ClearChat();

            HideBanner();

            view.RegisterCallback<GeometryChangedEvent>(OnViewGeometryChanged);

            MuseEditorDriver.instance.OnDataChanged += OnDataChanged;
            MuseEditorDriver.instance.OnConnectionChanged += OnConnectionChanged;
            MuseEditorDriver.instance.ConnectPlugin();

            MuseEditorDriver.instance.OnConversationHistoryChanged += OnConversationHistoryChanged;

            SetMusingActive(false);

            ClientStatus.Instance.OnClientStatusChanged += CheckClientStatus;
            CheckClientStatus(ClientStatus.Instance.Status);

            s_ShowNotificationEvent += OnShowNotification;

            Selection.selectionChanged += OnGameObjectSelectionChanged;
            EditorApplication.update += OnConsoleMessageSelectionChanged;

            OnGameObjectSelectionChanged();
            OnConsoleMessageSelectionChanged();

            MuseEditorDriver.instance.ViewInitialized();

            MuseEditorDriver.instance.WarmupContext();

            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;


#if UNITY_2022_3_20 || UNITY_2022_3_21 || UNITY_2022_3_22 || UNITY_2022_3_23 || UNITY_2022_3_24 || UNITY_2022_3_25
            bool showWarning = true;
            string versionsToUse = "2022.3.19f1 or lower, or 2022.3.26f1";
#elif UNITY_2023_2_16 || UNITY_2023_2_17 || UNITY_2023_2_18 || UNITY_2023_2_19
            bool showWarning = true;
            string versionsToUse = "2023.2.15f1 or lower, or 2023.2.20f1";
#else
            bool showWarning = false;
            string versionsToUse = "";
#endif

            if (showWarning)
            {
                const string warningShownKey = "MUSE_CHAT_WARNING_SHOWN";
                if (!SessionState.GetBool(warningShownKey, false))
                {
                    ShowBanner("This Unity version has performance issues with Muse Chat",
                        $"For the best experience, use version {versionsToUse} or later. A fix is coming around April 25th.",
                        () =>
                        {
                            SessionState.SetBool(warningShownKey, true);
                        });
                }
            }
        }

        void OnSelectedConsoleErrorButtonClicked()
        {
            MuseEditorDriver.instance.IsConsoleErrorSelected = !MuseEditorDriver.instance.IsConsoleErrorSelected;
            UpdateContextButton(m_SelectedConsoleErrorButton, MuseEditorDriver.instance.IsConsoleErrorSelected);
            UpdateSelectedContextWarning();
        }

        void OnSelectedConsoleWarnButtonClicked()
        {
            MuseEditorDriver.instance.IsConsoleWarningSelected = !MuseEditorDriver.instance.IsConsoleWarningSelected;
            UpdateContextButton(m_SelectedConsoleWarnButton, MuseEditorDriver.instance.IsConsoleWarningSelected);
            UpdateSelectedContextWarning();
        }

        void OnSelectedGameObjectButtonClicked()
        {
            MuseEditorDriver.instance.IsGameObjectSelected = !MuseEditorDriver.instance.IsGameObjectSelected;
            UpdateContextButton(m_SelectedGameObjectButton, MuseEditorDriver.instance.IsGameObjectSelected);
            UpdateSelectedContextWarning();
        }

        void OnSelectedConsoleInfoButtonClicked()
        {
            MuseEditorDriver.instance.IsConsoleInfoSelected = !MuseEditorDriver.instance.IsConsoleInfoSelected;
            UpdateContextButton(m_SelectedConsoleInfoButton, MuseEditorDriver.instance.IsConsoleInfoSelected);
            UpdateSelectedContextWarning();
        }

        void UpdateContextButton(Button button, bool isSelected)
        {
            if (isSelected)
            {
                button.AddToClassList("mui-selected-context-button-selected");
            }
            else
            {
                button.RemoveFromClassList("mui-selected-context-button-selected");
            }
        }

        public void Deinit()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;

            MuseEditorDriver.instance.DisconnectPlugin();
            MuseEditorDriver.instance.OnDataChanged -= OnDataChanged;
            MuseEditorDriver.instance.OnConnectionChanged -= OnConnectionChanged;
            MuseEditorDriver.instance.OnConversationHistoryChanged -= OnConversationHistoryChanged;
            Selection.selectionChanged -= OnGameObjectSelectionChanged;
            EditorApplication.update -= OnConsoleMessageSelectionChanged;
            MuseEditorDriver.instance.ClearForNewConversation();

            ClientStatus.Instance.OnClientStatusChanged -= CheckClientStatus;

            s_ShowNotificationEvent -= OnShowNotification;

            MuseChatEnvironment.instance.ClearSessionState();
        }

        public void PopulateConversation(MuseConversation conversation)
        {
            m_ConversationName.text = conversation.Title;

            var sw = new Stopwatch();
            sw.Start();
            try
            {
                ClearChat();
                for (var i = 0; i < conversation.Messages.Count; i++)
                {
                    AddOrUpdateChatElement(conversation.Messages[i]);
                }

                if (MuseChatEnvironment.instance.DebugModeEnabled)
                {
                    MuseEditorDriver.instance.OnDebugTrackMetricsRequest?.Invoke(m_ConversationContent);
                }
            } finally{
                sw.Stop();

                if (MuseChatEnvironment.instance.DebugModeEnabled)
                {
                    Debug.Log($"PopulateConversation took {sw.ElapsedMilliseconds}ms ({conversation.Messages.Count} Messages)");
                }
            }
        }

        public void PopulateSuggestionTopics(VisualElement suggestionParent)
        {
            var topics = SuggestionTopics.GetRandomList();

            foreach (var topic in topics)
            {
                var suggestionButton = new AppUI.UI.Button { title = topic };
                suggestionButton.AddToClassList("mui-chat-suggestion-button");
                suggestionButton.RegisterCallback<PointerUpEvent>(_ => OnSuggestionSelected(suggestionButton.title));

                suggestionParent.Add(suggestionButton);
            }
        }

        public void ClearChat()
        {
            m_ChatInput.ClearText();
            m_ConversationContent.Clear();
            k_ActiveChatElements.Clear();
            m_MusingElement.Hide();
            SetSuggestionVisible(true);
        }

        private void OnHistoryClicked(PointerUpEvent evt)
        {
            MuseEditorDriver.instance.StartConversationRefresh();

            m_HistoryPanelRoot.style.display = m_HistoryPanelRoot.style.display == DisplayStyle.None
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            MuseChatEnvironment.instance.IsHistoryOpen = m_HistoryPanelRoot.style.display == DisplayStyle.Flex;
        }

        void OnNewChatClicked(PointerUpEvent evt)
        {
            MuseEditorDriver.instance.ClearForNewConversation();
            ClearChat();
            SetMusingActive(false);

            ShowNotification("New Chat created", PopNotificationIconType.Info);
        }

        void OnGameObjectSelectionChanged()
        {
            m_SelectedGameObjectNum = Selection.gameObjects.Length;

            UpdateSelectedContextLabel(m_SelectedGameObjectNum, m_SelectedGameObjectButton,
                Selection.gameObjects.Length > 0 ? Selection.gameObjects[0].name : string.Empty, ref MuseEditorDriver.instance.IsGameObjectSelected);
            UpdateContextSelectionPanel();
        }

        void OnConsoleMessageSelectionChanged()
        {
            List<LogReference> logs = new List<LogReference>();
            ConsoleUtils.GetSelectedConsoleLogs(logs);
            m_SelectedConsoleMessageNum = logs.Count;

            var errorLogs = logs.Where(log => log.Mode == LogReference.ConsoleMessageMode.Error).ToList();
            var warningLogs = logs.Where(log => log.Mode == LogReference.ConsoleMessageMode.Warning).ToList();
            var infoLogs = logs.Where(log => log.Mode == LogReference.ConsoleMessageMode.Log).ToList();

            UpdateSelectedContextLabel(errorLogs.Count, m_SelectedConsoleErrorButton,
                errorLogs.Count > 0 ? errorLogs[0].Message : string.Empty, ref MuseEditorDriver.instance.IsConsoleErrorSelected);
            UpdateSelectedContextLabel(infoLogs.Count, m_SelectedConsoleInfoButton,
                infoLogs.Count > 0 ? infoLogs[0].Message : string.Empty, ref MuseEditorDriver.instance.IsConsoleInfoSelected);
            UpdateSelectedContextLabel(warningLogs.Count, m_SelectedConsoleWarnButton,
                warningLogs.Count > 0 ? warningLogs[0].Message : string.Empty, ref MuseEditorDriver.instance.IsConsoleWarningSelected);
            UpdateContextSelectionPanel();
        }

        void UpdateSelectedContextLabel(int entryNum, Button button, string text, ref bool isSelected)
        {
            switch (entryNum)
            {
                case 0:
                    isSelected = true;
                    UpdateContextButton(button, isSelected);
                    button.style.display = DisplayStyle.None;
                    break;
                case 1:
                {
                    button.style.display = DisplayStyle.Flex;
                    button.tooltip = text.GetTextWithMaxLength(100);
                    var trimmedText = text.Split("\n")[0];
                    if (trimmedText.Length == 0 && text.Split("\n")[1].Length>0)
                    {
                        trimmedText = text.Split("\n")[1];
                    }

                    button.title = trimmedText.GetTextWithMaxLength(14);

                    break;
                }
                default:
                    button.style.display = DisplayStyle.Flex;
                    button.title = entryNum.ToString();
                    button.tooltip = string.Empty;
                    break;
            }
        }

        void UpdateContextSelectionPanel()
        {
            m_SelectedContextRoot.style.display = (m_SelectedConsoleMessageNum + m_SelectedGameObjectNum == 0)? DisplayStyle.None : DisplayStyle.Flex;
        }

        void UpdateSelectedContextWarning(MouseEnterEvent evt = null)
        {
            if (MuseEditorDriver.instance.GetSelectedContextString(MuseChatConstants.PromptContextLimit, true).Length
                > MuseChatConstants.PromptContextLimit)
            {
                m_ExceedingSelectedConsoleMessageLimitRoot.style.display = DisplayStyle.Flex;
            }
            else
            {
                m_ExceedingSelectedConsoleMessageLimitRoot.style.display = DisplayStyle.None;
            }
        }

        private void OnSuggestionSelected(string suggestion)
        {
            m_ChatInput.SetText(suggestion);
        }

        private void OnDataChanged(MuseChatUpdateData data)
        {
            switch (data.Type)
            {
                case MuseChatUpdateType.ConversationChange:
                {
                    // Clear the dynamic content and regen based on the conversation
                    PopulateConversation(MuseEditorDriver.instance.GetActiveConversation());
                    break;
                }

                case MuseChatUpdateType.MessageDelete:
                {
                    DeleteChatElement(data.Message.Id);
                    break;
                }

                case MuseChatUpdateType.MessageIdChange:
                {
                    UpdateChatElementId(data.Message.Id, data.NewMessageId);
                    break;
                }

                case MuseChatUpdateType.NewMessage:
                case MuseChatUpdateType.MessageUpdate:
                {
                    // Add a new div to the dynamic content
                    AddOrUpdateChatElement(data.Message);

                    break;
                }

                case MuseChatUpdateType.ConversationClear:
                {
                    ClearChat();
                    break;
                }
            }

            SetSuggestionVisible(false);
            SetMusingActive(data.IsMusing);
        }

        private void SetMusingActive(bool state)
        {
            m_ChatInput.SetMusingState(state);

            bool hasActiveResponseShown = false;
            if (k_ActiveChatElements.Count > 0)
            {
                // Sort by timestamp and get latest:
                var sortedElements = k_ActiveChatElements.Values.ToArray();
                Array.Sort(sortedElements, (a, b) => a.Message.Timestamp.CompareTo(b.Message.Timestamp));

                var lastElement = sortedElements.Last();

                if (lastElement.Message.Role == MuseEditorDriver.k_AssistantRole &&
                    !string.IsNullOrEmpty(lastElement.Message.Content))
                {
                    hasActiveResponseShown = true;
                }
            }

            m_MusingInProgress = state;
            if (state && !hasActiveResponseShown)
            {
                m_MusingElement.Show();
            }
            else
            {
                m_MusingElement.Hide();
            }
        }

        private void OnConnectionChanged(string message, bool connected)
        {
            if (!connected)
            {
                m_ChatInput.Disable(message);
            }
            else
            {
                m_ChatInput.Enable();
            }
        }

        private void OnMuseRequestSubmit(string message)
        {
            // If musing is in progress and the submit button is pressed, stop the current request:
            if (m_MusingInProgress || string.IsNullOrEmpty(message))
            {
                if (m_MusingInProgress)
                {
                    MuseEditorDriver.instance.AbortPrompt();
                    MuseEditorDriver.instance.AddInternalMessage("Aborted!");
                    SetMusingActive(false);
                }

                return;
            }

            // Disable edit mode if the user is editing a message in the chat element while submitting a prompt in the chat text field:
            if (m_LastUserElement != null)
            {
                m_LastUserElement.EditEnabled = false;
            }

            m_ChatInput.ClearText();
            MuseEditorDriver.instance.ProcessPrompt(message);
        }

        private void CheckClientStatus(ClientStatusResponse clientStatus)
        {
            if (clientStatus.IsDeprecated)
            {
                m_FooterRoot.style.display = DisplayStyle.None;
                m_WarningStatusRoot.style.display = DisplayStyle.Flex;

                SetFooterClientStatus(TextContent.clientStatusDeprecatedMessage, _ => AccountUtility.UpdateMusePackages());
            }
            else if (clientStatus.WillBeDeprecated)
            {
                m_FooterRoot.style.display = DisplayStyle.Flex;
                m_WarningStatusRoot.style.display = DisplayStyle.Flex;

                SetFooterClientStatus(TextContent.ClientStatusWillBeDeprecatedMessage(ClientStatus.Instance.Status.ObsoleteDate), _ => AccountUtility.UpdateMusePackages());
            }
            else
            {
                m_FooterRoot.style.display = DisplayStyle.Flex;
                m_WarningStatusRoot.style.display = DisplayStyle.None;
            }
        }

        private void SetFooterClientStatus(string text, EventCallback<PointerUpEvent> onClick)
        {
            m_WarningStatusRoot.Clear();
            var textGroup = new VisualElement { name = "muse-node-disable-message-group" };
            textGroup.Add(new Text { text = text, enableRichText = true });
            textGroup.AddToClassList("muse-node-message-link");
            textGroup.RegisterCallback(onClick);
            m_WarningStatusRoot.Add(textGroup);
        }

        private void DeleteChatElement(MuseMessageId messageId)
        {
            if (!k_ActiveChatElements.TryGetValue(messageId, out var chatElement))
            {
                Debug.LogWarning("Delete Message called for non-existent message: " + messageId);
                return;
            }

            k_ActiveChatElements.Remove(messageId);
            m_ConversationContent.Remove(chatElement);
        }

        private void UpdateChatElementId(MuseMessageId currentId, MuseMessageId newId)
        {
            if (currentId == newId)
            {
                return;
            }

            if (!k_ActiveChatElements.TryGetValue(currentId, out var chatElement))
            {
                Debug.LogWarning("Change Message ID called for non-existent message: " + currentId);
                return;
            }

            var message = chatElement.Message;
            message.Id = newId;

            chatElement.SetData(message);
            k_ActiveChatElements.Remove(currentId);
            k_ActiveChatElements.Add(message.Id, chatElement);
        }

        private void AddOrUpdateChatElement(MuseMessage message)
        {
            if (!k_ActiveChatElements.TryGetValue(message.Id, out var chatElement))
            {
                // Add a new element
                if (message.Role == MuseEditorDriver.k_UserRole)
                {
                    chatElement = new ChatElementUser { EditEnabled = true };
                    if (m_LastUserElement != null)
                    {
                        m_LastUserElement.EditEnabled = false;
                    }

                    m_LastUserElement = (ChatElementUser)chatElement;
                }
                else
                {
                    chatElement = new ChatElementResponse();
                }

                chatElement.Initialize();
                k_ActiveChatElements.Add(message.Id, chatElement);
                m_ConversationContent.Add(chatElement);
            }

            if (chatElement.Message.Content == message.Content &&
                chatElement.Message.IsComplete == message.IsComplete)   // complete flag removes last word when false.
            {
                // No change to content, no need to update
                return;
            }

            chatElement.SetData(message);
        }

        void QueueScrollLock()
        {
            var lastScrollPosition = MuseChatEnvironment.instance.ScrollPosition;
            if (lastScrollPosition.HasValue)
            {
                // Need to set scroll position here and again in the next update to avoid the scrollview jumping after its internal update:
                m_ChatScroller.value = lastScrollPosition.Value;
                EditorApplication.delayCall += ScrollToLastPosition;
            }
            else if (m_ChatInput.HasFocus || (m_ChatScroller.value >= m_ChatScroller.highValue))
            {
                EditorApplication.delayCall += ApplyScrollLock;
            }
        }

        void ScrollToLastPosition()
        {
            var lastScrollPosition = MuseChatEnvironment.instance.ScrollPosition;
            if (lastScrollPosition.HasValue)
            {
                m_ChatScroller.value = lastScrollPosition.Value;
                MuseChatEnvironment.instance.ScrollPosition = null;
            }
        }

        void ApplyScrollLock()
        {
            m_ChatScroller.value = m_ChatScroller.highValue;
        }

        void SetSuggestionVisible(bool visible)
        {
            m_SuggestionRoot.style.display = visible ? DisplayStyle.Flex:DisplayStyle.None;
        }

        void OnViewGeometryChanged(GeometryChangedEvent evt)
        {
            bool isCompactView = evt.newRect.width < MuseChatConstants.CompactWindowThreshold;

            m_HistoryButton.EnableInClassList(MuseChatConstants.CompactStyle, isCompactView);
            m_NewChatButton.EnableInClassList(MuseChatConstants.CompactStyle, isCompactView);

            m_ConversationName.EnableInClassList(MuseChatConstants.CompactStyle, isCompactView);

            m_FooterRoot.EnableInClassList(MuseChatConstants.CompactStyle, isCompactView);

            m_SelectionLabel.EnableInClassList(MuseChatConstants.CompactStyle, isCompactView);
        }

        void OnConversationHistoryChanged()
        {
            m_HistoryPanel.Reload();
        }

        void OnBeforeAssemblyReload()
        {
            MuseChatEnvironment.instance.ScrollPosition = m_ChatScroller.value;

            var activeConversation = MuseEditorDriver.instance.GetActiveConversation();
            MuseChatEnvironment.instance.LastActiveConversationId = activeConversation == null
                ? null
                : activeConversation.Id.Value;
        }
    }
}
