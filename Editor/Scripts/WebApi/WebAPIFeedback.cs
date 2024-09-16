using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Client;
using Unity.Muse.Chat.BackendApi.Model;
using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0162 // Unreachable code detected

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        FeedbackRequestOperation m_ActiveFeedbackRequestOperation;

        public void SendFeedback(string text, string conversationID, string conversationFragmentId,
            Sentiment sentiment, Category feedbackType)
        {
            if (!GetOrganizationID(out string organizationId))
            {
                return;
            }

            if (MuseChatConstants.DebugMode)
            {
                return;
            }

            var request = new Feedback(
                details: text,
                conversationId: conversationID,
                conversationFragmentId: conversationFragmentId,
                sentiment: sentiment,
                organizationId: organizationId,
                category: feedbackType);

            try
            {
                var api = BoostrapAPI(InterceptFeedbackRequest, InterceptFeedbackResponse, out var cancellationTokenSource);

                // Construct a wrapper object that groups important resources
                m_ActiveFeedbackRequestOperation = new FeedbackRequestOperation
                {
                    Request = request,
                    CancellationTokenSource = cancellationTokenSource
                };

                // Start the request task, this should cause the Intercept code to
                // populate m_ActiveRequest with a UnityWebRequest
                Task requestTask = api.PostMuseFeedbackV1Async(request, cancellationTokenSource.Token);

                EditorApplication.update += Tick;

                void Tick()
                {
                    if(!requestTask.IsCompleted)
                        return;

                    if (!requestTask.IsCompletedSuccessfully)
                    {
                        if(requestTask.Exception != null)
                            Debug.LogException(requestTask.Exception);
                        else
                            Debug.LogError("Send Feedback Request failed");
                    }

                    EditorApplication.update -= Tick;
                }

                // Add the task too
                m_ActiveFeedbackRequestOperation.Task = requestTask;
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void InterceptFeedbackResponse(UnityWebRequest request, string path, RequestOptions ops, IReadableConfiguration config, object obj)
        {
            m_ActiveFeedbackRequestOperation.ConversationId = m_ActiveFeedbackRequestOperation.WebRequest.GetResponseHeader("X-Muse-Conversation-ID");
            m_ActiveFeedbackRequestOperation.FinalData = m_ActiveFeedbackRequestOperation.WebRequest.downloadHandler.text;
            m_ActiveFeedbackRequestOperation.IsComplete = true;
        }

        private void InterceptFeedbackRequest(UnityWebRequest request, string path, RequestOptions ops, IReadableConfiguration config)
        {
            // The ops.Data variable should be the options used to create the request which can be used to verify that
            // the UnityWebRequest is being linked to the correct object
            if (m_ActiveFeedbackRequestOperation.Request == ops.Data)
                m_ActiveFeedbackRequestOperation.WebRequest = request;
            else
                Debug.LogError($"The Request {m_ActiveFeedbackRequestOperation.Request} and {ops.Data} do not match. This means that the active request is not the same as the request being intercepted. This should not happen.");
        }

        class FeedbackRequestOperation
        {
            public Feedback Request;
            public Task Task;
            public UnityWebRequest WebRequest;
            public string ConversationId;
            public string FinalData;
            public bool IsComplete;
            public CancellationTokenSource CancellationTokenSource;
        }
    }
}
