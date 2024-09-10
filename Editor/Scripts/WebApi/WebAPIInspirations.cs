using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Api;
using Unity.Muse.Chat.BackendApi.Client;
using Unity.Muse.Chat.BackendApi.Model;
using UnityEngine;

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        string m_CurrentInspirationRequestId;
        Task<ApiResponse<ResponseGetMuseInspirationV1>> m_CurrentInspirationRequest;
        CancellationTokenSource m_CurrentInspirationCancellationTokenSource = new();

        /// <summary>
        /// Starts a task to return inspirations. Poll <see cref="GetInspirationsData"/> to wait for data to be
        /// received.
        /// </summary>
        /// <remarks>If the request is already in flight, the new <see cref="onComplete"/> callback will be ignored
        /// and the function will return immediately</remarks>
        public virtual void GetInspirations(
            ILoopRegistration loop,
            Action<IEnumerable<Inspiration>> onComplete,
            Action<Exception> onError)
        {
            if (m_CurrentInspirationRequest != null)
                return;

            Configuration configuration = CreateConfig();
            MuseChatBackendApi api = new(configuration);

            m_CurrentInspirationRequest = api.GetMuseInspirationV1Async(limit: MuseChatConstants.MaxInspirationCount, cancellationToken: this.m_CurrentInspirationCancellationTokenSource.Token);

            loop.Register(RequestTick);

            void RequestTick()
            {
                // If request is in progress, conversations are empty
                if (m_CurrentInspirationRequest is {IsCompleted: false})
                    return;

                loop.Unregister(RequestTick);

                var entries = m_CurrentInspirationRequest.Result.Data.GetList();

                m_CurrentInspirationRequest = null;

                CacheLastAndInvokeComplete(entries);
            }

            void CacheLastAndInvokeComplete(IEnumerable<Inspiration> entries)
            {
                onComplete?.Invoke(entries);
            }
        }

        CancellationTokenSource m_CurrentPostInspirationCancellationTokenSource = new();

        /// <summary>
        /// Starts a task to add a new inspiration. This is used to provide data that is relevant to the conversation
        /// scope and return a conversation in response.
        /// </summary>
        internal void AddInspiration(Inspiration inspiration,
            ILoopRegistration loop,
            Action<Inspiration> onComplete,
            Action<Exception> onError)
        {
            if (!GetOrganizationID(out string organizationId))
            {
                return;
            }

            // If there is another request will a different conversation id just cancel it. The RequestTick will
            // automatically unregister itself.
            m_CurrentPostInspirationCancellationTokenSource?.Cancel();
            m_CurrentPostInspirationCancellationTokenSource = new();

            // Send the request
            Configuration configuration = CreateConfig();
            MuseChatBackendApi api = new(configuration);
            var tsc = api.PostMuseInspirationV1Async(
                inspiration,
                m_CurrentInspirationCancellationTokenSource.Token
            );

            loop.Register(RequestTick);

            void RequestTick()
            {
                // If request is in progress, conversations are empty
                if (tsc is {IsCompleted: false})
                    return;

                loop.Unregister(RequestTick);

                if (tsc.IsCompletedSuccessfully)
                {
                    ApiResponse<ResponsePostMuseInspirationV1> res = tsc.Result;
                    if (res == null)
                    {
                        onError?.Invoke(new WebAPIException("Inspiration Add Exception", res));
                        return;
                    }

                    onComplete?.Invoke(res.Data.GetInspiration());
                }
                else
                    onError?.Invoke(GetExceptionFromTask(tsc));
            }
        }

        /// <summary>
        /// Starts a task to update a inspiration. This is used to provide data that is relevant to the conversation
        /// scope and return a conversation in response.
        /// </summary>
        internal void UpdateInspiration(Inspiration inspiration,
            ILoopRegistration loop,
            Action<Inspiration> onComplete,
            Action<Exception> onError)
        {
            if (!GetOrganizationID(out string organizationId))
            {
                return;
            }

            // If there is another request will a different conversation id just cancel it. The RequestTick will
            // automatically unregister itself.
            m_CurrentPostInspirationCancellationTokenSource?.Cancel();
            m_CurrentPostInspirationCancellationTokenSource = new();

            // Send the request
            Configuration configuration = CreateConfig();
            MuseChatBackendApi api = new(configuration);

            throw new NotImplementedException("Requires API Updates to avoid Migrating different enums (ModeEnum) with different implementations");
            /*var tsc = api.PutMuseInspirationUsingInspirationIdV1Async(
                inspiration.Id,
                new UpdateInspirationRequest(inspiration.Mode, inspiration.Value, inspiration.Description),
                m_CurrentInspirationCancellationTokenSource.Token
            );

            loop.Register(RequestTick);

            void RequestTick()
            {
                // If request is in progress, conversations are empty
                if (tsc is {IsCompleted: false})
                    return;

                loop.Unregister(RequestTick);

                if (tsc.IsCompletedSuccessfully)
                {
                    ApiResponse<ResponsePutMuseInspirationUsingInspirationIdV1> res = tsc.Result;
                    if (res == null)
                    {
                        onError?.Invoke(new WebAPIException("Inspiration Update Exception", res));
                        return;
                    }

                    onComplete?.Invoke(res.Data.GetInspiration());
                }
                else
                    onError?.Invoke(GetExceptionFromTask(tsc));
            }*/
        }
    }
}
