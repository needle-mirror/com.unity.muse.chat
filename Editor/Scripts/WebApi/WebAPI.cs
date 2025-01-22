using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Api;
using Unity.Muse.Chat.BackendApi.Client;
using Unity.Muse.Chat.BackendApi.Model;
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
            {
                ConversationId = info.ConversationId;
                Title = info.Title;
                LastMessageTimestamp = info.LastMessageTimestamp;
                IsContextual = isContextual;
                IsFavorite = info.IsFavorite;
            }

            public ContextIndicatedConversationInfo(bool isContextual, string conversationId = default(string), string title = default(string), long lastMessageTimestamp = default(long))
            {
                ConversationId = conversationId;
                Title = title;
                LastMessageTimestamp = lastMessageTimestamp;
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

        /// <summary>
        /// There are cases where the access_token fails to be set in the CloudProjectSettings correctly. This is often
        /// to do with tests. The AccessTokenBackup is a fallback in the case that the CloudProjectSettings access_token
        /// is null, the AccessTokenBackup will be used instead. If that is also null, the request will fail. It is for
        /// internal purposes only.
        /// </summary>
        internal static string AccessTokenBackup { get; set; }

        /// <summary>
        /// Similarly to the <see cref="AccessTokenBackup"/> variable, this is a backup value that can be set internally
        /// for situations where the OrganizationId is not set correctly. It is for internal purposes only.
        /// </summary>
        internal static string OrganizationIdBackup { get; set; }

        static WebAPI()
        {
            k_UnityVersionField = new [] { UnityDataUtils.GetProjectVersion(UnityDataUtils.VersionDetail.Revision) };
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

            config.DefaultHeaders.Add("Authorization", $"Bearer {GetAccessToken()}");
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

        MuseChatBackendApi BoostrapAPI(out CancellationTokenSource cancellationTokenSource)
        {
            var configuration = CreateConfig();
            MuseChatBackendApi api = new(configuration);
            cancellationTokenSource = new();
            return api;
        }

        bool GetOrganizationID(out string id)
        {
            id = AccountInfo.Instance.Organization?.Id;

            if (string.IsNullOrWhiteSpace(id))
            {
                id = OrganizationIdBackup;
                if (string.IsNullOrWhiteSpace(id))
                {
                    Debug.LogWarning("Cannot find a valid organization.");
                    return false;
                }
            }

            return true;
        }

        static string GetAccessToken()
        {
            string token = CloudProjectSettings.accessToken;

            if (string.IsNullOrWhiteSpace(token))
                token = AccessTokenBackup;

            return token;
        }
    }
}
