using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Common;
using Unity.Muse.Common.Account;
using Unity.Muse.Common.Utils;
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

        MuseConversationPanel m_ConversationPanel;

        MuseChatNotificationBanner m_Banner;
        VisualElement m_BannerRoot;

        VisualElement m_HistoryPanelRoot;
        VisualElement m_MusingElementRoot;
        MusingElement m_MusingElement;
        ChatElementUser m_LastUserElement;

        VisualElement m_SuggestionRoot;
        VisualElement m_SuggestionContent;

        HistoryPanel m_HistoryPanel;

        VisualElement m_HeaderRoot;
        VisualElement m_FooterRoot;

        VisualElement m_ChatInputRoot;
        MuseTextField m_ChatInput;

        Text m_SelectionLabel;

        VisualElement m_SelectedContextRoot;
        VisualElement m_ExceedingSelectedConsoleMessageLimitRoot;
        int m_SelectedConsoleMessageNum;
        Button m_SelectedConsoleInfoButton;
        Button m_SelectedConsoleWarnButton;
        Button m_SelectedConsoleErrorButton;
        Button m_SelectedGameObjectButton;
        string m_SelectedConsoleMessageContent;
        int m_SelectedObjectNum;
        string m_SelectedGameObjectName;

        bool m_MusingInProgress;

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
            m_HeaderRoot = view.Q<VisualElement>("headerRoot");
            m_HeaderRoot.AddManipulator(new SessionStatusTracker());

            m_RootMain = view.Q<VisualElement>("root-main");
            m_RootMain.RegisterCallback<MouseEnterEvent>(UpdateSelectedContextWarning);
            m_NotificationContainer = view.Q<VisualElement>("notificationContainer");

            m_NewChatButton = view.SetupButton("newChatButton", OnNewChatClicked);
            m_HistoryButton = view.SetupButton("historyButton", OnHistoryClicked);

            m_ConversationName = view.Q<Text>("conversationNameLabel");
            m_ConversationName.enableRichText = false;

            var panelRoot = view.Q<VisualElement>("chatPanelRoot");
            m_ConversationPanel = new MuseConversationPanel();
            m_ConversationPanel.Initialize();
            panelRoot.Add(m_ConversationPanel);

            m_HistoryPanelRoot = view.Q<VisualElement>("historyPanelRoot");
            m_HistoryPanel = new HistoryPanel();
            m_HistoryPanel.Initialize();
            m_HistoryPanelRoot.Add(m_HistoryPanel);
            m_HistoryPanelRoot.style.display = UserSessionState.instance.IsHistoryOpen ? DisplayStyle.Flex : DisplayStyle.None;

            m_MusingElementRoot = view.Q<VisualElement>("musingElementContainer");
            m_MusingElement = new MusingElement();
            m_MusingElement.Initialize();
            m_MusingElementRoot.Add(m_MusingElement);

            if (UserSessionState.instance.DebugModeEnabled)
            {
                ShowMusingElement();
            }

            var contentRoot = view.Q<VisualElement>("chatContentRoot");
            contentRoot.AddManipulator(new SessionStatusTracker());

            m_SuggestionRoot = view.Q<VisualElement>("suggestionRoot");
            m_SuggestionContent = view.Q<VisualElement>("suggestionContent");

            PopulateSuggestionTopics(m_SuggestionContent);

            m_HeaderRoot = view.Q<VisualElement>("headerRoot");
            m_FooterRoot = view.Q<VisualElement>("footerRoot");
            m_FooterRoot.AddManipulator(new SessionStatusTracker());

            m_ChatInputRoot = view.Q<VisualElement>("chatTextFieldRoot");
            m_ChatInput = new MuseTextField();
            m_ChatInput.Initialize();
            m_ChatInput.OnSubmit += OnMuseRequestSubmit;
            m_ChatInputRoot.Add(m_ChatInput);

            var notificationBanner = view.Q<VisualElement>("account-notifications");
            notificationBanner.Add(new SessionStatusNotifications());

            m_BannerRoot = view.Q<VisualElement>("notificationBannerRoot");
            m_Banner = new MuseChatNotificationBanner();
            m_Banner.Initialize(false);
            m_BannerRoot.Add(m_Banner);

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
            UpdateContextButton(m_SelectedGameObjectButton, MuseEditorDriver.instance.IsObjectSelected);
            UpdateSelectedContextWarning();

            m_SelectionLabel = view.Q<Text>("selectionLabel");
            m_SelectionLabel.SetEnabled(false);

            ClearChat();

            view.RegisterCallback<GeometryChangedEvent>(OnViewGeometryChanged);

            MuseEditorDriver.instance.OnDataChanged += OnDataChanged;
            MuseEditorDriver.instance.OnConnectionChanged += OnConnectionChanged;

            MuseEditorDriver.instance.OnConversationHistoryChanged += OnConversationHistoryChanged;
            MuseEditorDriver.instance.OnConversationTitleChanged += OnConversationTitleChanged;
            SetMusingActive(false);

            ClientStatus.Instance.OnClientStatusChanged += CheckClientStatus;
            CheckClientStatus(ClientStatus.Instance.Status);

            s_ShowNotificationEvent += OnShowNotification;

            Selection.selectionChanged += OnGameObjectSelectionChanged;
            EditorApplication.update += CheckForConsoleLogSelectionChange;

            OnGameObjectSelectionChanged();
            CheckForConsoleLogSelectionChange();

            MuseEditorDriver.instance.ViewInitialized();

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
                    m_Banner.Show("This Unity version has performance issues with Muse Chat",
                        $"For the best experience, use version {versionsToUse} or later.", dismissCallback:
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
            MuseEditorDriver.instance.IsObjectSelected = !MuseEditorDriver.instance.IsObjectSelected;
            UpdateContextButton(m_SelectedGameObjectButton, MuseEditorDriver.instance.IsObjectSelected);
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
            MuseEditorDriver.instance.OnConversationTitleChanged -= OnConversationTitleChanged;
            MuseEditorDriver.instance.OnConnectionChanged -= OnConnectionChanged;
            MuseEditorDriver.instance.OnConversationHistoryChanged -= OnConversationHistoryChanged;
            Selection.selectionChanged -= OnGameObjectSelectionChanged;
            EditorApplication.update -= CheckForConsoleLogSelectionChange;
            MuseEditorDriver.instance.ClearForNewConversation();

            ClientStatus.Instance.OnClientStatusChanged -= CheckClientStatus;

            s_ShowNotificationEvent -= OnShowNotification;

            UserSessionState.instance.Clear();
        }

        public void ChangeConversation(MuseConversation conversation)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                ClearChat();
                m_ConversationName.text = conversation.Title;

                m_ConversationPanel.Populate(conversation);
            } finally{
                sw.Stop();

                if (UserSessionState.instance.DebugModeEnabled)
                {
                    Debug.Log($"PopulateConversation took {sw.ElapsedMilliseconds}ms ({conversation.Messages.Count} Messages)");
                }
            }
        }

        public void OnConversationTitleChanged(string title)
        {
            m_ConversationName.text = title;
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
            m_ConversationPanel.ClearConversation();
            HideMusingElement();
            SetSuggestionVisible(true);
        }

        private void ShowMusingElement()
        {
            m_MusingElementRoot.SetDisplay(true);
            m_MusingElement.Start();

            var message = "Musing";
            switch (MuseEditorDriver.instance.CurrentPrompState)
            {
                case MuseEditorDriver.PrompState.GatheringContext:
                    message = "Gathering context";
                    break;
            }

            m_MusingElement.SetMessage(message);
        }

        private void HideMusingElement()
        {
            if (UserSessionState.instance.DebugModeEnabled)
            {
                return;
            }

            m_MusingElementRoot.SetDisplay(false);
            m_MusingElement.Stop();
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
            m_SelectedObjectNum = Selection.objects.Length;

            UpdateSelectedContextLabel(m_SelectedObjectNum, m_SelectedGameObjectButton,
                Selection.objects.Length > 0 ? Selection.objects[0].name : string.Empty, ref MuseEditorDriver.instance.IsObjectSelected);
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

                    button.title = trimmedText;

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
            m_SelectedContextRoot.style.display = (m_SelectedConsoleMessageNum + m_SelectedObjectNum == 0)? DisplayStyle.None : DisplayStyle.Flex;
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
                    ChangeConversation(MuseEditorDriver.instance.GetActiveConversation());
                    break;
                }

                case MuseChatUpdateType.ConversationClear:
                {
                    ClearChat();
                    break;
                }

                default:
                {
                    m_ConversationPanel.UpdateData(data);
                    break;
                }
            }

            SetSuggestionVisible(false);
            SetMusingActive(data.IsMusing);
        }

        private void SetMusingActive(bool state)
        {
            m_ChatInput.SetMusingState(state);

            var lastMessageHolder = MuseEditorDriver.instance.GetActiveConversation()?.Messages.LastOrDefault();
            if (lastMessageHolder.HasValue)
            {
                var lastMessage = lastMessageHolder.Value;
            }

            m_MusingInProgress = state;
            if (state)
            {
                ShowMusingElement();
            }
            else
            {
                HideMusingElement();
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
                    SetMusingActive(false);
                    MuseEditorDriver.instance.StartConversationReload();
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
            SetMusingActive(true);
        }

        private void ToggleEnabled(bool enabled)
        {
            m_HeaderRoot.SetEnabled(enabled);
            m_SuggestionContent.SetEnabled(enabled);
            m_FooterRoot.SetDisplay(enabled);
        }

        private void CheckClientStatus(ClientStatusResponse clientStatus)
        {
            if (clientStatus.IsDeprecated)
            {
                ToggleEnabled(false);
                m_Banner.Show("Update required", TextContent.clientStatusDeprecatedMessage, ClientStatus.Instance.OpenInPackageManager, "Update Packages", false);
                return;
            }

            ToggleEnabled(true);

            if (clientStatus.WillBeDeprecated)
            {
                m_Banner.Show("Update required soon", TextContent.ClientStatusWillBeDeprecatedMessage(ClientStatus.Instance.Status.ObsoleteDate), ClientStatus.Instance.OpenInPackageManager, "Update Packages");
            }
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
