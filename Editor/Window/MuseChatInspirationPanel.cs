using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;

namespace Unity.Muse.Chat
{
    internal class MuseChatInspirationPanel : ManagedTemplate
    {
        Button m_RefreshButton;

        VisualElement m_AskContent;
        VisualElement m_RunContent;
        VisualElement m_GenerateContent;

        private readonly IDictionary<ChatCommandType, IList<MuseChatInspiration>> k_InspirationCache = new Dictionary<ChatCommandType, IList<MuseChatInspiration>>();

        public MuseChatInspirationPanel()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public event Action<MuseChatInspiration> InspirationSelected;

        protected override void InitializeView(TemplateContainer view)
        {
            m_RefreshButton = view.SetupButton("refreshButton", _ => RefreshEntries());
            m_AskContent = view.Q<VisualElement>("askInspirationSectionContent");
            m_RunContent = view.Q<VisualElement>("runInspirationSectionContent");
            m_GenerateContent = view.Q<VisualElement>("generateInspirationSectionContent");

            MuseEditorDriver.instance.OnInspirationsChanged += RefreshEntries;
            BeginRefreshEntries();
        }

        private void RefreshCache()
        {
            k_InspirationCache.Clear();
            var entries = MuseEditorDriver.instance.Inspirations;
            foreach (MuseChatInspiration inspiration in entries)
            {
                if (!k_InspirationCache.TryGetValue(inspiration.Mode, out var entryList))
                {
                    entryList = new List<MuseChatInspiration>();
                    k_InspirationCache.Add(inspiration.Mode, entryList);
                }

                entryList.Add(inspiration);
            }
        }
        private void RefreshEntriesForCategory(VisualElement targetRoot, ChatCommandType mode, int maxEntries = 3)
        {
            targetRoot.Clear();

            if (!k_InspirationCache.TryGetValue(mode, out var inspirations))
            {
                return;
            }

            int entriesToSelect = Mathf.Min(maxEntries, inspirations.Count);
            var shuffledIndices = Enumerable.Range(0, inspirations.Count)
                .OrderBy(_ => UnityEngine.Random.value)
                .Take(entriesToSelect);

            foreach (var inspiration in shuffledIndices.Select(index => inspirations[index]))
            {
                var entry = new MuseChatInspirationEntry();
                entry.Initialize();
                entry.Value = inspiration;
                entry.Clicked += OnInspirationSelected;
                targetRoot.Add(entry);
            }
        }

        private void BeginRefreshEntries()
        {
            MuseEditorDriver.instance.StartInspirationRefresh();
        }

        private void RefreshEntries()
        {
            RefreshCache();
            RefreshEntriesForCategory(m_AskContent, ChatCommandType.Ask, maxEntries: 4);
            RefreshEntriesForCategory(m_RunContent, ChatCommandType.Run);
            RefreshEntriesForCategory(m_GenerateContent, ChatCommandType.Code);
        }

        private void OnInspirationSelected(MuseChatInspiration value)
        {
            InspirationSelected?.Invoke(value);
        }
    }
}
