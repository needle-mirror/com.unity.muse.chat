using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Muse.Chat.Model;
using Unity.Muse.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
        MuseConversation m_ResponseConversation;
        ContextRetrieval m_ContextRetrieval;

        float m_LastRefreshTokenTime;

        public event Action<MuseChatUpdateData> OnDataChanged;
        public event Action<string, bool> OnConnectionChanged;

        /// <summary>
        /// Indicates that the history has changed
        /// </summary>
        public event Action OnConversationHistoryChanged;

        /// <summary>
        /// The WebAPI implementation used to communicate with the Muse Backend.
        /// </summary>
        public WebAPI WebAPI { get; set; } = new();

        string m_LastContext = "";

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
                if (MuseChatEnvironment.instance.DebugModeEnabled && OnRequestDebugHistory != null)
                {
                    return OnRequestDebugHistory.Invoke();
                }

                return k_History;
            }
        }

        void StartPluginConnectUpdate()
        {
            EditorApplication.update -= PluginConnectUpdate;
            EditorApplication.update += PluginConnectUpdate;
        }

        void StopPluginConnectUpdate()
        {
            EditorApplication.update -= PluginConnectUpdate;
        }

        void PluginDisconnect()
        {
            WebAPI.DisconnectSession();
            StopPluginConnectUpdate();
            StopPluginCommunication();
        }

        void StartPluginCommunication()
        {
            EditorApplication.update -= PluginSendUpdate;
            EditorApplication.update -= PluginReceiveUpdate;
            EditorApplication.update += PluginSendUpdate;
            EditorApplication.update += PluginReceiveUpdate;
        }

        void StopPluginCommunication()
        {
            EditorApplication.update -= PluginSendUpdate;
            EditorApplication.update -= PluginReceiveUpdate;
        }

        void PluginConnectUpdate()
        {
            switch (WebAPI.pluginConnectStatus)
            {
                case WebAPI.RequestStatus.Complete:
                {
                    // Start send and receive tasks
                    OnConnectionChanged?.Invoke("Connected", true);
                    StartPluginCommunication();
                }
                break;

                case WebAPI.RequestStatus.Error:
                {
                    // Print error message to UI
                    OnConnectionChanged?.Invoke($"Could not connect to server: {WebAPI.GetConnectError()}", false);
                }
                break;

                case WebAPI.RequestStatus.Empty:
                {
                    // We should never have an empty state while this function is active
                    Debug.LogError("Plugin Update called without a corresponding connection request");
                }
                break;
            }
        }

        void PluginSendUpdate()
        {

        }

        void PluginReceiveUpdate()
        {

        }

        public void ConnectPlugin()
        {
            UnityDataUtils.CachePackageData(true);

            if (!MuseChatEnvironment.instance.PluginModeEnabled)
                return;

            OnConnectionChanged?.Invoke("Connecting...", false);
            StartPluginConnectUpdate();
            EditorApplication.quitting += PluginDisconnect;
            WebAPI.ConnectSession();
        }

        public void DisconnectPlugin()
        {
            StopPluginConnectUpdate();
            PluginDisconnect();
            EditorApplication.quitting -= PluginDisconnect;
        }

        void StartChatUpdate()
        {
            EditorApplication.update -= ChatUpdate;
            EditorApplication.update += ChatUpdate;
        }

        void StopChatUpdate()
        {
            EditorApplication.update -= ChatUpdate;
        }

        void ChangeConversationId(MuseConversationId newId)
        {
            if (m_ResponseConversation.Id == newId)
            {
                return;
            }

            // Change ID of the conversation and all it's current messages
            m_ResponseConversation.Id = newId;
            for (var i = 0; i < m_ResponseConversation.Messages.Count; i++)
            {
                var message = m_ResponseConversation.Messages[i];
                message.Id = new MuseMessageId(m_ResponseConversation.Id, message.Id.FragmentId, message.Id.Type);
                m_ResponseConversation.Messages[i] = message;
            }

            ExecuteUpdateImmediate(new MuseChatUpdateData
            {
                Type = MuseChatUpdateType.ConversationChange
            });
        }

        void ProcessMessageUpdate()
        {
            var messageIndex = m_ResponseConversation.Messages.Count - 1;

            var currentResponse = m_ResponseConversation.Messages[messageIndex];
            currentResponse.Content = WebAPI.GetChatResponseData(out string assistantFragmentId, out string userFragmentId);
            currentResponse.IsComplete = WebAPI.chatStatus is WebAPI.RequestStatus.Complete or WebAPI.RequestStatus.Error;

            if (WebAPI.chatStatus == WebAPI.RequestStatus.Error)
            {
                WebAPI.GetErrorDetails(out currentResponse.ErrorCode, out currentResponse.ErrorText);
                CheckForInvalidAccessToken(currentResponse.ErrorCode, ref currentResponse.ErrorText);
            }

            // Change the response ID to the external server id:
            if (!string.IsNullOrEmpty(assistantFragmentId))
            {
                var completeId = new MuseMessageId(m_ResponseConversation.Id, assistantFragmentId, MuseMessageIdType.External);
                if (completeId != currentResponse.Id)
                {
                    k_Updates.Enqueue(new MuseChatUpdateData
                    {
                        Type = MuseChatUpdateType.MessageIdChange,
                        Message = currentResponse,
                        IsMusing = false,
                        NewMessageId = completeId
                    });
                }

                currentResponse.Id = completeId;
            }
            else if (currentResponse.Id == default)
            {
                currentResponse.Id = MuseMessageId.GetNextIncompleteId(m_ResponseConversation.Id);
            }

            m_ResponseConversation.Messages[messageIndex] = currentResponse;

            // Change the request ID to the external server id:
            if (messageIndex > 0)
            {
                var currentRequest = m_ResponseConversation.Messages[messageIndex - 1];
                if (!string.IsNullOrEmpty(userFragmentId))
                {
                    var userMessageId = new MuseMessageId(m_ResponseConversation.Id, userFragmentId, MuseMessageIdType.External);
                    if (userMessageId != currentRequest.Id)
                    {
                        k_Updates.Enqueue(new MuseChatUpdateData
                        {
                            Type = MuseChatUpdateType.MessageIdChange,
                            Message = currentRequest,
                            IsMusing = false,
                            NewMessageId = userMessageId
                        });
                    }

                    currentRequest.Id = userMessageId;
                }

                m_ResponseConversation.Messages[messageIndex - 1] = currentRequest;
            }

            k_Updates.Enqueue(new MuseChatUpdateData
            {
                Type = MuseChatUpdateType.MessageUpdate,
                Message = currentResponse,
                IsMusing = WebAPI.chatStatus is WebAPI.RequestStatus.Empty or WebAPI.RequestStatus.InProgress
            });
        }

        void ChatUpdate()
        {
            // Check for updates from Muse Chat
            switch (WebAPI.chatStatus)
            {
                case WebAPI.RequestStatus.InProgress:
                {
                    ProcessMessageUpdate();
                }
                break;
                case WebAPI.RequestStatus.Complete:
                case WebAPI.RequestStatus.Error:
                {
                    // If this thread does not yet have an ID, read that in
                    if(!m_ResponseConversation.Id.IsValid)
                    {
                        ChangeConversationId(WebAPI.GetConversationID());
                    }

                    ProcessMessageUpdate();

                    WebAPI.ClearResponse();
                    StartConversationRefresh();
                    StopChatUpdate();

                    if (WebAPI.chatStatus == WebAPI.RequestStatus.Error)
                    {
                        MuseChatView.ShowNotification("Request failed, please try again", PopNotificationIconType.Error);
                    }

                    break;
                }
            }

            // If something changed, call our callback
            ProcessQueuedUpdates();
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

            if (MuseChatEnvironment.instance.DebugModeEnabled && OnRequestDebugConversation != null && OnRequestDebugConversation.Invoke(conversationId, out var debugConversation))
            {
                m_ActiveConversation = debugConversation;

                OnDataChanged?.Invoke(new MuseChatUpdateData
                {
                    IsMusing = false,
                    Type = MuseChatUpdateType.ConversationChange
                });

                return;
            }

			WebAPI.CancelChat();

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

            if (m_ActiveConversation != null && m_ActiveConversation.Id == conversationId)
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

        public void StartGetConversationTopic(MuseConversationId conversationId)
        {
            WebAPI.GetConversationTitle(conversationId.Value,
                EditorLoopUtilities.EditorLoopRegistration,
                suggestedTitle =>
                {
                    if (!string.IsNullOrEmpty(suggestedTitle))
                    {
                        StartConversationRename(conversationId, suggestedTitle.Trim('"'));
                    }
                }, Debug.LogException);
        }

        public void ClearForNewConversation()
        {
            m_ActiveConversation = null;
            m_ResponseConversation = null;

            WebAPI.CancelChat();
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
                Timestamp = DateTime.Now.Ticks
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
                Timestamp = DateTime.Now.Ticks
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
            if (!MuseChatEnvironment.instance.PluginModeEnabled)
            {
                await GetContextString(true, MuseChatConstants.PromptContextLimit, "");
            }
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
            WebAPI.CancelChat();

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
                    Id = default
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

            // Turn on Update Mode
            StartChatUpdate();

            // Send the prompt through the WebAPI
            m_ResponseConversation = m_ActiveConversation;

            var context = !MuseChatEnvironment.instance.PluginModeEnabled ? await GetContextString(isNewConversation, MuseChatConstants.PromptContextLimit - prompt.Length, prompt) : "";

            try
            {
                WebAPI.Chat(prompt, m_ActiveConversation.Id.Value, context);
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
                        Id = MuseMessageId.GetNextIncompleteId(m_ResponseConversation.Id),
                        Timestamp = DateTime.Now.Ticks
                    },
                    IsMusing = false
                });
            }
        }

        public void AbortPrompt()
        {
            if (m_ActiveConversation == null)
            {
                return;
            }

            StopChatUpdate();
            WebAPI.CancelChat();
        }

        public void SendFeedback(MessageFeedback feedback)
        {
            WebAPI.SendFeedback(feedback.Message, m_ActiveConversation.Id.Value, feedback.MessageId.FragmentId, feedback.Sentiment, feedback.Type);
        }

        public void ViewInitialized()
        {
            string lastConvId = MuseChatEnvironment.instance.LastActiveConversationId;
            if (!string.IsNullOrEmpty(lastConvId))
            {
                instance.StartConversationLoad(new MuseConversationId(lastConvId));
            }

            StartConversationRefresh();
        }

        private void CheckForInvalidAccessToken(int errorCode, ref string errorText)
        {
            if (errorCode == 401 && errorText.ToLower().Contains("unauthorized"))
            {
                // Editor access token can expire after a long time, we need to force a refresh
                if (Time.realtimeSinceStartup - m_LastRefreshTokenTime > 1f)
                {
                    UnityConnectUtils.ClearAccessToken();
                    CloudProjectSettings.RefreshAccessToken(_ => {});

                    m_LastRefreshTokenTime = Time.realtimeSinceStartup;
                    errorText = "TRY-REFRESH-TOKEN";
                }
            }
        }

        async Task<ContextRetrieval> GetContextRetrieval() => m_ContextRetrieval ??= await ContextRetrieval.Create();

        internal string GetSelectedContextString(int maxLength, bool isFullContext = false)
        {
            var contextString = new StringBuilder();

            // Grab any selected objects
            if (IsGameObjectSelected && Selection.gameObjects.Length > 0)
            {
                contextString.Append("\n\nHere is the serialization data of user selected game objects:\n");
                foreach (var currentObject in Selection.gameObjects)
                {
                    var objectContext = new UnityObjectContextSelection();
                    objectContext.SetTarget(currentObject);
                    var payload = ((IContextSelection)objectContext).Payload;
                    if (payload != null && (isFullContext || contextString.Length + payload.Length < maxLength))
                    {
                        contextString.Append(payload);
                    }
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
                    var payload = ((IContextSelection)consoleContext).Payload;
                    if (payload != null && (isFullContext || contextString.Length + payload.Length < maxLength))
                    {
                        contextString.Append($"\n\nHere is the user selected console {currentLog.Mode.ToString()}:\n{payload}");
                    }
                }
            }
            return contextString.ToString();
        }

        internal async Task<string> GetContextString(bool newConversation, int maxLength, string prompt)
        {
            if (newConversation)
                m_LastContext = "";

            // Initialize all context, if any context has changed, add it all
            var contextString = new StringBuilder();

            contextString.Append(GetSelectedContextString(maxLength));

            // Add retrieved project settings
            var contextRetrieval = await GetContextRetrieval();
            var classifiers = await contextRetrieval.GetClassifiers(prompt, k_TopK, k_MinScore);
            var smartContext = contextRetrieval.GetContext(classifiers.Select(c => c.classifier).ToArray());
            if (smartContext != null)
            {
                contextString.Append("\nHere are the project settings that might be related to user's question:\n");
                for (var i = Math.Min(k_TopK, smartContext.Length) - 1; i >= 0; i--)
                {
                    var payload = smartContext[i].Payload;
                    if (payload != null && contextString.Length + payload.Length < maxLength)
                    {
                        contextString.Append(payload);
                    }
                }
            }

            var finalContext = contextString.ToString();
            if (m_LastContext == finalContext)
                return "";

            m_LastContext = finalContext;
            return finalContext;
        }

        MuseConversation ConvertConversation(Conversation remoteConversation)
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

        void ConvertAndPushConversation(Conversation conversation)
        {
            m_ActiveConversation = ConvertConversation(conversation);

            OnDataChanged?.Invoke(new MuseChatUpdateData
            {
                IsMusing = false,
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
