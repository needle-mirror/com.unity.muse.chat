using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine.UIElements;
using Button = Unity.Muse.AppUI.UI.Button;
using Object = UnityEngine.Object;

namespace Unity.Muse.Chat
{
    internal class SelectionPopup : ManagedTemplate
    {
        internal class ListEntry
        {
            public Object Object;
            public LogReference LogReference;
            public Action<SelectionElement> OnAddRemoveButtonClicked;
            public bool IsSelected;
        }

        VisualElement m_Root;
        VisualElement m_AdaptiveListViewContainer;
        AdaptiveListView<ListEntry, SelectionElement> m_ListView;
        Button m_AddEditorSelectionButton;
        Text m_EmptyListHintText;

        double m_LastConsoleCheckTime;
        readonly float k_ConsoleCheckInterval = 0.2f;

        List<LogReference> m_LastUpdatedLogReferences = new ();

        public List<Object> ObjectSelection = new();
        public List<LogReference> ConsoleSelection = new();

        public Action OnSelectionChanged;
        public Action<Object> OnContextObjectAdded;
        public Action<LogReference> OnContextLogAdded;

        const float k_MaxWidth = 500;
        const float k_RightMargin = 60;

        public SelectionPopup()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public void SetSelection(List<Object> selection, List<LogReference> consoleSelection, bool notify = true)
        {
            ObjectSelection.Clear();
            ConsoleSelection.Clear();

            if (selection != null)
            {
                foreach (var obj in selection)
                {
                    if (obj != null)
                        ObjectSelection.Add(obj);
                }
            }

            if (consoleSelection != null)
            {
                foreach (var logRef in consoleSelection)
                {
                    if (logRef != null)
                        ConsoleSelection.Add(logRef);
                }
            }

            if (notify)
                OnSelectionChanged?.Invoke();
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Root = view.Q<VisualElement>("popupRoot");

            m_AdaptiveListViewContainer = view.Q<VisualElement>("adaptiveListViewContainer");
            m_ListView = new()
            {
                EnableDelayedElements = false,
                EnableVirtualization = false,
                EnableScrollLock = true,
                EnableHorizontalScroll = false
            };
            m_ListView.Initialize();
            m_AdaptiveListViewContainer.Add(m_ListView);

            m_ListView.style.minHeight = 200;
            m_ListView.style.minWidth = 200;

            m_AddEditorSelectionButton = view.Q<Button>("addSelectionButton");
            m_AddEditorSelectionButton.clickable.clicked += () =>
            {
                PopulateListView(true);
            };

            m_EmptyListHintText = view.Q<Text>("emptyListHint");

            Selection.selectionChanged += () => PopulateListView();

            PopulateListView();

            m_LastConsoleCheckTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += DetectLogChanges;
        }

        public void PopulateListView(bool addSelection = false)
        {
            m_ListView.ClearData();

            ConsoleUtils.GetSelectedConsoleLogs(m_LastUpdatedLogReferences);

            // Add selected objects
            if (Selection.objects.Length > 0)
            {
                for (int i = 0; i < Selection.objects.Length; i++)
                {
                    var obj = Selection.objects[i];

                    if (!IsSupportedAsset(obj))
                        continue;

                    var entry = new ListEntry()
                    {
                        Object = obj,
                        OnAddRemoveButtonClicked = (SelectionElement e) => SelectedObject(obj, e),
                        IsSelected = addSelection || ObjectSelection.Contains(obj)
                    };

                    m_ListView.AddData(entry);

                    if (addSelection && !ObjectSelection.Contains(obj))
                        AddObjectToSelection(obj, true);
                }
            }

            // Add console log entries
            foreach (var logRef in m_LastUpdatedLogReferences)
            {
                var entry = new ListEntry()
                {
                    Object = null,
                    LogReference = logRef,
                    OnAddRemoveButtonClicked = (SelectionElement e) => SelectedLogReference(logRef, e),
                    IsSelected = addSelection || ConsoleUtils.HasEqualLogReference(ConsoleSelection, logRef)
                };

                if (addSelection && !ConsoleUtils.HasEqualLogReference(ConsoleSelection, logRef))
                    AddLogReferenceToSelection(logRef, true);

                m_ListView.AddData(entry);
            }

            // Add placeholder text or history as fallback
            if (m_ListView.Data.Count == 0)
            {
                m_EmptyListHintText.style.display = DisplayStyle.Flex;
            }
            else
            {
                m_EmptyListHintText.style.display = DisplayStyle.None;
            }

            RefreshSelectionCount();
        }

