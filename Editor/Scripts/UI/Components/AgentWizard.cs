using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Chat.UI;
using Unity.Muse.Chat.UI.Components;
using Unity.Muse.Common;
using Unity.Muse.Common.Account;
using Unity.Muse.Common.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    class AgentWizard : WizardPanel<AgentWizard>
    {
        private const string k_ExperimentalLabelText = "Experimental";

        private const string k_ExperimentalFooterLabelText =
            "This is an experimental feature and should only be used for testing.";

        private const string k_ExperimentalFooterButtonText =
            "I understand, continue";

        private const string k_FooterNextButtonText =
            "Next";

        private const string k_FooterPreviousButtonText =
            "Previous";

        private const string k_FooterOpenChatButtonText =
            "Open Chat";

        private const string k_VideoClickFullscreenTooltip = "View fullscreen";

        private const string k_VideoPath = "Packages/com.unity.muse.chat/Editor/UI/Assets/agent_wizard/";
        private const string k_VideoFormat = ".webm";

        class PageData
        {
            public class PageString
            {
                public string Text;
                public string Style;
                public ChatCommandType CommandType;

                public PageString(string text, string style, ChatCommandType commandType = ChatCommandType.Ask)
                {
                    Text = text;
                    Style = style;
                    CommandType = commandType;
                }
            }

            public ChatCommandType? CommandType;
            public string Video;
            public string Header;
            public PageString[] Texts;
            public string PreviousButtonText = k_FooterPreviousButtonText;
            public string NextButtonText = k_FooterNextButtonText;

            public int CustomPage;
        }

        private readonly PageData[] k_ExistingUserPages =
        {
#if ENABLE_ASSISTANT_BETA_FEATURES
            new()
            {
                Video = "Video 1 - Get Started",
                Header = "Code and automation at your fingertips",
                Texts = new[]
                {
                    new PageData.PageString(
                        "Realize your vision faster by using Muse to automate time-consuming or tedious tasks, or generate code.",
                        k_ContentTextClass),
                    new PageData.PageString(
                        "Get started",
                        k_HeaderTextClass),
                    new PageData.PageString(
                        "Switch between asking questions, generating code, and sending commands.",
                        k_ContentTextClass),
                    new PageData.PageString(
                        "You can also type <b>/ask</b>, <b>/run</b>, or <b>/code</b> to switch between modes using your keyboard.",
                        k_ContentTextClass),
                },
                NextButtonText = k_ExperimentalFooterButtonText
            },
            new()
            {
                Video = "Video 2 - Add Gravity",
                CommandType = ChatCommandType.Run,
                Header = "Modify Components",
                Texts = new[]
                {
                    new PageData.PageString(
                        "Transform Components like scripts, lights, meshes, and colliders easily with Muse. Take control of your GameObjects’ behavior and adjust how they interact within the game world to quickly craft the perfect experience.",
                        k_ContentTextClass),
                    new PageData.PageString(
                        "Disable all AudioSource Components in the scene",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Change the Layer of an object with a Light component to 'Ignore Raycast'",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Adjust selected objects' rotation to match the surface under them",
                        k_SampleTextClass),
                },
                PreviousButtonText = null
            }
#endif
        };

        readonly PageData[] k_NewUserPages =
        {
#if ENABLE_ASSISTANT_BETA_FEATURES
            new()
            {
                Header = "Code and automation at your fingertips",
                Texts = new[]
                {
                    new PageData.PageString(
                        "Realize your vision faster by using Muse to automate time-consuming or tedious tasks, or generate code.",
                        k_ContentTextClass)
                }
            },
            new()
            {
                Video = "Video 1 - Get Started",
                Header = "Get started",
                Texts = new[]
                {
                    new PageData.PageString(
                        "Toggle between asking questions, generating code, and sending commands.",
                        k_ContentTextClass),
                    new PageData.PageString(
                        "You can also type <b>/ask</b>, <b>/run</b>, or <b>/code</b> before sending your prompt to switch between modes using your keyboard.",
                        k_ContentTextClass)
                }
            },
            new()
            {
                Video = "Video 2 - Add Gravity",
                CommandType = ChatCommandType.Run,
                Header = "Modify Components",
                Texts = new[]
                {
                    new PageData.PageString(
                        "Transform Components like scripts, lights, meshes, and colliders easily with Muse. Take control of your GameObjects’ behavior and adjust how they interact within the game world to quickly craft the perfect experience.",
                        k_ContentTextClass),
                    new PageData.PageString(
                        "Disable all AudioSource Components in the scene",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Change the Layer of an object with a Light component to 'Ignore Raycast'",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Adjust selected objects' rotation to match the surface under them",
                        k_SampleTextClass)
                }
            }
#endif
        };

        private readonly PageData[] k_SharedPages =
        {
#if ENABLE_ASSISTANT_BETA_FEATURES
            new()
            {
                CommandType = ChatCommandType.Run,
                Video = "Video 3 - Replace with Attached Object",
                Header = "Build your scene",
                Texts = new[]
                {
                    new PageData.PageString(
                        "Design your game world faster and more efficiently with the help of Muse Chat. Rapidly prototype experiences to quickly find what your players enjoy most.",
                        k_ContentTextClass),
                    new PageData.PageString(
                        "Remove all colliders from the scene",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Add a NavMeshSurface and bake it",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Find all instances of the attached prefab and revert each instance to the latest version",
                        k_SampleTextClass)
                }
            },
            new()
            {
                CommandType = ChatCommandType.Run,
                Video = "Video 4 - Mesh to Sphere Colliders",
                Header = "Customize GameObjects",
                Texts = new[]
                {
                    new PageData.PageString(
                        "Get multiple tasks done at once by enabling Chat to alter GameObjects at scale. Bulk-automate customizations in your scene to rapidly shift the experience and try new things.",
                        k_ContentTextClass),
                    new PageData.PageString(
                        "Replace the selected objects’ mesh colliders with box colliders",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Set a random intensity between 1 and 10 to all the lights in my scene",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Evenly distribute light probes in the volume of the attached object",
                        k_SampleTextClass)
                }
            },
            new()
            {
                CommandType = ChatCommandType.Run,
                Video = "Video 5 - Rename",
                Header = "Reorganize the project hierarchy",
                Texts = new[]
                {
                    new PageData.PageString(
                        "Understand and optimize your scene hierarchy to better manage the complexity of your game's environments and interactions. Use Muse Chat to automate grouping, renaming, and other general cleanup tasks so you can stay focused on what matters."
                        , k_ContentTextClass),
                    new PageData.PageString(
                        "Rename selected objects to include the prefix 'Tmp_'",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Batch rename and reorganize assets based on their type and usage (e.g., Textures/Environment/Rock_01)",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Sort child objects alphabetically from the selected parent object",
                        k_SampleTextClass)
                }
            },
            new()
            {
                CommandType = ChatCommandType.Run,
                Video = "Video 6 - Select Triangle Mesh",
                Header = "Locate specific items",
                Texts = new[]
                {
                    new PageData.PageString(
                        "Perfect your creation by using Muse Chat to track down outliers or highlight missing pieces. Leverage Chat to quickly locate items that may need attention, or generate reports of potential conflicts."
                        , k_ContentTextClass),
                    new PageData.PageString(
                        "Find objects with a missing script component in the scene",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Find all objects that contain multiple instances of the same Component",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Generate a report of all shaders used in the scene",
                        k_SampleTextClass)
                }
            },
            new()
            {
                CommandType = ChatCommandType.Code,
                Video = "Video 7 - Code Gen",
                Header = "Use Muse Chat to generate code",
                Texts = new[]
                {
                    new PageData.PageString(
                        "Reduce the time and effort required to structure gameplay mechanics by leveraging Chat to write basic scripts, allowing you to focus on other aspects of game development."
                        , k_ContentTextClass),
                    new PageData.PageString(
                        "What is the best way to implement a double jump feature in a Unity 2D platformer game?",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "Script a basic enemy that patrols between waypoints",
                        k_SampleTextClass),
                    new PageData.PageString(
                        "How do I write a Unity script that simulates gravity for a custom object?",
                        k_SampleTextClass),
                }
            },
            new()
            {
                Header = "Start experimenting with Muse Chat",
                CustomPage = 1,
                Texts = new[]
                {
                    new PageData.PageString(
                        "Muse Chat makes automation of time-consuming tasks possible, enabling your productivity. Generate code to realize your vision faster.",
                        k_ContentTextClass),
                    new PageData.PageString(
                        "Try one of the prompts below to get started.",
                        k_ContentTextClass),

                    new PageData.PageString(
                        String.Empty,
                        k_ContentTextClass),

                    new PageData.PageString(
                        "Run commands",
                        k_ContentTextClass),

                    new PageData.PageString(
                        "Create a screenshot of the current scene view and save it to the project folder",
                        k_SampleTextWithButtonClass,
                        ChatCommandType.Run),
                    new PageData.PageString(
                        "Report all disabled Components",
                        k_SampleTextWithButtonClass,
                        ChatCommandType.Run),
                    new PageData.PageString(
                        "Find an object with a scale bigger than 10",
                        k_SampleTextWithButtonClass,
                        ChatCommandType.Run),

                    new PageData.PageString(
                        String.Empty,
                        k_ContentTextClass),

                    new PageData.PageString(
                        "Generate code",
                        k_ContentTextClass),
                    new PageData.PageString(
                        "Write a script to spawn enemies at random positions",
                        k_SampleTextWithButtonClass,
                        ChatCommandType.Code),
                    new PageData.PageString(
                        "How can I create a script to make a character jump in Unity?",
                        k_SampleTextWithButtonClass,
                        ChatCommandType.Code),
                    new PageData.PageString(
                        "Write a script to implement player health regeneration over time",
                        k_SampleTextWithButtonClass,
                        ChatCommandType.Code)
                }
            }
#endif
        };

        #region Styles

        // ClassNames:
        private const string k_HeaderContainerClass = "mui-wizard-page-header";
        private const string k_HeaderTextClass = "mui-wizard-page-header-text";
        private const string k_ContentContainerClass = "mui-wizard-page-content";
        private const string k_ContentTextClass = "mui-wizard-page-content-text";
        private const string k_SampleTextClass = "mui-wizard-page-sample-text";
        private const string k_SampleTextWithButtonClass = "mui-wizard-page-sample-text-with-button";
        private const string k_SampleButtonClass = "mui-wizard-page-sample-button -muse-chat-button";
        private const string k_FooterContainerClass = "mui-wizard-page-footer-container";
        private const string k_FooterClass = "mui-wizard-page-footer";
        private const string k_FooterButtonClass = "mui-wizard-page-footer-button -muse-chat-button";
        private const string k_MuseIconClass = "appui-icon--mui-icon-muse";
        private const string k_SubmitIconClass = "appui-icon--mui-icon-submit";
        private const string k_CheckboxContainerClass = "mui-wizard-page-checkbox-container";
        private const string k_CheckboxClass = "mui-wizard-page-checkbox";
        private const string k_VideoClass = "mui-agent-wizard-page-video";
        private const string k_VideoIconClass = "mui-agent-wizard-page-video-icon";
        private const string k_LargeVideoIconClass = "mui-agent-wizard-page-large-video-icon";
        private const string k_VideoLargeClass = "mui-agent-wizard-page-video-large";
        private const string k_CommandClass = "mui-wizard-command";

        #endregion

        private int m_CurrentPage = -1;
        private VisualElement m_PageContainer;
        private bool m_NewUser;

        private int SharedPageNumberOffset => m_NewUser ? k_NewUserPages.Length : k_ExistingUserPages.Length;

        protected override void InitializeView(TemplateContainer view)
        {
            var account = AccountInfo.Instance;
            m_NewUser = !account.LegalConsent.HasConsented;

            base.InitializeView(view);
        }

        protected override void ShowPage(int offset)
        {
            m_CurrentPage += offset;
            var scrollView = contentContainer.Q<ScrollView>(className: "mui-wizard-scrollview");
            if (m_PageContainer != null)
            {
                DestroyVideoPlayer();

                SetVideo(null, null);
                scrollView.Remove(m_PageContainer);
            }

            m_PageContainer = new VisualElement();
            m_PageContainer.AddToClassList("mui-wizard-page");
            scrollView.Add(m_PageContainer);

            var pageShown = m_NewUser ? ShowNewUserPage() : ShowExistingUserPage();

            if (!pageShown)
            {
                Close();
            }
        }

        bool ShowExistingUserPage()
        {
            PageData pageContent = null;
            if (m_CurrentPage >= 0 && m_CurrentPage < k_ExistingUserPages.Length)
            {
                pageContent = k_ExistingUserPages[m_CurrentPage];
            }

            if (pageContent != null)
            {
                if (m_CurrentPage == 0)
                {
                    // Header:
                    CreateVideo(pageContent.Video, m_PageContainer);

                    var spacer = CreateVisualElement(null, m_PageContainer);
                    spacer.style.marginTop = 20;
                    CreateExperimentalElement(m_PageContainer);
                    var headerContainer = CreateVisualElement(k_HeaderContainerClass, m_PageContainer);
                    CreateLabel(pageContent.Header, k_HeaderTextClass, headerContainer);

                    // Content:
                    var pageContentContainer = CreateVisualElement(k_ContentContainerClass, m_PageContainer);
                    CreateLabels(pageContent.Texts, pageContentContainer);

                    // Footer:
                    var footerContainer = CreateVisualElement(k_FooterContainerClass, m_PageContainer);
                    var footer = CreateVisualElement(k_FooterClass, footerContainer);
                    CreateLabel(k_ExperimentalFooterLabelText, k_ContentTextClass, footer);

                    CreateButton(
                        pageContent.NextButtonText,
                        k_FooterButtonClass,
                        _ => { ShowPage(1); },
                        footer);
                }
                else
                {
                    ShowPageData(pageContent);
                }

                return true;
            }

            return ShowSharedPage();
        }

        bool ShowNewUserPage()
        {
            PageData pageContent = null;
            if (m_CurrentPage >= 0 && m_CurrentPage < k_NewUserPages.Length)
            {
                pageContent = k_NewUserPages[m_CurrentPage];
            }

            if (pageContent != null)
            {
                if (m_CurrentPage == 0)
                {
                    // Header:
                    var spacer = CreateVisualElement(null, m_PageContainer);
                    spacer.style.marginTop = 100;

                    CreateMuseIcon();

                    spacer = CreateVisualElement(null, m_PageContainer);
                    spacer.style.marginTop = 20;
                    CreateExperimentalElement(m_PageContainer);
                    var headerContainer = CreateVisualElement(k_HeaderContainerClass, m_PageContainer);
                    CreateLabel(pageContent.Header, k_HeaderTextClass, headerContainer);

                    // Content:
                    var pageContentContainer = CreateVisualElement(k_ContentContainerClass, m_PageContainer);
                    CreateLabels(pageContent.Texts, pageContentContainer);

                    var termsContainer = CreateVisualElement(k_CheckboxContainerClass, pageContentContainer);
                    var termsCheckbox = CreateCheckbox(k_CheckboxClass, termsContainer);
                    var termsOfServiceLabel = CreateLabel(TextContent.subConfirmTermsOfService,
                        k_ContentTextClass,
                        termsContainer);
                    termsOfServiceLabel.RegisterCallback<PointerDownLinkTagEvent>(TermsOfServiceClick);
                    termsOfServiceLabel.RegisterCallback<PointerOverLinkTagEvent>(_ =>
                    {
                        termsOfServiceLabel.AddToClassList("muse-link-hover");
                    });
                    termsOfServiceLabel.RegisterCallback<PointerOutLinkTagEvent>(_ =>
                    {
                        termsOfServiceLabel.RemoveFromClassList("muse-link-hover");
                    });
                    var privacyContainer = CreateVisualElement(k_CheckboxContainerClass, pageContentContainer);
                    var privacyCheckbox = CreateCheckbox(k_CheckboxClass, privacyContainer);
                    var privacyPolicy = CreateLabel(TextContent.subConfirmPrivacy,
                        k_ContentTextClass,
                        privacyContainer);
                    privacyPolicy.RegisterCallback<PointerDownLinkTagEvent>(PrivacyPolicyClick);
                    privacyPolicy.RegisterCallback<PointerOverLinkTagEvent>(_ =>
                    {
                        privacyPolicy.AddToClassList("muse-link-hover");
                    });
                    privacyPolicy.RegisterCallback<PointerOutLinkTagEvent>(_ =>
                    {
                        privacyPolicy.RemoveFromClassList("muse-link-hover");
                    });

                    // Footer:
                    var footerContainer = CreateVisualElement(k_FooterContainerClass, m_PageContainer);
                    var footer = CreateVisualElement(k_FooterClass, footerContainer);
                    CreateLabel(k_ExperimentalFooterLabelText, k_ContentTextClass, footer);

                    var organization = AccountInfo.Instance.Organization;

                    Button nextPageButton = null;
                    nextPageButton = CreateButton(
                        k_ExperimentalFooterButtonText,
                        k_FooterButtonClass,
                        NextPageCallback,
                        footer);
                    nextPageButton.SetEnabled(false);
                    termsCheckbox.RegisterValueChangedCallback(CheckboxCallback);
                    privacyCheckbox.RegisterValueChangedCallback(CheckboxCallback);

                    return true;

                    async void NextPageCallback(PointerUpEvent _)
                    {
                        nextPageButton.SetEnabled(false);
                        termsCheckbox.SetEnabled(false);
                        privacyCheckbox.SetEnabled(false);

                        var legalConsent = new LegalConsentRequest
                        {
                            terms_of_service_legal_info = termsCheckbox.value == CheckboxState.Checked,
                            privacy_policy_gen_ai = privacyCheckbox.value == CheckboxState.Checked,
                            content_usage_data_training = true
                        };
                        await GenerativeAIBackend.SetLegalConsent(legalConsent);

                        // Fetch server information to update the account status
                        await AccountInfo.Instance.UpdateAccountInformation();

                        // Organization can be null if the user is not starting a trial and only being asked for legal consent.
                        if (organization is not null)
                        {
                            AccountInfo.Instance.Organization = organization; // Switch to trial form's organization
                        }

                        ShowPage(1);
                    }

                    void CheckboxCallback(ChangeEvent<CheckboxState> _) =>
                        nextPageButton.SetEnabled(
                            termsCheckbox.value == CheckboxState.Checked &&
                            privacyCheckbox.value == CheckboxState.Checked);
                }

                if (m_CurrentPage == 1)
                {
                    // Header:
                    CreateVideo(pageContent.Video, m_PageContainer);
                    var headerContainer = CreateVisualElement(k_HeaderContainerClass, m_PageContainer);
                    CreateLabel(pageContent.Header, k_HeaderTextClass, headerContainer);

                    // Content:
                    var pageContentContainer = CreateVisualElement(k_ContentContainerClass, m_PageContainer);

                    CreateLabels(pageContent.Texts, pageContentContainer);

                    // Footer:
                    var footerContainer = CreateVisualElement(k_FooterContainerClass, m_PageContainer);
                    var footer = CreateVisualElement(k_FooterClass, footerContainer);
                    CreateButton(k_FooterNextButtonText, k_FooterButtonClass,
                        _ => { ShowPage(1); },
                        footer);
                    return true;
                }

                if (m_CurrentPage == 2)
                {
                    // Header:
                    CreateVideo(pageContent.Video, m_PageContainer);
                    CreateCommandLabel(pageContent.CommandType);
                    var headerContainer = CreateVisualElement(k_HeaderContainerClass, m_PageContainer);
                    CreateLabel(pageContent.Header, k_HeaderTextClass, headerContainer);

                    // Content:
                    var pageContentContainer = CreateVisualElement(k_ContentContainerClass, m_PageContainer);
                    CreateLabels(pageContent.Texts, pageContentContainer);

                    // Footer:
                    CreatePreviousAndNextFooter();
                    return true;
                }
            }

            return ShowSharedPage();
        }

        void ShowPageData(PageData pageData)
        {
            // Header:
            CreateVideo(pageData.Video, m_PageContainer);
            CreateCommandLabel(pageData.CommandType);
            var headerContainer = CreateVisualElement(k_HeaderContainerClass, m_PageContainer);
            CreateLabel(pageData.Header, k_HeaderTextClass, headerContainer);

            // Content:
            var pageContentContainer = CreateVisualElement(k_ContentContainerClass, m_PageContainer);

            CreateLabels(pageData.Texts, pageContentContainer);

            // Footer:
            CreatePreviousAndNextFooter(pageData.PreviousButtonText, pageData.NextButtonText);
        }

        bool ShowSharedPage()
        {
            var page = m_CurrentPage - SharedPageNumberOffset;

            if (page >= 0 && page < k_SharedPages.Length)
            {
                var pageContent = k_SharedPages[page];

                if (pageContent.CustomPage == 0)
                {
                    ShowPageData(pageContent);
                }
                else if (pageContent.CustomPage == 1)
                {
                    // This is the final page with all the preview buttons:
                    var spacer = CreateVisualElement(null, m_PageContainer);
                    spacer.style.marginTop = 20;

                    CreateMuseIcon();

                    // Header:
                    var headerContainer = CreateVisualElement(k_HeaderContainerClass, m_PageContainer);
                    CreateLabel(pageContent.Header, k_HeaderTextClass, headerContainer);

                    // Content:
                    var pageContentContainer = CreateVisualElement(k_ContentContainerClass, m_PageContainer);

                    foreach (var contentText in pageContent.Texts)
                    {
                        if (!string.IsNullOrEmpty(contentText.Text))
                        {
                            if (contentText.Style == k_SampleTextWithButtonClass)
                            {
                                var question = contentText.Text;
                                var commandType = contentText.CommandType;
                                var button = CreateButton(question, k_SampleButtonClass,
                                    _ => { PreviewButtonPressed(question, commandType); }, pageContentContainer);

                                var icon = CreateIcon(k_SubmitIconClass, button);
                                icon.size = IconSize.L;
                            }
                            else
                            {
                                CreateLabel(contentText.Text, contentText.Style, pageContentContainer);
                            }
                        }
                        else
                        {
                            spacer = CreateVisualElement(null, pageContentContainer);
                            spacer.style.marginTop = 20;
                        }
                    }

                    // Footer:
                    CreatePreviousAndNextFooter(k_FooterPreviousButtonText, k_FooterOpenChatButtonText);
                }

                return true;
            }

            return false;
        }

        void CreateCommandLabel(ChatCommandType? type)
        {
            if (!type.HasValue)
            {
                return;
            }

            string label = "";
            switch (type.Value)
            {
                case ChatCommandType.Ask:
                    label = "/ask";
                    break;
#if ENABLE_ASSISTANT_BETA_FEATURES
                case ChatCommandType.Code:
                    label = "/code";
                    break;
                case ChatCommandType.Run:
                    label = "/run";
                    break;
#endif
            }

            CreateLabel(label, k_CommandClass, m_PageContainer);
        }

        void CreatePreviousAndNextFooter(
            string previousButtonText = k_FooterPreviousButtonText,
            string nextButtonText = k_FooterNextButtonText)
        {
            var footerContainer = CreateVisualElement(k_FooterContainerClass, m_PageContainer);
            var footer = CreateVisualElement(k_FooterClass, footerContainer);
            var footerButtonContainer = CreateVisualElement(k_FooterContainerClass, footer);

            if (!string.IsNullOrEmpty(previousButtonText))
            {
                CreateButton(previousButtonText, k_FooterButtonClass,
                    _ => { ShowPage(-1); },
                    footerButtonContainer);
            }

            CreateButton(nextButtonText, k_FooterButtonClass,
                _ => { ShowPage(1); },
                footerButtonContainer);
        }

        # region Element Creation

        VisualElement CreateVisualElement(string className, VisualElement parent = null)
        {
            var element = new VisualElement();
            element.AddToClassList(className);
            parent?.Add(element);
            return element;
        }

        void CreateLabels(PageData.PageString[] pageStrings, VisualElement parent = null)
        {
            foreach (var stringData in pageStrings)
            {
                var label = new Label(stringData.Text);
                label.AddToClassList(stringData.Style);
                parent?.Add(label);
            }
        }

        Label CreateLabel(string text, string className, VisualElement parent = null)
        {
            var label = new Label(text);
            label.AddToClassList(className);
            parent?.Add(label);
            return label;
        }

        Icon CreateIcon(string className, VisualElement parent = null)
        {
            var icon = new Icon();
            icon.AddToClassList(className);
            parent?.Add(icon);
            return icon;
        }

        Image CreateImage(string className, VisualElement parent = null)
        {
            var image = new Image();
            image.AddToClassList(className);
            parent?.Add(image);
            return image;
        }

        Checkbox CreateCheckbox(string className, VisualElement parent = null)
        {
            var toggle = new Checkbox();
            toggle.AddToClassList(className);
            parent?.Add(toggle);
            return toggle;
        }

        Button CreateButton(string text, string classNames, EventCallback<PointerUpEvent> callback,
            VisualElement parent = null)
        {
            var button = new Button();
            button.title = text;
            foreach (var className in classNames.Split(" "))
            {
                button.AddToClassList(className);
            }

            parent?.Add(button);
            button.RegisterCallback(callback);
            return button;
        }

        VisualElement CreateVideo(string videoFileName, VisualElement parent = null)
        {
            if (videoFileName == null)
            {
                return null;
            }

            var videoElement = new VisualElement();
            videoElement.AddToClassList(k_VideoClass);
            parent?.Add(videoElement);

            var videoFile = Path.Combine(k_VideoPath, videoFileName + k_VideoFormat);

            videoElement.RegisterCallback<ClickEvent>(_ =>
            {
                var largeVideoElement = CreateVisualElement(k_VideoLargeClass, contentContainer);

                DestroyVideoPlayer();
                SetVideo(largeVideoElement, videoFile);

                largeVideoElement.RegisterCallback<ClickEvent>(_ =>
                {
                    DestroyVideoPlayer();
                    largeVideoElement.RemoveFromHierarchy();

                    SetVideo(videoElement, videoFile);
                });

                CreateIcon(k_LargeVideoIconClass, largeVideoElement);
            });

            var iconElement = CreateIcon(k_VideoIconClass, videoElement);
            iconElement.pickingMode = PickingMode.Position;
            iconElement.tooltip = k_VideoClickFullscreenTooltip;

            SetVideo(videoElement, videoFile);

            return videoElement;
        }

        Label CreateExperimentalElement(VisualElement parent)
        {
            return CreateLabel(k_ExperimentalLabelText, "mui-wizard-experimental", parent);
        }

        Image CreateMuseIcon()
        {
            var image = CreateImage(k_MuseIconClass, m_PageContainer);
            const int museIconSize = 24;
            image.style.width = museIconSize;
            image.style.height = museIconSize;

            return image;
        }

        #endregion

        # region Button callbacks

        static void TermsOfServiceClick(PointerDownLinkTagEvent evt)
        {
            if (evt.linkID == "terms")
                AccountLinks.TermsOfService();
            else if (evt.linkID == "legal")
                AccountLinks.LegalInfo();
        }

        static void PrivacyPolicyClick(PointerDownLinkTagEvent evt)
        {
            if (evt.linkID == "policy")
                AccountLinks.PrivacyPolicy();
            else if (evt.linkID == "supplemental")
                AccountLinks.PrivacyStatement();
        }

        #endregion

        internal static bool ShowIfNeeded(MuseChatView view)
        {
            if (!AccountInfo.Instance.LegalConsent.HasConsented || !EditorPrefs.GetBool(WizardSeenPrefsKey(), false))
            {
                view.ShowWizard(new AgentWizard());
                return true;
            }

            return false;
        }
    }
}
