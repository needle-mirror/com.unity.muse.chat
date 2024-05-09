using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.Api;
using Unity.Muse.Chat.Client;
using Unity.Muse.Chat.Model;
using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine;

#pragma warning disable CS0162 // Unreachable code detected

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
        ClientWebSocket m_ClientSocket;

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

        public void ClearResponse()
        {
            m_DebugMessage = null;
            m_ActiveChatRequestOperation = null;
        }

        public RequestStatus pluginConnectStatus
        {
            get
            {
                if (MuseChatConstants.DebugMode)
                {
                    return RequestStatus.Complete;
                }

                if (m_ConnectTask == null)
                {
                    if (m_ClientSocket != null)
                    {
                        if (m_ClientSocket.State == WebSocketState.Open)
                            return RequestStatus.Complete;
                    }

                    return RequestStatus.Empty;
                }

                if (!m_ConnectTask.IsCompleted)
                    return RequestStatus.InProgress;

                if (m_ConnectTask.IsCompletedSuccessfully)
                {
                    return RequestStatus.Complete;
                }

                return RequestStatus.Error;
            }
        }

        public void ConnectSession()
        {
            if (m_ClientSocket == null)
            {
                if (MuseChatEnvironment.instance.DebugModeEnabled)
                {
                    Debug.Log("Creating socket...");
                }

                m_ClientSocket = new ClientWebSocket();
            }

            if (MuseChatEnvironment.instance.DebugModeEnabled)
            {
                Debug.Log("Connecting to server...");
            }

            m_ConnectTask = m_ClientSocket.ConnectAsync(new Uri(MuseChatEnvironment.instance.PluginEditorUrl), CancellationToken.None);
        }

        public void DisconnectSession(bool requested = true)
        {
            if (m_ClientSocket == null)
                return;

            Debug.Log($"Muse Editor disconnecting, requested: {requested}");
            switch (m_ClientSocket.State)
            {
                case WebSocketState.Open:
                    m_ClientSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Editor disconnect", CancellationToken.None);
                    break;
                case WebSocketState.Connecting:
                    m_ClientSocket.Abort();
                    break;

                case WebSocketState.CloseSent:
                case WebSocketState.Closed:
                case WebSocketState.None:
                case WebSocketState.Aborted:
                    // We're just disposing
                    break;
            }

            m_ClientSocket.Dispose();
            m_ClientSocket = null;
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

        string GetErrorStringFromTask(Task task)
        {
            return task.Exception?.InnerExceptions[0].Message ?? "Something went wrong";
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
