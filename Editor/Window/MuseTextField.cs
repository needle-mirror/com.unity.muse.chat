using System;
using Unity.Muse.AppUI.UI;
using UnityEngine;
using UnityEngine.UIElements;
using TextField = UnityEngine.UIElements.TextField;

namespace Unity.Muse.Chat
{
    internal class MuseTextField : ManagedTemplate
    {
        const string k_ChatFocusClass = "mui-mft-input-focused";
        const string k_ChatHoverClass = "mui-mft-input-hovered";
        const string k_ScrollVisibleClass = "mui-mft-scroll-active";
        const string k_MusingActiveStyle = "mui-musing-active";
        const string k_ChatSubmitEnabledClass = "mui-submit-enabled";

        const string k_DefaultAskPlaceholderText = "Ask Muse";
        const string k_DefaultRunPlaceholderText = "Run a command";
        const string k_DefaultCodePlaceholderText = "Use a dedicated code generator";

        VisualElement m_Root;

        Icon m_ChatInputIconDefault;
        Icon m_ChatInputIconMusing;
        VisualElement m_SubmitButton;

        ScrollView m_InputScrollView;
        TextField m_ChatInput;
        Text m_ChatCharCount;
        Text m_Placeholder;
        VisualElement m_PlaceholderContent;

        bool m_TextHasFocus;
        bool m_ShowPlaceholder;
        bool m_HighlightFocus;
        bool m_IsMusing;

        public MuseTextField()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public bool HasFocus => m_TextHasFocus;

        public bool ShowPlaceholder
        {
            get => m_ShowPlaceholder;
            set
            {
                if (m_ShowPlaceholder == value)
                {
                    return;
                }

                m_ShowPlaceholder = value;
                RefreshUI();
            }
        }

        public bool HighlightFocus
        {
            get => m_HighlightFocus;
            set
            {
                if (m_HighlightFocus == value)
                {
                    return;
                }

                m_HighlightFocus = value;
                RefreshUI();
            }
        }

        public event Action<string> OnSubmit;

        public void ClearText()
        {
            m_ChatInput.SetValueWithoutNotify(string.Empty);
        }

        public void SetText(string text)
        {
            m_ChatInput.SetValueWithoutNotify(text);
            m_ChatInput.Focus();
        }

        public void SetMusingState(bool isMusing)
        {
            m_IsMusing = isMusing;
            RefreshUI();
        }

        public void Enable()
        {
            switch (UserSessionState.instance.SelectedCommandMode)
            {
                case ChatCommandType.Ask:
                    m_Placeholder.text = k_DefaultAskPlaceholderText;
                    break;
                case ChatCommandType.Run:
                    m_Placeholder.text = k_DefaultRunPlaceholderText;
                    break;
                case ChatCommandType.Code:
                    m_Placeholder.text = k_DefaultCodePlaceholderText;
                    break;
            }
            m_ChatInput.SetEnabled(true);
        }

        public void Disable(string reason)
        {
            m_Placeholder.text = reason;
            m_ChatInput.SetEnabled(false);
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Root = view.Q<VisualElement>("textFieldRoot");
            m_Root.RegisterCallback<PointerUpEvent>(_ =>
            {
                if(!m_TextHasFocus) { m_ChatInput.Focus(); }
            });

            m_SubmitButton = view.Q<VisualElement>("submitButton");
            m_SubmitButton.RegisterCallback<PointerUpEvent>(_ => OnSubmit?.Invoke(m_ChatInput.value));

            m_ChatInputIconDefault = view.Q<Icon>("submitIconDefault");
            m_ChatInputIconMusing = view.Q<Icon>("submitIconMusing");

            m_InputScrollView = view.Q<ScrollView>("inputScrollView");

            m_ChatInput = view.Q<TextField>("input");
            m_ChatInput.maxLength = MuseChatConstants.MaxMuseMessageLength;
            m_ChatInput.multiline = true;
            m_ChatInput.selectAllOnFocus = false;
            m_ChatInput.RegisterCallback<KeyUpEvent>(OnChatKeyEvent);
            m_ChatInput.RegisterCallback<KeyDownEvent>(OnChatKeyDownEvent);
            m_ChatInput.RegisterValueChangedCallback(OnChatValueChanged);
            m_ChatInput.RegisterCallback<FocusInEvent>(_ => SetTextFocused(true));
            m_ChatInput.RegisterCallback<FocusOutEvent>(_ => SetTextFocused(false));
            m_ChatInput.RegisterCallback<PointerEnterEvent>(_ => m_SubmitButton.AddToClassList(k_ChatHoverClass));
            m_ChatInput.RegisterCallback<PointerLeaveEvent>(_ => m_SubmitButton.RemoveFromClassList(k_ChatHoverClass));
            m_ChatInput.RegisterCallback<GeometryChangedEvent>(OnInputGeometryChanged);

            m_PlaceholderContent = view.Q<VisualElement>("placeholderContent");
            m_Placeholder = view.Q<Text>("placeholderText");

            m_ChatCharCount = view.Q<Text>("characterCount");

            RefreshChatCharCount();
            ShowPlaceholder = true;
            HighlightFocus = true;
            SetMusingState(false);
        }

