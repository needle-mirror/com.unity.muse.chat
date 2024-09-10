using System;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    internal class MuseChatInspirationEntry : ManagedTemplate
    {
        private MuseChatInspiration m_Value;
        private Button m_Button;

        public MuseChatInspirationEntry()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public event Action<MuseChatInspiration> Clicked;

        public MuseChatInspiration Value
        {
            get => m_Value;

            set
            {
                m_Value = value;
                m_Button.title = value.Value;
            }
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Button = view.SetupButton("inspirationEntryButton", OnEntryClicked);
        }

        private void OnEntryClicked(PointerUpEvent evt)
        {
            Clicked?.Invoke(m_Value);
        }
    }
}
