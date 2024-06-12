using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Common.Utils;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    class HistoryPanel : ManagedTemplate
    {
        static readonly IDictionary<string, List<MuseConversationInfo>> k_ConversationCache = new Dictionary<string, List<MuseConversationInfo>>();

        private readonly IList<object> k_TempList = new List<object>();

        VisualElement m_ContentRoot;
        AdaptiveListView<object, HistoryPanelEntry> m_ContentList;

        MuseConversationId m_SelectedConversation;

        public HistoryPanel()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public void Reload()
        {
            k_ConversationCache.Clear();

            m_ContentList.ClearData();
            m_ContentList.ClearSelection();

            var nowRaw = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var activeConversation = MuseEditorDriver.instance.GetActiveConversation();

            k_TempList.Clear();
            LoadData(MuseEditorDriver.instance.History, k_TempList, nowRaw);
            int selectedIndex = -1;
            m_ContentList.BeginUpdate();
            for (var i = 0; i < k_TempList.Count; i++)
            {
                var entry = k_TempList[i];
                if (activeConversation != null && entry is MuseConversationInfo info && info.Id == activeConversation.Id)
                {
                    selectedIndex = i;
                }

                m_ContentList.AddData(entry);
            }

            m_ContentList.EndUpdate(false);
            if (selectedIndex >= 0)
            {
                m_ContentList.SetSelectionWithoutNotify(selectedIndex, true);
            }

            m_SelectedConversation = activeConversation?.Id ?? default;

            m_ContentList.SetDisplay(m_ContentList.Data.Count != 0);
        }

        public static void LoadData(IEnumerable<MuseConversationInfo> data, IList<object> result, long nowRaw)
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

        public void SelectionChanged(int index, object data)
        {
            if (index == -1 || data is string)
            {
                return;
            }

            var conversationInfo = (MuseConversationInfo)data;
            m_SelectedConversation = conversationInfo.Id;

            MuseEditorDriver.instance.StartConversationLoad(m_SelectedConversation);
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_ContentRoot = view.Q<VisualElement>("historyContentRoot");
            m_ContentList = new AdaptiveListView<object, HistoryPanelEntry>
            {
                EnableVirtualization = true
            };
            m_ContentList.Initialize();
            m_ContentList.SelectionChanged += SelectionChanged;
            m_ContentRoot.Add(m_ContentList);

            // Schedule a history update every 5 minutes
            schedule.Execute(MuseEditorDriver.instance.StartConversationRefresh).Every(1000 * 60 * 5);
        }
    }
}
