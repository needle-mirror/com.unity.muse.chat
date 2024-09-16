using System.Collections.Generic;
using System.Linq;
using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    partial class MuseChatView
    {
        private DropZone m_DropZone;
        private VisualElement m_DropZoneContent;

        private bool m_TrackingDragEvent;

        private bool TryGetDroppableFromPath(string path, out object droppable)
        {
            droppable = path;
            m_DropZone.pickingMode = PickingMode.Position;
            return true;
        }

        private bool TryGetDroppableFromUnityObjects(Object[] objects, out List<object> droppables)
        {
            if (objects == null || objects.Length == 0)
            {
                droppables = null;
                return false;
            }

            droppables = new List<object> { objects.ToList() };
            m_DropZone.pickingMode = PickingMode.Position;
            return true;
        }

        private void OnDropped(IEnumerable<object> obj)
        {
            foreach (object droppedObject in obj)
            {
                if (droppedObject.GetType().IsGenericType && droppedObject is System.Collections.IList)
                {
                    bool anyAdded = false;

                    var list = droppedObject as System.Collections.IList;
                    foreach (var item in list)
                    {
                        var unityObject = item as Object;
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

                    if (anyAdded)
                        UpdateContextSelectionElements(true);
                }
            }

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
            var mode = active ? PickingMode.Position : PickingMode.Ignore;
            var display = active ? DisplayStyle.Flex : DisplayStyle.None;

            if (m_DropZone.pickingMode == mode && m_DropZoneContent.style.display == display)
                return;

            SetPickingModeRecursive(m_DropZone, mode);
            m_DropZoneContent.style.display = display;
        }

        private void SetPickingModeRecursive(VisualElement element, PickingMode mode)
        {
            m_DropZone.pickingMode = mode;

            foreach (var child in element.Children())
                SetPickingModeRecursive(child, mode);
        }

        private void OnDragStarted()
        {
            StartDraggingEvent();
        }

        private void OnDragEnded()
        {
            Reset();
        }

        private void Reset()
        {
            m_DropZone.state = DropZoneState.Default;
            m_DropZone.visibleIndicator = false;

            m_TrackingDragEvent = false;

            SetDropZoneActive(false);
        }

        private void DragEnter(DragEnterEvent evt)
        {
            SetDropZoneActive(true);
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
                m_DropZone.state = DropZoneState.AcceptDrag;

                SetDropZoneActive(true);
            }
        }

        private bool IsDraggingObjects()
        {
            return DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0;
        }
    }
}
