using System;
using System.Linq;
using System.Threading.Tasks;
using Unity.Muse.Chat.BackendApi.Model;
using Unity.Muse.Common.Account;
using UnityEditor;

namespace Unity.Muse.Chat
{
    static class ServerCompatibility
    {
        // Hard code the version this client is compatible with
        const string k_Version = "v1";

        // Assume the server is compatible to being with
        public static CompatibilityStatus Status { get; private set; } = CompatibilityStatus.Supported;

        public static Action<CompatibilityStatus> OnCompatibilityChanged;
        static IAssistantBackend s_AssistantBackend;

        const double k_RefreshRateSeconds = 60 * 2; // Check every 2 minutes for compatibility changes
        static double s_TimeSinceLastRefresh;

        static ServerCompatibility()
        {
            SessionStatus.OnChanges -= Refresh;
            SessionStatus.OnChanges += Refresh;

            EditorApplication.update -= TimedRefresh;
            EditorApplication.update += TimedRefresh;
        }

        public static void SetBackend(IAssistantBackend backend)
        {
            s_AssistantBackend = backend;
        }

        /// <summary>
        /// Binds the event handler to the compatibility changed event. Immediately invokes the event handler with
        /// the current compatibility status.
        /// </summary>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public static void Bind(Action<CompatibilityStatus> eventHandler)
        {
            eventHandler?.Invoke(Status);
            OnCompatibilityChanged += eventHandler;
            Refresh();
        }

        /// <summary>
        /// Check the server compatibility status.
        /// </summary>
        public static void Refresh()
        {
            _ = UpdateCompatibility();
        }

        static async Task UpdateCompatibility()
        {
            try
            {
                if(s_AssistantBackend == null)
                    return;

                var versionInfo = await s_AssistantBackend.GetVersionSupportInfo(k_Version);

                if (versionInfo != null)
                {
                    var relevantVersion = versionInfo
                        .FirstOrDefault(v => v.RoutePrefix == k_Version);

                    if(relevantVersion == default)
                        Status = CompatibilityStatus.Unsupported;
                    else
                        Status = GetCompatibilityStatus(relevantVersion.SupportStatus);

                    OnCompatibilityChanged?.Invoke(Status);
                }
                else
                {
                    Status = CompatibilityStatus.Unknown;
                    OnCompatibilityChanged?.Invoke(Status);
                }
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception _)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                Status = CompatibilityStatus.Unknown;
                OnCompatibilityChanged?.Invoke(Status);
            }

            s_TimeSinceLastRefresh = EditorApplication.timeSinceStartup;
        }

        static CompatibilityStatus GetCompatibilityStatus(VersionSupportInfo.SupportStatusEnum status)
        {
            return status switch
            {
                VersionSupportInfo.SupportStatusEnum.Supported => CompatibilityStatus.Supported,
                VersionSupportInfo.SupportStatusEnum.Deprecated => CompatibilityStatus.Deprecated,
                VersionSupportInfo.SupportStatusEnum.Unsupported => CompatibilityStatus.Unsupported,
                _ => CompatibilityStatus.Unsupported
            };
        }

        static void TimedRefresh()
        {
            if (EditorApplication.timeSinceStartup - s_TimeSinceLastRefresh > k_RefreshRateSeconds)
                Refresh();
        }

        public enum CompatibilityStatus
        {
            /// <summary>
            /// Compatibility status is unknown when the server is not reachable, therefore the server cannot report
            /// its status. This should not be used to block the user and other mechanisms should be used to handle
            /// detecting network issues.
            /// </summary>
            Unknown,

            /// <summary>
            /// The server has reported that this client is supported.
            /// </summary>
            Supported,

            /// <summary>
            /// The server has reported that this client is deprecated.
            /// </summary>
            Deprecated,

            /// <summary>
            /// The server has reported that this client is unsupported.
            /// </summary>
            Unsupported
        }
    }
}
