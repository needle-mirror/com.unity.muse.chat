using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Chat.BackendApi.Api;
using Unity.Muse.Chat.BackendApi.Model;

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        /// <summary>
        /// Builds a <see cref="MuseChatStreamHandler"/> for a chat prompt. This will not send the request immediately,
        /// but can be used to register to events that will occur during the streaming process.
        /// </summary>
        /// <param name="prompt">The chat prompt to send</param>
        /// <param name="conversationID">The conversationID, if this is an ongoing conversation</param>
        /// <param name="context">The context EditorContextModel</param>
        /// <param name="chatCommand">The type of command <see cref="ChatCommandType"/></param>
        /// <param name="extraBody">Extra body parameters to forward to the server</param>
        /// <returns></returns>
        /// <exception cref="Exception">Throws if a valid organization is not found</exception>
        public MuseChatStreamHandler BuildChatStream(string prompt, string conversationID = "", EditorContextReport context = null,
            ChatCommandType chatCommand = ChatCommandType.Ask, Dictionary<string, string> extraBody = null, List<SelectedContextMetadataItems> selectionContext = null)
        {
            if (!GetOrganizationID(out string organizationId))
            {
                throw new Exception("No valid organization found.");
            }

            object options;
            switch (chatCommand)
            {
#if ENABLE_ASSISTANT_BETA_FEATURES
                case ChatCommandType.Run:
                    options = new ActionRequest(organizationId, prompt, true)
                    {
                        Context = context == null ? null : new BackendApi.Model.Context(context),
                        ConversationId = string.IsNullOrWhiteSpace(conversationID) ? null : conversationID,
                        StreamResponse = false,
                        DependencyInformation = UnityDataUtils.GetPackageMap(),
                        ProjectSummary = UnityDataUtils.GetProjectSettingSummary(),
                        UnityVersions = k_UnityVersionField.ToList(),
                        SelectedContextMetadata = selectionContext,
                        Tags = new List<string>(new[] { UnityDataUtils.GetProjectId() }),
                        Debug = false,
                        ExtraBody = extraBody
                    };
                    break;
                case ChatCommandType.Code:
                    options = new CodeGenRequest(organizationId, prompt, true)
                    {
                        Context = context == null ? null : new BackendApi.Model.Context(context),
                        ConversationId = string.IsNullOrWhiteSpace(conversationID) ? null : conversationID,
                        DependencyInformation = UnityDataUtils.GetPackageMap(),
                        ProjectSummary = UnityDataUtils.GetProjectSettingSummary(),
                        UnityVersions = k_UnityVersionField.ToList(),
                        SelectedContextMetadata = selectionContext,
                        Tags = new List<string>(new[] { UnityDataUtils.GetProjectId() }),
                        Debug = false,
                        ExtraBody = extraBody
                    };
                    break;
#endif
                default:
                    options = new ChatRequest(organizationId, prompt, true)
                    {
                        Context = context == null ? null : new BackendApi.Model.Context(context),
                        ConversationId = string.IsNullOrWhiteSpace(conversationID) ? null : conversationID,
                        DependencyInformation = UnityDataUtils.GetPackageMap(),
                        ProjectSummary = UnityDataUtils.GetProjectSettingSummary(),
                        UnityVersions = k_UnityVersionField.ToList(),
                        SelectedContextMetadata = selectionContext,
                        Tags = new List<string>(new[] { UnityDataUtils.GetProjectId() }),
                        ExtraBody = new Dictionary<string, object>
                        {
                            { "enable_plugins", true },
                            { "muse_guard", true },
                            {
                                "mediation_system_prompt", string.IsNullOrWhiteSpace(MuseChatConstants.MediationPrompt)
                                    ? null
                                    : MuseChatConstants.MediationPrompt
                            },
                            { "skip_planning", MuseChatConstants.SkipPlanning }
                        }
                    };
                    break;
            }

            MuseChatBackendApi api = new(CreateConfig());

            MuseChatStreamHandler.MuseChatStreamRequestDelegate request = chatCommand switch
            {
#if ENABLE_ASSISTANT_BETA_FEATURES
                ChatCommandType.Run => api.PostMuseAgentActionV1Builder(options as ActionRequest).Build().SendAsync,
                ChatCommandType.Code => api.PostMuseAgentCodegenV1Builder(options as CodeGenRequest).Build().SendAsync,
#endif
                _ => api.PostMuseChatV1Builder(options as ChatRequest).Build().SendAsync,
            };

            MuseChatStreamHandler updateHandler = new(conversationID,  request);
            return updateHandler;
        }
    }
}
