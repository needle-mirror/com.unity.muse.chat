using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Api;
using Unity.Muse.Chat.BackendApi.Model;

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        public async Task<Object> CodeRepair(int messageIndex, string errorToRepair,
            string scriptToRepair, CancellationToken cancellationToken, string conversationID = null,
            ScriptType scriptType = ScriptType.AgentAction, Dictionary<string, string> extraBody = null,
            string userPrompt = null)
        {
            if (!GetOrganizationID(out string organizationId))
            {
                throw new Exception("No valid organization found.");
            }

            var request = new ActionCodeRepairRequest(
                conversationId: string.IsNullOrWhiteSpace(conversationID) ? null : conversationID,
                streamResponse: true,
                organizationId: organizationId,
                messageIndex: messageIndex,
                errorToRepair: errorToRepair,
                scriptToRepair: scriptToRepair,
                scriptType: scriptType,
                debug: false,
                userPrompt: userPrompt,
                extraBody: extraBody
            );

            {
                MuseChatBackendApi api = new(CreateConfig());
                var result = await api.PostMuseAgentCodeRepairV1Async(request, cancellationToken);
                return result.Data;
            }
        }
    }
}
