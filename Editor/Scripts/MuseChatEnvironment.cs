using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat
{
    [FilePath("MuseChatEnv", FilePathAttribute.Location.PreferencesFolder)]
    internal class MuseChatEnvironment : ScriptableSingleton<MuseChatEnvironment>
    {
        const string k_DefaultApiUrl = "https://rest-api.prd.azure.muse.unity.com";
        const string k_DefaultApiAccessToken = "f5vv0wIGD-iJB3ulhlr5knNaywrccISH_PTOLopnjUk";
        const string k_DefaultPluginEditorUrl = "ws://rest-api.prd.azure.muse.unity.com/editor/session";

        const string k_HistoryOpen = "MuseChatEnvironment_HistoryOpen";
        const string k_LastActiveConversationId = "MuseChatEnvironment_LastActiveConversationId";
        const string k_ScrollPosition = "MuseChatEnvironment_ScrollPosition";

        [SerializeField]
        public string ApiUrl = k_DefaultApiUrl;

        [SerializeField]
        public string ApiAccessToken = k_DefaultApiAccessToken;

        [SerializeField]
        public bool PluginModeEnabled;

        [SerializeField]
        public string PluginEditorUrl = k_DefaultPluginEditorUrl;

        [SerializeField]
        public bool DebugModeEnabled;

        public bool HasActiveSession => IsHistoryOpen || LastActiveConversationId != null;

        public bool IsHistoryOpen
        {
            get => SessionState.GetBool(k_HistoryOpen, false);
            set => SessionState.SetBool(k_HistoryOpen, value);
        }

        public string LastActiveConversationId
        {
            get => SessionState.GetString(k_LastActiveConversationId, null);
            set => SessionState.SetString(k_LastActiveConversationId, value);
        }

        public float? ScrollPosition
        {
            get
            {
                var v = SessionState.GetFloat(k_ScrollPosition, float.NaN);
                if (float.IsNaN(v))
                {
                    return null;
                }

                return v;
            }

            set => SessionState.SetFloat(k_ScrollPosition, value ?? float.NaN);
        }

        /// <summary>
        /// Clears data that should survive domain reload.
        /// </summary>
        public void ClearSessionState()
        {
            IsHistoryOpen = false;
            LastActiveConversationId = null;
            ScrollPosition = null;
        }

        internal void SetApi(string apiUrl, string apiAccessToken, bool pluginModeEnabled, string pluginEditorUrl)
        {
            ApiUrl = apiUrl;
            ApiAccessToken = apiAccessToken;
            PluginModeEnabled = pluginModeEnabled;
            PluginEditorUrl = pluginEditorUrl;
            Save(true);
        }

        internal void Reset()
        {
            ApiUrl = k_DefaultApiUrl;
            ApiAccessToken = k_DefaultApiAccessToken;
            PluginModeEnabled = false;
            PluginEditorUrl = k_DefaultPluginEditorUrl;
            DebugModeEnabled = false;
            Save(true);
        }
    }
}
