using System;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Common.Utils;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    internal class MuseChatNotificationBanner : ManagedTemplate
    {
        VisualElement m_NotificationBanner;
        Text m_NotificationBannerTitle;
        Text m_NotificationBannerMessage;
        Button m_DismissButton;
        Button m_ActionButton;

        Action m_ActionCallback;
        Action m_DismissCallback;

        public MuseChatNotificationBanner()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public void Show(string title, string message, Action actionCallback = null, string actionCallbackText = "", bool canDismiss = true, Action dismissCallback = null)
        {
            Show();
            m_NotificationBannerTitle.text = title;
            m_NotificationBannerMessage.text = message;

            m_ActionCallback = actionCallback;
            m_DismissCallback = dismissCallback;

            m_DismissButton.SetDisplay(canDismiss);
            m_ActionButton.SetDisplay(actionCallback != null);
            m_ActionButton.title = actionCallbackText;
        }

        void DismissClicked(PointerUpEvent evt)
        {
            m_DismissCallback?.Invoke();
            m_DismissCallback = null;

            Hide();
        }

        void ActionClicked(PointerUpEvent evt)
        {
            m_ActionCallback?.Invoke();
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_NotificationBanner = view.Q<VisualElement>("notificationBanner");
            m_NotificationBannerTitle = view.Q<Text>("notificationBannerTitle");
            m_NotificationBannerMessage = view.Q<Text>("notificationBannerMessage");
            m_ActionButton = view.SetupButton("notificationBannerActionButton", ActionClicked);
            m_DismissButton = view.SetupButton("notificationBannerDismissButton", DismissClicked);

            Show(false);
            Hide(false);
        }
    }
}
