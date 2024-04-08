using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    internal class ChatElementUser : ChatElementBase
    {
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
        private Text m_UserInitials;

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

        internal static string GetUserInitialsFromName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            var nameElements = Regex.Replace(name, @"/\s+/g", " ", RegexOptions.IgnoreCase).Trim().Split(' ');

            nameElements = nameElements.Where(element => !string.IsNullOrEmpty(element) &&
                Regex.IsMatch(element[0].ToString(), @"[A-Za-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]")).ToArray() ?? new string[0];

            if (nameElements.Length > 1)
                return $"{nameElements[0][0]}{nameElements[nameElements.Length - 1][0]}".ToUpper();
            else if (nameElements.Length == 1)
                return $"{nameElements[0][0]}".ToUpper();
            return string.Empty;
        }

        string GetUserInitials()
        {
            try
            {
                var connectAssembly = typeof(CloudProjectSettings).Assembly;
                var unityConnectType = connectAssembly.GetType("UnityEditor.Connect.UnityConnect");
                var userInfoM = unityConnectType.GetMethod("GetUserInfo");
                var instance = unityConnectType.GetProperty("instance", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);

                var userInfo = userInfoM.Invoke(instance, null);

                var userInfoType = connectAssembly.GetType("UnityEditor.Connect.UserInfo");
                var displayNameProp = userInfoType.GetProperty("displayName");
                var displayName = (string)displayNameProp.GetValue(userInfo);

                var userIdProp = userInfoType.GetProperty("userId");
                var userId = (string)userIdProp.GetValue(userInfo);

                SetUserAvatar(userId);

                return GetUserInitialsFromName(displayName);
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

                    // Hide initials, we got the image:
                    m_UserInitials.style.display = DisplayStyle.None;
                }
            });
        }

        protected override void InitializeView(TemplateContainer view)
        {
            base.InitializeView(view);

            m_ChatRoot = view.Q<VisualElement>("chatRoot");

            m_EditControls = view.Q<VisualElement>("editControls");
            m_EditButton = view.SetupButton("editButton", OnEditClicked);
            m_EditCancelButton = view.SetupButton("editCancelButton", x => { SetEditMode(false); });

            m_TextFieldRoot = view.Q<VisualElement>("textFieldRoot");

            m_EditField = new MuseTextField();
            m_EditField.Initialize();
            m_EditField.ShowPlaceholder = false;
            m_EditField.HighlightFocus = false;
            m_EditField.OnSubmit += OnEditFieldSubmit;
            m_EditField.style.display = DisplayStyle.None;
            m_EditField.AddToClassList(k_EditModeRWTextFieldClass);
            m_TextFieldRoot.parent.Add(m_EditField);

            // Hide the icon until we find a way to display that:
            m_UserIcon = view.Q<AppUI.UI.Avatar>("userIcon");
            m_UserIcon.style.display = DisplayStyle.None;

            m_UserInitials = view.Q<Text>("userInitials");
            m_UserInitials.text = GetUserInitials();
        }

        static string TrimDisplayString(string s)
        {
            return s.Trim('\n', '\r');
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

            if (m_EditModeActive)
            {
                m_EditButton.style.display = DisplayStyle.None;
                m_EditCancelButton.style.display = DisplayStyle.Flex;
                m_TextFieldRoot.style.display = DisplayStyle.None;
                m_EditField.style.display = DisplayStyle.Flex;
            }
            else
            {
                m_EditButton.style.display = DisplayStyle.Flex;
                m_EditCancelButton.style.display = DisplayStyle.None;
                m_TextFieldRoot.style.display = DisplayStyle.Flex;
                m_EditField.style.display = DisplayStyle.None;
            }
        }
    }
}
