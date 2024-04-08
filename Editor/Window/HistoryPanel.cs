using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    class HistoryPanel : ManagedTemplate
    {
        static readonly IDictionary<string, List<MuseConversationInfo>> k_ConversationCache = new Dictionary<string, List<MuseConversationInfo>>();

        readonly IList k_ContentData = new List<object>();

        ListView m_Content;

        HistoryPanelEntry m_LastSelectedEntry;
        MuseConversationId m_SelectedConversation;

        public HistoryPanel()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public void Reload()
        {
            k_ContentData.Clear();
            k_ConversationCache.Clear();

            m_Content.ClearSelection();

            var nowRaw = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            LoadData(MuseEditorDriver.instance.History, k_ContentData, nowRaw);

            var activeConversation = MuseEditorDriver.instance.GetActiveConversation();
            m_LastSelectedEntry = null;
            m_SelectedConversation = activeConversation?.Id ?? default;

            m_Content.RefreshItems();
            m_Content.style.display = k_ContentData.Count == 0 ? DisplayStyle.None : DisplayStyle.Flex;
        }

        public static void LoadData(IEnumerable<MuseConversationInfo> data, IList result, long nowRaw)
        {
            k_ConversationCache.Clear();
            result.Clear();
            foreach (var conversationInfo in data)
            {
                string groupKey = MessageUtils.GetMessageTimestampGroup(conversationInfo.LastMessageTimestamp, nowRaw);
                if (!k_ConversationCache.TryGetValue(groupKey, out var groupInfos))
                {
                    groupInfos = new List<MuseConversationInfo>();
                    k_ConversationCache.Add(groupKey, groupInfos);
                }

                groupInfos.Add(conversationInfo);
            }

            var orderedKeys = k_ConversationCache.Keys.OrderBy(x => x).ToArray();
            for (var i = 0; i < orderedKeys.Length; i++)
            {
                var title = orderedKeys[i].Split('#')[1];
                result.Add(title);

                var groupContent = k_ConversationCache[orderedKeys[i]];
                groupContent.Sort((e1, e2) => DateTimeOffset.Compare(DateTimeOffset.FromUnixTimeMilliseconds(e2.LastMessageTimestamp), DateTimeOffset.FromUnixTimeMilliseconds(e1.LastMessageTimestamp)));
                foreach (var info in groupContent)
                {
                    result.Add(info);
                }
            }
        }

        public void ChangeSelection(HistoryPanelEntry newSelectedEntry)
        {
            if (m_LastSelectedEntry != null)
            {
                m_LastSelectedEntry.SetSelected(false);
            }

            m_SelectedConversation = newSelectedEntry.Data.Id;
            m_LastSelectedEntry = newSelectedEntry;
            m_LastSelectedEntry.SetSelected(true);

            MuseEditorDriver.instance.StartConversationLoad(m_SelectedConversation);
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Content = view.Q<ListView>("historyContent");
            m_Content.itemsSource = k_ContentData;
            m_Content.makeItem = MakeItem;
            m_Content.bindItem = BindItem;
            m_Content.selectionType = SelectionType.None;
            m_Content.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

            // Schedule a history update every 5 minutes
            schedule.Execute(MuseEditorDriver.instance.StartConversationRefresh).Every(1000 * 60 * 5);
        }

        void BindItem(VisualElement element, int index)
        {
            var entry = (HistoryPanelEntry)element;

            if (k_ContentData[index] is string headerText)
            {
                entry.SetAsHeader(headerText);
                entry.SetSelected(false);
            }
            else
            {
                var data = (MuseConversationInfo)k_ContentData[index];
                entry.SetAsData(data);
                entry.SetSelected(m_SelectedConversation == data.Id);

                if (m_SelectedConversation == data.Id)
                {
                    m_LastSelectedEntry = entry;
                }
            }
        }

        VisualElement MakeItem()
        {
            HistoryPanelEntry entry = new(this);
            entry.Initialize();
            return entry;
        }
    }
}