        private void OnInputGeometryChanged(GeometryChangedEvent evt)
        {
            m_SubmitButton.EnableInClassList(k_ScrollVisibleClass, m_InputScrollView.verticalScroller.style.display != DisplayStyle.None);
        }

        private void SetTextFocused(bool state)
        {
            m_TextHasFocus = state;
            RefreshUI();
        }

        private void RefreshUI()
        {
            if (!ShowPlaceholder || m_TextHasFocus || !string.IsNullOrEmpty(m_ChatInput.value))
            {
                m_PlaceholderContent.style.display = DisplayStyle.None;
            }
            else
            {
                m_PlaceholderContent.style.display = DisplayStyle.Flex;
            }

            m_Root.EnableInClassList(k_ChatFocusClass, m_TextHasFocus && m_HighlightFocus);
            m_SubmitButton.EnableInClassList(k_ChatFocusClass, m_TextHasFocus);
            m_ChatInputIconDefault.EnableInClassList(k_ChatFocusClass, m_TextHasFocus);
            m_ChatInputIconMusing.EnableInClassList(k_ChatFocusClass, m_TextHasFocus);

            m_SubmitButton.EnableInClassList(k_MusingActiveStyle, m_IsMusing);
            m_ChatInputIconDefault.EnableInClassList(k_MusingActiveStyle, m_IsMusing);
            m_ChatInputIconMusing.EnableInClassList(k_MusingActiveStyle, m_IsMusing);

            if (m_IsMusing)
            {
                m_ChatInputIconDefault.style.display = DisplayStyle.None;
                m_ChatInputIconMusing.style.display = DisplayStyle.Flex;
            }
            else
            {
                m_ChatInputIconDefault.style.display = DisplayStyle.Flex;
                m_ChatInputIconMusing.style.display = DisplayStyle.None;
            }
        }

        private void OnChatValueChanged(ChangeEvent<string> evt)
        {
            RefreshChatCharCount();
            m_SubmitButton.EnableInClassList(k_ChatSubmitEnabledClass, string.IsNullOrEmpty(m_ChatInput.value));
        }

        private void RefreshChatCharCount()
        {
            m_ChatCharCount.text = $"{m_ChatInput.value.Length.ToString()}/{MuseChatConstants.MaxMuseMessageLength}";
        }

        void OnChatKeyDownEvent(KeyDownEvent evt)
        {
            // Shift + enter adds a line break.
            // We get 2 similar events when the user presses shift+enter and want to stop both from propagating but only add one line break!
            if (evt.shiftKey &&
                (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter || evt.character == '\n'))
            {
                if (evt.character == '\n')
                {
                    var isAtEnd = m_ChatInput.cursorIndex == m_ChatInput.value.Length;

                    m_ChatInput.SetValueWithoutNotify(m_ChatInput.value.Insert(m_ChatInput.cursorIndex, "\n"));
                    m_ChatInput.cursorIndex += 1;

                    if (isAtEnd)
                    {
                        m_ChatInput.selectIndex = m_ChatInput.cursorIndex + 1;
                    }
                    else
                    {
                        m_ChatInput.selectIndex = m_ChatInput.cursorIndex;
                    }
                }

                evt.StopPropagation();

#if !UNITY_2023_1_OR_NEWER
                evt.PreventDefault();
#endif
            }
            else if (evt.character == '\n')
            {
                // Don't do default behaviour of adding a new line with return:
                evt.StopPropagation();

#if !UNITY_2023_1_OR_NEWER
                evt.PreventDefault();
#endif
            }
        }

        private void OnChatKeyEvent(KeyUpEvent evt)
        {
            RefreshChatCharCount();

            switch (evt.keyCode)
            {
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                {
                    break;
                }

                default:
                {
                    return;
                }
            }

            if (evt.ctrlKey || evt.altKey || evt.shiftKey)
            {
                return;
            }

            evt.StopPropagation();

            // Do not allow submit while waiting for a response:
            if (!m_IsMusing)
            {
                OnSubmit?.Invoke(m_ChatInput.value);
            }
        }
    }
}
