using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    internal class ChatElementContextEntry : ManagedTemplate
    {
        Button m_Link;

        /// <summary>
        /// Create a new shared chat element
        /// </summary>
        public ChatElementContextEntry()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        /// <summary>
        /// Set the data for this context element
        /// </summary>
        /// <param name="index">the index of the context</param>
        /// <param name="contextEntry">the entry defining context object</param>
        public void SetData(int index, StubWebAPIContextEntry contextEntry)
        {
            Index = index;
            ContextEntry = contextEntry;
            RefreshDisplay();
        }

        public int Index { get; private set; }

        public StubWebAPIContextEntry ContextEntry { get; private set; }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Link = view.SetupButton("contextLink", OnSourceClicked);
        }

        private void OnSourceClicked(PointerUpEvent evt)
        {
        }

        private void RefreshDisplay()
        {
        }
    }
}
