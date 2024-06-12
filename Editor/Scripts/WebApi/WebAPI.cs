using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.Api;
using Unity.Muse.Chat.Client;
using Unity.Muse.Chat.Model;
using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        internal enum RequestStatus
        {
            Empty,
            InProgress,
            Complete,
            Error
        }

        [Serializable]
        public class ContextIndicatedConversationInfo : ConversationInfo
        {
            public ContextIndicatedConversationInfo(bool isContextual, ConversationInfo info)
                : base(info.ConversationId, info.Title, info.LastMessageTimestamp)
            {
                IsContextual = isContextual;
            }

            public ContextIndicatedConversationInfo(bool isContextual, string conversationId = default(string), string title = default(string), long lastMessageTimestamp = default(long))
                : base(conversationId, title, lastMessageTimestamp)
            {
                IsContextual = isContextual;
            }

            /// <summary>
            /// Indicated whether this conversation has been created by providing context to the LLM or not. This boils
            /// down to whether or not the chat was started from the web or from the editor.
            /// </summary>
            public bool IsContextual { get; set; }
        }

        static string[] k_UnityVersionField;

        Task m_ConnectTask;

        static WebAPI()
        {
            k_UnityVersionField = new string[1] { UnityDataUtils.GetProjectVersion(UnityDataUtils.VersionDetail.Major) };
        }

        [Serializable]
        public struct SourceBlock
        {
            public string source;
            public string reason;
        }

        static Configuration CreateConfig()
        {
            Configuration config = new()
            {
                BasePath = MuseChatEnvironment.instance.ApiUrl
            };

            config.ApiKey.Add("access_token", MuseChatEnvironment.instance.ApiAccessToken);
            config.DefaultHeaders.Add("Authorization", $"Bearer {CloudProjectSettings.accessToken}");

            return config;
        }

        public string GetConnectError()
        {
            if (m_ConnectTask == null)
                return "No connection attempt made";

            if (m_ConnectTask.IsCompletedSuccessfully)
                return "Success";

            if (m_ConnectTask.IsFaulted)
                return m_ConnectTask.Exception?.Message;

            return "Unknown error";
        }

        Exception GetExceptionFromTask(Task task)
        {
            return task.Exception?.InnerExceptions[0];
        }

        DefaultApi BoostrapAPI(RequestInterceptDelegate requestIntercept, ResponseInterceptDelegate responseIntercept, out CancellationTokenSource cancellationTokenSource)
        {
            var configuration = CreateConfig();
            DefaultApi api = new(configuration);

            // Make sure Intercept Request is added the the event
            api.ApiClient.OnRequestIntercepted -= requestIntercept;
            api.ApiClient.OnRequestIntercepted += requestIntercept;

            api.ApiClient.OnResponseIntercepted -= responseIntercept;
            api.ApiClient.OnResponseIntercepted += responseIntercept;

            cancellationTokenSource = new();

            return api;
        }

        bool GetOrganizationID(out string id)
        {
            id = AccountInfo.Instance.Organization?.Id;

            if (string.IsNullOrWhiteSpace(id))
            {
                Debug.LogWarning("Cannot find a valid organization.");
                return false;
            }

            return true;
        }
    }
}