        private bool IsSupportedAsset(Object obj)
        {
            if (obj is DefaultAsset)
                return false;

            return true;
        }

        void RefreshSelectionCount()
        {
            var nonSelectedItemCount = 0;

            var logs = new List<LogReference>();
            ConsoleUtils.GetSelectedConsoleLogs(logs);
            foreach (var log in logs)
                if (!ConsoleUtils.HasEqualLogReference(ConsoleSelection, log))
                    nonSelectedItemCount++;

            foreach (var obj in Selection.objects)
                if (!ObjectSelection.Contains(obj) && IsSupportedAsset(obj))
                    nonSelectedItemCount++;

            m_AddEditorSelectionButton.title = $"Add Editor selection ({nonSelectedItemCount})";
            m_AddEditorSelectionButton.SetEnabled(nonSelectedItemCount > 0);
        }

        void SelectedObject(Object obj, SelectionElement e)
        {
            if (!ObjectSelection.Contains(obj))
            {
                AddObjectToSelection(obj);
                e.SetSelected(true);
            }
            else
            {
                ObjectSelection.Remove(obj);
                e.SetSelected(false);
            }

            OnSelectionChanged?.Invoke();

            RefreshSelectionCount();
        }

        void AddObjectToSelection(Object obj, bool notifySelectionChanged = false)
        {
            ObjectSelection.Add(obj);
            OnContextObjectAdded?.Invoke(obj);

            if (notifySelectionChanged)
                OnSelectionChanged?.Invoke();
        }

        void SelectedLogReference(LogReference logRef, SelectionElement e)
        {
            if (!ConsoleUtils.HasEqualLogReference(ConsoleSelection, logRef))
            {
                AddLogReferenceToSelection(logRef);
                e.SetSelected(true);
            }
            else
            {
                ConsoleSelection.RemoveAll(e => e.Equals(logRef));
                e.SetSelected(false);
            }

            OnSelectionChanged?.Invoke();

            RefreshSelectionCount();
        }

        void AddLogReferenceToSelection(LogReference logRef, bool notifySelectionChanged = false)
        {
            ConsoleSelection.Add(logRef);
            OnContextLogAdded?.Invoke(logRef);

            if (notifySelectionChanged)
                OnSelectionChanged?.Invoke();
        }

        void DetectLogChanges()
        {
            if (EditorApplication.timeSinceStartup < m_LastConsoleCheckTime + k_ConsoleCheckInterval)
                return;

            List<LogReference> logs = new();
            ConsoleUtils.GetSelectedConsoleLogs(logs);

            if (m_LastUpdatedLogReferences.Count != logs.Count
                || m_LastUpdatedLogReferences.Any(log => !ConsoleUtils.HasEqualLogReference(logs, log))
                || logs.Any(log => !ConsoleUtils.HasEqualLogReference(m_LastUpdatedLogReferences, log)) )
            {
                PopulateListView();
            }

            m_LastConsoleCheckTime = EditorApplication.timeSinceStartup;
        }

        public void SetAdjustToPanel(Panel rootPanel)
        {
            m_Root.style.maxWidth = Math.Min(rootPanel.contentRect.width - k_RightMargin, k_MaxWidth);
            m_Root.style.width = k_MaxWidth;

            rootPanel.RegisterCallback<GeometryChangedEvent>(evt =>
            {
                m_Root.style.maxWidth = Math.Min(rootPanel.contentRect.width - k_RightMargin, k_MaxWidth);
            });
        }
    }
}
