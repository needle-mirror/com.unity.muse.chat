using System;
using UnityEngine;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Allows a console log message and  associated file to be sent to the LLM for evaluation
    /// </summary>
    internal class ConsoleContextSelection : IContextSelection
    {
        LogReference m_Target;

        internal void SetTarget(LogReference target)
        {
            m_Target = target;
        }

        string IContextSelection.Classifier => "Console, Editor Log, Player Log";

        string IContextSelection.Description
        {
            get
            {
                if (m_Target == null)
                    return "No log selected";

                return $"{m_Target.Message.Substring(0, Mathf.Min(m_Target.Message.Length, 200))}";
            }
        }

        string IContextSelection.Payload
        {
            get
            {
                if (m_Target == null)
                    return null;

                return $"Unity Console Log Message:\n{UnityDataUtils.OutputLogData(m_Target, true)}";
            }
        }

        bool IEquatable<IContextSelection>.Equals(IContextSelection other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (this == null || other == null)
                return false;

            if (other is not ConsoleContextSelection otherSelection)
                return false;

            var asConsoleContext = other as ConsoleContextSelection;

            return asConsoleContext.m_Target?.Message == m_Target?.Message;
        }
}
}
