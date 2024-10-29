using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Muse.Agent.Dynamic;
using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Unity.Muse.Chat
{
    class ChatElementRunExecutionEntry : ManagedTemplate
    {
        const string k_ActionCursorClassName = "mui-action-cursor";

        Text m_ExecutionLabel;
        ExecutionLog m_Content;

        public ChatElementRunExecutionEntry(ExecutionLog content)  : base(MuseChatConstants.UIModulePath)
        {
            m_Content = content;
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_ExecutionLabel = view.Q<Text>("executionLabel");
            m_ExecutionLabel.text = m_Content.Log;

            var executionIcon = view.Q<Icon>("executionIcon");
            if (m_Content.LogType != LogType.Log)
            {
                executionIcon.iconName = "mui-icon-fail";
            }

            m_ExecutionLabel.RegisterCallback<PointerUpLinkTagEvent>(ClickOnObjectLink);
            m_ExecutionLabel.RegisterCallback<PointerOverLinkTagEvent>(OnLinkOver);
            m_ExecutionLabel.RegisterCallback<PointerOutLinkTagEvent>(OnLinkOut);
        }

        void OnLinkOut(PointerOutLinkTagEvent evt)
        {
            if (evt.target is Text text)
                text.RemoveFromClassList(k_ActionCursorClassName);
        }

        void OnLinkOver(PointerOverLinkTagEvent evt)
        {
            if (evt.target is Text text)
                text.AddToClassList(k_ActionCursorClassName);
        }

        void ClickOnObjectLink(PointerUpLinkTagEvent evt)
        {
            if (!int.TryParse(evt.linkID, out int gameObjectInstanceId))
                return;

            var obj = EditorUtility.InstanceIDToObject(gameObjectInstanceId);
            EditorGUIUtility.PingObject(obj);
        }
    }
}
