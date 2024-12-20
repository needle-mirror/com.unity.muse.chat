using UnityEngine;

namespace Unity.Muse.Chat.UI
{
    interface IContextReferenceVisualElement
    {
        void RefreshVisualElement(Object activeTargetObject, Component activeTargetComponent);
    }
}
