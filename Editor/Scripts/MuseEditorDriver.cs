using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.Context.SmartContext;
using Unity.Muse.Chat.FunctionCalling;
using Unity.Muse.Chat.BackendApi;
using Unity.Muse.Chat.BackendApi.Model;
using Unity.Muse.Chat.Plugins;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

#if MUSE_INTERNAL
using System.Diagnostics;
using Debug = UnityEngine.Debug;
#endif

namespace Unity.Muse.Chat
{
    internal class MuseEditorDriver : ScriptableSingleton<MuseEditorDriver>
    {
        internal static readonly string k_UserRole = "user";
        internal static readonly string k_AssistantRole = "assistant";

        const int k_MaxInternalConversationTitleLength = 30;

        readonly List<MuseConversationInfo> k_History = new();
        readonly List<MuseChatInspiration> k_InspirationEntries = new();
        readonly Queue<MuseChatUpdateData> k_Updates = new();

        MuseConversation m_ActiveConversation;

        private readonly List<MuseMessageUpdateHandler> k_MessageUpdaters = new();
        internal int MessageUpdatersNum => k_MessageUpdaters.Count;
        private CancellationTokenSource m_SmartContextCancellationTokenSource;
        CancellationTokenSource m_ActiveRequestCancellationTokenSource;
        SmartContextToolbox m_SmartContextToolbox;
        PluginToolbox m_PluginToolbox;
        HashSet<MuseMessageId> m_MessagesUnderRepair = new();

        public event Action<MuseChatUpdateData> OnDataChanged;

#pragma warning disable CS0067 // Event is never used
        public event Action<string, bool> OnConnectionChanged;
#pragma warning restore CS0067

#if MUSE_INTERNAL
        internal event Action<TimeSpan, SmartContextResponse> OnSmartContextCallDone;
        internal event Action<TimeSpan, FunctionCall> OnSmartContextExtracted;
        internal event Action<MuseConversation> OnFinalResponseReceived;
        internal bool IsProcessingConversations => k_MessageUpdaters.Count > 0;
#endif

        /// <summary>
        /// Indicates that the history has changed
        /// </summary>
        public event Action OnConversationHistoryChanged;

        /// <summary>
        /// Indicates the conversation title has changed
        /// </summary>
        public event Action<string> OnConversationTitleChanged;

        /// <summary>
        /// Agent that can executes actions in the project
        /// </summary>
        public MuseAgent Agent { get; } = new();

        /// <summary>
        /// Validator for generated script files
        /// </summary>
        public CodeBlockValidator CodeBlockValidator { get; } = new();

        /// Indicates that the inspiration entries have changed
        /// </summary>
        public event Action OnInspirationsChanged;

        /// <summary>
        /// The WebAPI implementation used to communicate with the Muse Backend.
        /// </summary>
        public WebAPI WebAPI { get; set; } = new();

        public FunctionCache FunctionCache { get; } = new(new AttributeBasedFunctionSource());
        public SmartContextToolbox SmartContextToolbox { get; private set; }
        public PluginToolbox PluginToolbox { get; private set; }


        void OnEnable()
        {
            PluginToolbox = new PluginToolbox(FunctionCache);
            SmartContextToolbox = new SmartContextToolbox(FunctionCache);
        }

        internal delegate bool DebugConversationRequest(MuseConversationId conversationId, out MuseConversation result);
        internal DebugConversationRequest OnRequestDebugConversation;

        internal delegate List<MuseConversationInfo> DebugHistoryRequest();
        internal DebugHistoryRequest OnRequestDebugHistory;

        internal Action<VisualElement> OnDebugTrackMetricsRequest;

        internal List<Object> m_ObjectAttachments;
        internal List<LogReference> m_ConsoleAttachments;

        internal enum PromptState
        {
            None,
            GatheringContext,
            Musing,
            Streaming,
            RepairCode
        }

        internal PromptState CurrentPromptState {get; private set;}


        internal List<MuseConversationInfo> History
        {
            get
            {
                if (UserSessionState.instance.DebugUIModeEnabled && OnRequestDebugHistory != null)
                {
                    return OnRequestDebugHistory.Invoke();
                }

                return k_History;
            }
        }

        internal List<MuseChatInspiration> Inspirations
        {
            get
            {
                return k_InspirationEntries;
            }
        }

        /// <summary>
        /// Starts a request to refresh the list of conversations available. This is non-blocking.
        /// </summary>
        public void StartInspirationRefresh()
        {
            WebAPI.GetInspirations(
                EditorLoopUtilities.EditorLoopRegistration,
                OnInspirationsReceived,
                Debug.LogException
            );
        }

