using System.Collections.Generic;
using System.Text;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Editor.Markup;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Unity.Muse.Chat
{
    internal abstract class ChatElementBase : ManagedTemplate
    {
        // Note: We may have to tweak this dynamically based on what content we intend to add to the text element
        private const int k_MessageChunkSize = 5000;

        private const string k_LinkCursorClassName = "mui-link-cursor";

        private IList<WebAPI.SourceBlock> m_SourceBlocks;

        private readonly IList<VisualElement> m_NewTextElements = new List<VisualElement>();

        protected ChatElementBase()
            : base(MuseChatConstants.UIModulePath)
        {
            MessageChunks = new List<string>();
            HideWhenEmpty = true;
        }

        public virtual void SetData(MuseMessage message)
        {
            Message = message;
            BuildMessageChunks();

            if (HideWhenEmpty && string.IsNullOrEmpty(message.Content))
            {
                style.display = DisplayStyle.None;
            }
            else
            {
                style.display = DisplayStyle.Flex;
            }
        }

        public MuseMessageId Id => Message.Id;

        public MuseMessage Message { get; private set; }

        public bool HideWhenEmpty { get; set; }

        protected IList<string> MessageChunks { get; }

        protected IList<WebAPI.SourceBlock> SourceBlocks => m_SourceBlocks;

        protected override void InitializeView(TemplateContainer view)
        {
            LoadStyle("ChatElementShared");
        }

        protected void RefreshText(VisualElement root, IList<VisualElement> textFields)
        {
            if (Message.Role == MuseEditorDriver.k_UserRole)
            {
                // Synchronize each text element for our message chunks
                for (var i = 0; i < MessageChunks.Count; i++)
                {
                    var text = MessageChunks[i];

                    if (Message.Role == MuseEditorDriver.k_UserRole)
                        text = MarkupUtil.QuoteCarriageReturn(text);

                    if (textFields.Count <= i)
                    {
                        var textElement = new Text(text);
                        textElement.AddToClassList("mui-textbox");
                        textElement.RegisterCallback<PointerDownLinkTagEvent>(OnLinkClicked);
                        textElement.RegisterCallback<PointerOverLinkTagEvent>(OnLinkOver);
                        textElement.RegisterCallback<PointerOutLinkTagEvent>(OnLinkOut);
                        textElement.selection.isSelectable = true;
                        textFields.Add(textElement);
                        root.Add(textElement);
                    }
                    else
                    {
                        var textField = textFields[i] as Text;
                        textField.text = text;
                    }
                }

                for (var id = textFields.Count - 1; id >= MessageChunks.Count; id++)
                {
                    var obsoleteField = textFields[id];
                    obsoleteField.RemoveFromHierarchy();
                    obsoleteField.UnregisterCallback<PointerDownLinkTagEvent>(OnLinkClicked);
                    obsoleteField.UnregisterCallback<PointerOverLinkTagEvent>(OnLinkOver);
                    obsoleteField.UnregisterCallback<PointerOutLinkTagEvent>(OnLinkOut);
                    textFields.RemoveAt(id);
                }

                return;
            }

            // For chat responses, parse chunks and add text elements with formatting/rich text tags where applicable
            m_NewTextElements.Clear();
            for (var i = 0; i < MessageChunks.Count; i++)
            {
                var text = MessageChunks[i];

                MarkdownAPI.MarkupText(text, m_SourceBlocks, m_NewTextElements);

                for (var id = 0; id < m_NewTextElements.Count; id++)
                {
                    var visualElement = m_NewTextElements[id];

                    if (textFields.Count <= id)
                    {
                        visualElement.RegisterCallback<PointerDownLinkTagEvent>(OnLinkClicked);
                        visualElement.RegisterCallback<PointerOverLinkTagEvent>(OnLinkOver);
                        visualElement.RegisterCallback<PointerOutLinkTagEvent>(OnLinkOut);

                        if (visualElement is Text textElement)
                        {
                            textElement.AddToClassList("mui-textbox");
                            textElement.selection.isSelectable = true;
                        }
                        else if (visualElement is ChatElementCodeBlock codeBlock)
                        {
                            codeBlock.SetSelectable(true);
                        }

                        textFields.Add(visualElement);
                        root.Add(visualElement);
                    }
                    else
                    {
                        int oldIndex = root.IndexOf(textFields[id]);
                        root.Insert(oldIndex, visualElement);
                        root.Remove(textFields[id]);
                        textFields[id] = visualElement;
                    }
                }

                for (var id = textFields.Count - 1; id >= m_NewTextElements.Count; id--)
                {
                    var obsoleteField = textFields[id];
                    obsoleteField.RemoveFromHierarchy();
                    obsoleteField.UnregisterCallback<PointerDownLinkTagEvent>(OnLinkClicked);
                    obsoleteField.UnregisterCallback<PointerOverLinkTagEvent>(OnLinkOver);
                    obsoleteField.UnregisterCallback<PointerOutLinkTagEvent>(OnLinkOut);
                    textFields.RemoveAt(id);
                }
            }
        }

        private void OnLinkOut(PointerOutLinkTagEvent evt)
        {
            if (evt.target is Text text)
            {
                text.RemoveFromClassList(k_LinkCursorClassName);
            }
        }

        private void OnLinkOver(PointerOverLinkTagEvent evt)
        {
            if (evt.target is Text text)
            {
                text.AddToClassList(k_LinkCursorClassName);
            }
        }

        private void OnLinkClicked(PointerDownLinkTagEvent evt)
        {
            if (!MessageUtils.GetAssetFromLink(evt.linkID, out var asset))
            {
                Debug.LogWarning("Asset not found: " + evt.linkID);
                return;
            }

            Selection.activeObject = asset;
        }

        void BuildMessageChunks()
        {
            m_SourceBlocks?.Clear();
            MessageChunks.Clear();
            if (string.IsNullOrEmpty(Message.Content))
            {
                return;
            }

            MessageUtils.ProcessText(Message, ref m_SourceBlocks, out var messageContent);
            string[] lines = messageContent.Split("\n");
            var chunk = new StringBuilder();
            for (var i = 0; i < lines.Length; i++)
            {
                var chunkContent = SplitChunkContent(lines[i]);
                for (var ic = 0; ic < chunkContent.Length; ic++)
                {
                    if (chunk.Length > k_MessageChunkSize)
                    {
                        MessageChunks.Add(chunk.ToString());
                        chunk.Clear();
                    }

                    if (ic < chunkContent.Length - 1)
                    {
                        chunk.Append(chunkContent[ic]);
                    }
                    else
                    {
                        chunk.AppendLine(chunkContent[ic]);
                    }
                }
            }

            if (chunk.Length > 0)
            {
                MessageChunks.Add(chunk.ToString());
            }
        }

        string[] SplitChunkContent(string content)
        {
            int lineChunks = 1 + (int)Mathf.Floor(content.Length / (float)k_MessageChunkSize);
            var result = new string[lineChunks];
            for(var i = 0; i < lineChunks; i++)
            {
                int start = i * k_MessageChunkSize;
                int length = Mathf.Min(content.Length - start, k_MessageChunkSize);
                result[i] = content.Substring(start, length);
            }

            return result;
        }
    }
}
