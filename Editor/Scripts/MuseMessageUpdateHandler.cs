using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Muse.Chat.Client;
using Unity.Muse.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Encapsulates the update handling of a chat request.
    /// </summary>
    internal sealed class MuseMessageUpdateHandler
    {
        #region Fields and Props

        private readonly WebAPI k_WebAPI;

        internal MuseConversation Conversation { get; private set; }
        private WebAPI.ChatRequestOperation m_ChatRequestOperation;
        private Action<MuseMessageUpdateHandler> m_UpdateCallback;
        private Action<MuseMessageUpdateHandler> m_CompletedCallback;
        private Action<MuseMessageUpdateHandler, MuseChatUpdateData> m_QueueUpdateCallback;

        private static float s_LastRefreshTokenTime;

        private bool NeedsTopic { get; set; }

        public bool IsInProgress => ChatStatus == WebAPI.RequestStatus.InProgress;

        private WebAPI.RequestStatus ChatStatus
        {
            get
            {
                if (m_ChatRequestOperation == null)
                    return WebAPI.RequestStatus.Empty;

                // Task completion indicates that the whole request has completed
                if (m_ChatRequestOperation.Task.IsCompletedSuccessfully)
                {
                    return WebAPI.RequestStatus.Complete;
                }

                if (m_ChatRequestOperation.Task.IsCanceled || m_ChatRequestOperation.Task.IsFaulted)
                {
                    return WebAPI.RequestStatus.Error;
                }

                return WebAPI.RequestStatus.InProgress;
            }
        }

        #endregion

        #region Initialization

        public MuseMessageUpdateHandler(WebAPI webAPI)
        {
            k_WebAPI = webAPI;
        }

        /// <summary>
        /// Initializes from data the Web API can provide.
        /// </summary>
        /// <param name="chatRequestOperation">The chat request operation this handler is for.</param>
        public void InitFromWebAPI(WebAPI.ChatRequestOperation chatRequestOperation)
        {
            m_ChatRequestOperation = chatRequestOperation;
        }

        /// <summary>
        /// Initializes from data the driver can provide.
        /// </summary>
        /// <param name="conversation">The conversation the request is for.</param>
        /// <param name="queueUpdateCallback">Callback to enqueue a message update.</param>
        /// <param name="updateCallback">Callback to process the enqueued message updates.</param>
        /// <param name="completedCallback">Called back when the request has been completed</param>
        /// <param name="needsTopic">Whether a topic should be set, needed for new conversations.</param>
        public void InitFromDriver(
            MuseConversation conversation,
            Action<MuseMessageUpdateHandler, MuseChatUpdateData> queueUpdateCallback,
            Action<MuseMessageUpdateHandler> updateCallback,
            Action<MuseMessageUpdateHandler> completedCallback,
            bool needsTopic)
        {
            Conversation = conversation;
            m_QueueUpdateCallback = queueUpdateCallback;
            m_UpdateCallback = updateCallback;
            m_CompletedCallback = completedCallback;
            NeedsTopic = needsTopic;
        }

        #endregion

        #region Update handling

        /// <summary>
        /// Schedule updating for this handler.
        /// </summary>
        internal void Start()
        {
            EditorApplication.update -= ChatUpdate;
            EditorApplication.update += ChatUpdate;
        }

        /// <summary>
        /// Stops scheduled updates for this handler.
        /// </summary>
        private void Stop()
        {
            EditorApplication.update -= ChatUpdate;

            m_CompletedCallback?.Invoke(this);
        }

        /// <summary>
        /// Cancels the current request and stops this handler.
        /// </summary>
        internal void Abort()
        {
            m_ChatRequestOperation.CancellationTokenSource.Cancel();

            Stop();
        }

        /// <summary>
        /// Updates local data from server data.
        /// </summary>
        private void ChatUpdate()
        {
            // Check for updates from Muse Chat
            switch (ChatStatus)
            {
                case WebAPI.RequestStatus.InProgress:
                {
                    ProcessMessageUpdate();
                }
                    break;
                case WebAPI.RequestStatus.Complete:
                case WebAPI.RequestStatus.Error:
                {
                    // If this thread does not yet have a topic, set it now:
                    if (NeedsTopic)
                    {
                        StartGetConversationTopic(Conversation.Id);
                    }

                    ProcessMessageUpdate();

                    if (ChatStatus == WebAPI.RequestStatus.Error)
                    {
                        MuseChatView.ShowNotification("Request failed, please try again",
                            PopNotificationIconType.Error);
                    }

                    // The request is finished, unregister update:
                    Stop();

                    break;
                }
            }

            // If something changed, call our callback
            m_UpdateCallback?.Invoke(this);
        }

        /// <summary>
        /// Updates the current conversation with data received from server.
        /// </summary>
        private void ProcessMessageUpdate()
        {
            var messageIndex = Conversation.Messages.Count - 1;
            if (messageIndex < 0)
            {
                return;
            }

            var currentResponse = Conversation.Messages[messageIndex];
            currentResponse.Content =
                GetChatResponseData(out string assistantFragmentId, out string userFragmentId);
            currentResponse.IsComplete =
                ChatStatus is WebAPI.RequestStatus.Complete
                    or WebAPI.RequestStatus.Error;

            if (ChatStatus == WebAPI.RequestStatus.Error)
            {
                GetErrorDetails(
                    out currentResponse.ErrorCode,
                    out currentResponse.ErrorText);
                CheckForInvalidAccessToken(currentResponse.ErrorCode, ref currentResponse.ErrorText);
            }

            // Change the response ID to the external server id:
            if (!string.IsNullOrEmpty(assistantFragmentId))
            {
                var completeId = new MuseMessageId(
                    Conversation.Id,
                    assistantFragmentId,
                    MuseMessageIdType.External);

                if (completeId != currentResponse.Id)
                {
                    m_QueueUpdateCallback?.Invoke(this,
                        new MuseChatUpdateData
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
                currentResponse.Id = MuseMessageId.GetNextIncompleteId(Conversation.Id);
            }

            Conversation.Messages[messageIndex] = currentResponse;

            // Change the request ID to the external server id:
            if (messageIndex > 0)
            {
                var currentRequest = Conversation.Messages[messageIndex - 1];
                if (!string.IsNullOrEmpty(userFragmentId))
                {
                    var userMessageId = new MuseMessageId(Conversation.Id, userFragmentId,
                        MuseMessageIdType.External);
                    if (userMessageId != currentRequest.Id)
                    {
                        m_QueueUpdateCallback?.Invoke(this,
                            new MuseChatUpdateData
                            {
                                Type = MuseChatUpdateType.MessageIdChange,
                                Message = currentRequest,
                                IsMusing = false,
                                NewMessageId = userMessageId
                            });
                    }

                    currentRequest.Id = userMessageId;
                }

                Conversation.Messages[messageIndex - 1] = currentRequest;
            }

            m_QueueUpdateCallback?.Invoke(this,
                new MuseChatUpdateData
                {
                    Type = MuseChatUpdateType.MessageUpdate,
                    Message = currentResponse,
                    IsMusing = ChatStatus is WebAPI.RequestStatus.Empty or WebAPI.RequestStatus.InProgress
                });
        }

        #endregion

        #region Conversation logic

        /// <summary>
        /// Changes the ID of the current conversation.
        /// </summary>
        private void ChangeConversationId(MuseConversationId newId)
        {
            if (Conversation.Id == newId)
            {
                return;
            }

            // Change ID of the conversation and all its current messages
            Conversation.Id = newId;
            for (var i = 0; i < Conversation.Messages.Count; i++)
            {
                var message = Conversation.Messages[i];
                message.Id = new MuseMessageId(Conversation.Id, message.Id.FragmentId, message.Id.Type);
                Conversation.Messages[i] = message;
            }

            m_QueueUpdateCallback?.Invoke(this,
                new MuseChatUpdateData { Type = MuseChatUpdateType.ConversationChange });
            m_UpdateCallback?.Invoke(this);
        }

        /// <summary>
        /// Called back when a chat request received its final response.
        /// </summary>
        internal void InterceptChatResponse(UnityWebRequest request, string path, RequestOptions ops,
            IReadableConfiguration config, object obj)
        {
            ExtractChatRequestIdInfo();
            m_ChatRequestOperation.FinalData = m_ChatRequestOperation.WebRequest.downloadHandler.text;
            m_ChatRequestOperation.IsComplete = true;
        }

        /// <summary>
        /// Called back when a chat request was sent.
        /// </summary>
        internal void InterceptChatRequest(UnityWebRequest request, string path, RequestOptions ops,
            IReadableConfiguration config)
        {
            // The ops.Data variable should be the options used to create the request which can be used to verify that
            // the UnityWebRequest is being linked to the correct object
            if (m_ChatRequestOperation.Options == ops.Data)
                m_ChatRequestOperation.WebRequest = request;
            else
                Debug.LogError(
                    $"The options {m_ChatRequestOperation.Options} and {ops.Data} do not match. This means that the active request is not the same as the request being intercepted. This should not happen.");
        }

        /// <summary>
        /// Extracts fragment IDs and messages from the current chat operation.
        /// </summary>
        /// <param name="assistantFragmentId">Set to the assistant's message fragment ID or null if not set.</param>
        /// <param name="userFragmentId">Set to the user's message fragment ID or null if not set.</param>
        /// <returns>The response text that has been streamed in.</returns>
        private string GetChatResponseData(out string assistantFragmentId, out string userFragmentId)
        {
            assistantFragmentId = null;
            userFragmentId = null;

            if (m_ChatRequestOperation == null)
                return "Error - no active request";

            // No need to wait until the request is complete, headers can arrive before streaming finished:
            ExtractChatRequestIdInfo();

            if (!string.IsNullOrEmpty(m_ChatRequestOperation.AssistantMessageFragmentId))
            {
                assistantFragmentId = m_ChatRequestOperation.AssistantMessageFragmentId;
            }

            if (!string.IsNullOrEmpty(m_ChatRequestOperation.UserMessageFragmentId))
            {
                userFragmentId = m_ChatRequestOperation.UserMessageFragmentId;
            }

            if (m_ChatRequestOperation.IsComplete)
            {
                return m_ChatRequestOperation.FinalData;
            }

            if (m_ChatRequestOperation.Task.IsCompleted && !m_ChatRequestOperation.Task.IsCompletedSuccessfully)
                return GetErrorStringFromTask(m_ChatRequestOperation.Task);

            if (m_ChatRequestOperation.WebRequest == null)
                return string.Empty;

            try
            {
                return m_ChatRequestOperation.WebRequest.downloadHandler.text;
            }
            catch (Exception)
            {
                // When an error occurs in the m_ActiveChatRequestOperation.Task, the underlying UnityWebRequest is
                // disposed. This can lead to an exception when trying to access the objects properties.
            }

            return "";
        }

        /// <summary>
        /// Gets conversation and fragment identifiers from the response headers.
        /// </summary>
        private void ExtractChatRequestIdInfo()
        {
            try
            {
                if (m_ChatRequestOperation.WebRequest.GetResponseHeaders() == null)
                {
                    return;
                }

                m_ChatRequestOperation.ConversationId =
                    m_ChatRequestOperation.WebRequest.GetResponseHeader("x-muse-conversation-id");
                m_ChatRequestOperation.AssistantMessageFragmentId =
                    m_ChatRequestOperation.WebRequest.GetResponseHeader("x-muse-response-conversation-fragment-id");
                m_ChatRequestOperation.UserMessageFragmentId =
                    m_ChatRequestOperation.WebRequest.GetResponseHeader("x-muse-user-prompt-conversation-fragment-id");

                // If this thread does not yet have an ID, use the one from the header:
                if (!Conversation.Id.IsValid)
                {
                    var newId = new MuseConversationId(m_ChatRequestOperation.ConversationId);
                    if (newId.IsValid)
                    {
                        NeedsTopic = true;
                        ChangeConversationId(newId);

                        // If we got a new conversation id, the new conversation now exists on the server and should be shown in the history panel:
                        MuseEditorDriver.instance.StartConversationRefresh();
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Gets a suggested conversation title and renames the conversation to that.
        /// </summary>
        /// <param name="conversationId">Id of the conversation to rename</param>
        private void StartGetConversationTopic(MuseConversationId conversationId)
        {
            k_WebAPI.GetConversationTitle(conversationId.Value,
                EditorLoopUtilities.EditorLoopRegistration,
                suggestedTitle =>
                {
                    if (!string.IsNullOrEmpty(suggestedTitle))
                    {
                        MuseEditorDriver.instance.StartConversationRename(conversationId, suggestedTitle.Trim('"'));
                    }
                }, Debug.LogException);
        }

        #endregion

        #region Error handling

        /// <summary>
        /// Refreshes the access token if we receive any "unauthorized" errors.
        /// </summary>
        /// <param name="errorCode">The error code received from server</param>
        /// <param name="errorText">The error text received from server, will be overwritten for "unauthorized" errors</param>
        private static void CheckForInvalidAccessToken(int errorCode, ref string errorText)
        {
            if (errorCode == 401 && errorText.ToLower().Contains("unauthorized"))
            {
                // Editor access token can expire after a long time, we need to force a refresh
                if (Time.realtimeSinceStartup - s_LastRefreshTokenTime > 1f)
                {
                    UnityConnectUtils.ClearAccessToken();
                    CloudProjectSettings.RefreshAccessToken(_ => { });

                    s_LastRefreshTokenTime = Time.realtimeSinceStartup;
                    errorText = "TRY-REFRESH-TOKEN";
                }
            }
        }

        /// <summary>
        /// Returns the error message for the exception in the given task.
        /// </summary>
        /// <param name="task">Task with error</param>
        private static string GetErrorStringFromTask(Task task)
        {
            return task.Exception?.InnerExceptions[0].Message ?? "Something went wrong";
        }

        /// <summary>
        /// Sets error code and text from any exception in the current chat operation.
        /// </summary>
        private void GetErrorDetails(out int errorCode, out string errorText)
        {
            if (m_ChatRequestOperation.Task.Exception?.InnerException is not ApiException apiException)
            {
                errorCode = 1;

                if (m_ChatRequestOperation.Task.Exception?.InnerException is ConnectionException connectionException)
                {
                    errorText = connectionException.Error;
                }
                else
                {
                    errorText = null;
                }

                return;
            }

            errorCode = apiException.ErrorCode;
            errorText = apiException.ErrorText;
        }

        #endregion
    }
}
