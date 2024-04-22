using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    partial class MuseChatView
    {
        const int k_NotificationDuration = 5000;  // in ms
        const int k_NotificationLimit = 3;

        static event Action<string, PopNotificationIconType, long> s_ShowNotificationEvent;

        readonly List<PopupNotification> k_Notifications = new();

        Action m_BannerButtonClickedCallback;

        static readonly Dictionary<PopNotificationIconType, string> k_IconTypeToName = new()
        {
            { PopNotificationIconType.Info, "mui-icon-tick"},
            { PopNotificationIconType.Error, "mui-icon-error" }
        };

        public static void ShowNotification(
            string message,
            PopNotificationIconType iconType,
            long timeOut = k_NotificationDuration)
        {
            s_ShowNotificationEvent?.Invoke(message, iconType, timeOut);
        }

        void OnShowNotification(string message, PopNotificationIconType iconType, long timeOut)
        {
            // Limit number of notifications shown simultaneously:
            for (int i = 0; i < k_Notifications.Count; i++)
            {
                if (k_Notifications.Count < k_NotificationLimit)
                {
                    break;
                }

                // Only dismiss notifications that have a timeout:
                var existingNotification = k_Notifications[i];
                if (existingNotification.TimeOut > 0)
                {
                    existingNotification.Dismiss(false);
                    i--;
                }
            }

            var notification = new PopupNotification();
            notification.Initialize();
            notification.SetData(new PopupNotification.PopupNotificationContext
            {
                message = message, icon = k_IconTypeToName[iconType], timeOut = timeOut
            });
            notification.OnDismissed += OnNotificationDismissed;

            m_NotificationContainer.Add(notification);

            k_Notifications.Add(notification);

            if (timeOut > 0)
            {
                schedule.Execute(() => { notification.Dismiss(); }).StartingIn(timeOut);
            }
        }

        void OnNotificationDismissed(PopupNotification notification)
        {
            k_Notifications.Remove(notification);
        }

        void ShowBanner(string title, string message, Action callback)
        {
            m_NotificationBannerTitle.text = title;
            m_NotificationBannerMessage.text = message;

            m_BannerButtonClickedCallback = callback;

            m_NotificationBanner.style.display = DisplayStyle.Flex;
        }

        void BannerButtonClicked(PointerUpEvent evt)
        {
            m_BannerButtonClickedCallback?.Invoke();
            m_BannerButtonClickedCallback = null;

            HideBanner();
        }

        void HideBanner()
        {
            m_NotificationBanner.style.display = DisplayStyle.None;
        }
    }
}
