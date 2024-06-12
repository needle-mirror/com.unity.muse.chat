using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal partial class AdaptiveListView<TD, TV> : VisualElement
        where TV: AdaptiveListViewEntry
    {
        private const int k_DelayedScrollActions = 2;

        private ScrollState m_ScrollState = ScrollState.None;
        private bool m_CheckForScrollLock;
        private bool m_EnforcementQueued;
        private int m_DelayedScrollActions;

        private enum ScrollState
        {
            None,
            ScrollToEnd,
            Locked
        }

        public void ScrollToEnd()
        {
            ChangeScrollState(ScrollState.ScrollToEnd, true);
        }

        private void ChangeScrollState(ScrollState newState, bool force = false)
        {
            if (!force && m_ScrollState == newState)
            {
                return;
            }

            m_DelayedScrollActions = k_DelayedScrollActions;
            m_ScrollState = newState;
            QueueEnforceScrollState();
        }

        private void QueueEnforceScrollState()
        {
            if (m_EnforcementQueued)
            {
                return;
            }

            m_EnforcementQueued = true;
            EditorApplication.delayCall += EnforceScrollState;
        }

        private void EnforceScrollState()
        {
            if (k_Data.Count == 0)
            {
                return;
            }

            m_EnforcementQueued = false;
            if (!EnableVirtualization && k_VisualElements.Count < k_Data.Count)
            {
                // Not all elements are made yet, come back later
                QueueEnforceScrollState();
                return;
            }

            RefreshIfRequired();

            m_CheckForScrollLock = false;

            switch (m_ScrollState)
            {
                case ScrollState.ScrollToEnd:
                {
                    if (EnableVirtualization)
                    {
                        m_InnerList.ScrollToItem(k_Data.Count - 1);
                    }
                    else
                    {
                        m_InnerScroll.ScrollTo(k_VisualElements[^1]);
                    }

                    if (m_VerticalScroller.value < m_VerticalScroller.highValue)
                    {
                        m_VerticalScroller.value = m_VerticalScroller.highValue;
                    }

                    break;
                }
            }

            m_CheckForScrollLock = true;

            if (m_DelayedScrollActions > 0)
            {
                m_DelayedScrollActions--;
                QueueEnforceScrollState();
            }
        }

        private void OnVerticallyScrolled(float newValue)
        {
            if (!EnableScrollLock || !m_CheckForScrollLock)
            {
                return;
            }

            if (!m_CheckForScrollLock)
            {
                return;
            }

            if (newValue < m_VerticalScroller.highValue)
            {
                ChangeScrollState(ScrollState.Locked);
            }
        }
        /*public void ScrollToEnd()
        {
            ScrollToData(k_Data.Count - 1);
        }

        public void ScrollToData(int index)
        {
            if (m_ScrollLockActive)
            {
                return;
            }

            m_ScrollTarget = index;
            EditorApplication.delayCall += DoScrollToTarget;
        }

        private void DoScrollToTarget()
        {
            if (m_ScrollTarget < 0)
            {
                return;
            }

            if (!EnableVirtualization && k_VisualElements.Count < k_Data.Count)
            {
                // Not all elements are made yet, come back later
                EditorApplication.delayCall += DoScrollToTarget;
                return;
            }

            if (DateTime.Now < m_ScrollDelayTime)
            {
                EditorApplication.delayCall += DoScrollToTarget;
                return;
            }

            if (m_ScrollTarget > k_Data.Count)
            {
                m_ScrollTarget = k_Data.Count - 1;
            }

            RefreshIfRequired();

            m_CheckForScrollLock = false;
            if (EnableVirtualization)
            {
                m_InnerList.ScrollToItem(m_ScrollTarget);
            }
            else if (m_ScrollTarget < k_VisualElements.Count)
            {
                m_InnerScroll.ScrollTo(k_VisualElements[m_ScrollTarget]);
            }

            // If the targte is the end and we did not get all the way down adjust
            if (m_ScrollTarget == k_Data.Count - 1 && m_VerticalScroller.value < m_VerticalScroller.highValue)
            {
                m_VerticalScroller.value = m_VerticalScroller.highValue;
            }

            m_CheckForScrollLock = true;
            m_ScrollTarget = -1;

            // Next scroll will be delayed
            m_ScrollDelayTime = DateTime.Now + k_ScrollDelay;
        }*/
    }
}
