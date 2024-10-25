using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Muse.AppUI.UI;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal class ActionPreviewEntry : ManagedTemplate
    {
        const string k_ParamTagStart = "<param>";
        const string k_ParamTagEnd = "</param>";

        VisualElement m_PreviewContainer;
        AppUI.UI.Text m_PreviewLabel;
        AppUI.UI.Button m_PreviewAction;

        string m_PreviewText;
        AgentAction m_Action;

        public ActionPreviewEntry(string previewText, AgentAction action)  : base(MuseChatConstants.UIModulePath)
        {
            m_PreviewText = previewText;
            m_Action = action;
        }

        protected override void InitializeView(TemplateContainer view)
        {
            var segments = Regex.Split(m_PreviewText, $"({k_ParamTagStart}.*?{k_ParamTagEnd})");
            var stringBuilder = new StringBuilder();

            foreach (var segment in segments)
            {
                if (segment.StartsWith(k_ParamTagStart) && segment.EndsWith(k_ParamTagEnd))
                {
                    string fieldName = segment.Substring(k_ParamTagStart.Length, segment.Length - k_ParamTagStart.Length - k_ParamTagEnd.Length);
                    Type instanceType = m_Action.Instance.GetType();
                    FieldInfo fieldInfo = instanceType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    if (fieldInfo != null)
                    {
                        var fieldValue = fieldInfo.GetValue(m_Action.Instance);
                        stringBuilder.Append("<u><color=white>");
                        stringBuilder.Append(fieldValue);
                        stringBuilder.Append("</color></u>");
                    }
                }
                else
                {
                    stringBuilder.Append(segment);
                }
            }

            m_PreviewContainer = view.Q<VisualElement>("previewRoot");

            m_PreviewLabel = view.Q<Text>("previewLabel");
            m_PreviewLabel.text = stringBuilder.ToString();

            m_PreviewAction = view.Q<AppUI.UI.Button>("previewAction");
            m_PreviewAction.style.display = DisplayStyle.None;
        }

        public void RegisterAction(Action entryAction)
        {
            m_PreviewContainer.AddToClassList("preview-entry-actionable");
            m_PreviewAction.style.display = DisplayStyle.Flex;

            m_PreviewAction.clicked += entryAction;
        }
    }
}
