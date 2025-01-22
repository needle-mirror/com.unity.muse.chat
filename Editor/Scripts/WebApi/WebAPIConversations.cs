using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Api;
using Unity.Muse.Chat.BackendApi.Client;
using Unity.Muse.Chat.BackendApi.Model;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        Task m_CurrentConversationsRequest;
        Task<ApiResponse<List<ConversationInfo>>> m_CurrentContextualConvosRequest;
        Task<ApiResponse<List<ConversationInfo>>> m_CurrentContextlessConvosRequest;

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
            MuseChatBackendApi api = new(configuration);

            m_CurrentContextualConvosRequest = api.GetMuseConversationV1Builder()
                .SetLimit(MuseChatConstants.MaxConversationHistory)
                .SetTags($"{UnityDataUtils.GetProjectId()}")
                .BuildAndSendAsync();


            m_CurrentContextlessConvosRequest = api.GetMuseConversationV1Builder()
                .SetSkipProjectTag(true)
                .SetLimit(MuseChatConstants.MaxConversationHistory)
                .BuildAndSendAsync();

            m_CurrentConversationsRequest =
                Task.WhenAll(m_CurrentContextualConvosRequest, m_CurrentContextlessConvosRequest);

            loop.Register(RequestTick);

            void RequestTick()
            {
                // If request is in progress, conversations are empty
                if (m_CurrentConversationsRequest is { IsCompleted: false })
                    return;

                loop.Unregister(RequestTick);

                var contextualTask = m_CurrentContextualConvosRequest;
                var contextlessTask = m_CurrentContextlessConvosRequest;

                m_CurrentConversationsRequest = null;
                m_CurrentContextualConvosRequest = null;
                m_CurrentContextlessConvosRequest = null;

                if (contextualTask.IsCompletedSuccessfully && contextlessTask.IsCompletedSuccessfully)
                {
                    var contextInfos = contextualTask.Result.Data;
                    var contextlessInfos = contextlessTask.Result.Data;

                    List<ContextIndicatedConversationInfo> infos = new();

                    // if there are context Infos, there is a comparision to do
                    if (contextInfos != null)
                    {
                        infos.AddRange(contextInfos.Select(c => new ContextIndicatedConversationInfo(true, c)));

                        if (contextlessInfos != null)
                        {
                            // Make sure the there are no duplicates. m_CurrentContextlessConvosRequest infos picks up all conversations
                            var deduplicatedInfos = contextlessInfos
                                .Where(c => contextInfos.All(v => v.ConversationId != c.ConversationId))
                                .Select(c => new ContextIndicatedConversationInfo(false, c));

                            infos.AddRange(deduplicatedInfos);
                        }
                    }
                    else if (contextlessInfos != null)
                    {
                        infos.AddRange(contextlessInfos
                            .Select(c => new ContextIndicatedConversationInfo(false, c)));
                    }

                    CacheLastAndInvokeComplete(infos);
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
        Task<ApiResponse<ResponseGetMuseConversationUsingConversationIdV1>> m_CurrentConversationRequest;
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
            if (m_CurrentConversationRequestId == conversationId)
                return;

            // If there is another request will a different conversation id just cancel it. The RequestTick will
            // automatically unregister itself.
            if (m_CurrentConversationRequest != null)
                m_CurrentConversationCancellationTokenSource.Cancel();

            // Send the request
            Configuration configuration = CreateConfig();
            MuseChatBackendApi api = new(configuration);
            m_CurrentConversationCancellationTokenSource = new();
            m_CurrentConversationRequestId = conversationId;
            m_CurrentConversationRequest = api.GetMuseConversationUsingConversationIdV1Builder(conversationId)
                .BuildAndSendAsync(m_CurrentConversationCancellationTokenSource.Token);

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
                if (m_CurrentConversationRequest is { IsCompleted: false })
                    return;

                loop.Unregister(RequestTick);
                var tsc = m_CurrentConversationRequest;
                m_CurrentConversationRequest = null;
                m_CurrentConversationRequestId = null;

                if (tsc.IsCompletedSuccessfully)
                {
                    ResponseGetMuseConversationUsingConversationIdV1 res = tsc.Result;

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
        /// Starts a task to delete the data associated with a conversation
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
            MuseChatBackendApi api = new(configuration);
            Task<ApiResponse<ErrorResponse>> tsc =
                api.DeleteMuseConversationUsingConversationIdV1Builder(conversationId)
                    .BuildAndSendAsync(CancellationToken.None);

            loop.Register(RequestTick);

            void RequestTick()
            {
                // If request is in progress, conversations are empty
                if (tsc is { IsCompleted: false })
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

        CancellationTokenSource m_CurrentPostConversationCancellationTokenSource = new();

        /// <summary>
        /// Starts a task to post a conversation. This is used to provide data that is relevant to the conversation
        /// scope and return a conversation in response.
        /// </summary>
        public async Task<Conversation> PostConversation(List<FunctionDefinition> functions)
        {
            if (!GetOrganizationID(out string organizationId))
            {
                return null;
            }

            // If there is another request will a different conversation id just cancel it. The RequestTick will
            // automatically unregister itself.
            m_CurrentPostConversationCancellationTokenSource?.Cancel();
            m_CurrentPostConversationCancellationTokenSource = new();

            // Send the request
            Configuration configuration = CreateConfig();
            MuseChatBackendApi api = new(configuration);


            return await api.PostMuseConversationV1Builder(
                    new CreateConversationRequest(organizationId) { FunctionCatalog = functions })
                .BuildAndSendAsync(m_CurrentConversationCancellationTokenSource.Token);
        }

        /// <summary>
        /// Starts a task to rename a conversation
        /// </summary>
        /// <remarks>
        /// if <see cref="conversationId"/> is null or empty, function returns.
        /// </remarks>
        public void GetConversationTitle(string conversationId, ILoopRegistration loop, Action<string> onComplete,
            Action<Exception> onError)
        {
            var configuration = CreateConfig();
            if (!GetOrganizationID(out string organizationId))
            {
                return;
            }

            MuseChatBackendApi api = new(configuration);
            var tsc = api.GetMuseTopicUsingConversationIdV1Builder(conversationId, organizationId)
                .BuildAndSendAsync();

            loop.Register(RequestTick);

            void RequestTick()
            {
                if (tsc is { IsCompleted: false })
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

        public void SetConversationFavoriteState(string conversationId, bool favoriteState, ILoopRegistration loop,
            Action onComplete, Action<Exception> onError)
        {
            if (string.IsNullOrEmpty(conversationId))
                return;

            var configuration = CreateConfig();
            var payload = new ConversationPatchRequest { IsFavorite = favoriteState };
            MuseChatBackendApi api = new(configuration);
            var tsc = api.PatchMuseConversationUsingConversationIdV1Builder(conversationId, payload)
                .BuildAndSendAsync();

            loop.Register(RequestTick);

            void RequestTick()
            {
                // If request is in progress, conversations are empty
                if (tsc is { IsCompleted: false })
                    return;

                loop.Unregister(RequestTick);

                if (tsc.IsCompletedSuccessfully)
                {
                    ErrorResponse res = tsc.Result;
                    if (res != null)
                    {
                        onError?.Invoke(new WebAPIException("Conversation Favorite State change Exception", res));
                        return;
                    }

                    onComplete?.Invoke();
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
            var payload = new ConversationPatchRequest() { Title = newName };
            MuseChatBackendApi api = new(configuration);
            var tsc = api.PatchMuseConversationUsingConversationIdV1Builder(conversationId, payload)
                .BuildAndSendAsync();

            loop.Register(RequestTick);

            void RequestTick()
            {
                // If request is in progress, conversations are empty
                if (tsc is { IsCompleted: false })
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
            MuseChatBackendApi api = new(configuration);

            var tsc = api
                .DeleteMuseConversationFragmentUsingConversationIdAndFragmentIdV1Builder(conversationId.Value,
                    fragmentId)
                .BuildAndSendAsync(CancellationToken.None);

            return tsc;
        }
    }
}
