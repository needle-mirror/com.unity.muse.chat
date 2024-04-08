using System.Text;
using Markdig.Renderers;
using Markdig.Syntax;
using Unity.Muse.Chat;

namespace Unity.Muse.Editor.Markup.Renderers
{
    internal class FencedCodeBlockRenderer : MarkdownObjectRenderer<ChatMarkdownRenderer, FencedCodeBlock>
    {
        protected override void Write(ChatMarkdownRenderer renderer, FencedCodeBlock obj)
        {
            renderer.AppendText($"<color={renderer.m_CodeColor}>");

            StringBuilder fullCodeBlock = new StringBuilder();

            for (int i = 0; i < obj.Lines.Count; i++)
            {
                var lineWithoutEscapes = obj.Lines.Lines[i].ToString().Replace(@"\", @"\\");;

                fullCodeBlock.Append(lineWithoutEscapes);
                if(i < obj.Lines.Count - 1)
                    fullCodeBlock.Append("\n");
            }

            var codeText = fullCodeBlock.ToString();

            var codeLabel = new ChatElementCodeBlock();
            codeLabel.Initialize();
            codeLabel.SetData(codeText);

            renderer.m_OutputTextElements.Add(codeLabel);

            renderer.AppendText("</color>");
        }
    }
}
