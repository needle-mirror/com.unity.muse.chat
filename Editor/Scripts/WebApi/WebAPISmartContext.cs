using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Api;
using Unity.Muse.Chat.BackendApi.Client;
using Unity.Muse.Chat.BackendApi.Model;
using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0162 // Unreachable code detected

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        public async Task<SmartContextResponse> PostSmartContextAsync(
            string prompt,
            EditorContextReport editorContext,
            List<FunctionDefinition> catalog,
            string conversationId,
            CancellationToken cancellationToken)
        {
            if (!GetOrganizationID(out string organizationId))
                return null;

            var request = new SmartContextRequest(organizationId, prompt)
            {
                ConversationId = conversationId, JsonCatalog = catalog, EditorContext = new EditorContext(editorContext)
            };

            try
            {
                MuseChatBackendApi api = new(CreateConfig());
                return await api.PostSmartContextV1Builder(request)
                    .BuildAndSendAsync(cancellationToken);
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}