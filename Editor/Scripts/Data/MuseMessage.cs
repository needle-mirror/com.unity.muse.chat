using System;

namespace Unity.Muse.Chat
{
    [Serializable]
    internal struct MuseMessage
    {
        public MuseMessageId Id;
        public string Role;
        public string Content;
        public bool IsComplete;
        public int ErrorCode;
        public string ErrorText;
        public bool IsError => ErrorCode != 0;
        public long Timestamp;
    }
}
