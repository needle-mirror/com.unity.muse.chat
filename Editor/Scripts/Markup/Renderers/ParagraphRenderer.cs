using Markdig.Renderers;
using Markdig.Syntax;
using Unity.Muse.AppUI.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Editor.Markup.Renderers
{
    internal class ParagraphRenderer : MarkdownObjectRenderer<ChatMarkdownRenderer, ParagraphBlock>
    {
        protected override void Write(ChatMarkdownRenderer renderer, ParagraphBlock obj)
        {
            renderer.m_VisitingState = ChatMarkdownRenderer.VisitingState.Text;
            if(obj.Inline != null)
                renderer.WriteChildren(obj.Inline);
            string text = renderer.ClearText();
            // case with multiple contiguous source blocks - because of the newline in the response text, it looks like multiple markdown paragraphs when it should be one
            if (text.StartsWith(" <sprite") && renderer.m_OutputTextElements.Count > 0)
            {
                var visualElement = renderer.m_OutputTextElements[renderer.m_OutputTextElements.Count - 1];
                var textElement = visualElement as Text;

                if (textElement != null)
                    textElement.text += text;
                else
                    Debug.LogError($"Expected a Text element, not {visualElement.GetType()}. Cannot append text to the previous element.");
            }
            else
            {
                var textElement = new Text(text);
                textElement.AddToClassList("mui-textbox");
                textElement.selection.isSelectable = true;

                textElement.style.marginBottom = new Length(12);

                // Use actual indentation for lists (not tabs or spaces)
                if (renderer.k_OptionUseListIndentation && renderer.m_CurrentListBlock != null)
                {
                    textElement.style.marginLeft = new Length(14);
                }

                renderer.m_OutputTextElements.Add(textElement);
            }
        }
    }
}
