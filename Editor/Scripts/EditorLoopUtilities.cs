using System;
using System.Collections.Generic;
using UnityEditor;

namespace Unity.Muse.Chat
{
    interface ILoopRegistration
    {
        void Register(Action action);
        void Unregister(Action action);
    }

    class EditorLoopRegistration : ILoopRegistration
    {
        Dictionary<Action, EditorApplication.CallbackFunction> m_CallbackMap = new();

        public void Register(Action action)
        {
            if (m_CallbackMap.ContainsKey(action))
                return;

            EditorApplication.CallbackFunction newCallback = () => action();
            EditorApplication.update += newCallback;
            m_CallbackMap.Add(action, newCallback);
        }

        public void Unregister(Action action)
        {
            if (!m_CallbackMap.TryGetValue(action, out EditorApplication.CallbackFunction callback))
                return;

            EditorApplication.update -= callback;
        }
    }

    static class EditorLoopUtilities
    {
        public static readonly EditorLoopRegistration EditorLoopRegistration = new();
    }
}
