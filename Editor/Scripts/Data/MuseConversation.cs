using System;
using System.Collections.Generic;

namespace Unity.Muse.Chat
{
    [Serializable]
    internal class MuseConversation
    {
        public MuseConversation()
        {
            Messages = new List<MuseMessage>();
        }

        public string Title;
        public MuseConversationId Id;
        public readonly List<MuseMessage> Messages;
    }
}
