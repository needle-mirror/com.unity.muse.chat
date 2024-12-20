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

            var request = new ActionCodeRepairRequest(errorToRepair, messageIndex, organizationId, scriptToRepair, true)
            {
                ConversationId = string.IsNullOrWhiteSpace(conversationID) ? null : conversationID,
                Tags = new List<string>(new[] { UnityDataUtils.GetProjectId() }),
                Debug = false,
                UserPrompt = userPrompt,
                ScriptType = scriptType,
                ExtraBody = extraBody
            };
            {
                MuseChatBackendApi api = new(CreateConfig());
                var result = await api.PostMuseAgentCodeRepairV1Builder(request)
                    .BuildAndSendAsync(cancellationToken);
                return result.Data;
            }
        }
    }
}
