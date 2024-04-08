using Unity.Muse.AppUI.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    internal class ChatElementSourceEntry : ManagedTemplate
    {
        // TODO: Switch to Text .MeasureString() and truncate better
        const int k_MaxTextLength = 42;

        Button m_SourceLink;
        Text m_SourceLinkHint;
        LocalizedTextElement m_SourceLinkTextElement;

        /// <summary>
        /// Create a new shared chat element
        /// </summary>
        public ChatElementSourceEntry()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        /// <summary>
        /// Set the data for this source element
        /// </summary>
        /// <param name="index">the index of the source</param>
        /// <param name="sourceBlock">the source block defining the URL and title</param>
        public void SetData(int index, WebAPI.SourceBlock sourceBlock)
        {
            Index = index;
            SourceBlock = sourceBlock;
            RefreshDisplay();
        }

        public int Index { get; private set; }

        public WebAPI.SourceBlock SourceBlock { get; private set; }

        protected override void InitializeView(TemplateContainer view)
        {
            m_SourceLink = view.SetupButton("sourceLink", OnSourceClicked);
            m_SourceLinkTextElement = m_SourceLink.Q<LocalizedTextElement>("appui-button__title");
            m_SourceLinkTextElement.style.flexWrap = Wrap.NoWrap;
            m_SourceLinkTextElement.RegisterCallback<GeometryChangedEvent>(_ => RefreshDisplay());

            m_SourceLinkHint = view.Q<Text>("sourceLinkHint");
        }

        private void OnSourceClicked(PointerUpEvent evt)
        {
            Application.OpenURL(SourceBlock.source);
        }

        private void RefreshDisplay()
        {
            string titleText = $"{Index + 1}. {SourceBlock.reason}".GetTextWithMaxLength(k_MaxTextLength);

            m_SourceLink.title = titleText;
            m_SourceLink.tooltip = SourceBlock.source;

            m_SourceLinkHint.text = SourceBlock.source;
        }
    }
}
