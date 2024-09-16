using System;
using System.Collections.Generic;
using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Unity.Muse.Chat
{
    internal class MusingElement : ManagedTemplate
    {
        private const string k_MusingProgressTime = "MUSING_PROGRESS_TIME";

        private float k_ProgressTime = SessionState.GetFloat(k_MusingProgressTime, 10);

        private ProgressBar m_ProgressBar;
        private VisualElement m_ProgressBarOverlay;
        private Text m_Message;
        private bool m_Running;
        private ValueAnimation<float> m_ProgressAnimation;

        private const string k_ProgressCompleteMessage = "Almost ready";
        private const string k_MusingMessage = "Musing";
        private const string k_ProcessingMessage = "Processing request";
        private const string k_AnalyzingMessage = "Analyzing project context";
        private const string k_RunningMessage = "Running command";
        private const string k_RefiningMessage = "Refining code";
        private const string k_ReattemptingMessage = "Reattempting";

        private readonly string[] k_DefaultStateStrings = { k_MusingMessage, k_ProcessingMessage, k_AnalyzingMessage };
        private readonly string[] k_RunExecutingStateStrings = { k_MusingMessage, k_RunningMessage };
        private readonly string[] k_CodeRepairStateStrings = { k_MusingMessage, k_RefiningMessage };

        public MusingElement()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_ProgressBar = view.Q<ProgressBar>("musingProgressBar");
            m_Message = view.Q<Text>("musingProgressMessage");

            m_ProgressBarOverlay = view.Q(className: "unity-progress-bar__progress");

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void UpdateMessage()
        {
            // TODO: Get this from the active conversation:
            var commandMode = UserSessionState.instance.SelectedCommandMode;

            string message;
            var progress = GetProgress();
            if (progress >= 100)
            {
                message = k_ProgressCompleteMessage;
            }
            else
            {
                string[] stringsForState = k_DefaultStateStrings;
                switch (commandMode)
                {
                    case ChatCommandType.Run:
                        // TODO: How to check if we're executing a command?
                        // stringsForState = k_RunExecutingStateStrings;
                        break;
                    case ChatCommandType.Code:
                        // TODO: How to check if we're refining code?
                        // stringsForState = k_CodeRepairStateStrings;
                        // TODO: Check if code did not compile and do this:
                        // message = k_ReattemptingMessage
                        break;
                }

                var index = Math.Min(stringsForState.Length, (int)(progress / 100f * stringsForState.Length));
                message = stringsForState[index];
            }

            m_Message.text = message;
        }

        public void Start()
        {
            if (m_Running)
            {
                return;
            }

            m_Running = true;
            m_ProgressBar.value = 0;

            EditorApplication.update += UpdateProgress;
            AnimateProgressBar();
            UpdateProgress();
        }

        public void Stop()
        {
            if (!m_Running)
            {
                return;
            }

            m_Running = false;
            EditorApplication.update -= UpdateProgress;

            // If the response started streaming, update progress time for next time:
            if (MuseEditorDriver.instance.CurrentPromptState == MuseEditorDriver.PromptState.Streaming)
            {
                var startTime = MuseEditorDriver.instance.GetActiveConversation().StartTime;

                var timeTaken = (float)(EditorApplication.timeSinceStartup - startTime);
                // Never let it get too big, could be delayed if there are breakpoints or other long running tasks:
                k_ProgressTime = Mathf.Min(100, (k_ProgressTime + timeTaken) / 2);

                SessionState.SetFloat(k_MusingProgressTime, k_ProgressTime);
            }

            if (m_ProgressAnimation != null)
            {
                m_ProgressAnimation.Stop();
                m_ProgressAnimation = null;

                // Reset progress bar offset:
                var pos = new StyleBackgroundPosition(new BackgroundPosition(BackgroundPositionKeyword.Left));
                var p = new BackgroundPosition { keyword = BackgroundPositionKeyword.Left };
                p.offset.value = 0;
                pos.value = p;
                m_ProgressBarOverlay.style.backgroundPositionX = pos;
            }
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            Stop();
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
        }

        private void UpdateProgress()
        {
            UpdateMessage();

            m_ProgressBar.value = Mathf.Min(100, GetProgress());
        }

        private float GetProgress()
        {
            var conversation = MuseEditorDriver.instance.GetActiveConversation();
            if (conversation == null)
            {
                return 0;
            }
             
            var startTime = conversation.StartTime;
            // Init the start time if needed:
            if (startTime < 1)
            {
                startTime = EditorApplication.timeSinceStartup;
                conversation.StartTime = startTime;
            }

            var deltaTime = EditorApplication.timeSinceStartup - startTime;

            return (float)(deltaTime / k_ProgressTime) * 100;
        }

        void AnimateProgressBar()
        {
            var x = m_ProgressBarOverlay.style.backgroundPositionX.value.offset.value;

            m_ProgressAnimation = m_ProgressBarOverlay.experimental.animation.Start(x, x + 1000, 10000, (element, f) =>
            {
                var pos = new StyleBackgroundPosition(new BackgroundPosition(BackgroundPositionKeyword.Left));
                var p = new BackgroundPosition { keyword = BackgroundPositionKeyword.Left };
                p.offset.value = f;
                pos.value = p;
                m_ProgressBarOverlay.style.backgroundPositionX = pos;
            }).Ease(Easing.Linear).OnCompleted(AnimateProgressBar);
        }
    }
}