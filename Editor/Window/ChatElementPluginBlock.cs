using System.Linq;
using System.Threading.Tasks;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Chat.Plugins;
using Unity.Muse.Editor.Markup;
using UnityEditor;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal class ChatElementPluginBlock : ManagedTemplate
    {
        VisualElement m_BlockRoot;
        Text m_TitleText;

        /// <summary>
        /// Create a new shared chat element
        /// </summary>
        public ChatElementPluginBlock()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        /// <summary>
        /// Set the data for this source element
        /// </summary>
        /// <param name="index">the index of the source</param>
        /// <param name="pluginCalls">the source block defining the URL and title</param>
        public void SetData(PluginCall[] pluginCalls)
        {
            PluginCalls = pluginCalls;
            RefreshDisplay();
        }

        public PluginCall[] PluginCalls { get; private set; }

        protected override void InitializeView(TemplateContainer view)
        {
            m_TitleText = view.Q("titleText") as Text;
            m_BlockRoot = view.Q("blockRoot");
        }

        private void RefreshDisplay()
        {
            if (PluginCalls.Length == 0)
            {
                m_TitleText.text = "Loading Plugin";
                return;
            }

            m_TitleText.text = TEMPDetermineTitleFromFunction(PluginCalls[0].Function);

            RenderPluginCalls();
        }

        void RenderPluginCalls()
        {
            RemoveNonTitleElements();

            foreach (PluginCall call in PluginCalls)
            {
                ChatElementPluginButton button = new();
                button.Initialize();
                button.SetData(TEMPUpdatePluginCall(call));
                m_BlockRoot.Add(button);
            }
        }

        void RemoveNonTitleElements()
        {
            foreach (var element in m_BlockRoot.Children().Where(element => element != m_TitleText).ToArray())
                m_BlockRoot.Remove(element);
        }

        string TEMPDetermineTitleFromFunction(string function)
        {
            switch (function)
            {
                case "GenerateTexture": return "Muse Texture";
                case "GenerateSprite": return "Muse Sprite";
                case "GenerateAnimationsFromPrompt": return "Muse Animate";
                case "TriggerAgentFromPrompt": return "Run this with Muse?";
            }

            return "Plugin";
        }

        PluginCall TEMPUpdatePluginCall(PluginCall call)
        {
            var newCall = call;
            switch (call.Function)
            {
                case "TriggerAgentFromPrompt":
                    newCall.Label = "Run Command";
                    newCall.Parameters[0] = call.Parameters[0].Replace("prompt:", "");
                    break;
            }
            return newCall;
        }
    }
}
