using UnityEngine;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Allows a Unity object or asset to be sent to the LLM for evaluation
    /// </summary>
    internal class UnityObjectContextSelection : IContextSelection
    {
        Object m_Target;

        public void SetTarget(Object target)
        {
            m_Target = target;
        }

        string IContextSelection.Classifier
        {
            get
            {
                if (m_Target == null)
                    return "Null";

                // We might want to special path for gameobjects to include all their components
                return $"UnityEngine.Object, {m_Target.GetType().Name}";
            }
        }

        string IContextSelection.Description
        {
            get
            {
                if (m_Target == null)
                    return "No object selected";

                return $"{m_Target.name} - {m_Target.GetType().Name}";
            }
        }

        string IContextSelection.Payload
        {
            get
            {
                if (m_Target == null)
                    return null;

                return $"\n{UnityDataUtils.OutputUnityObject(m_Target, true, false, 1)}";
            }
        }

        string IContextSelection.DownsizedPayload
        {
            get
            {
                if (m_Target == null)
                    return null;

                return $"\n{UnityDataUtils.OutputUnityObject(m_Target, true, false, 0)}";
            }
        }

        string IContextSelection.ContextType => $"serialization data of game object in JSON format";

        string IContextSelection.TargetName => $"{m_Target.name}";

        bool System.IEquatable<IContextSelection>.Equals(IContextSelection other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (this == null || other == null)
                return false;

            if (other is not UnityObjectContextSelection otherSelection)
                return false;

            var asObjectContext = other as UnityObjectContextSelection;

            return asObjectContext.m_Target == m_Target;
        }
    }
}
