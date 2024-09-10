using System.Collections.Generic;
using System.Linq;
using Unity.AppUI.Core;
using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using DragAndDrop = UnityEditor.DragAndDrop;

namespace Unity.Muse.Chat
{
    partial class MuseChatView
    {
        private DropZone m_DropZone;
        private VisualElement m_DropZoneContent;

        private bool m_TrackingDragEvent;

        private bool OnAcceptDrag(IEnumerable<object> arg)
        {
            if (arg == null || arg.Count() == 0)
            {
                return false;
            }

            return true;
        }

        private void OnDropped(IEnumerable<object> obj)
        {
            bool anyAdded = false;

            foreach (object droppedObject in obj)
            {
                if (droppedObject is Object unityObject)
                {
                    if (unityObject == null)
                        continue;

                    if (!IsSupportedAsset(unityObject))
                        continue;

                    if (unityObject != null && !ObjectSelection.Contains(unityObject))
                    {
                        ObjectSelection.Add(unityObject);
                        anyAdded = true;
                    }
                }
            }

            if (anyAdded)
                UpdateContextSelectionElements(true);

            m_DropZone.visibleIndicator = false;

            SetDropZoneActive(false);
        }

        private bool IsSupportedAsset(Object unityObject)
        {
            if (unityObject is DefaultAsset)
                return false;

            return true;
        }

        private void SetDropZoneActive(bool active)
        {
            var display = active ? DisplayStyle.Flex : DisplayStyle.None;

            if (m_DropZoneContent.style.display == display)
                return;

            m_DropZoneContent.style.display = display;
        }


        private void OnDragEntered()
        {
            StartDraggingEvent();
        }


        private void OnDragEnded()
        {
            Reset();
        }

        private void Reset()
        {
            m_DropZone.state = DragAndDropState.Default;
            m_DropZone.visibleIndicator = false;

            m_TrackingDragEvent = false;

            SetDropZoneActive(false);
        }

        private void DragEnter(DragEnterEvent evt)
        {
            StartDraggingEvent();
            //SetDropZoneActive(true);
        }

        private void DragLeave(DragLeaveEvent evt)
        {
            SetDropZoneActive(false);
        }

        private void DragUpdate(DragUpdatedEvent evt)
        {
            SetDropZoneActive(true);
        }

        private void StartDraggingEvent()
        {
            if (m_TrackingDragEvent)
                return;

            if (IsDraggingObjects())
            {
                m_TrackingDragEvent = true;

                m_DropZone.visibleIndicator = true;
                m_DropZone.state = DragAndDropState.AcceptDrag;

                SetDropZoneActive(true);
            }
        }

        private bool IsDraggingObjects()
        {
            return DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0;
        }
    }
}
