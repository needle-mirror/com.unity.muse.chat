using Unity.Muse.Chat.UI.Components;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal class RoutesPopupEntry : ManagedTemplate
    {
        Label m_LabelElement;
        Label m_DescriptionElement;

        readonly string m_Label;
        readonly string m_Description;

        public RoutesPopupEntry(
            string label,
            string description
        )
            : base(MuseChatConstants.UIModulePath)
        {
            m_Label = label;
            m_Description = description;
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_LabelElement = view.Q<Label>("commandItemText");
            m_LabelElement.text = m_Label;

            m_DescriptionElement = view.Q<Label>("commandItemDescription");
            m_DescriptionElement.text = m_Description;
        }
    }
}
