using System;
using Unity.Muse.AppUI.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;
using TextOverflow = UnityEngine.UIElements.TextOverflow;

namespace Unity.Muse.Chat
{
    internal class SelectionElement : AdaptiveListViewEntry
    {
        Text m_Text;
        Icon m_Icon;
        Button m_Button;
        VisualElement m_Checkmark;

        SelectionPopup.ListEntry m_Entry;

        public Action<SelectionElement> OnAddRemoveButtonClicked;
        bool m_IsSelected;

        readonly string k_PrefabInSceneStyleClass = "mui-chat-selection-prefab-text-color";

        protected override void InitializeView(TemplateContainer view)
        {
            m_Text = view.Q<Text>("selectionElementText");
            m_Icon = view.Q<Icon>("selectionElementIcon");
            m_Button = view.Q<Button>("selectionElementAddRemoveButton");
            m_Checkmark = view.Q<VisualElement>("mui-selection-element-checkmark");

            m_Button.visible = false;

            m_Text.style.overflow = Overflow.Hidden;
            m_Text.style.whiteSpace = WhiteSpace.NoWrap;

            view.RegisterCallback<MouseEnterEvent>(MouseEntered);
            view.RegisterCallback<MouseLeaveEvent>(MouseLeft);
        }

        private void MouseEntered(MouseEnterEvent evt)
        {
            if (!m_IsSelected)
                m_Button.visible = true;
        }

        private void MouseLeft(MouseLeaveEvent evt)
        {
            if (!m_IsSelected)
                m_Button.visible = false;
        }

        public override void SetData(int index, object data, bool isSelected = false)
        {
            base.SetData(index, data);

            m_Entry = data as SelectionPopup.ListEntry;

            if (m_Entry == null)
            {
                return;
            }

            if (m_Entry.LogReference != null)
            {
                SetIconName(MessageUtils.GetLogIconClassName(m_Entry.LogReference.Mode));
                SetText(m_Entry.LogReference.Message);

                string[] lines = m_Entry.LogReference.Message.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

                if (lines.Length > 0)
                    m_Text.tooltip = $"Console {m_Entry.LogReference.Mode}:\n{lines[0]}";

                m_Text.style.textOverflow = TextOverflow.Ellipsis;
            }
            else
            {
                if (MessageUtils.IsPrefabType(m_Entry.Object))
                {
                    SetIconName("mui-icon-prefab");
                }
                else
                {
                    var texture = MessageUtils.GetTextureForObject(m_Entry.Object);
                    SetIcon(texture);
                }

                SetText(m_Entry.Object.name);
                ShowTextAsPrefabInScene(MessageUtils.IsPrefabInScene(m_Entry.Object));

                var type = m_Entry.Object.GetType().ToString();
                if (type.StartsWith("UnityEngine."))
                    m_Text.tooltip = $"{type.Substring(12)}:\n{m_Entry.Object.name}";
                else
                    m_Text.tooltip = $"{type}:\n{m_Entry.Object.name}";

                m_Text.style.textOverflow = TextOverflow.Clip;
            }

            SetSelected(m_Entry.IsSelected);

            m_Button.clicked += () => m_Entry.OnAddRemoveButtonClicked?.Invoke(this);
        }

        public void SetText(string text)
        {
            m_Text.text = text;
        }

        public void ShowTextAsPrefabInScene(bool isPrefab)
        {
            if (isPrefab)
                m_Text.AddToClassList(k_PrefabInSceneStyleClass);
            else
                m_Text.RemoveFromClassList(k_PrefabInSceneStyleClass);
        }

        public void SetIcon(Texture2D texture)
        {
            m_Icon.image = texture;
        }

        public void SetIconName(string className)
        {
            m_Icon.iconName = className;
        }

        public void SetSelected(bool selected)
        {
            if (selected)
            {
                m_Button.title = "Remove";
                m_Button.visible = true;

                m_Checkmark.visible = true;
            }
            else
            {
                m_Button.title = "Attach";

                m_Checkmark.visible = false;
            }

            m_IsSelected = selected;
        }
    }
}
