using Unity.Muse.AppUI.UI;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    static class AppUIUtils
    {
        /// <summary>
        /// Fixes issue with the foldout icon in the accordion where alignment is incorrect.
        /// </summary>
        /// <param name="target">Accordion element to target</param>
        public static void RealignFoldoutIcon(this Accordion target)
        {
            var icon = target.Q<Icon>("appui-accordionitem__indicator");
            icon.RemoveFromClassList("appui-icon--caret-down--regular");
            icon.AddToClassList("appui-icon--caret-down--fill");
            var parent = icon.parent;

            parent.Remove(icon);
            parent.Insert(0, icon);
        }
    }
}
