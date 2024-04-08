using System.Collections.Generic;
using Markdig;
using Markdig.Parsers.Inlines;
using Unity.Muse.Chat;
using UnityEngine.UIElements;

namespace Unity.Muse.Editor.Markup
{
    internal class MarkdownAPI
    {
        internal static void MarkupText(string text, IList<WebAPI.SourceBlock> sourceBlocks, IList<VisualElement> newTextElements)
        {
            MarkdownPipelineBuilder pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.InlineParsers.TryRemove<EscapeInlineParser>();
            pipelineBuilder.InlineParsers.TryRemove<LinkInlineParser>();
            pipelineBuilder.InlineParsers.AddIfNotAlready<ChatLinkInlineParser>();

            var markdownPipeline = pipelineBuilder.Build();
            var ourRenderer = new ChatMarkdownRenderer(sourceBlocks, newTextElements);

            markdownPipeline.Setup(ourRenderer);

            Markdown.Convert(text, ourRenderer, markdownPipeline);
        }
    }
}
