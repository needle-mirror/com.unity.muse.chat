using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Api;
using Unity.Muse.Chat.BackendApi.Client;
using UnityEngine;
using Object = System.Object;

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        Task<ApiResponse<object>> m_CurrentBetaEntitlementCheckRequest;
        List<Action<bool>> m_BetaEntitlementCheckCallbacks = new();

        public void CheckBetaEntitlement(ILoopRegistration loop, Action<bool> onComplete)
        {
            try
            {
                m_BetaEntitlementCheckCallbacks.Add(onComplete);

                // UI calls this from many different places, avoid lots of simultaneous calls:
                if (m_CurrentBetaEntitlementCheckRequest != null)
                {
                    return;
                }

                var configuration = CreateConfig();
                MuseChatBackendApi api = new(configuration);
                m_CurrentBetaEntitlementCheckRequest = api.GetMuseBetaCheckEntitlementV1Builder()
                    .BuildAndSendAsync(m_CurrentInspirationCancellationTokenSource.Token);

                loop.Register(RequestTick);

                void RequestTick()
                {
                    if (m_CurrentBetaEntitlementCheckRequest is { IsCompleted: false })
                        return;

                    loop.Unregister(RequestTick);

                    var entitled = false;
                    if (m_CurrentBetaEntitlementCheckRequest.IsCompletedSuccessfully)
                    {
                        // If there is no error, we are authorized:
                        entitled = m_CurrentBetaEntitlementCheckRequest.Result.StatusCode == HttpStatusCode.OK;
                    }

                    foreach (var callback in m_BetaEntitlementCheckCallbacks)
                    {
                        callback?.Invoke(entitled);
                    }

                    m_BetaEntitlementCheckCallbacks.Clear();
                    m_CurrentBetaEntitlementCheckRequest = null;
                }
            }
            catch (ApiException e)
            {
                Debug.LogException(e);
                throw;
            }
        }
    }
}
