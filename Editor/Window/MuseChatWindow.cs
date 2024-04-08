using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine;

namespace Unity.Muse.Chat
{
    internal class MuseChatWindow : EditorWindow
    {
        const string k_WindowName = "Muse Chat";

        static Vector2 k_MinSize = new(400, 400);

        MuseChatView m_View;

        [MenuItem("Muse/Chat")]
        public static void ShowWindow()
        {
            var editor = GetWindow<MuseChatWindow>();
            editor.titleContent = new GUIContent(k_WindowName);
            editor.Show();
            editor.minSize = k_MinSize;
        }

        void CreateGUI()
        {
            m_View = new MuseChatView();
            m_View.Initialize();
            rootVisualElement.Add(m_View);

            AccountController.Register(this);
        }

        void OnDestroy()
        {
            m_View.Deinit();
        }
    }
}
