using System;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal abstract class AdaptiveListViewEntry : ManagedTemplate
    {
        private int m_Index;

        protected AdaptiveListViewEntry()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public event Action<int, VisualElement> SelectionChanged;

        public int Index => m_Index;

        public virtual void SetData(int index, object data, bool isSelected = false)
        {
            m_Index = index;
        }

        protected void NotifySelectionChanged()
        {
            SelectionChanged?.Invoke(m_Index, this);
        }
    }
}
