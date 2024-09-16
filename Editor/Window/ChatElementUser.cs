using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Common.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    internal class ChatElementUser : ChatElementBase
    {
        private const string k_ConnectAssembly = "UnityEditor.Connect.UnityConnect";
        private const string k_UserInfoType = "UnityEditor.Connect.UserInfo";
        private const string k_UserInfoMethod = "GetUserInfo";
        private const string k_UserInstanceProperty = "instance";
        private const string k_UserInfoDisplayNameProperty = "displayName";
        private const string k_UserInfoIdProperty = "userId";

        private const string k_EditModeActiveClass = "mui-um-edit-mode-active";
        private const string k_EditModeRWTextFieldClass = "mui-user-edit-read-write-field";

        private readonly IList<VisualElement> m_TextFields = new List<VisualElement>();

        private VisualElement m_ChatRoot;

        private VisualElement m_EditControls;
        private Button m_EditButton;
        private Button m_EditCancelButton;

        private VisualElement m_TextFieldRoot;
        private MuseTextField m_EditField;
        private AppUI.UI.Avatar m_UserIcon;
        private Text m_UserName;
        private Accordion m_ContextFoldout;
        private VisualElement m_ContextContent;

        private bool m_EditEnabled = true;
        private bool m_EditModeActive;

        public bool EditEnabled
        {
            get => m_EditEnabled;
            set
            {
                if (m_EditEnabled == value)
                {
                    return;
                }

                m_EditEnabled = value;

                if(!m_EditEnabled)
                {
                    SetEditMode(false);
                }

                RefreshUI();
            }
        }

        /// <summary>
        /// Set the user data used by this element
        /// </summary>
        /// <param name="message">the message to display</param>
        /// <param name="id">id of the message, used for edit callbacks</param>
        public override void SetData(MuseMessage message)
        {
            base.SetData(message);

            EditEnabled = true;

            RefreshText(m_TextFieldRoot, m_TextFields);
            RefreshUI();
        }

        string GetUserName()
        {
            try
            {
                var connectAssembly = TypeDef<CloudProjectSettings>.Assembly;
                var unityConnectType = connectAssembly.GetType(k_ConnectAssembly);
                var userInfoMethod = unityConnectType.GetMethod(k_UserInfoMethod);
                var instanceProperty = unityConnectType.GetProperty(k_UserInstanceProperty, BindingFlags.Public | BindingFlags.Static);
                var instance = instanceProperty.GetValue(null, null);

                var userInfo = userInfoMethod.Invoke(instance, null);

                var userInfoType = connectAssembly.GetType(k_UserInfoType);
                var displayNameProp = userInfoType.GetProperty(k_UserInfoDisplayNameProperty);
                var displayName = (string)displayNameProp.GetValue(userInfo);

                var userIdProp = userInfoType.GetProperty(k_UserInfoIdProperty);
                var userId = (string)userIdProp.GetValue(userInfo);

                SetUserAvatar(userId);
                return displayName;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            return string.Empty;
        }

        void SetUserAvatar(string userId)
        {
            UserAvatarHelper.GetUserAvatar(userId, (icon) =>
            {
                if (icon != null)
                {
                    m_UserIcon.style.display = DisplayStyle.Flex;

                    m_UserIcon.src = Background.FromTexture2D(icon);
                }
            });
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_ChatRoot = view.Q<VisualElement>("chatRoot");

            m_ContextFoldout = view.Q<Accordion>("contextFoldout");
            m_ContextFoldout.RealignFoldoutIcon();

            m_ContextContent = view.Q<VisualElement>("contextContent");

            m_EditControls = view.Q<VisualElement>("editControls");
            m_EditButton = view.SetupButton("editButton", OnEditClicked);
            m_EditCancelButton = view.SetupButton("editCancelButton", x => { SetEditMode(false); });

            m_TextFieldRoot = view.Q<VisualElement>("userMessageTextFieldRoot");

            // Hide the icon until we find a way to display that:
            m_UserIcon = view.Q<AppUI.UI.Avatar>("userIcon");
            m_UserIcon.style.display = DisplayStyle.None;

            m_UserName = view.Q<Text>("userName");
            m_UserName.text = GetUserName();
        }

        static string TrimDisplayString(string s)
        {
            return s.Trim('\n', '\r');
        }

        private void InitializeEditField()
        {
            if (m_EditField != null)
            {
                return;
            }

            m_EditField = new MuseTextField();
            m_EditField.Initialize();
            m_EditField.ShowPlaceholder = false;
            m_EditField.HighlightFocus = false;
            m_EditField.OnSubmit += OnEditFieldSubmit;
            m_EditField.style.display = DisplayStyle.None;
            m_EditField.AddToClassList(k_EditModeRWTextFieldClass);
            m_TextFieldRoot.parent.Add(m_EditField);
        }

        private void OnEditFieldSubmit(string value)
        {
            if (!m_EditModeActive)
            {
                return;
            }

            value = TrimDisplayString(value);

            string changedText = value;
            if (changedText != TrimDisplayString(Message.Content))
            {
                MuseEditorDriver.instance.ProcessEditPrompt(changedText, Message.Id);
            }

            SetEditMode(false);
        }

        private void OnEditClicked(PointerUpEvent evt)
        {
            SetEditMode(!m_EditModeActive);
        }

        private void SetEditMode(bool state)
        {
            // Never allow going into edit state if editing is disabled:
            if (!EditEnabled)
            {
                state = false;
            }

            InitializeEditField();

            m_EditModeActive = state;
            if (m_EditModeActive)
            {
                // Editing started, we only show the first chunk of the message
                m_EditField.SetText(TrimDisplayString(MessageChunks[0]));
            }
            else
            {
                // Clear out the edit field to save some memory, message could be large
                m_EditField.ClearText();
            }

            RefreshUI();
        }

        private void RefreshUI()
        {
            m_EditControls.style.display = EditEnabled ? DisplayStyle.Flex : DisplayStyle.None;
            m_ChatRoot.EnableInClassList(k_EditModeActiveClass, m_EditModeActive);

            m_EditButton.SetDisplay(!m_EditModeActive);
            m_EditCancelButton.SetDisplay(m_EditModeActive);
            m_TextFieldRoot.SetDisplay(!m_EditModeActive);

            if (m_EditField != null)
            {
                m_EditField.SetDisplay(m_EditModeActive);
            }

            RefreshContext();
        }

        private void RefreshContext()
        {
            if (ContextEntries == null || ContextEntries.Count == 0)
            {
                m_ContextFoldout.style.display = DisplayStyle.None;
                return;
            }

            m_ContextFoldout.style.display = DisplayStyle.Flex;

            m_ContextContent.Clear();
            for (var index = 0; index < ContextEntries.Count; index++)
            {
                var contextEntry = ContextEntries[index];
                var entry = new ChatElementContextEntry();
                entry.Initialize();
                entry.SetData(index, contextEntry);
                m_ContextContent.Add(entry);
            }
        }
    }
}
