using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Used by tests to emulate editor loop registration.
    /// </summary>
    class ManualCoroutineLoopRegistration : ILoopRegistration
    {
        List<Action> m_Actions = new();
        bool isComplete = false;

        public void Register(Action action)
        {
            m_Actions.Add(action);
        }

        public void Unregister(Action action)
        {
            m_Actions.Remove(action);
        }

        public IEnumerator TickManuallyUntilSignal()
        {
            while (true)
            {
                if(isComplete)
                    break;

                Action[] actions = m_Actions.ToArray();
                foreach (Action action in actions)
                    action();

                yield return null;
            }
        }

        public void SignalComplete()
        {
            isComplete = true;
        }
    }
}
