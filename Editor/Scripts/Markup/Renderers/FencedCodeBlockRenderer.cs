using System.Text;
using Markdig.Renderers;
using Markdig.Syntax;
using Unity.Muse.Chat;
using UnityEditor;

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
                var lineWithoutEscapes = obj.Lines.Lines[i].ToString().Replace(@"\", @"\\");

                fullCodeBlock.Append(lineWithoutEscapes);
                if (i < obj.Lines.Count - 1)
                    fullCodeBlock.Append("\n");
            }

            var codeText = fullCodeBlock.ToString();

            switch (obj.Info)
            {
                case "csx":
                {
                    var actionBlock = new ChatElementActionBlock();
                    actionBlock.Initialize();
                    actionBlock.SetData(codeText);

                    renderer.m_OutputTextElements.Add(actionBlock);
                    // if the code block is completed, the action can be created
                    if (!obj.IsOpen)
                    {
                        // Use async function here instead of delegate to avoid lambda returning type (Task) mismatches with delegate expected type (void).
                        EditorApplication.delayCall += () => SetUpAgentAction(actionBlock);
                    }
                }
                    break;
                case ChatElementRunExecutionBlock.FencedBlockTag:
                {
                    var executeBlock = new ChatElementRunExecutionBlock();
                    executeBlock.Initialize();
                    executeBlock.SetData(codeText);
                    renderer.m_OutputTextElements.Add(executeBlock);
                }
                    break;
                case "validate-csharp":
                {
                    var codeLabel = new ChatElementCodeBlock();
                    codeLabel.Initialize();
                    codeLabel.SetData(codeText, true);
                    renderer.m_OutputTextElements.Add(codeLabel);
                    if (!obj.IsOpen)
                    {
                        // Use async function here instead of delegate to avoid lambda returning type (Task) mismatches with delegate expected type (void).
                        EditorApplication.delayCall += () => SetUpValidationState(codeLabel);
                    }
                }
                    break;
                default: // csharp, uss ...
                {
                    var codeLabel = new ChatElementCodeBlock();
                    codeLabel.Initialize();
                    codeLabel.SetData(codeText, false);
                    renderer.m_OutputTextElements.Add(codeLabel);
                }
                    break;
            }

            renderer.AppendText("</color>");
        }

        async void SetUpAgentAction(ChatElementActionBlock actionBlock)
        {
            await actionBlock.SetupAction();
        }

        async void SetUpValidationState(ChatElementCodeBlock codeBlock)
        {
            await codeBlock.ValidateCode();
        }
    }
}
