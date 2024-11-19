using System;
using System.Collections.Generic;
using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unity.Muse.Chat
{
    internal class MuseChatWindow : EditorWindow
    {
        const string k_WindowName = "Muse Chat";

        static Vector2 k_MinSize = new(400, 400);

        MuseChatView m_View;

        [SerializeField]
        internal List<Object> m_ObjectSelection = new();

        [SerializeField]
        internal List<LogReference> m_ConsoleSelection = new();

        internal Action OnLostWindowFocus;

        [UnityEditor.MenuItem("Muse/Chat")]
        public static void ShowWindow()
        {
            var editor = GetWindow<MuseChatWindow>();
            editor.titleContent = new GUIContent(k_WindowName);
            editor.Show();
            editor.minSize = k_MinSize;
        }

        void CreateGUI()
        {
            m_View = new MuseChatView(this);
            m_View.Initialize();
            m_View.style.flexGrow = 1;
            m_View.style.minWidth = 400;
            rootVisualElement.Add(m_View);

            AccountController.Register(this);

            m_View.InitializeThemeAndStyle();
        }

        void OnDestroy()
        {
            m_View?.Deinit();
        }

        private void OnLostFocus()
        {
            OnLostWindowFocus?.Invoke();
        }
    }
}
