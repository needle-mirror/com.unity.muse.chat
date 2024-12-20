using System;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat.UI
{
    /// <summary>
    /// Updates elements base on the compatibility of this frontend against the current live server.
    /// </summary>
    class ServerCompatibilityChanges : Manipulator
    {
        Action m_Callback;

        public ServerCompatibilityChanges(Action callback, bool callImmediately = true)
        {
            m_Callback = callback;
            if (callImmediately)
                Refresh(ServerCompatibility.Status);
        }

        protected override void RegisterCallbacksOnTarget()
            => ServerCompatibility.OnCompatibilityChanged += Refresh;

        protected override void UnregisterCallbacksFromTarget()
            => ServerCompatibility.OnCompatibilityChanged -= Refresh;

        void Refresh(ServerCompatibility.CompatibilityStatus compatibilityStatus) => m_Callback?.Invoke();
    }
}
