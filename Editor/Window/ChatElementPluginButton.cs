using System.Text;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Editor.Markup;
using UnityEngine;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    internal class ChatElementPluginButton : ManagedTemplate
    {
        Button m_Button;
        Text m_Text;

        /// <summary>
        /// Create a new shared chat element
        /// </summary>
        public ChatElementPluginButton()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        /// <summary>
        /// Set the data for this source element
        /// </summary>
        /// <param name="index">the index of the source</param>
        /// <param name="call">the source block defining the URL and title</param>
        public void SetData(PluginCall call)
        {
            PluginCall = call;
            RefreshDisplay();
        }

        public PluginCall PluginCall { get; private set; }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Text = view.Q<Text>("label");

            m_Button = view.SetupButton("button", OnSourceClicked);
            m_Button.style.width = new StyleLength(StyleKeyword.Auto);
        }

        private void OnSourceClicked(PointerUpEvent evt)
        {
            if(!MuseEditorDriver.instance.PluginToolbox.TryRunToolByName(PluginCall.Function, PluginCall.Parameters))
                Debug.LogWarning($"Failed to call plugin {PluginCall.Label}");
        }

        private void RefreshDisplay()
        {
            m_Text.text = PluginCall.Parameters != null && PluginCall.Parameters.Length > 0
                ? PluginCall.Parameters[0]
                : $"{PluginCall.Function}({ string.Join(", ", PluginCall.Parameters) })";

            m_Button.title = PluginCall.Label;
            m_Button.tooltip = PluginCall.Label;
        }
    }
}
