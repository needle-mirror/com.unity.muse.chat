using System;
using System.Collections.Generic;

namespace Unity.Muse.Chat
{
    internal static class MuseChatHistoryBlackboard
    {
        private static readonly IDictionary<MuseConversationId, bool> k_FavoriteCache = new Dictionary<MuseConversationId, bool>();

        public static Action HistoryPanelReloadRequired;
        public static Action HistoryPanelRefreshRequired;

        public static void SetFavoriteCache(MuseConversationId id, bool state)
        {
            k_FavoriteCache[id] = state;
        }

        public static bool GetFavoriteCache(MuseConversationId id)
        {
            if (k_FavoriteCache.TryGetValue(id, out bool state))
            {
                return state;
            }

            return false;
        }
    }
}
