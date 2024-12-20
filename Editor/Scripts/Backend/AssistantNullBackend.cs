using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Model;

namespace Unity.Muse.Chat
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    internal class AssistantNullBackend : IAssistantBackend
    {
        public bool SessionStatusTrackingEnabled => false;

        public bool RequestInProgress => false;

        public void Cancel()
        {
        }

        public void ConversationRefresh(Action<IEnumerable<MuseConversationInfo>> callback)
        {
        }

        public void ConversationLoad(MuseConversationId conversationId, Action<MuseConversation> callback)
        {
        }

        public void ConversationFavoriteToggle(MuseConversationId conversationId, bool isFavorite)
        {
        }


        public async Task<MuseConversationId> ConversationCreate()
        {
            return MuseConversationId.GetNextInternalId();
        }

        public void ConversationRename(MuseConversationId conversationId, string newName, Action onComplete)
        {
            onComplete?.Invoke();
        }

        public void ConversationSetAutoTitle(MuseConversationId id, Action onComplete)
        {
            onComplete?.Invoke();
        }

        public void ConversationDelete(MuseConversationInfo conversation, Action onComplete)
        {
            onComplete?.Invoke();
        }

        public async Task ConversationDeleteFragment(MuseConversationId conversationId, string fragment)
        {
        }

        public void InspirationRefresh(Action<IEnumerable<MuseChatInspiration>> callback)
        {
            callback?.Invoke(new List<MuseChatInspiration>());
        }

        public void InspirationUpdate(MuseChatInspiration inspiration)
        {
        }

        public void InspirationDelete(MuseChatInspiration inspiration)
        {
        }

        public void SendFeedback(MuseConversationId conversationId, MessageFeedback feedback)
        {
        }

        public void CheckEntitlement(Action<bool> callback)
        {
            callback?.Invoke(false);
        }

        public async Task<SmartContextResponse> SendSmartContext(MuseConversationId conversationId, string prompt, EditorContextReport context)
        {
            return new SmartContextResponse(new List<FunctionCall>());
        }

        public async Task<MuseChatStreamHandler> SendPrompt(MuseConversationId conversationId, string prompt, EditorContextReport context,
            ChatCommandType commandType, List<MuseChatContextEntry> selectionContext)
        {
            return null;
        }

        public async Task<object> RepairCode(MuseConversationId conversationId, int messageIndex, string errorToRepair, string scriptToRepair,
            ScriptType scriptType)
        {
            return null;
        }

        public Task<List<VersionSupportInfo>> GetVersionSupportInfo(string version)
        {
            return null;
        }
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}
