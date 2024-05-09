using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.Api;
using Unity.Muse.Chat.Client;
using Unity.Muse.Chat.Model;
using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0162 // Unreachable code detected

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        ChatRequestOperation m_ActiveChatRequestOperation;

        public virtual RequestStatus chatStatus
        {
            get
            {
                if (MuseChatConstants.DebugMode)
                {
                    if (m_DebugMessage != null)
                    {
                        return m_DebugMessage.RequestStatus;

                    }
                    return RequestStatus.Empty;
                }

                if (m_ActiveChatRequestOperation == null)
                    return RequestStatus.Empty;

                // Task completion indicates that the whole request has completed
                if (m_ActiveChatRequestOperation.Task.IsCompletedSuccessfully)
                {
                    return RequestStatus.Complete;
                }
                else if (m_ActiveChatRequestOperation.Task.IsCanceled || m_ActiveChatRequestOperation.Task.IsFaulted)
                {
                    return RequestStatus.Error;
                }

                return RequestStatus.InProgress;
            }
        }

        public void GetErrorDetails(out int errorCode, out string errorText)
        {
            if (chatStatus != RequestStatus.Error)
            {
                errorCode = 0;
                errorText = null;
                return;
            }

            if (m_ActiveChatRequestOperation.Task.Exception?.InnerException is not ApiException apiException)
            {
                errorCode = 1;

                if (m_ActiveChatRequestOperation.Task.Exception?.InnerException is ConnectionException connectionException)
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

        public void CancelChat()
        {
            if (m_ActiveChatRequestOperation == null)
            {
                return;
            }

            Debug.Log($"Canceling last request");
            m_ActiveChatRequestOperation.CancellationTokenSource.Cancel();
            m_ActiveChatRequestOperation = null;
        }

        public void Chat(string prompt, string conversationID = "", string context = "")
        {
            CancelChat();

            if (!GetOrganizationID(out string organizationId))
            {
                throw new Exception("No valid organization found.");
            }

            AssistantModelsMuseRequestsChatRequest options = new(
                prompt: prompt,
                context: string.IsNullOrWhiteSpace(context) ? null : context,
                streamResponse: true,
                conversationId: string.IsNullOrWhiteSpace(conversationID) ? null : conversationID,
                organizationId: organizationId,
                dependencyInformation: UnityDataUtils.GetPackageMap(),
                projectSummary: UnityDataUtils.GetProjectSettingSummary(),
                unityVersions: k_UnityVersionField.ToList(),
                mediationSystemPrompt: string.IsNullOrWhiteSpace(MuseChatConstants.MediationPrompt) ? null : MuseChatConstants.MediationPrompt,
                skipPlanning: MuseChatConstants.SkipPlanning,
                tags: new List<string>(new[] { UnityDataUtils.GetProjectId() })
            );

            if (MuseChatConstants.DebugMode)
            {
                m_DebugMessage = new DebugResponse();
                m_DebugMessage.Send();
                return;
            }

            try
            {
                var api = BoostrapAPI(InterceptChatRequest, InterceptChatResponse, out var cancellationTokenSource);

                // Construct a wrapper object that groups important resources
                m_ActiveChatRequestOperation = new ChatRequestOperation()
                {
                    Options = options,
                    CancellationTokenSource = cancellationTokenSource
                };

                // Start the request task, this should cause the Intercept code to
                // populate m_ActiveRequest with a UnityWebRequest
                Task request = api.ChatMuseChatPostAsync(options, cancellationTokenSource.Token);

                // Add the task too
                m_ActiveChatRequestOperation.Task = request;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public MuseConversationId GetConversationID()
        {
            if (MuseChatConstants.DebugMode)
            {
                return new MuseConversationId("Debug");
            }

            if (m_ActiveChatRequestOperation == null)
            {
                Debug.LogWarning("Can't get conversation ID - no active request");
                return default;
            }

            MuseConversationId conversationId = new MuseConversationId(m_ActiveChatRequestOperation.ConversationId);

            if (MuseChatEnvironment.instance.DebugModeEnabled)
            {
                Debug.Log($"Conversation started with ID: {conversationId}");
            }

            MuseEditorDriver.instance.StartGetConversationTopic(conversationId);
            return conversationId;
        }

        public virtual string GetChatResponseData(out string assistantFragmentId, out string userFragmentId)
        {
            assistantFragmentId = null;
            userFragmentId = null;
            if (MuseChatConstants.DebugMode)
                return m_DebugMessage.GetResponseData();

            if (m_ActiveChatRequestOperation == null)
                return "Error - no active request";

            // No need to wait until the request is complete, headers can arrive before streaming finished:
            ExtractChatRequestIdInfo();

            if (!string.IsNullOrEmpty(m_ActiveChatRequestOperation.AssistantMessageFragmentId))
            {
                assistantFragmentId = m_ActiveChatRequestOperation.AssistantMessageFragmentId;
            }

            if (!string.IsNullOrEmpty(m_ActiveChatRequestOperation.UserMessageFragmentId))
            {
                userFragmentId = m_ActiveChatRequestOperation.UserMessageFragmentId;
            }

            if (m_ActiveChatRequestOperation.IsComplete)
            {
                return m_ActiveChatRequestOperation.FinalData;
            }

            if (m_ActiveChatRequestOperation.Task.IsCompleted && !m_ActiveChatRequestOperation.Task.IsCompletedSuccessfully)
                return GetErrorStringFromTask(m_ActiveChatRequestOperation.Task);

            if (m_ActiveChatRequestOperation.WebRequest == null)
                return string.Empty;

            try
            {
                return m_ActiveChatRequestOperation.WebRequest.downloadHandler.text;
            }
            catch (Exception)
            {
                // When an error occurs in the m_ActiveChatRequestOperation.Task, the underlying UnityWebRequest is
                // disposed. This can lead to an exception when trying to access the objects properties.
            }

            return "";
        }

        void ExtractChatRequestIdInfo()
        {
            try
            {
                if (m_ActiveChatRequestOperation.WebRequest.GetResponseHeaders() == null)
                {
                    return;
                }

                m_ActiveChatRequestOperation.ConversationId = m_ActiveChatRequestOperation.WebRequest.GetResponseHeader("x-muse-conversation-id");
                m_ActiveChatRequestOperation.AssistantMessageFragmentId = m_ActiveChatRequestOperation.WebRequest.GetResponseHeader("x-muse-response-conversation-fragment-id");
                m_ActiveChatRequestOperation.UserMessageFragmentId = m_ActiveChatRequestOperation.WebRequest.GetResponseHeader("x-muse-user-prompt-conversation-fragment-id");
            }
            catch
            {
                // ignored
            }
        }

        void InterceptChatResponse(UnityWebRequest request, string path, RequestOptions ops, IReadableConfiguration config, object obj)
        {
            ExtractChatRequestIdInfo();
            m_ActiveChatRequestOperation.FinalData = m_ActiveChatRequestOperation.WebRequest.downloadHandler.text;
            m_ActiveChatRequestOperation.IsComplete = true;
        }

        void InterceptChatRequest(UnityWebRequest request, string path, RequestOptions ops, IReadableConfiguration config)
        {
            // The ops.Data variable should be the options used to create the request which can be used to verify that
            // the UnityWebRequest is being linked to the correct object
            if (m_ActiveChatRequestOperation.Options == ops.Data)
                m_ActiveChatRequestOperation.WebRequest = request;
            else
                Debug.LogError($"The options {m_ActiveChatRequestOperation.Options} and {ops.Data} do not match. This means that the active request is not the same as the request being intercepted. This should not happen.");
        }

        class ChatRequestOperation
        {
            public object Options;
            public Task Task;
            public UnityWebRequest WebRequest;
            public string ConversationId;
            public string AssistantMessageFragmentId;
            public string UserMessageFragmentId;
            public string FinalData;
            public bool IsComplete;
            public CancellationTokenSource CancellationTokenSource;
        }
    }
}
