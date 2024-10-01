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
            foreach (var (context, userAttachment) in m_ContextList)
            {
                var payload = context.Payload;
                var downsizedPayload = context.DownsizedPayload;
                var prefix = userAttachment ? $"The following describes an object that the user has attached as context for the query. The attached object is of type: {context.ContextType}]. The data about the object the user has attached to context is:" : $"The current project includes ({context.ContextType}):";
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
                    if (userAttachment && contextString.Length + errorMessage.Length < contextLimit)
                        contextString.Append(errorMessage);
                }
            }
            return contextString.ToString();
        }
    }
}
