using System;
using System.Collections.Generic;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Chat.Model;
using UnityEngine;
using UnityEngine.UIElements;
using Avatar = Unity.Muse.AppUI.UI.Avatar;
using Button = Unity.Muse.AppUI.UI.Button;
using TextField = UnityEngine.UIElements.TextField;

namespace Unity.Muse.Chat
{
    internal class ChatElementResponse : ChatElementBase
    {
        private const string k_FeedbackButtonActiveClass = "mui-feedback-button-active";

        private readonly IList<VisualElement> m_TextFields = new List<VisualElement>();

        private static Texture2D k_MuseAvatarImage;

        private Accordion m_SourcesFoldout;
        private VisualElement m_SourcesContent;

        private VisualElement m_TextFieldRoot;

        private VisualElement m_OptionsSection;
        private VisualElement m_FeedbackParamSection;
        private VisualElement m_ErrorSection;

        private Button m_CopyButton;
        private Button m_UpVoteButton;
        private Button m_DownVoteButton;

        private Checkbox m_FeedbackFlagInappropriateCheckbox;
        private Dropdown m_FeedbackTypeDropdown;
        private TextField m_FeedbackText;
        private Button m_FeedbackSendButton;

        private FeedbackEditMode m_FeedbackMode = FeedbackEditMode.None;

        private MuseMessageId m_MessageId;
        static readonly int k_TextAnimationDelay = 500; // in ms
        private IVisualElementScheduledItem m_ScheduledAnim;

        private bool m_FeedbackParametersSetup = false;

        private static readonly Dictionary<MuseMessageId, int> k_AnimationIndices = new();

        private int AnimationIndex
        {
            get
            {
                if(k_AnimationIndices.TryGetValue(m_MessageId, out var index))
                {
                    return index;
                }

                k_AnimationIndices[m_MessageId] = 0;

                return 0;
            }
            set
            {
                // Don't store non-external message IDs:
                if (m_MessageId.Type == MuseMessageIdType.External)
                {
                    k_AnimationIndices[m_MessageId] = value;
                }
            }
        }

        private enum FeedbackEditMode
        {
            None,
            UpVote,
            DownVote
        }

        /// <summary>
        /// Set the data for this response chat element
        /// </summary>
        /// <param name="message">the message to display</param>
        /// <param name="id">the id of the message, used for feedback</param>
        public override void SetData(MuseMessage message)
        {
            if (message.IsError)
            {
                m_ErrorSection.style.display = DisplayStyle.Flex;

                // Override message text for errors to make it more user-friendly:
                message.Content = ErrorTranslator.GetErrorMessage(message.ErrorCode, message.ErrorText, message.Content);
            }

            if (m_MessageId != message.Id)
            {
                if (k_AnimationIndices.ContainsKey(m_MessageId))
                {
                    k_AnimationIndices.Remove(m_MessageId);
                }
            }

            m_MessageId = message.Id;

            base.SetData(message);

            m_FeedbackMode = FeedbackEditMode.None;

            if (message.Id.Type == MuseMessageIdType.Internal || message.IsError)
            {
                m_OptionsSection.style.display = DisplayStyle.None;
            }

            RefreshText(m_TextFieldRoot, m_TextFields);
            RefreshSourceBlocks();
            RefreshFeedbackParameters();

            // Cancel any active animations:
            if (m_ScheduledAnim != null)
            {
                m_ScheduledAnim.Pause();
                m_ScheduledAnim = null;
            }

            // Schedule update to animate text for incomplete messages:
            if (!message.IsComplete)
            {
                GetAnimationInfo(message.Content, out var remainingSpaces, out _);
                var delay = k_TextAnimationDelay / Math.Max(1, remainingSpaces);
                m_ScheduledAnim = schedule.Execute(() =>
                {
                    SetData(message);
                }).StartingIn(delay);
            }

            RemoveCompleteMessageFromAnimationDictionary();
        }

        public override void Reset()
        {
            // If the message is complete, set the animation index to a high value to start the next animation at the last space:
            if (!Message.IsComplete)
            {
                AnimationIndex = int.MaxValue;
            }
        }

        private void RemoveCompleteMessageFromAnimationDictionary()
        {
            // No need to keep complete messages in animation data dictionary:
            if (Message.IsComplete && k_AnimationIndices.ContainsKey(m_MessageId))
            {
                k_AnimationIndices.Remove(m_MessageId);
            }
        }

        void GetAnimationInfo(string message, out int remainingSpaces, out int nextSpace)
        {
            if (string.IsNullOrEmpty(message))
            {
                nextSpace = 0;
                remainingSpaces = 0;
                return;
            }

            AnimationIndex = Math.Min(AnimationIndex, message.Length - 1);
            nextSpace = message.IndexOf(' ', AnimationIndex);

            remainingSpaces = 0;
            if (nextSpace > 0)
            {
                remainingSpaces = 0;
                for (var i = nextSpace + 1; i < message.Length; i++)
                {
                    if (message[i] == ' ') remainingSpaces++;
                }

                remainingSpaces = Math.Max(1, remainingSpaces);
            }
        }

