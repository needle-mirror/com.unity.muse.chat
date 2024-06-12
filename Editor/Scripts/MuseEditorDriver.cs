using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Muse.Chat.Context.SmartContext;
using Unity.Muse.Chat.Model;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

namespace Unity.Muse.Chat
{
    internal class MuseEditorDriver : ScriptableSingleton<MuseEditorDriver>
    {
        internal static readonly string k_UserRole = "user";
        internal static readonly string k_AssistantRole = "assistant";

        const int k_MaxInternalConversationTitleLength = 30;
        const int k_TopK = 2;
        const float k_MinScore = 0.15f;

        readonly List<MuseConversationInfo> k_History = new();
        readonly Queue<MuseChatUpdateData> k_Updates = new();

        MuseConversation m_ActiveConversation;
        ContextRetrieval m_ContextRetrieval;

        private readonly List<MuseMessageUpdateHandler> k_MessageUpdaters = new();

        public event Action<MuseChatUpdateData> OnDataChanged;

#pragma warning disable CS0067 // Event is never used
        public event Action<string, bool> OnConnectionChanged;
#pragma warning restore CS0067

        /// <summary>
        /// Indicates that the history has changed
        /// </summary>
        public event Action OnConversationHistoryChanged;

        /// <summary>
        /// The WebAPI implementation used to communicate with the Muse Backend.
        /// </summary>
        public WebAPI WebAPI { get; set; } = new();

        /// <summary>
        /// The Toolbox used to provide access to smart context features
        /// </summary>
        public Toolbox SmartContextToolbox { get; } = new(new AttributeBasedContextProvider());

        Tuple<MuseConversationId, string> m_LastContextForConversation = new(default, default);

        internal delegate bool DebugConversationRequest(MuseConversationId conversationId, out MuseConversation result);
        internal DebugConversationRequest OnRequestDebugConversation;

        internal delegate List<MuseConversationInfo> DebugHistoryRequest();
        internal DebugHistoryRequest OnRequestDebugHistory;

        internal Action<VisualElement> OnDebugTrackMetricsRequest;

        internal bool IsConsoleInfoSelected = true;
        internal bool IsConsoleWarningSelected = true;
        internal bool IsConsoleErrorSelected = true;
        internal bool IsGameObjectSelected = true;

        internal List<MuseConversationInfo> History
        {
            get
            {
                if (UserSessionState.instance.DebugModeEnabled && OnRequestDebugHistory != null)
                {
                    return OnRequestDebugHistory.Invoke();
                }

                return k_History;
            }
        }

        /// <summary>
        /// Starts a request to refresh the list of conversations available. This is non-blocking.
        /// </summary>
        public void StartConversationRefresh()
        {
            WebAPI.GetConversations(
                EditorLoopUtilities.EditorLoopRegistration,
                OnConversationHistoryReceived,
                _ => OnConversationHistoryChanged?.Invoke()
            );
        }

        /// <summary>
        /// Starts a webrequest that attempts to load the conversation with <see cref="conversationId"/>.
        /// </summary>
        /// <param name="conversationId">If not null or empty function acts as noop.</param>
        public void StartConversationLoad(MuseConversationId conversationId)
        {
            if (!conversationId.IsValid)
            {
                return;
            }

            if (UserSessionState.instance.DebugModeEnabled && OnRequestDebugConversation != null && OnRequestDebugConversation.Invoke(conversationId, out var debugConversation))
            {
                m_ActiveConversation = debugConversation;

                OnDataChanged?.Invoke(new MuseChatUpdateData
                {
                    IsMusing = false,
                    Type = MuseChatUpdateType.ConversationChange
                });

                return;
            }

            WebAPI.GetConversation(
                conversationId.Value,
                EditorLoopUtilities.EditorLoopRegistration,
                ConvertAndPushConversation,
                Debug.LogException
            );
        }

