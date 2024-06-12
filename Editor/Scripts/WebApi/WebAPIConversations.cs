using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.Api;
using Unity.Muse.Chat.Client;
using Unity.Muse.Chat.Model;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        Task m_CurrentConversationsRequest;
        Task<List<ConversationInfo>> m_CurrentContextualConvosRequest;
        Task<List<ConversationInfo>> m_CurrentContextlessConvosRequest;

        public IEnumerable<WebAPI.ContextIndicatedConversationInfo> LastConversations { get; private set; }


        /// <summary>
        /// Starts a task to return conversations. Poll <see cref="GetConversationsData"/> to wait for data to be
        /// received.
        /// </summary>
        /// <remarks>If the request is already in flight, the new <see cref="onComplete"/> callback will be ignored
        /// and the function will return immediately</remarks>
        public virtual void GetConversations(
            ILoopRegistration loop,
            Action<IEnumerable<ContextIndicatedConversationInfo>> onComplete,
            Action<Exception> onError)
        {
            if (m_CurrentConversationsRequest != null)
                return;

            Configuration configuration = CreateConfig();
            DefaultApi api = new(configuration);

            m_CurrentContextualConvosRequest = api.GetConversationsMuseConversationGetAsync(limit: MuseChatConstants.MaxConversationHistory, tags: $"{UnityDataUtils.GetProjectId()}");
            m_CurrentContextlessConvosRequest = api.GetConversationsMuseConversationGetAsync(skipProjectTag: true, limit: MuseChatConstants.MaxConversationHistory);
            m_CurrentConversationsRequest = Task.WhenAll(m_CurrentContextualConvosRequest, m_CurrentContextlessConvosRequest);

            loop.Register(RequestTick);

            void RequestTick()
            {
                // If request is in progress, conversations are empty
                if (m_CurrentConversationsRequest is {IsCompleted: false})
                    return;

                loop.Unregister(RequestTick);

                Task<List<ConversationInfo>> contextualTask = m_CurrentContextualConvosRequest;
                Task<List<ConversationInfo>> contextlessTask = m_CurrentContextlessConvosRequest;

                m_CurrentConversationsRequest = null;
                m_CurrentContextualConvosRequest = null;
                m_CurrentContextlessConvosRequest = null;

                if (contextualTask.IsCompletedSuccessfully && contextlessTask.IsCompletedSuccessfully)
                {
                    var contextInfos = contextualTask.Result
                        .Select(c => new ContextIndicatedConversationInfo(true, c));

                    var contextlessInfos = contextlessTask.Result
                        .Where(c => contextInfos.All(v => v.ConversationId != c.ConversationId))
                        .Select(c => new ContextIndicatedConversationInfo(false, c));

                    CacheLastAndInvokeComplete(contextInfos.Concat(contextlessInfos));
                }
                else
                {
                    if (!contextualTask.IsCompletedSuccessfully && !contextlessTask.IsCompletedSuccessfully)
                        onError?.Invoke(new AggregateException(GetExceptionFromTask(contextualTask),
                            GetExceptionFromTask(contextlessTask)));
                    else
                        onError?.Invoke(GetExceptionFromTask(!contextualTask.IsCompletedSuccessfully
                            ? contextualTask
                            : contextlessTask));
                }
            }

            void CacheLastAndInvokeComplete(IEnumerable<ContextIndicatedConversationInfo> contextlessInfos)
            {
                LastConversations = contextlessInfos;
                onComplete?.Invoke(contextlessInfos);
            }
        }

        string m_CurrentConversationRequestId;
        Task<ResponseGetConversationMuseConversationConversationIdGet> m_CurrentConversationRequest;
        CancellationTokenSource m_CurrentConversationCancellationTokenSource = new();

        /// <summary>
        /// Starts a task to return the data associated with a conversation
        /// </summary>
        /// <remarks>
        /// If this function is called again before a prior call is complete, the prior calls <see cref="onComplete"/>
        /// and <see cref="onError"/> functions will not be triggered. They will be ignored completely.
        ///
        /// If this function is called again with the same <see cref="conversationId"/> before a prior call is complete,
        /// the new calls <see cref="onComplete"/> and <see cref="onError"/> functions will be ignored. The original
        /// call will complete.
        ///
        /// if <see cref="conversationId"/> is null or empty, function returns.
        /// </remarks>
        public void GetConversation(
            string conversationId,
            ILoopRegistration loop,
            Action<ClientConversation> onComplete,
            Action<Exception> onError)
        {
            if (string.IsNullOrEmpty(conversationId))
                return;

            // Noop if a request with the conversation id is already in flight
            if(m_CurrentConversationRequestId == conversationId)
                return;

            // If there is another request will a different conversation id just cancel it. The RequestTick will
            // automatically unregister itself.
            if (m_CurrentConversationRequest != null)
                m_CurrentConversationCancellationTokenSource.Cancel();

            // Send the request
            Configuration configuration = CreateConfig();
            DefaultApi api = new(configuration);
            m_CurrentConversationCancellationTokenSource = new();
            m_CurrentConversationRequestId = conversationId;
            m_CurrentConversationRequest =
                api.GetConversationMuseConversationConversationIdGetAsync(
                    conversationId,
                    m_CurrentConversationCancellationTokenSource.Token
                );

            loop.Register(RequestTick);

            void RequestTick()
            {
                // This occurs when a new call comes in. The old request gets canceled and the RequestTick unregisters
                if (conversationId != m_CurrentConversationRequestId)
                {
                    loop.Unregister(RequestTick);
                    return;
                }

                // If request is in progress, conversations are empty
                if (m_CurrentConversationRequest is {IsCompleted: false})
                    return;

                loop.Unregister(RequestTick);
                Task<ResponseGetConversationMuseConversationConversationIdGet> tsc = m_CurrentConversationRequest;
                m_CurrentConversationRequest = null;
                m_CurrentConversationRequestId = null;

                if (tsc.IsCompletedSuccessfully)
                {
                    ResponseGetConversationMuseConversationConversationIdGet res = tsc.Result;

                    switch (res.ActualInstance)
                    {
                        case ClientConversation c:
                            onComplete?.Invoke(c);
                            break;
                        default:
                            onError?.Invoke(new WebAPIException("Get Conversation Error", res.ActualInstance));
                            break;
                    }
                }
                else
                    onError?.Invoke(GetExceptionFromTask(tsc));
            }
        }

        /// <summary>
        /// Starts a task to return the data associated with a conversation
        /// </summary>
        /// <remarks>
        /// if <see cref="conversationId"/> is null or empty, function returns.
        /// </remarks>
        public void DeleteConversation(
            string conversationId,
            ILoopRegistration loop,
            Action onComplete,
            Action<Exception> onError)
        {
            if (string.IsNullOrEmpty(conversationId))
                return;

            Configuration configuration = CreateConfig();
            DefaultApi api = new(configuration);
            Task<ErrorResponse> tsc =
                api.DeleteConversationMuseConversationConversationIdDeleteAsync(conversationId, CancellationToken.None);

            loop.Register(RequestTick);

            void RequestTick()
            {
                // If request is in progress, conversations are empty
                if (tsc is {IsCompleted: false})
                    return;

                loop.Unregister(RequestTick);

                if (tsc.IsCompletedSuccessfully)
                {
                    ErrorResponse res = tsc.Result;
                    if (res != null)
                    {
                        onError?.Invoke(new WebAPIException("Conversation Delete Exception", res));
                        return;
                    }

                    onComplete?.Invoke();
                }
                else
                    onError?.Invoke(GetExceptionFromTask(tsc));
            }
        }

        /// <summary>
        /// Starts a task to rename a conversation
        /// </summary>
        /// <remarks>
        /// if <see cref="conversationId"/> is null or empty, function returns.
        /// </remarks>
        public void GetConversationTitle(string conversationId, ILoopRegistration loop, Action<string> onComplete, Action<Exception> onError)
        {
            var configuration = CreateConfig();
            if (!GetOrganizationID(out string organizationId))
            {
                return;
            }

            DefaultApi api = new(configuration);
            var tsc = api.GetTopicMuseTopicConversationIdGetAsync(conversationId, organizationId);

            loop.Register(RequestTick);

            void RequestTick()
            {
                if (tsc is {IsCompleted: false})
                    return;

                loop.Unregister(RequestTick);

                if (tsc.IsCompletedSuccessfully)
                {
                    onComplete?.Invoke(tsc.Result);
                }
                else
                    onError?.Invoke(GetExceptionFromTask(tsc));
            }
        }

        public void RenameConversation(string conversationId, string newName, ILoopRegistration loop, Action onComplete,
            Action<Exception> onError)
        {
            if (string.IsNullOrEmpty(conversationId))
                return;

            var configuration = CreateConfig();
            var payload = new ConversationPatchRequest(newName);
            DefaultApi api = new(configuration);
            var tsc = api.PatchConversationMuseConversationConversationIdPatchAsync(conversationId, payload);

            loop.Register(RequestTick);

            void RequestTick()
            {
                // If request is in progress, conversations are empty
                if (tsc is {IsCompleted: false})
                    return;

                loop.Unregister(RequestTick);

                if (tsc.IsCompletedSuccessfully)
                {
                    ErrorResponse res = tsc.Result;
                    if (res != null)
                    {
                        onError?.Invoke(new WebAPIException("Conversation Rename Exception", res));
                        return;
                    }

                    onComplete?.Invoke();
                }
                else
                    onError?.Invoke(GetExceptionFromTask(tsc));
            }
        }

       public Task DeleteConversationFragment(
            MuseConversationId conversationId,
            string fragmentId)
        {
            if (string.IsNullOrEmpty(conversationId.Value) || string.IsNullOrEmpty(fragmentId))
            {
                Debug.LogError($"conversationId and fragmentId need to be set!");
                return null;
            }

            Configuration configuration = CreateConfig();
            DefaultApi api = new(configuration);

            var tsc = api.DeleteConversationFragmentMuseConversationConversationIdFragmentFragmentIdDeleteAsync(conversationId.Value, fragmentId, CancellationToken.None);

            return tsc;
        }
    }
}