        protected override string GetAnimatedMessage(string message)
        {
            if (message.Length > 0)
            {
                GetAnimationInfo(message, out _, out var nextSpace);

                if (nextSpace > 0)
                {
                    AnimationIndex = nextSpace + 1;
                }

                message = message.Substring(0, AnimationIndex);
            }

            return message;
        }

        protected override void InitializeView(TemplateContainer view)
        {
            LoadSharedAsset("icons/muse.png", ref k_MuseAvatarImage);
            view.Q<Avatar>("museAvatar").src = Background.FromTexture2D(k_MuseAvatarImage);

            m_TextFieldRoot = view.Q<VisualElement>("textFieldRoot");

            m_SourcesFoldout = view.Q<Accordion>("sourcesFoldout");
            m_SourcesContent = view.Q<VisualElement>("sourcesContent");

            m_OptionsSection = view.Q<VisualElement>("optionsSection");
            m_CopyButton = view.SetupButton("copyButton", OnCopyClicked);
            m_UpVoteButton = view.SetupButton("upVoteButton", OnUpvoteClicked);
            m_DownVoteButton = view.SetupButton("downVoteButton", OnDownvoteClicked);

            m_FeedbackParamSection = view.Q<VisualElement>("feedbackParamSection");

            m_ErrorSection = view.Q<VisualElement>("errorFrame");
            m_ErrorSection.style.display = DisplayStyle.None;
        }

        private void SetupFeedbackParameters()
        {
            if (m_FeedbackParametersSetup)
            {
                return;
            }

            m_FeedbackParametersSetup = true;
            m_FeedbackFlagInappropriateCheckbox = m_FeedbackParamSection.Q<Checkbox>("feedbackFlagCheckbox");
            m_FeedbackTypeDropdown = m_FeedbackParamSection.SetupEnumDropdown<Category>("feedbackType", GetFeedbackTypeDisplayString);
            m_FeedbackTypeDropdown.RegisterValueChangedCallback(_ => CheckFeedbackState());

            m_FeedbackText = m_FeedbackParamSection.Q<TextField>("feedbackValueText");
            m_FeedbackText.multiline = true;
            m_FeedbackText.maxLength = MuseChatConstants.MaxFeedbackMessageLength;
            m_FeedbackText.RegisterValueChangedCallback(_ => CheckFeedbackState());
            m_FeedbackSendButton = m_FeedbackParamSection.SetupButton("feedbackSendButton", OnSendFeedback);
        }

        private string GetFeedbackTypeDisplayString(Category type)
        {
            switch (type)
            {
                case Category.ResponseQuality: return "Response quality";
                case Category.CodeGeneration: return "Code generation";
                case Category.SpeedToResponse: return "Speed of response";
                case Category.Sources: return "Sources";
                case Category.AdditionalResources: return "Additional resources";
                default: return type.ToString();
            }
        }

        private void CheckFeedbackState()
        {
            if (string.IsNullOrEmpty(m_FeedbackText.value))
            {
                m_FeedbackSendButton.SetEnabled(false);
            }
            else
            {
                m_FeedbackSendButton.SetEnabled(true);
            }
        }

        private bool GetSelectedFeedbackType(out Category type)
        {
            if (m_FeedbackTypeDropdown.selectedIndex < 0 ||
                m_FeedbackTypeDropdown.selectedIndex >= EnumDef<Category>.Count)
            {
                type = default;
                return false;
            }

            type = EnumDef<Category>.Values[m_FeedbackTypeDropdown.selectedIndex];
            return true;
        }

        private void OnSendFeedback(PointerUpEvent evt)
        {
            if (string.IsNullOrEmpty(m_FeedbackText.value))
            {
                MuseChatView.ShowNotification($"Failed to send Feedback: 'your feedback' section is empty", PopNotificationIconType.Error);
                return;
            }
            string message = m_FeedbackText.value.Trim();

            if(!GetSelectedFeedbackType(out var type))
            {
                MuseChatView.ShowNotification($"Failed to send Feedback: 'Add a feedback category' requires a category to be selected", PopNotificationIconType.Error);
                return;
            }

            if (m_FeedbackMode != FeedbackEditMode.DownVote && m_FeedbackMode != FeedbackEditMode.UpVote)
            {
                MuseChatView.ShowNotification($"Failed to send Feedback: Sentiment must be set", PopNotificationIconType.Error);
                return;
            }

            if (m_FeedbackFlagInappropriateCheckbox.value == CheckboxState.Checked)
            {
                message += " (Message was flagged as inappropriate.)";
            }

            var feedback = new MessageFeedback
            {
                MessageId = Id,
                FlagInappropriate = m_FeedbackFlagInappropriateCheckbox.value == CheckboxState.Checked,
                Type = type,
                Message = message,
                Sentiment = m_FeedbackMode == FeedbackEditMode.UpVote
                    ? Sentiment.Positive
                    : Sentiment.Negative
            };

            MuseEditorDriver.instance.SendFeedback(feedback);
            m_FeedbackMode = FeedbackEditMode.None;
            ClearFeedbackParameters();

            MuseChatView.ShowNotification("Feedback sent", PopNotificationIconType.Info);
        }

