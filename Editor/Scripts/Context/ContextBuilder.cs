using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.Muse.Chat
{
    /// <summary>
    /// Context builder is used to build a context string from a list of context selections.
    /// </summary>
    public class ContextBuilder
    {
        Dictionary<IContextSelection, bool> m_ContextList = new ();

        internal void InjectContext(IContextSelection contextSelection, bool userSelected)
        {
            m_ContextList[contextSelection] = userSelected;
        }

        internal void ClearContext()
        {
            m_ContextList.Clear();
        }

        internal string BuildContext(int contextLimit)
        {
            var contextString = new StringBuilder();
            foreach (var (context, userSelected) in m_ContextList)
            {
                var payload = context.Payload;
                var downsizedPayload = context.DownsizedPayload;
                var prefix = userSelected ? $"I'm about to describe what I am currently selecting. I might refer to what I am currently selecting as 'this', 'it' or 'that'. I am currently selecting something of type: {context.ContextType}]. The data about what I am currently selecting is:" : $"The current project includes ({context.ContextType}):";
                if (!string.IsNullOrWhiteSpace(payload) && contextString.Length + payload.Length < contextLimit)
                {
                    contextString.Append($"\n\n{prefix}\n" + payload);
                }
                else if (!string.IsNullOrWhiteSpace(downsizedPayload) && contextString.Length + downsizedPayload.Length < contextLimit)
                {
                    contextString.Append($"\n\n{prefix}\n" + downsizedPayload);
                }
                else
                {
                    var errorMessage =
                        $"Failed to extract user selected {context.ContextType} {context.TargetName}. It's too large for the current context limit.";
                    if (userSelected && contextString.Length + errorMessage.Length < contextLimit)
                        contextString.Append(errorMessage);
                }
            }
            return contextString.ToString();
        }
    }
}
