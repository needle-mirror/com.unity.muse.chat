using System;

namespace Unity.Muse.Chat.UI
{
    interface IMuseChatHost
    {
        Action FocusLost { get; set; }
    }
}
