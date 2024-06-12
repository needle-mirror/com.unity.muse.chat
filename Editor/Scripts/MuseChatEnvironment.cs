using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat
{
    [FilePath("MuseChatEnv", FilePathAttribute.Location.PreferencesFolder)]
    internal class MuseChatEnvironment : ScriptableSingleton<MuseChatEnvironment>
    {
        const string k_DefaultApiUrl = "https://rest-api.prd.azure.muse.unity.com";
        const string k_DefaultApiAccessToken = "f5vv0wIGD-iJB3ulhlr5knNaywrccISH_PTOLopnjUk";

        [SerializeField]
        public string ApiUrl = k_DefaultApiUrl;

        [SerializeField]
        public string ApiAccessToken = k_DefaultApiAccessToken;

        internal void SetApi(string apiUrl, string apiAccessToken)
        {
            ApiUrl = apiUrl;
            ApiAccessToken = apiAccessToken;
            Save(true);
        }

        internal void Reset()
        {
            ApiUrl = k_DefaultApiUrl;
            ApiAccessToken = k_DefaultApiAccessToken;
            Save(true);
        }
    }
}
