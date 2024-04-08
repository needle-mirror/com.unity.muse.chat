using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Muse.Chat.Model;

namespace Unity.Muse.Chat
{
    interface IWebAPI
    {
        internal enum RequestStatus
        {
            Empty,
            InProgress,
            Complete,
            Error
        }

        [Serializable]
        public class ContextIndicatedConversationInfo : ConversationInfo
        {
            public ContextIndicatedConversationInfo(bool isContextual, ConversationInfo info) : base(info.ConversationId, info.Title, info.LastMessageTimestamp)
            {
                IsContextual = isContextual;
            }

            public ContextIndicatedConversationInfo(bool isContextual, string conversationId = default(string), string title = default(string), long lastMessageTimestamp = default(long)) : base(conversationId, title, lastMessageTimestamp)
            {
                IsContextual = isContextual;
            }

            /// <summary>
            /// Indicated whether this conversation has been created by providing context to the LLM or not. This boils
            /// down to whether or not the chat was started from the web or from the editor.
            /// </summary>
            public bool IsContextual { get; set; }
        }

        void ClearResponse();
        RequestStatus pluginConnectStatus { get; }
        RequestStatus chatStatus { get; }
        void GetErrorDetails(out int errorCode, out string errorText);
        IEnumerable<ContextIndicatedConversationInfo> LastConversations { get; }
        void ConnectSession();
        void DisconnectSession(bool requested = true);
        string GetConnectError();
        void CancelChat();
        void Chat(string prompt, string conversationID = "", string context = "");
        MuseConversationId GetConversationID();
        string GetChatResponseData(out string assistantFragmentId, out string userFragmentId);
        void SendFeedback(string text, string conversationID, string conversationFragmentId,
            AssistantModelsMuseRequestsSentiment sentiment, Category feedbackType);

        /// <summary>
        /// Starts a task to return conversations. Poll <see cref="GetConversationsData"/> to wait for data to be
        /// received.
        /// </summary>
        /// <remarks>If the request is already in flight, the new <see cref="onComplete"/> callback will be ignored
        /// and the function will return immediately</remarks>
        void GetConversations(
            ILoopRegistration loop,
            Action<IEnumerable<ContextIndicatedConversationInfo>> onComplete,
            Action<Exception> onError);

        /// <summary>
        /// Starts a task to return the data associated with a conversation
        /// </summary>
        void GetConversation(
            string conversationId,
            ILoopRegistration loop,
            Action<Conversation> onComplete,
            Action<Exception> onError);

        /// <summary>
        /// Starts a task to return the data associated with a conversation
        /// </summary>
        void DeleteConversation(
            string conversationId,
            ILoopRegistration loop,
            Action onComplete,
            Action<Exception> onError);

        /// <summary>
        /// Starts a task to rename the associated conversation
        /// </summary>
        void RenameConversation(string conversationId,
            string newName,
            ILoopRegistration loop,
            Action onComplete,
            Action<Exception> onError);

        /// <summary>
        /// Starts a task to return the title of a conversation
        /// </summary>
        void GetConversationTitle(string conversationId,
            ILoopRegistration loop,
            Action<string> onComplete,
            Action<Exception> onError);

        /// <summary>
        /// Starts a task to delete the given fragment from the given conversation.
        /// </summary>
        Task DeleteConversationFragment(
            MuseConversationId conversationId,
            string fragmentId);
    }
}