        /// <summary>
        /// Starts a webrequest that attempts to rename a conversation with <see cref="conversationId"/>.
        /// </summary>
        /// <param name="conversationId">If not null or empty function acts as noop.</param>
        /// <param name="newName">New name of the conversation</param>
        public void StartConversationRename(MuseConversationId conversationId, string newName)
        {
            if (!conversationId.IsValid)
            {
                return;
            }

            if (m_ActiveConversation != null && m_ActiveConversation.Id == conversationId && m_ActiveConversation.Title != newName)
            {
                m_ActiveConversation.Title = newName;

                ExecuteUpdateImmediate(new MuseChatUpdateData
                {
                    Type = MuseChatUpdateType.ConversationChange
                });
            }

            WebAPI.RenameConversation(conversationId.Value,
                newName,
                EditorLoopUtilities.EditorLoopRegistration,
                StartConversationRefresh,
                Debug.LogException);
        }

        /// <summary>
        /// Starts a webrequest that attempts to delete a conversation with <see cref="conversation"/>.
        /// </summary>
        /// <param name="conversation">If not null or empty function acts as noop.</param>
        public void StartDeleteConversation(MuseConversationInfo conversation)
        {
            if (!conversation.Id.IsValid || !k_History.Contains(conversation))
            {
                return;
            }

            k_History.Remove(conversation);
            OnConversationHistoryChanged?.Invoke();

            WebAPI.DeleteConversation(
                conversation.Id.Value,
                EditorLoopUtilities.EditorLoopRegistration,
                StartConversationRefresh,
                Debug.LogException
            );

            // If this is the active conversation, reset active:
            if (conversation.Id == m_ActiveConversation?.Id)
            {
                ClearForNewConversation();
                OnDataChanged?.Invoke(new() { Type = MuseChatUpdateType.ConversationClear });
            }
        }

        public void ClearForNewConversation()
        {
            m_ActiveConversation = null;
        }

        public MuseConversation GetActiveConversation()
        {
            return m_ActiveConversation;
        }

