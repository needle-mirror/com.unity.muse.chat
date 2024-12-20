using Unity.Muse.Common.Account;

namespace Unity.Muse.Chat.UI
{
    class ServerCompatibilityNotSupportedNotificationView : NotificationView
    {
        public ServerCompatibilityNotSupportedNotificationView(bool inlineButton = false) : base(
            new()
            {
                titleText = ServerCompatibilityText.NotSupportedTitle,
                description = ServerCompatibilityText.NotSupportedMessage,
            }
        )
        {
        }
    }
}
