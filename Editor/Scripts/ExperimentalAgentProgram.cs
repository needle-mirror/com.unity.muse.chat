using System;
using System.Threading;
using Unity.Muse.Common.Account;
using UnityEngine;

namespace Unity.Muse.Chat
{
    class ExperimentalAgentProgram : IExperimentalProgramInstance
    {
        private WebAPI m_WebApi = new WebAPI();

        internal const string k_ProgramMode = "muse-chat-beta";

        public void Dispose()
        {
        }

        public event Action Changed;

        public void Init()
        {
        }

        public void IsUserAuthorized(Action<bool> callback)
        {
            m_WebApi.CheckBetaEntitlement(EditorLoopUtilities.EditorLoopRegistration, callback);
        }

        public void GetBalance(IExperimentalProgramInstance.GetBalanceCallback callback)
        {
        }

        public void RequestUpdate()
        {
            Changed?.Invoke();
        }
    }
}