        public MuseMessage AddInternalMessage(string text, string role = null, bool musing = true, bool sendUpdate = true)
        {
            var message = new MuseMessage
            {
                Id = MuseMessageId.GetNextInternalId(m_ActiveConversation.Id),
                IsComplete = true,
                Content = text,
                Role = role,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            if (sendUpdate)
            {
                ExecuteUpdateImmediate(new MuseChatUpdateData
                {
                    Type = MuseChatUpdateType.NewMessage,
                    Message = message,
                    IsMusing = musing
                });
            }

            m_ActiveConversation.Messages.Add(message);
            return message;
        }

        public MuseMessage AddIncompleteMessage(string text, string role = null, bool musing = true, bool sendUpdate = true)
        {
            var message = new MuseMessage
            {
                Id = MuseMessageId.GetNextIncompleteId(m_ActiveConversation.Id),
                IsComplete = false,
                Content = text,
                Role = role,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };

            if (sendUpdate)
            {
                ExecuteUpdateImmediate(new MuseChatUpdateData
                {
                    Type = MuseChatUpdateType.NewMessage,
                    Message = message,
                    IsMusing = musing
                });
            }

            m_ActiveConversation.Messages.Add(message);
            return message;
        }

        public async void WarmupContext()
        {
            await GetContextString(default, MuseChatConstants.PromptContextLimit, "");
        }

        public async void ProcessEditPrompt(string editedPrompt, MuseMessageId messageId)
        {
            if (string.IsNullOrEmpty(editedPrompt))
            {
                return;
            }

            if (m_ActiveConversation == null)
            {
                return;
            }

            // Editing works by deleting the given prompt and all responses after it and then sending a new prompt.

            // Cancel any active operation to ensure no additional messages arrive while or after we're deleting:
            AbortPrompt();

            // Find the index of the message to edit:
            var editedMessageIndex = m_ActiveConversation.Messages.FindIndex(m => m.Id.FragmentId == messageId.FragmentId);

            if (editedMessageIndex < 0)
            {
                Debug.LogError("Cannot find message to edit in the current conversation!");
                return;
            }

            try
            {
                // Delete the edited message and any messages after it:
                for (int i = editedMessageIndex; i < m_ActiveConversation.Messages.Count; i++)
                {
                    var messageToDelete = m_ActiveConversation.Messages[i];
                    var messageIdToDelete = messageToDelete.Id;

                    // Remove message from local conversation:
                    m_ActiveConversation.Messages.RemoveAt(i--);

                    k_Updates.Enqueue(new MuseChatUpdateData
                    {
                        Type = MuseChatUpdateType.MessageDelete,
                        Message = messageToDelete
                    });

                    // Can't delete fragments we don't have the external id for.
                    // If we don't have the external id, we never received the
                    // response header with the id information, it was likely
                    // cancelled before the server receive it.
                    if (messageIdToDelete.Type == MuseMessageIdType.External)
                    {
                        await WebAPI.DeleteConversationFragment(messageIdToDelete.ConversationId, messageIdToDelete.FragmentId);
                    }
                }

                // Editing a prompt could delete the message that had the context, ensure context is sent again:
                m_LastContextForConversation = new Tuple<MuseConversationId, string>(default, default);

                // Now post the given prompt as a new chat:
                ProcessPrompt(editedPrompt);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public async void ProcessPrompt(string prompt)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                return;
            }

            bool isNewConversation = false;

            // Create a thread if needed
            if (m_ActiveConversation == null)
            {
                isNewConversation = true;
                string conversationTitle = prompt;
                if (conversationTitle.Length > k_MaxInternalConversationTitleLength)
                {
                    conversationTitle = conversationTitle.Substring(0, k_MaxInternalConversationTitleLength) + "...";
                }

                m_ActiveConversation = new MuseConversation
                {
                    Title = conversationTitle,
                    Id = MuseConversationId.GetNextInternalId()
                };

                OnConversationHistoryChanged?.Invoke();

                // Clear old updates as this conversation has changed
                k_Updates.Clear();
                k_Updates.Enqueue(new MuseChatUpdateData
                {
                    Type = MuseChatUpdateType.ConversationChange,
                    IsMusing = true
                });
            }

            AddInternalMessage(prompt, role: k_UserRole, sendUpdate: !isNewConversation);
            AddIncompleteMessage(string.Empty, k_AssistantRole, sendUpdate: !isNewConversation);

            var context = await GetContextString(m_ActiveConversation.Id, MuseChatConstants.PromptContextLimit - prompt.Length, prompt);

            try
            {
                var updateHandler = WebAPI.Chat(prompt, m_ActiveConversation.Id.Value, context);

                updateHandler.InitFromDriver(
                    m_ActiveConversation,
                    delegate(MuseMessageUpdateHandler updater, MuseChatUpdateData updateData)
                    {
                        if (updater.Conversation.Id == m_ActiveConversation?.Id)
                        {
                            k_Updates.Enqueue(updateData);
                        }
                    },
                    delegate(MuseMessageUpdateHandler updater)
                    {
                        // Ensure only the active conversation receives updates
                        if (updater.Conversation.Id == m_ActiveConversation?.Id)
                        {
                            // The updater always has the most up-to-date conversation, replace ours with that:
                            m_ActiveConversation = updater.Conversation;

                            ProcessQueuedUpdates();
                        }
                    },
                    delegate(MuseMessageUpdateHandler updater)
                    {
                        k_MessageUpdaters.Remove(updater);
                    });

                k_MessageUpdaters.Add(updateHandler);
            }
            catch (Exception e)
            {
                ExecuteUpdateImmediate(new MuseChatUpdateData
                {
                    Type = MuseChatUpdateType.NewMessage,
                    Message = new MuseMessage
                    {
                        IsComplete = true,
                        Content = e.Message,
                        Role = k_AssistantRole,
                        ErrorCode = 403,
                        Id = MuseMessageId.GetNextIncompleteId(m_ActiveConversation.Id),
                        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    },
                    IsMusing = false
                });
            }
        }

        /// <summary>
        /// Finds and returns the message updater for the given conversation ID.
        /// </summary>
        MuseMessageUpdateHandler GetUpdaterForConversation(MuseConversationId conversationId)
        {
            for (var i = 0; i < k_MessageUpdaters.Count; i++)
            {
                var updater = k_MessageUpdaters[i];
                if (updater.Conversation.Id == conversationId)
                {
                    return updater;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if there are message updaters with an internal conversation id.
        /// </summary>
        /// <returns>True if there is an updater with an internal ID.</returns>
        bool HasInternalIdUpdaters()
        {
            for (var i = 0; i < k_MessageUpdaters.Count; i++)
            {
                var updater = k_MessageUpdaters[i];
                if (!updater.Conversation.Id.IsValid)
                {
                    return true;
                }
            }

            return false;
        }

        public void AbortPrompt()
        {
            if (m_ActiveConversation == null)
            {
                return;
            }

            // Stop active handler for the conversation:
            GetUpdaterForConversation(m_ActiveConversation.Id)?.Abort();
        }

        public void SendFeedback(MessageFeedback feedback)
        {
            WebAPI.SendFeedback(feedback.Message, m_ActiveConversation.Id.Value, feedback.MessageId.FragmentId, feedback.Sentiment, feedback.Type);
        }

        public void ViewInitialized()
        {
            string lastConvId = UserSessionState.instance.LastActiveConversationId;
            if (!string.IsNullOrEmpty(lastConvId))
            {
                instance.StartConversationLoad(new MuseConversationId(lastConvId));
            }

            StartConversationRefresh();
        }

        async Task<ContextRetrieval> GetContextRetrieval() => m_ContextRetrieval ??= await ContextRetrieval.Create();

        /// <summary>
        /// Get the context string from the selected objects and selected console logs.
        /// </summary>
        /// <param name="maxLength"> The string length limitation. </param>
        /// <param name="contextBuilder"> The context builder reference for temporary context string creation. </param>
        /// <returns></returns>
        internal void GetSelectedContextString(ref ContextBuilder contextBuilder)
        {
            // Grab any selected objects
            if (IsGameObjectSelected && Selection.gameObjects.Length > 0)
            {
                foreach (var currentObject in Selection.gameObjects)
                {
                    var objectContext = new UnityObjectContextSelection();
                    objectContext.SetTarget(currentObject);

                    contextBuilder.InjectContext(objectContext,true);
                }
            }

            // Grab any console logs
            var logs = new List<LogReference>();
            ConsoleUtils.GetSelectedConsoleLogs(logs);
            foreach (var currentLog in logs)
            {
                if ((IsConsoleInfoSelected && currentLog.Mode == LogReference.ConsoleMessageMode.Log)||
                    (IsConsoleWarningSelected && currentLog.Mode == LogReference.ConsoleMessageMode.Warning)||
                    (IsConsoleErrorSelected && currentLog.Mode == LogReference.ConsoleMessageMode.Error))
                {
                    var consoleContext = new ConsoleContextSelection();
                    consoleContext.SetTarget(currentLog);
                    contextBuilder.InjectContext(consoleContext,true);
                }
            }
        }

        internal async Task<string> GetContextString(MuseConversationId conversationId, int maxLength, string prompt)
        {
            // Initialize all context, if any context has changed, add it all
            var contextBuilder = new ContextBuilder();
            GetSelectedContextString(ref contextBuilder);

            // Add retrieved project settings
#if SMART_CONTEXT_V2
            Debug.Log("Started V2 Smart Context Extraction");
            var smartContextResponse = await WebAPI.PostSmartContextAsync(prompt, SmartContextToolbox.GetToolDescriptions());

            foreach (FunctionCall call in smartContextResponse.FunctionCalls)
            {
                if (!SmartContextToolbox.TryRunToolByName(call.Function, call.Parameters.ToArray(),
                        out IContextSelection result))
                {
                    continue;
                }

                Debug.Log($"Called Function {call.Function} with parameters {string.Join(", ", call.Parameters)} and extracted the following :\n\n{result.Payload}");
                contextBuilder.InjectContext(result, false);
            }

            if(smartContextResponse.FunctionCalls.Count == 0)
                Debug.Log("No Smart Context Functions were called");
#else
            var contextRetrieval = await GetContextRetrieval();
            var classifiers = await contextRetrieval.GetClassifiers(prompt, k_TopK, k_MinScore);
            var smartContext = contextRetrieval.GetContext(classifiers.Select(c => c.classifier).ToArray());
            if (smartContext != null)
            {
                for (var i = Math.Min(k_TopK, smartContext.Length) - 1; i >= 0; i--)
                {
                    contextBuilder.InjectContext(smartContext[i], false);
                }
            }
#endif

            var finalContext = contextBuilder.BuildContext(maxLength);

#if !SMART_CONTEXT_V2
            if (conversationId == m_LastContextForConversation.Item1 && m_LastContextForConversation.Item2 == finalContext)
                return string.Empty;

            m_LastContextForConversation = new Tuple<MuseConversationId, string>(conversationId, finalContext);
#else
            Debug.Log($"Final Context ({finalContext.Length} character):\n\n {finalContext}");
#endif

            return finalContext;
        }

        MuseConversation ConvertConversation(ClientConversation remoteConversation)
        {
            var conversationId = new MuseConversationId(remoteConversation.Id);
            MuseConversation localConversation = new()
            {
                Id = conversationId,
                Title = remoteConversation.Title
            };

            foreach (var fragment in remoteConversation.History)
            {
                var message = new MuseMessage
                {
                    Id = new MuseMessageId(conversationId, fragment.Id, MuseMessageIdType.External),
                    IsComplete = true,
                    Role = fragment.Role,
                    Content = fragment.Content,
                    Timestamp = fragment.Timestamp
                };

                localConversation.Messages.Add(message);
            }

            return localConversation;
        }

        bool IsActiveConversationMusing()
        {
            var updater = GetUpdaterForConversation(m_ActiveConversation.Id);

            // If the message is streaming in and has no response yet, set musing to true.
            // If there is an updater with an internal ID, we are musing, but can't be sure for which conversation,
            // if the given conversation's last message is from the user or the last message has no content, we are musing.
            if (updater != null || HasInternalIdUpdaters())
            {
                var lastMessage = m_ActiveConversation.Messages.LastOrDefault();
                return m_ActiveConversation.Messages.Count <= 1 || lastMessage.Role == k_UserRole || string.IsNullOrEmpty(lastMessage.Content);
            }

            return false;
        }

        void ConvertAndPushConversation(ClientConversation conversation)
        {
            m_ActiveConversation = ConvertConversation(conversation);

            OnDataChanged?.Invoke(new MuseChatUpdateData
            {
                IsMusing = IsActiveConversationMusing(),
                Type = MuseChatUpdateType.ConversationChange
            });
        }

        void OnConversationHistoryReceived(IEnumerable<WebAPI.ContextIndicatedConversationInfo> historyData)
        {
            k_History.Clear();
            foreach (WebAPI.ContextIndicatedConversationInfo remoteInfo in historyData)
            {
                var localInfo = new MuseConversationInfo
                {
                    Id = new MuseConversationId(remoteInfo.ConversationId),
                    Title = remoteInfo.Title,
                    LastMessageTimestamp = remoteInfo.LastMessageTimestamp,
                    IsContextual = remoteInfo.IsContextual
                };

                k_History.Add(localInfo);
            }

            OnConversationHistoryChanged?.Invoke();
        }

        void ExecuteUpdateImmediate(MuseChatUpdateData entry)
        {
            k_Updates.Enqueue(entry);
            ProcessQueuedUpdates();
        }

        void ProcessQueuedUpdates()
        {
            while (k_Updates.Count > 0)
            {
                OnDataChanged?.Invoke(k_Updates.Dequeue());
            }
        }

        void OnDisable()
        {
            m_ContextRetrieval?.Dispose();
            m_ContextRetrieval = default;
        }
    }
}