        private void ClearFeedbackParameters()
        {
            m_FeedbackTypeDropdown.value = default;

            m_FeedbackTypeDropdown.Q<LocalizedTextElement>("appui-dropdown-item__label")
                .text = "Select";

            m_FeedbackFlagInappropriateCheckbox.value = CheckboxState.Unchecked;
            m_FeedbackText.value = string.Empty;
            RefreshFeedbackParameters();
        }

        private void OnDownvoteClicked(PointerUpEvent evt)
        {
            if (m_FeedbackMode == FeedbackEditMode.DownVote)
            {
                m_FeedbackMode = FeedbackEditMode.None;
                RefreshFeedbackParameters();
                return;
            }

            m_FeedbackMode = FeedbackEditMode.DownVote;
            RefreshFeedbackParameters();
        }

        private void OnUpvoteClicked(PointerUpEvent evt)
        {
            if (m_FeedbackMode == FeedbackEditMode.UpVote)
            {
                m_FeedbackMode = FeedbackEditMode.None;
                RefreshFeedbackParameters();
                return;
            }

            m_FeedbackMode = FeedbackEditMode.UpVote;
            RefreshFeedbackParameters();
            m_FeedbackFlagInappropriateCheckbox.value = CheckboxState.Unchecked;
        }

        private void OnCopyClicked(PointerUpEvent evt)
        {
            string disclaimerHeader = string.Format(MuseChatConstants.DisclaimerText, DateTime.Now.ToShortDateString());

            // Format message with footnotes (indices to sources)
            IList<WebAPI.SourceBlock> sourceBlocks = new List<WebAPI.SourceBlock>();
            MessageUtils.ProcessText(Message, ref sourceBlocks, out var outMessage,
                MessageUtils.FootnoteFormat.SimpleIndexForClipboard);

            // Add sources in same order of footnote indices
            MessageUtils.AppendSourceBlocks(sourceBlocks, ref outMessage);

            GUIUtility.systemCopyBuffer = string.Concat(disclaimerHeader, outMessage);

            MuseChatView.ShowNotification("Copied to clipboard", PopNotificationIconType.Info);
        }

        private void RefreshSourceBlocks()
        {
            if (Message.IsError || !Message.IsComplete || SourceBlocks == null || SourceBlocks.Count == 0)
            {
                m_SourcesFoldout.style.display = DisplayStyle.None;
                return;
            }

            m_SourcesFoldout.style.display = DisplayStyle.Flex;
            m_SourcesContent.Clear();

            for (var index = 0; index < SourceBlocks.Count; index++)
            {
                var sourceBlock = SourceBlocks[index];
                var entry = new ChatElementSourceEntry();
                entry.Initialize();
                entry.SetData(index, sourceBlock);
                m_SourcesContent.Add(entry);
            }
        }

        private void RefreshFeedbackParameters()
        {
            if (Message.IsError || !Message.IsComplete)
            {
                m_UpVoteButton.SetEnabled(false);
                m_DownVoteButton.SetEnabled(false);
                m_FeedbackParamSection.style.display = DisplayStyle.None;
                return;
            }

            m_UpVoteButton.SetEnabled(true);
            m_DownVoteButton.SetEnabled(true);

            switch (m_FeedbackMode)
            {
                case FeedbackEditMode.None:
                {
                    m_FeedbackParamSection.style.display = DisplayStyle.None;
                    m_UpVoteButton.RemoveFromClassList(k_FeedbackButtonActiveClass);
                    m_DownVoteButton.RemoveFromClassList(k_FeedbackButtonActiveClass);
                    return;
                }

                case FeedbackEditMode.DownVote:
                {
                    SetupFeedbackParameters();
                    m_FeedbackParamSection.style.display = DisplayStyle.Flex;
                    m_FeedbackFlagInappropriateCheckbox.style.display = DisplayStyle.Flex;
                    m_UpVoteButton.RemoveFromClassList(k_FeedbackButtonActiveClass);
                    m_DownVoteButton.AddToClassList(k_FeedbackButtonActiveClass);
                    break;
                }

                case FeedbackEditMode.UpVote:
                {
                    SetupFeedbackParameters();
                    m_FeedbackParamSection.style.display = DisplayStyle.Flex;
                    m_FeedbackFlagInappropriateCheckbox.style.display = DisplayStyle.None;
                    m_UpVoteButton.AddToClassList(k_FeedbackButtonActiveClass);
                    m_DownVoteButton.RemoveFromClassList(k_FeedbackButtonActiveClass);
                    break;
                }
            }
        }
    }
}
