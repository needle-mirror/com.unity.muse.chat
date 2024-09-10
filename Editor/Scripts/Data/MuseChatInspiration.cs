using System;

namespace Unity.Muse.Chat
{
    [Serializable]
    internal struct MuseChatInspiration
    {
        public MuseInspirationId Id;
        public ChatCommandType Mode;
        public string Description;
        public string Value;
    }
}
