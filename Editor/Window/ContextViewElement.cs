using System;
using Unity.Muse.AppUI.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    internal class ContextViewElement : ManagedTemplate
    {
        Icon m_Icon;
        Text m_Text;
        Button m_Button;

        internal Action OnRemoveButtonClicked;

        readonly string k_PrefabInSceneStyleClass = "mui-chat-selection-prefab-text-color";

        public ContextViewElement() :
            base(MuseChatConstants.UIModulePath)
        {
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Icon = view.Q<Icon>("contextViewElementIcon");
            m_Text = view.Q<Text>("contextViewElementText");
            m_Button = view.Q<Button>("contextViewElementRemoveButton");

            m_Button.clicked += () => OnRemoveButtonClicked?.Invoke();
        }

        public void SetData(object obj)
        {
            if (obj is LogReference logRef)
            {
                SetIconName(MessageUtils.GetLogIconClassName(logRef.Mode));

                string[] lines = logRef.Message.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

                if (lines.Length > 0)
                {
                    SetText(lines[0].Substring(0, Math.Min(20, lines[0].Length)) + "...");
                    m_Text.tooltip = $"Console {logRef.Mode}:\n{lines[0]}";
                }
            }
            else if (obj is UnityEngine.Object unityObj)
            {
                if (MessageUtils.IsPrefabType(unityObj))
                {
                    SetIconName("mui-icon-prefab");
                }
                else
                {
                    var texture = MessageUtils.GetTextureForObject(unityObj);
                    SetIcon(texture);
                }

                SetText(unityObj.name);
                ShowTextAsPrefabInScene(MessageUtils.IsPrefabInScene(unityObj));

                var type = unityObj.GetType().ToString();
                if (type.StartsWith("UnityEngine."))
                    m_Text.tooltip = $"{type.Substring(12)}:\n{unityObj.name}";
                else
                    m_Text.tooltip = $"{type}:\n{unityObj.name}";
            }
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
    }
}
