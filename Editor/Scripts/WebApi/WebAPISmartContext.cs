using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.Api;
using Unity.Muse.Chat.Client;
using Unity.Muse.Chat.Model;
using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0162 // Unreachable code detected

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        public async Task<SmartContextResponse> PostSmartContextAsync(string prompt, List<FunctionDefinition> catalog)
        {
            if (!GetOrganizationID(out string organizationId))
                return null;

            var request = new SmartContextRequest(
                prompt: prompt,
                organizationId: organizationId,
                jsonCatalog: catalog);

            try
            {
                DefaultApi api = new(CreateConfig());
                return await api.SmartContextSmartContextPostAsync(request, CancellationToken.None);
            }
            catch (ApiException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
