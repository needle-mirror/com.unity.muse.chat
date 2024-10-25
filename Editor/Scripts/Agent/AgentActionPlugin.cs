using Unity.Muse.Common.Editor.Integration;
using UnityEngine;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Static class which exposes Muse Agent to the plugin system.
    /// </summary>
    static class AgentActionPlugin
    {
        /// <summary>
        /// Triggers Muse Agent based on a prompt string.
        /// </summary>
        /// <param name="prompt">The prompt to send to Muse Agent.</param>
        [Plugin("Plugin for triggering Muse Agent given prompts that can be performed as editor actions, for example, for creating primitives, changing project settings, manipulating the scene hierarchy, etc.")]
        static void TriggerAgentFromPrompt([Parameter("The prompt that will instruct the agent on what editor actions to perform.")] string prompt)
        {
            prompt = "/" + ChatCommandType.Run.ToString().ToLower() + " " + prompt;
            MuseEditorDriver.instance.ProcessPrompt(prompt);
        }
    }
}
