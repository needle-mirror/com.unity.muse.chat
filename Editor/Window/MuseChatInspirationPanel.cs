using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Common.Utils;
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

        VisualElement m_AskSection;
        VisualElement m_RunSection;
        VisualElement m_CodeSection;

        private readonly IDictionary<ChatCommandType, IList<MuseChatInspiration>> k_InspirationCache = new Dictionary<ChatCommandType, IList<MuseChatInspiration>>();

        public MuseChatInspirationPanel()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public event Action<MuseChatInspiration> InspirationSelected;

        public void RefreshEntries()
        {
            var mode = UserSessionState.instance.SelectedCommandMode;

            RefreshCache();

            HideSections();

            switch (mode)
            {
                case ChatCommandType.Ask:
                    m_AskSection.SetDisplay(true);
                    RefreshEntriesForCategory(m_AskContent, ChatCommandType.Ask);
                    break;
                case ChatCommandType.Run:
                    m_RunSection.SetDisplay(true);
                    RefreshEntriesForCategory(m_RunContent, ChatCommandType.Run);
                    break;
                case ChatCommandType.Code:
                    m_CodeSection.SetDisplay(true);
                    RefreshEntriesForCategory(m_GenerateContent, ChatCommandType.Code);
                    break;
            }
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_RefreshButton = view.SetupButton("refreshButton", _ => RefreshEntries());
            m_AskContent = view.Q<VisualElement>("askInspirationSectionContent");
            m_RunContent = view.Q<VisualElement>("runInspirationSectionContent");
            m_GenerateContent = view.Q<VisualElement>("generateInspirationSectionContent");

            m_AskSection = view.Q<VisualElement>("askSection");
            m_RunSection = view.Q<VisualElement>("runSection");
            m_CodeSection = view.Q<VisualElement>("codeSection");

            HideSections();

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
        private void RefreshEntriesForCategory(VisualElement targetRoot, ChatCommandType mode, int maxEntries = 4)
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

        void HideSections()
        {
            m_AskSection.SetDisplay(false);
            m_RunSection.SetDisplay(false);
            m_CodeSection.SetDisplay(false);
        }

        private void BeginRefreshEntries()
        {
            MuseEditorDriver.instance.StartInspirationRefresh();
        }

        private void OnInspirationSelected(MuseChatInspiration value)
        {
            InspirationSelected?.Invoke(value);
        }
    }
}