        /// <summary>
        /// Starts a request to refresh the list of conversations available. This is non-blocking.
        /// </summary>
        public void StartConversationRefresh()
        {
            WebAPI.GetConversations(
                EditorLoopUtilities.EditorLoopRegistration,
                OnConversationHistoryReceived,
                Debug.LogException
            );
        }

        /// <summary>
        /// Initiates a reload of the currently active conversation
        /// </summary>
        public void StartConversationReload()
        {
            if (m_ActiveConversation == null)
            {
                return;
            }

            StartConversationLoad(m_ActiveConversation.Id);
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

            if (UserSessionState.instance.DebugUIModeEnabled && OnRequestDebugConversation != null && OnRequestDebugConversation.Invoke(conversationId, out var debugConversation))
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
        /// Starts a webrequest that attempts to rename change the favorite state of a conversation with <see cref="conversationId"/>.
        /// </summary>
        /// <param name="conversationId">If not null or empty function acts as noop.</param>
        /// <param name="isFavorite">New favorite state of the conversation</param>
        public void StartConversationFavoriteToggle(MuseConversationId conversationId, bool isFavorite)
        {
            if (!conversationId.IsValid)
            {
                return;
            }

            WebAPI.SetConversationFavoriteState(conversationId.Value,
                isFavorite,
                EditorLoopUtilities.EditorLoopRegistration,
                null,
                Debug.LogException);
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

                OnConversationTitleChanged?.Invoke(newName);
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

        /// <summary>
        /// Starts a webrequest that attempts to add or update a inspiration.
        /// </summary>
        /// <param name="inspiration">the inspiration data to update.</param>
        public void StartInspirationUpdate(MuseChatInspiration inspiration)
        {
            var externalData = inspiration.ToExternal();
            if (!inspiration.Id.IsValid)
            {
                externalData.Id = null;
                WebAPI.AddInspiration(externalData,
                    EditorLoopUtilities.EditorLoopRegistration,
                    null,
                    Debug.LogException);
            }
            else
            {
                WebAPI.UpdateInspiration(externalData,
                    EditorLoopUtilities.EditorLoopRegistration,
                    null,
                    Debug.LogException);
            }
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

            CurrentPromptState = PromptState.GatheringContext;

            bool isNewConversation = false;

            // Create a thread if needed
            // PATCH NOTES: After a domain reload, if the m_ActiveConversation is null FOR SOME REASON it is no longer
            // null and is instead an empty version of a MuseConversation
            // {Id = null, Title = null, Messages = new List}. This needed patching quickly, but I didn't manage to
            // find the root cause of the issue. This check at least catches the problem
            if (m_ActiveConversation == null || m_ActiveConversation.Messages.Count == 0)
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

                m_ActiveConversation.StartTime = EditorApplication.timeSinceStartup;

                OnConversationHistoryChanged?.Invoke();

                // Clear old updates as this conversation has changed
                k_Updates.Clear();
                k_Updates.Enqueue(new MuseChatUpdateData
                {
                    Type = MuseChatUpdateType.ConversationChange,
                    IsMusing = true
                });

                try
                {
                    var conversation = await WebAPI.PostConversation(FunctionCache.AllFunctionDefinitions);
                    m_ActiveConversation.Id = new MuseConversationId(conversation.Id);
                }
                catch (Exception e)
                {
                    InternalLog.LogException(e);
                    CurrentPromptState = PromptState.None;

                    ExecuteUpdateImmediate(new MuseChatUpdateData
                    {
                        Type = MuseChatUpdateType.NewMessage,
                        Message = new MuseMessage
                        {
                            IsComplete = true,
                            Content = e.Message,
                            Role = k_AssistantRole,
                            ErrorCode = 403,
                            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                        },
                        IsMusing = false
                    });

                    return;
                }
            }
            else
            {
                // Reset start time for next progress bar:
                m_ActiveConversation.StartTime = EditorApplication.timeSinceStartup;
            }

            // Check if the prompt contains a command
            var command = UserSessionState.instance.SelectedCommandMode;
            if (ChatCommandParser.IsCommand(prompt))
                (command, prompt) = ChatCommandParser.Parse(prompt);

            AddInternalMessage(prompt, role: k_UserRole, sendUpdate: true);
            AddIncompleteMessage(string.Empty, k_AssistantRole, sendUpdate: !isNewConversation);

            try
            {
                m_SmartContextCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                var context = await GetContextString(m_ActiveConversation.Id, MuseChatConstants.PromptContextLimit - prompt.Length, prompt, m_SmartContextCancellationTokenSource.Token);

                if (m_SmartContextCancellationTokenSource.IsCancellationRequested)
                {
                    CurrentPromptState = PromptState.None;
                    return;
                }

                CurrentPromptState = PromptState.Musing;

                var updateHandler = WebAPI.Chat(prompt, m_ActiveConversation.Id.Value, context, command);

                m_ActiveRequestCancellationTokenSource = updateHandler.ActiveRequestCancellationTokenSource;

                updateHandler.InitFromDriver(
                    m_ActiveConversation,
                    delegate(MuseMessageUpdateHandler updater, MuseChatUpdateData updateData)
                    {
                        if (updater.Conversation.Id == m_ActiveConversation?.Id)
                        {
                            k_Updates.Enqueue(updateData);

                            // If there is a message in the update, get out of the musing state to hide the musing element:
                            if (updateData.Message.Content.Length > 0)
                            {
                                CurrentPromptState = PromptState.Streaming;
                            }
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

#if MUSE_INTERNAL
                        OnFinalResponseReceived?.Invoke(updater.Conversation);
#endif
                    },
                    isNewConversation);

                k_MessageUpdaters.Add(updateHandler);
            }
            catch (Exception e)
            {
                if (m_ActiveConversation != null)
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
        }


        /// <summary>
        /// Repair the script with the given error and script.
        /// </summary>
        internal async Task<string> RepairScript(MuseMessageId messageId, int messageIndex, string errorToRepair, string scriptToRepair, ScriptType scriptType = ScriptType.AgentAction)
        {
            // Add the message to the list of scripts under repair so it doesn't get repaired twice
            m_MessagesUnderRepair.Add(messageId);
            // Call the repair route and invoke OnCodeRepairComplete event when the repair is done
            var webAPI = new WebAPI();

            CurrentPromptState = PromptState.RepairCode;

            OnDataChanged?.Invoke(new MuseChatUpdateData
            {
                IsMusing = true,
                Type = MuseChatUpdateType.CodeRepair
            });

            var cancellationToken = m_ActiveRequestCancellationTokenSource?.Token ?? new CancellationToken();

            var repairedMessage = await webAPI.CodeRepair(conversationID: messageId.ConversationId.Value,
                messageIndex: messageIndex,errorToRepair: errorToRepair, scriptToRepair: scriptToRepair,
                cancellationToken: cancellationToken, scriptType: scriptType);

            OnDataChanged?.Invoke(new MuseChatUpdateData
            {
                IsMusing = false,
                Type = MuseChatUpdateType.CodeRepair
            });

            m_MessagesUnderRepair.Remove(messageId);
            return repairedMessage as string;
        }

        public bool IsUnderRepair(MuseMessageId messageId)
        {
            return m_MessagesUnderRepair.Contains(messageId);
        }

        /// <summary>
        /// Returns whether or not the given messageId can be sent for repair.
        /// Only the most recent message in the active message can be repaired.
        /// </summary>
        /// <param name="messageId">The id of the message to be repaired</param>
        /// <returns>True if a repair call can be sent for the given message, false otherwise</returns>
        public bool ValidRepairTarget(MuseMessageId messageId)
        {
            // Messages can be repaired if they are the last message in the conversation
            // And there is a token cancellation source from the particular conversation
            if (m_ActiveRequestCancellationTokenSource == null)
                return false;

            return (m_ActiveConversation.Messages.FindIndex(match => match.Id == messageId) == (m_ActiveConversation.Messages.Count - 1));
        }

        /// <summary>
        /// Finds and returns the message updater for the given conversation ID.
        /// </summary>
        internal MuseMessageUpdateHandler GetUpdaterForConversation(MuseConversationId conversationId)
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
            m_SmartContextCancellationTokenSource?.Cancel();
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
            StartInspirationRefresh();
        }

        /// <summary>
        /// Get the context string from the selected objects and selected console logs.
        /// </summary>
        /// <param name="maxLength"> The string length limitation. </param>
        /// <param name="contextBuilder"> The context builder reference for temporary context string creation. </param>
        /// <returns></returns>
        internal void GetAttachedContextString(ref ContextBuilder contextBuilder)
        {
            // Grab any selected objects
            var attachment = GetValidAttachment(m_ObjectAttachments);
            if (attachment.Count > 0)
            {
                foreach (var currentObject in attachment)
                {
                    var objectContext = new UnityObjectContextSelection();
                    objectContext.SetTarget(currentObject);

                    contextBuilder.InjectContext(objectContext,true);
                }
            }

            // Grab any console logs
            if (m_ConsoleAttachments != null)
            {
                foreach (var currentLog in m_ConsoleAttachments)
                {
                    var consoleContext = new ConsoleContextSelection();
                    consoleContext.SetTarget(currentLog);
                    contextBuilder.InjectContext(consoleContext, true);
                }
            }
        }

        internal List<Object> GetValidAttachment(List<Object> contextAttachments)
        {
            if (contextAttachments == null)
                return new List<Object>();

            if (contextAttachments.Any(obj => obj == null))
                return contextAttachments.Where(obj => obj != null).ToList();

            return contextAttachments;
        }

        internal bool HasNullAttachments(List<Object> contextAttachment)
        {
            if (contextAttachment == null)
                return false;

            return contextAttachment.Any(obj => obj == null);
        }

        internal async Task<string> GetContextString(MuseConversationId conversationId, int maxLength, string prompt,
            CancellationToken cancellationToken, bool enableSmartContext = true)
        {
            // Initialize all context, if any context has changed, add it all
            var contextBuilder = new ContextBuilder();
            GetAttachedContextString(ref contextBuilder);

            // Add retrieved project settings
            if (enableSmartContext)
            {
                try
                {
#if MUSE_INTERNAL
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    InternalLog.Log("Started V2 Smart Context Extraction");
#endif

                    var editorContext = contextBuilder.BuildContext(maxLength);
                    var smartContextResponse = await WebAPI.PostSmartContextAsync(prompt, editorContext,
                        SmartContextToolbox.Tools.Select(c => c.FunctionDefinition).ToList(),
                        conversationId.Value,
                        cancellationToken);

#if MUSE_INTERNAL
                    stopwatch.Stop();
                    InternalLog.Log(
                        $"Time taken for smart context call: {stopwatch.Elapsed}");
                    OnSmartContextCallDone?.Invoke(stopwatch.Elapsed, smartContextResponse);
                    stopwatch.Restart();
#endif

                    UnityDataUtils.CachePackageData(false);

                    for (var i = 0; !UnityDataUtils.PackageDataReady() && i < 10; i++)
                    {
                        await Task.Delay(10);
                    }

                    var smartContextMaxLength = maxLength;

                    foreach (FunctionCall call in FunctionCall.Deduplicate(smartContextResponse.FunctionCalls))
                    {
#if MUSE_INTERNAL
                        InternalLog.Log(
                            $"Received Function {call.Function} with parameters {string.Join(", ", call.Parameters)}");
#endif

                        if (!SmartContextToolbox.TryRunToolByName(call.Function, call.Parameters.ToArray(),
                                smartContextMaxLength, out IContextSelection result))
                        {
                            continue;
                        }

                        // We don't know if the downsized or full payload will be used, use lower one for now:
                        smartContextMaxLength = Math.Max(0, smartContextMaxLength - result.DownsizedPayload.Length);

#if MUSE_INTERNAL
                        var info =
                            $"Called Function {call.Function} with parameters {string.Join(", ", call.Parameters)} and extracted the following:\n\n{result.Payload}";
                        InternalLog.Log(info);
                        stopwatch.Stop();
                        InternalLog.Log(
                            $"Time taken for smart context extraction: {stopwatch.Elapsed}");
                        OnSmartContextExtracted?.Invoke(stopwatch.Elapsed, call);
                        stopwatch.Restart();
#endif
                        contextBuilder.InjectContext(result, false);
                    }

                    if (smartContextResponse.FunctionCalls.Count == 0)
                    {
                        InternalLog.Log("No Smart Context Functions were called");
                    }
                }
                catch (Exception e)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return string.Empty;
                    }

                    InternalLog.Log($"Failed to get smart context: {e.Message}");
                }
            }

            var finalContext = contextBuilder.BuildContext(maxLength);

            InternalLog.Log($"Final Context ({finalContext.Length} character):\n\n {finalContext}");

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

            for (var i = 0; i < remoteConversation.History.Count; i++)
            {
                var fragment = remoteConversation.History[i];
                var message = new MuseMessage
                {
                    Id = new MuseMessageId(conversationId, fragment.Id, MuseMessageIdType.External),
                    IsComplete = true,
                    Role = fragment.Role,
                    Author = fragment.Author,
                    Content = fragment.Content,
                    Timestamp = fragment.Timestamp,
                    MessageIndex = i
                };

                localConversation.Messages.Add(message);
            }

            return localConversation;
        }

        bool IsActiveConversationMusing()
        {
            if (HasInternalIdUpdaters())
            {
                // If there is an updater with an internal ID, we are musing, but can't be sure for which conversation,
                return true;
            }

            var updater = GetUpdaterForConversation(m_ActiveConversation.Id);
            if (updater == null)
            {
                // If there is no updater, we are not musing.
                return false;
            }

            if (updater.IsInProgress)
            {
                // If the message is streaming in set musing to true.
                return true;
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

        void OnInspirationsReceived(IEnumerable<Inspiration> inspirationData)
        {
            k_InspirationEntries.Clear();
            foreach (var entry in inspirationData)
            {
                k_InspirationEntries.Add(entry.ToInternal());
            }

            OnInspirationsChanged?.Invoke();
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
                    IsContextual = remoteInfo.IsContextual,
                    IsFavorite = remoteInfo.IsFavorite != null && remoteInfo.IsFavorite.Value
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
    }
}
