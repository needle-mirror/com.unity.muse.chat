using System;
using System.Collections.Generic;
using Unity.Muse.Chat.BackendApi.Model;

namespace Unity.Muse.Chat
{
    internal partial class Assistant
    {
        readonly List<MuseChatInspiration> k_InspirationEntries = new();

        /// Indicates that the inspiration entries have changed
        /// </summary>
        public event Action OnInspirationsChanged;

        internal List<MuseChatInspiration> Inspirations => k_InspirationEntries;

        public void RefreshInspirations()
        {
            m_Backend.InspirationRefresh(OnInspirationsReceived);
        }

        public void InspirationUpdate(MuseChatInspiration inspiration)
        {
            m_Backend.InspirationUpdate(inspiration);
        }

        public void InspirationDelete(MuseChatInspiration inspiration)
        {
            m_Backend.InspirationDelete(inspiration);
        }

        void OnInspirationsReceived(IEnumerable<MuseChatInspiration> inspirationData)
        {
            k_InspirationEntries.Clear();
            k_InspirationEntries.AddRange(inspirationData);

            OnInspirationsChanged?.Invoke();
        }
    }
}
