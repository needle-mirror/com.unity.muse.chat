using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Model;

namespace Unity.Muse.Chat
{
    internal interface IAssistantBackend
    {
        bool SessionStatusTrackingEnabled { get; }

        bool RequestInProgress { get; }

        void Cancel();

        void ConversationRefresh(Action<IEnumerable<MuseConversationInfo>> callback);
        void ConversationLoad(MuseConversationId conversationId, Action<MuseConversation> callback);
        void ConversationFavoriteToggle(MuseConversationId conversationId, bool isFavorite);
        Task<MuseConversationId> ConversationCreate();
        void ConversationRename(MuseConversationId conversationId, string newName, Action onComplete);
        void ConversationSetAutoTitle(MuseConversationId id, Action onComplete);
        void ConversationDelete(MuseConversationInfo conversation, Action onComplete);
        Task ConversationDeleteFragment(MuseConversationId conversationId, string fragment);

        void InspirationRefresh(Action<IEnumerable<MuseChatInspiration>> callback);
        void InspirationUpdate(MuseChatInspiration inspiration);
        void InspirationDelete(MuseChatInspiration inspiration);

        void SendFeedback(MuseConversationId conversationId, MessageFeedback feedback);

        void CheckEntitlement(Action<bool> callback);

        Task<SmartContextResponse> SendSmartContext(MuseConversationId conversationId, string prompt, EditorContextReport context);

        Task<MuseChatStreamHandler> SendPrompt(MuseConversationId conversationId, string prompt, EditorContextReport context, ChatCommandType commandType, List<MuseChatContextEntry> selectionContext);

        Task<object> RepairCode(MuseConversationId conversationId, int messageIndex, string errorToRepair, string scriptToRepair, ScriptType scriptType);

        /// <summary>
        /// Returns version support info that can used to check if the version of the server the client wants to
        /// communicate with is supported. Returns null if the version support info could not be retrieved.
        /// </summary>
        /// <param name="version">Server version the client wants to hit expressed as the url name. Example: v1</param>
        Task<List<VersionSupportInfo>> GetVersionSupportInfo(string version);
    }
}
