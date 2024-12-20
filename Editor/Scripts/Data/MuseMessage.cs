using System;

namespace Unity.Muse.Chat
{
    [Serializable]
    internal struct MuseMessage
    {
        public MuseMessageId Id;
        public string Author;
        public string Role;
        public string Content;
        public MuseChatContextEntry[] Context;
        public bool IsComplete;
        public int ErrorCode;
        public string ErrorText;
        public bool IsError => ErrorCode != 0;
        public long Timestamp;
        public int MessageIndex;

        public readonly ChatCommandType ChatCommand(ChatCommandType defaultCommand)
        {
            if (string.IsNullOrEmpty(Author))
                return defaultCommand;

#if ENABLE_ASSISTANT_BETA_FEATURES
            if (Author.ToLower().Contains("agent"))
                return ChatCommandType.Run;

            if (Author.ToLower().Contains("codegen"))
                return ChatCommandType.Code;
#endif
            return ChatCommandType.Ask;
        }
    }
}
