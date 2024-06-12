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
        const double k_ConsoleCheckInterval = 0.3f;
        static readonly List<LogReference> k_LogCheckTempList = new();
        static double s_LastConsoleCheckTime;

        static Texture2D s_NewChatButtonImage;
        static Texture2D s_HistoryButtonImage;

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
        VisualElement m_ConversationRoot;
        AdaptiveListView<MuseMessage, ChatElementWrapper> m_ConversationList;

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

            m_ConversationRoot = view.Q<VisualElement>("conversationRoot");
            m_ConversationList = new AdaptiveListView<MuseMessage, ChatElementWrapper>
            {
                EnableDelayedElements = false,
                EnableVirtualization = false,
                EnableScrollLock = true
            };
            m_ConversationList.Initialize();
            m_ConversationRoot.Add(m_ConversationList);

            m_HistoryPanelRoot = view.Q<VisualElement>("historyPanelRoot");
            m_HistoryPanel = new HistoryPanel();
            m_HistoryPanel.Initialize();
            m_HistoryPanelRoot.Add(m_HistoryPanel);
            m_HistoryPanelRoot.style.display = UserSessionState.instance.IsHistoryOpen ? DisplayStyle.Flex : DisplayStyle.None;

            m_MusingElement = new ChatElementResponse();
            m_MusingElement.Initialize();
            m_MusingElement.SetData(new MuseMessage { Id = new MuseMessageId(default, string.Empty, MuseMessageIdType.Internal), IsComplete = true, Content = "Musing...", Role = null });
            view.Q<VisualElement>("musingPlaceholderRoot").Add(m_MusingElement);

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

            MuseEditorDriver.instance.OnConversationHistoryChanged += OnConversationHistoryChanged;

            SetMusingActive(false);

            ClientStatus.Instance.OnClientStatusChanged += CheckClientStatus;
            CheckClientStatus(ClientStatus.Instance.Status);

            s_ShowNotificationEvent += OnShowNotification;

            Selection.selectionChanged += OnGameObjectSelectionChanged;
            EditorApplication.update += CheckForConsoleLogSelectionChange;

            OnGameObjectSelectionChanged();
            CheckForConsoleLogSelectionChange();

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

        private void OnSelectedConsoleErrorButtonClicked()
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

            MuseEditorDriver.instance.OnDataChanged -= OnDataChanged;
            MuseEditorDriver.instance.OnConnectionChanged -= OnConnectionChanged;
            MuseEditorDriver.instance.OnConversationHistoryChanged -= OnConversationHistoryChanged;
            Selection.selectionChanged -= OnGameObjectSelectionChanged;
            EditorApplication.update -= CheckForConsoleLogSelectionChange;
            MuseEditorDriver.instance.ClearForNewConversation();

            ClientStatus.Instance.OnClientStatusChanged -= CheckClientStatus;

            s_ShowNotificationEvent -= OnShowNotification;

            UserSessionState.instance.Clear();
        }

        public void PopulateConversation(MuseConversation conversation)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                ClearChat();
                m_ConversationName.text = conversation.Title;

                m_ConversationList.BeginUpdate();
                for (var i = 0; i < conversation.Messages.Count; i++)
                {
                    m_ConversationList.AddData(conversation.Messages[i]);
                }

                m_ConversationList.EndUpdate(true);

                if (UserSessionState.instance.DebugModeEnabled)
                {
                    MuseEditorDriver.instance.OnDebugTrackMetricsRequest?.Invoke(m_ConversationRoot);
                }
            } finally{
                sw.Stop();

                if (UserSessionState.instance.DebugModeEnabled)
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
                var suggestionButton = new Button { title = topic };
                suggestionButton.AddToClassList("mui-chat-suggestion-button");
                suggestionButton.RegisterCallback<PointerUpEvent>(_ => OnSuggestionSelected(suggestionButton.title));

                suggestionParent.Add(suggestionButton);
            }
        }

        public void ClearChat()
        {
            m_ConversationName.text = "New chat";
            m_ChatInput.ClearText();
            m_ConversationList.ClearData();
            m_MusingElement.Hide();
            SetSuggestionVisible(true);
        }

        private void OnHistoryClicked(PointerUpEvent evt)
        {
            MuseEditorDriver.instance.StartConversationRefresh();

            m_HistoryPanelRoot.style.display = m_HistoryPanelRoot.style.display == DisplayStyle.None
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            UserSessionState.instance.IsHistoryOpen = m_HistoryPanelRoot.style.display == DisplayStyle.Flex;
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


        void CheckForConsoleLogSelectionChange()
        {
            if (EditorApplication.timeSinceStartup < s_LastConsoleCheckTime + k_ConsoleCheckInterval)
            {
                return;
            }

            s_LastConsoleCheckTime = EditorApplication.timeSinceStartup;
            k_LogCheckTempList.Clear();
            ConsoleUtils.GetSelectedConsoleLogs(k_LogCheckTempList);
            m_SelectedConsoleMessageNum = k_LogCheckTempList.Count;

            string warnMsg = string.Empty;
            string logMsg = string.Empty;
            string errMsg = string.Empty;

            int logCount = 0;
            int warnCount = 0;
            int errCount = 0;

            for (var i = 0; i < k_LogCheckTempList.Count; i++)
            {
                LogReference logRef = k_LogCheckTempList[i];
                switch (logRef.Mode)
                {
                    case LogReference.ConsoleMessageMode.Log:
                    {
                        if (string.IsNullOrEmpty(logMsg))
                        {
                            logMsg = logRef.Message;
                        }

                        logCount++;
                        break;
                    }

                    case LogReference.ConsoleMessageMode.Warning:
                    {
                        if (string.IsNullOrEmpty(warnMsg))
                        {
                            warnMsg = logRef.Message;
                        }

                        warnCount++;
                        break;
                    }

                    case LogReference.ConsoleMessageMode.Error:
                    {
                        if (string.IsNullOrEmpty(errMsg))
                        {
                            errMsg = logRef.Message;
                        }

                        errCount++;
                        break;
                    }
                }
            }

            UpdateSelectedContextLabel(errCount, m_SelectedConsoleErrorButton,
                errCount > 0 ? errMsg : string.Empty, ref MuseEditorDriver.instance.IsConsoleErrorSelected);
            UpdateSelectedContextLabel(logCount, m_SelectedConsoleInfoButton,
                logCount > 0 ? logMsg : string.Empty, ref MuseEditorDriver.instance.IsConsoleInfoSelected);
            UpdateSelectedContextLabel(warnCount, m_SelectedConsoleWarnButton,
                warnCount > 0 ? warnMsg : string.Empty, ref MuseEditorDriver.instance.IsConsoleWarningSelected);

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
            var contextBuilder = new ContextBuilder();
            MuseEditorDriver.instance.GetSelectedContextString(ref contextBuilder);
            if (contextBuilder.BuildContext(int.MaxValue).Length > MuseChatConstants.PromptContextLimit)
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
                    DeleteChatMessage(data.Message.Id);
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
                    UpdateOrChangeChatMessage(data.Message);
                    m_ConversationList.ScrollToEnd();
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

            var lastMessageHolder = MuseEditorDriver.instance.GetActiveConversation()?.Messages.LastOrDefault();
            if (lastMessageHolder.HasValue)
            {
                var lastMessage = lastMessageHolder.Value;

                if (lastMessage.Role == MuseEditorDriver.k_AssistantRole &&
                    !string.IsNullOrEmpty(lastMessage.Content))
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

        private bool TryGetChatMessageIndex(MuseMessageId id, out int index)
        {
            for (var i = 0; i < m_ConversationList.Data.Count; i++)
            {
                if (m_ConversationList.Data[i].Id == id)
                {
                    index = i;
                    return true;
                }
            }

            index = default;
            return false;
        }

        private void UpdateOrChangeChatMessage(MuseMessage message)
        {
            if (TryGetChatMessageIndex(message.Id, out int existingMessageIndex))
            {
                if (UserSessionState.instance.DebugModeEnabled && message.Content != m_ConversationList.Data[existingMessageIndex].Content)
                {
                    Debug.Log($"MSG_UPD: {message.Id} - {message.Content?.Length}");
                }

                m_ConversationList.UpdateData(existingMessageIndex, message);
                return;
            }

            if (UserSessionState.instance.DebugModeEnabled)
            {
                Debug.Log($"MSG_ADD: {message.Id} - {message.Content?.Length}");
            }

            m_ConversationList.AddData(message);
        }

        private void DeleteChatMessage(MuseMessageId messageId)
        {
            if (TryGetChatMessageIndex(messageId, out var messageIndex))
            {
                if (UserSessionState.instance.DebugModeEnabled)
                {
                    Debug.Log($"MSG_DEL: {messageIndex} - {messageId}");
                }

                m_ConversationList.RemoveData(messageIndex);
            }
        }

        private void UpdateChatElementId(MuseMessageId currentId, MuseMessageId newId)
        {
            if (currentId == newId)
            {
                return;
            }

            if(!TryGetChatMessageIndex(currentId, out var messageIndex))
            {
                if (UserSessionState.instance.DebugModeEnabled)
                    Debug.LogWarning("Change Message ID called for non-existent message: " + currentId);

                return;
            }

            var messageData = m_ConversationList.Data[messageIndex];

            if (UserSessionState.instance.DebugModeEnabled)
            {
                Debug.Log($"MSG_ID_CHANGE: {messageIndex} - {messageData.Id} -> {newId}");
            }

            messageData.Id = newId;
            m_ConversationList.UpdateData(messageIndex, messageData);
        }

        void SetSuggestionVisible(bool value)
        {
            m_SuggestionRoot.style.display = value ? DisplayStyle.Flex:DisplayStyle.None;
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
            var activeConversation = MuseEditorDriver.instance.GetActiveConversation();
            UserSessionState.instance.LastActiveConversationId = activeConversation == null
                ? null
                : activeConversation.Id.Value;
        }
    }
}
