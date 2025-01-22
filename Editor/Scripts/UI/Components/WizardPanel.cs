using System;
using System.IO;
using System.Reflection;
using Unity.Muse.Chat.UI.Components;
using Unity.Muse.Common.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using UnityEngine.Video;
using Object = UnityEngine.Object;

namespace Unity.Muse.Chat.UI
{
    abstract class WizardPanelBase : ManagedTemplate
    {
        internal event Action<WizardPanelBase> OnClose;

        internal WizardPanelBase()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        protected virtual void Close()
        {
            parent.Remove(this);
            OnClose?.Invoke(this);
        }
    }

    abstract class WizardPanel<T> : WizardPanelBase
    {
        private VisualElement m_CurrentPage;

        private VideoPlayer m_VideoPlayer;
        private IVisualElementScheduledItem m_CurrentVideoAnim;
        private bool m_ShouldPlayVideo;
        private VisualElement m_CurrentPageVideoElement;
        private string m_CurrentPageVideoFilename;


        public static string WizardSeenPrefsKey()
        {
            return
                $"MuseChat_HasSeenWizard_{typeof(T).Name}";
        }

        protected override void Close()
        {
            EditorPrefs.SetBool(WizardSeenPrefsKey(), true);

            base.Close();
        }

        void InitVideoPlayer()
        {
            if (m_VideoPlayer != null)
            {
                return;
            }

            var videoPlayerGo = new GameObject("MuseWizardVideoPlayer")
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            m_VideoPlayer = videoPlayerGo.AddComponent<VideoPlayer>();
            m_VideoPlayer.renderMode = VideoRenderMode.RenderTexture;
            m_VideoPlayer.isLooping = false;
            m_VideoPlayer.playOnAwake = false;
            m_VideoPlayer.audioOutputMode = VideoAudioOutputMode.None;

            m_VideoPlayer.Play();

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            m_ShouldPlayVideo = true;
            if (m_CurrentVideoAnim != null)
            {
                return;
            }

            PlayVideo();
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            DestroyVideoPlayer();
        }

        protected void DestroyVideoPlayer()
        {
            m_ShouldPlayVideo = false;

            m_CurrentVideoAnim?.Pause();
            m_CurrentVideoAnim = null;

            if (m_VideoPlayer != null)
            {
                RenderTexture.ReleaseTemporary(m_VideoPlayer.targetTexture);
                Object.DestroyImmediate(m_VideoPlayer.gameObject);
                m_VideoPlayer = null;
            }
        }

        protected override void InitializeView(TemplateContainer view)
        {
            AddToClassList("muse-wizard");
            pickingMode = PickingMode.Position;
        }

        public override void Show(bool sendVisibilityChanged = true)
        {
            base.Show(sendVisibilityChanged);

            // Set up next page buttons:
            var nextPageButtons = contentContainer.Query<VisualElement>("nextPageButton");
            foreach (var nextPageButton in nextPageButtons.Build())
            {
                nextPageButton.RegisterCallback<PointerUpEvent>(_ => { ShowPage(1); });
            }

            // Set up next previous buttons:
            var previousPageButtons = contentContainer.Query<VisualElement>("previousPageButton");
            foreach (var previousPageButton in previousPageButtons.Build())
            {
                previousPageButton.RegisterCallback<PointerUpEvent>(_ => { ShowPage(-1); });
            }

            // Set up preview buttons:
            var previewButtons = contentContainer.Query<VisualElement>(className: "mui-wizard-page-sample-button");
            foreach (var previewButton in previewButtons.Build())
            {
                var question =
                    previewButton.parent.parent.Q<Label>(className: "mui-wizard-page-sample-text-with-button").text;
                previewButton.RegisterCallback<PointerUpEvent>(_ =>
                {
#if ENABLE_ASSISTANT_BETA_FEATURES
                    PreviewButtonPressed(question, ChatCommandType.Run);
#endif
                });
            }

            ShowPage(1);
        }

        protected void PreviewButtonPressed(string prompt, ChatCommandType chatCommandType)
        {
            var prefix = chatCommandType switch
            {
                ChatCommandType.Ask => "/ask ",
#if ENABLE_ASSISTANT_BETA_FEATURES
                ChatCommandType.Run => "/run ",
                ChatCommandType.Code => "/code ",
#endif
                _ => ""
            };

            Assistant.instance.ClearForNewConversation();
            _ = Assistant.instance.ProcessPrompt(prefix + prompt);
            Close();
        }

        protected virtual void ShowPage(int offset)
        {
            DestroyVideoPlayer();

            var pages = contentContainer.Query<VisualElement>(className: "mui-wizard-page").ToList();
            // Hide all pages:
            foreach (var page in pages)
            {
                page.SetDisplay(false);
            }

            var current = pages.IndexOf(m_CurrentPage);
            var pageToShow = current + offset;
            m_CurrentPage = null;
            for (var i = 0; i < pages.Count; i++)
            {
                var page = pages[i];

                if (i == pageToShow)
                {
                    m_CurrentPage = page;
                    page.style.display = DisplayStyle.Flex;
                }
                else
                {
                    page.style.display = DisplayStyle.None;
                }
            }

            if (m_CurrentPage != null)
            {
                m_ShouldPlayVideo = true;
                PlayVideo();
            }
            else
            {
                Close();
            }
        }

        protected void SetVideo(VisualElement videoElement, string videoFilename)
        {
            m_ShouldPlayVideo = videoElement != null;
            m_CurrentPageVideoElement = videoElement;
            m_CurrentPageVideoFilename = videoFilename;

            PlayVideo();
        }

        protected void PlayVideo()
        {
            if (!m_ShouldPlayVideo)
            {
                return;
            }

            // Make sure only 1 video anim is playing at a time:
            m_CurrentVideoAnim?.Pause();

            InitVideoPlayer();

            // var videoElement = m_CurrentPage.Q<VisualElement>(className: "mui-agent-wizard-page-video");

            if (m_CurrentPageVideoElement == null)
            {
                return;
            }

            var clipPath = m_CurrentPageVideoFilename;
            var clip = AssetDatabase.LoadAssetAtPath<VideoClip>(clipPath);

            if (clip == null)
            {
                InternalLog.LogError($"Cannot find wizard video clip at path: {clipPath}");
                return;
            }

            var renderTex = m_VideoPlayer.targetTexture;

            if (renderTex != null && (renderTex.width != clip.width || renderTex.height != clip.height))
            {
                RenderTexture.ReleaseTemporary(renderTex);
            }

            if (renderTex == null)
            {
                renderTex = RenderTexture.GetTemporary((int)clip.width, (int)clip.height);
            }

            m_VideoPlayer.targetTexture = renderTex;

            m_VideoPlayer.clip = clip;
            var value = m_CurrentPageVideoElement.style.backgroundImage.value;
            value.renderTexture = m_VideoPlayer.targetTexture;
            m_CurrentPageVideoElement.style.backgroundImage = value;

            m_VideoPlayer.Prepare();

            var videoStartTime = DateTime.Now;
            m_CurrentVideoAnim = schedule.Execute(() =>
            {
                if (m_VideoPlayer == null)
                {
                    return;
                }

                var newTime = (DateTime.Now - videoStartTime).TotalSeconds;
                if (newTime > clip.length)
                {
                    newTime = 0;
                    videoStartTime = DateTime.Now;
                }

                m_VideoPlayer.time = newTime;

                m_VideoPlayer.Play();
                m_CurrentPageVideoElement.MarkDirtyRepaint();
            }).Every(10);
        }
    }
}
