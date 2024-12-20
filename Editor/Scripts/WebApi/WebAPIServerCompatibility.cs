using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Api;
using Unity.Muse.Chat.BackendApi.Client;
using Unity.Muse.Chat.BackendApi.Model;

namespace Unity.Muse.Chat
{
    partial class WebAPI
    {
        public async Task<ApiResponse<List<VersionSupportInfo>>> GetServerCompatibility(string version)
        {
            Configuration configuration = CreateConfig();
            MuseChatBackendApi api = new(configuration);
            return await api.GetVersionsBuilder().BuildAndSendAsync(CancellationToken.None);
        }
    }
}
