using System;
using System.Collections.Generic;

namespace Unity.Muse.Chat
{
    partial class MuseChatView
    {
        const int k_NotificationDuration = 5000;  // in ms
        const int k_NotificationLimit = 3;

        static event Action<string, PopNotificationIconType> s_ShowNotificationEvent;

        readonly List<PopupNotification> k_Notifications = new();

        static readonly Dictionary<PopNotificationIconType, string> k_IconTypeToName = new()
        {
            { PopNotificationIconType.Info, "mui-icon-tick"},
            { PopNotificationIconType.Error, "mui-icon-error" }
        };

        public static void ShowNotification(string message, PopNotificationIconType iconType)
        {
            s_ShowNotificationEvent?.Invoke(message, iconType);
        }

        void OnShowNotification(string message, PopNotificationIconType iconType)
        {
            // Limit number of notifications shown simultaneously:
            while (k_Notifications.Count >= k_NotificationLimit)
            {
                k_Notifications[0].Dismiss(false);
            }

            var notification = new PopupNotification();
            notification.Initialize();
            notification.SetData(new PopupNotification.PopupNotificationContext { message = message, icon = k_IconTypeToName[iconType] });
            notification.OnDismissed += OnNotificationDismissed;

            m_NotificationContainer.Add(notification);

            k_Notifications.Add(notification);

            schedule.Execute(() => { notification.Dismiss(); }).StartingIn(k_NotificationDuration);
        }

        void OnNotificationDismissed(PopupNotification notification)
        {
            k_Notifications.Remove(notification);
        }
    }
}
