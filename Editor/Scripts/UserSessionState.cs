using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat
{
    internal class UserSessionState : ScriptableSingleton<UserSessionState>
    {
        const string k_HistoryOpen = "MuseChatUserSession_HistoryOpen";
        const string k_LastActiveConversationId = "MuseChatUserSession_LastActiveConversationId";
        const ChatCommandType k_DefaultCommandMode = ChatCommandType.Ask;

        public ChatCommandType SelectedCommandMode
        {
            get => k_DefaultCommandMode;
            set => throw new System.NotImplementedException();
        }

        public bool HasActiveSession => IsHistoryOpen || LastActiveConversationId != null;

        public bool DebugUIModeEnabled;

        public bool IsHistoryOpen
        {
            get => SessionState.GetBool(k_HistoryOpen, false);
            set => SessionState.SetBool(k_HistoryOpen, value);
        }

        public string LastActiveConversationId
        {
            get => SessionState.GetString(k_LastActiveConversationId, null);
            set => SessionState.SetString(k_LastActiveConversationId, value);
        }

        /// <summary>
        /// Clears data that should survive domain reload.
        /// </summary>
        public void Clear()
        {
            IsHistoryOpen = false;
            LastActiveConversationId = null;
        }
    }
}
