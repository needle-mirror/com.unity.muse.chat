using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.AppUI.Core;
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
        static double s_LastConsoleCheckTime;

        static Texture2D s_NewChatButtonImage;
        static Texture2D s_HistoryButtonImage;

        VisualElement m_RootMain;
        Panel m_RootPanel;
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

        VisualElement m_InspirationRoot;
        MuseChatInspirationPanel m_InspirationPanel;

        HistoryPanel m_HistoryPanel;

        VisualElement m_HeaderRoot;
        VisualElement m_FooterRoot;

        VisualElement m_ChatInputRoot;
        MuseTextField m_ChatInput;
        ActionGroup m_CommandActionGroup;

        Button m_AddContextButton;
        Button m_ClearContextButton;

        ScrollView m_SelectedContextScrollView;
        VisualElement m_SelectedContextScrollViewContent;

        VisualElement m_ExceedingSelectedConsoleMessageLimitRoot;
        int m_SelectedConsoleMessageNum;
        string m_SelectedConsoleMessageContent;
        string m_SelectedGameObjectName;

        List<Object> ObjectSelection
        {
            set
            {
                if (m_Window != null) m_Window.m_ObjectSelection = value;
            }
            get => m_Window != null ? m_Window.m_ObjectSelection : null;
        }
        List<LogReference> ConsoleSelection
        {
            set
            {
                if (m_Window != null) m_Window.m_ConsoleSelection = value;
            }
            get => m_Window != null ? m_Window.m_ConsoleSelection : null;
        }

        bool m_MusingInProgress;
        MuseChatWindow m_Window;

        internal static MuseChatView m_Instance;
        bool m_DelayedUpdateSelectionElements;

        private SelectionPopup m_CurrentSelectionPopup;

        /// <summary>
        /// Constructor for the MuseChatView.
        /// </summary>
        public MuseChatView()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public MuseChatView(MuseChatWindow window)
            : base(MuseChatConstants.UIModulePath)
        {
            m_Window = window;
        }

        public void InitializeThemeAndStyle()
        {
            LoadStyle(m_RootPanel, EditorGUIUtility.isProSkin ? MuseChatConstants.MuseChatSharedStyleDark : MuseChatConstants.MuseChatSharedStyleLight);
            LoadStyle(m_RootPanel, MuseChatConstants.MuseChatBaseStyle, true);
        }

        /// <summary>
        /// Initialize the view and its component, called by the managed template
        /// </summary>
        /// <param name="view">the template container of the current element</param>
        protected override void InitializeView(TemplateContainer view)
        {
            this.style.flexGrow = 1;
            view.style.flexGrow = 1;

            m_HeaderRoot = view.Q<VisualElement>("headerRoot");
            m_HeaderRoot.SetSessionTracked();

            m_RootMain = view.Q<VisualElement>("root-main");
            m_RootMain.RegisterCallback<MouseEnterEvent>(UpdateSelectedContextWarning);
            m_NotificationContainer = view.Q<VisualElement>("notificationContainer");

            m_RootPanel = view.Q<Panel>("root-panel");

            m_NewChatButton = view.SetupButton("newChatButton", OnNewChatClicked);
            m_NewChatButton.SetSessionTracked();
            m_HistoryButton = view.SetupButton("historyButton", OnHistoryClicked);
            m_HistoryButton.SetSessionTracked();

            m_ConversationName = view.Q<Text>("conversationNameLabel");
            m_ConversationName.enableRichText = false;

            var panelRoot = view.Q<VisualElement>("chatPanelRoot");
            m_ConversationPanel = new MuseConversationPanel();
            m_ConversationPanel.Initialize();
            m_ConversationPanel.RegisterCallback<MouseUpEvent>(OnConversationPanelClicked);
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

            if (UserSessionState.instance.DebugUIModeEnabled)
            {
                ShowMusingElement();
            }

            var contentRoot = view.Q<VisualElement>("chatContentRoot");
            contentRoot.SetSessionTracked();

            m_InspirationRoot = view.Q<VisualElement>("inspirationPanelRoot");
            m_InspirationPanel = new MuseChatInspirationPanel();
            m_InspirationPanel.Initialize();
            m_InspirationPanel.InspirationSelected += OnInspirationSelected;
            m_InspirationRoot.Add(m_InspirationPanel);

            m_HeaderRoot = view.Q<VisualElement>("headerRoot");
            m_FooterRoot = view.Q<VisualElement>("footerRoot");
            m_FooterRoot.SetSessionTracked();

            m_ChatInputRoot = view.Q<VisualElement>("chatTextFieldRoot");
            m_ChatInput = new MuseTextField();
            m_ChatInput.Initialize();
            m_ChatInput.OnSubmit += OnMuseRequestSubmit;
            m_ChatInputRoot.Add(m_ChatInput);

            m_CommandActionGroup = view.Q<ActionGroup>("commandGroup");

            SetupCommandButton(view.Q<ActionButton>("commandAsk"), ChatCommandType.Ask, 0);
            SetupCommandButton(view.Q<ActionButton>("commandRun"), ChatCommandType.Run, 1);
            SetupCommandButton(view.Q<ActionButton>("commandCode"), ChatCommandType.Code, 2);

            // Hide commands until features are ready
            m_CommandActionGroup.style.display = DisplayStyle.None;

            m_AddContextButton = view.Q<Button>("addContextButton");
            m_AddContextButton.clicked += ShowSelectionPopup;

            m_ClearContextButton = view.Q<Button>("clearContextButton");
            m_ClearContextButton.clicked += ClearContext;

            var notificationBanner = view.Q<VisualElement>("account-notifications");
            notificationBanner.Add(new SessionStatusNotifications());

            m_BannerRoot = view.Q<VisualElement>("notificationBannerRoot");
            m_Banner = new MuseChatNotificationBanner();
            m_Banner.Initialize(false);
            m_BannerRoot.Add(m_Banner);

            m_SelectedContextScrollView = view.Q<ScrollView>("userSelectedContextListView");

            m_SelectedContextScrollViewContent = m_SelectedContextScrollView.Q<VisualElement>("unity-content-container");
            m_SelectedContextScrollViewContent.style.flexDirection = FlexDirection.Row;
            m_SelectedContextScrollViewContent.style.flexWrap = Wrap.Wrap;

            m_ExceedingSelectedConsoleMessageLimitRoot = view.Q<VisualElement>("userSelectedContextWarningRoot");

            UpdateMuseEditorDriverSelection();
            UpdateSelectedContextWarning();

            EditorApplication.hierarchyChanged += OnHierarchChanged;

            ClearChat();


            m_DropZone = view.Q<DropZone>("chatDropZone");
            m_DropZone.controller.acceptDrag += OnAcceptDrag;
            m_DropZone.controller.dropped += OnDropped;
            m_DropZone.controller.dragEnded += OnDragEnded;

            m_DropZoneContent = view.Q<VisualElement>("chatDropZoneContent");

            SetDropZoneActive(false);

            m_RootMain.RegisterCallback<DragEnterEvent>(DragEnter);
            m_RootMain.RegisterCallback<DragLeaveEvent>(DragLeave);
            m_RootMain.RegisterCallback<DragUpdatedEvent>(DragUpdate);

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

            OnGameObjectSelectionChanged();

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

        void SetupCommandButton(ActionButton command, ChatCommandType type, int index)
        {
            command.clicked += () =>
            {
                UserSessionState.instance.SelectedCommandMode = type;
                m_ChatInput.Enable();
            };

            if (UserSessionState.instance.SelectedCommandMode == type)
                m_CommandActionGroup.SetSelectionWithoutNotify(new []{ index });
        }

        private void OnConversationPanelClicked(MouseUpEvent evt)
        {
            SetHistoryDisplay(false);
        }

        private void OnSuggestionRootClicked(MouseUpEvent evt)
        {
            SetHistoryDisplay(false);
        }

        public void Deinit()
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;

            MuseEditorDriver.instance.OnDataChanged -= OnDataChanged;
            MuseEditorDriver.instance.OnConversationTitleChanged -= OnConversationTitleChanged;
            MuseEditorDriver.instance.OnConnectionChanged -= OnConnectionChanged;
            MuseEditorDriver.instance.OnConversationHistoryChanged -= OnConversationHistoryChanged;
            Selection.selectionChanged -= OnGameObjectSelectionChanged;
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

                InternalLog.Log($"PopulateConversation took {sw.ElapsedMilliseconds}ms ({conversation.Messages.Count} Messages)");
            }
        }

        public void OnConversationTitleChanged(string title)
        {
            m_ConversationName.text = title;
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
        }

        private void HideMusingElement()
        {
            if (UserSessionState.instance.DebugUIModeEnabled)
            {
                return;
            }

            m_MusingElementRoot.SetDisplay(false);
            m_MusingElement.Stop();
        }

        private void OnHistoryClicked(PointerUpEvent evt)
        {
            MuseEditorDriver.instance.StartConversationRefresh();

            bool status = !(m_HistoryPanelRoot.style.display == DisplayStyle.Flex);
            SetHistoryDisplay(status);
        }

        private void SetHistoryDisplay(bool isVisible)
        {
            m_HistoryPanelRoot.style.display = isVisible
                ? DisplayStyle.Flex
                : DisplayStyle.None;

            UserSessionState.instance.IsHistoryOpen = isVisible;
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
            UpdateContextSelectionElements();
        }

        void OnHierarchChanged()
        {
            if (MuseEditorDriver.instance.HasNullAttachments(ObjectSelection))
                UpdateContextSelectionElements();
        }

        void OnDeleteAsset(string path)
        {
            if (m_DelayedUpdateSelectionElements)
                return;

            foreach (var obj in ObjectSelection)
            {
                if (AssetDatabase.Contains(obj))
                {
                    var assetPath = AssetDatabase.GetAssetPath(obj);

                    if (path == assetPath)
                    {
                        m_DelayedUpdateSelectionElements = true;
                        EditorApplication.delayCall += () => UpdateContextSelectionElements(true);
                    }
                }
            }
        }

        void UpdateContextSelectionElements(bool updatePopup = false)
        {
            if (updatePopup && m_CurrentSelectionPopup != null)
            {
                m_CurrentSelectionPopup.SetSelection(ObjectSelection, ConsoleSelection, false);
                m_CurrentSelectionPopup.PopulateListView();
            }

            ObjectSelection = MuseEditorDriver.instance.GetValidAttachment(ObjectSelection);

            m_SelectedContextScrollView.Clear();

            if (ObjectSelection != null)
            {
                foreach (var obj in ObjectSelection)
                {
                    var newElement = new ContextViewElement();
                    newElement.Initialize();
                    newElement.SetData(obj);

                    m_SelectedContextScrollView.Add(newElement);

                    newElement.OnRemoveButtonClicked += () =>
                    {
                        ObjectSelection.Remove(obj);
                        UpdateContextSelectionElements();
                    };
                }
            }
            if (ConsoleSelection != null)
            {
                foreach (var logRef in ConsoleSelection)
                {
                    var newElement = new ContextViewElement();
                    newElement.Initialize();
                    newElement.SetData(logRef);

                    m_SelectedContextScrollView.Add(newElement);

                    newElement.OnRemoveButtonClicked += () =>
                    {
                        ConsoleSelection.Remove(logRef);
                        UpdateContextSelectionElements();
                    };
                }
            }

            m_SelectedContextScrollView.MarkDirtyRepaint();

            UpdateSelectedContextWarning();
            UpdateMuseEditorDriverSelection();
            UpdateClearContextButton();
        }

        void UpdateSelectedContextWarning(MouseEnterEvent evt = null)
        {
            var contextBuilder = new ContextBuilder();
            MuseEditorDriver.instance.GetAttachedContextString(ref contextBuilder);
            if (contextBuilder.BuildContext(int.MaxValue).Length > MuseChatConstants.PromptContextLimit)
            {
                m_ExceedingSelectedConsoleMessageLimitRoot.style.display = DisplayStyle.Flex;
            }
            else
            {
                m_ExceedingSelectedConsoleMessageLimitRoot.style.display = DisplayStyle.None;
            }
        }

        void UpdateClearContextButton()
        {
            if (ObjectSelection != null && ObjectSelection.Count > 0
                || ConsoleSelection != null && ConsoleSelection.Count > 0)
            {
                m_ClearContextButton.style.display = DisplayStyle.Flex;
            }
            else
            {
                m_ClearContextButton.style.display = DisplayStyle.None;
            }
        }

        private void OnInspirationSelected(MuseChatInspiration inspiration)
        {
            m_ChatInput.SetText(inspiration.Value);
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

            m_MusingInProgress = state;
            if (state && MuseEditorDriver.instance.CurrentPromptState != MuseEditorDriver.PromptState.Streaming)
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
            m_InspirationPanel.SetEnabled(enabled);
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
            m_InspirationRoot.style.display = value ? DisplayStyle.Flex:DisplayStyle.None;
        }

        void OnViewGeometryChanged(GeometryChangedEvent evt)
        {
            bool isCompactView = evt.newRect.width < MuseChatConstants.CompactWindowThreshold;

            m_HistoryButton.EnableInClassList(MuseChatConstants.CompactStyle, isCompactView);
            m_NewChatButton.EnableInClassList(MuseChatConstants.CompactStyle, isCompactView);

            m_ConversationName.EnableInClassList(MuseChatConstants.CompactStyle, isCompactView);

            m_FooterRoot.EnableInClassList(MuseChatConstants.CompactStyle, isCompactView);
        }

        void OnConversationHistoryChanged()
        {
            MuseChatHistoryBlackboard.HistoryPanelReloadRequired?.Invoke();
        }

        void OnBeforeAssemblyReload()
        {
            var activeConversation = MuseEditorDriver.instance.GetActiveConversation();
            UserSessionState.instance.LastActiveConversationId = activeConversation == null
                ? null
                : activeConversation.Id.Value;
        }

        void ShowSelectionPopup()
        {
            Popover modal = null;

            var popup = new SelectionPopup();

            // Restore previous context selection
            popup.SetSelection(ObjectSelection, ConsoleSelection);

            popup.Initialize();

            popup.SetAdjustToPanel(m_RootPanel);

            modal = Popover.Build(m_RootPanel, popup);
            modal.SetAnchor(m_AddContextButton);
            modal.SetPlacement(PopoverPlacement.BottomLeft);

            m_AddContextButton.AddToClassList("mui-selected-context-button-open");
            m_AddContextButton.RemoveFromClassList("mui-selected-context-button-default-behavior");

            modal.Show();

            modal.dismissed += (Popover _, DismissType t) =>
            {
                m_AddContextButton.RemoveFromClassList("mui-selected-context-button-open");
                m_AddContextButton.AddToClassList("mui-selected-context-button-default-behavior");
            };

            popup.OnSelectionChanged += () =>
            {
                // Memorize current context selection
                ObjectSelection = popup.ObjectSelection.ToList();
                ConsoleSelection = popup.ConsoleSelection.ToList();

                UpdateContextSelectionElements();
            };

            m_CurrentSelectionPopup = popup;

            if (m_Window != null)
                m_Window.OnLostWindowFocus += () => modal.Dismiss();
        }

        void ClearContext()
        {
            ObjectSelection.Clear();
            ConsoleSelection.Clear();

            UpdateMuseEditorDriverSelection();

            UpdateContextSelectionElements();
        }

        void UpdateMuseEditorDriverSelection()
        {
            MuseEditorDriver.instance.m_ObjectAttachments = ObjectSelection;
            MuseEditorDriver.instance.m_ConsoleAttachments = ConsoleSelection;
        }

        public class TrackDeletedSelectedAssetsProcessor : AssetModificationProcessor
        {
            static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt)
            {
                m_Instance?.OnDeleteAsset(path);

                return AssetDeleteResult.DidNotDelete;
            }
        }
    }
}
