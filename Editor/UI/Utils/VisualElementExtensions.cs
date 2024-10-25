using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Muse.AppUI.UI;
using Unity.Muse.Common;
using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal static class VisualElementExtensions
    {
        /// <summary>
        /// Helper method to retrieve and setup a button with a callback.
        /// </summary>
        /// <param name="root">The root element to search on</param>
        /// <param name="id">The name of the button element</param>
        /// <param name="callback">The callback to register when clicking the button</param>
        /// <returns>The button element</returns>
        /// <exception cref="InvalidDataException">if no element could be found</exception>
        public static AppUI.UI.Button SetupButton(this VisualElement root, string id, EventCallback<PointerUpEvent> callback)
        {
            var element = root.Q<AppUI.UI.Button>(id);
            if (element == null)
            {
                throw new InvalidDataException("No such Button: " + id);
            }

            element.RegisterCallback(callback);
            return element;
        }

        /// <summary>
        /// Helper method to setup a dropdown field based on an enum
        /// </summary>
        /// <param name="root">the root that the element lives under</param>
        /// <param name="id">name of the element</param>
        /// <param name="enumDisplayResolver">resolve callback to get the enum names, if null it will resolve to the enum string</param>
        /// <param name="selectionType">selection type of the dropdown</param>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <returns>The dropdown element</returns>
        /// <exception cref="InvalidDataException">if the element does not exist as a member of the root</exception>
        public static Dropdown SetupEnumDropdown<T>(this VisualElement root, string id, Func<T, string> enumDisplayResolver = null, PickerSelectionType selectionType = PickerSelectionType.Single)
            where T: Enum
        {
            var element = root.Q<Dropdown>(id);
            if (element == null)
            {
                throw new InvalidDataException("No such Dropdown: " + id);
            }

            element.sourceItems = EnumDef<T>.Values;
            element.selectionType = selectionType;
            element.bindTitle = (x,y) => ResolveEnumDropdownTitle(x,y,enumDisplayResolver);
            element.bindItem = (x,y) => ResolveEnumItemLabel(x,y,enumDisplayResolver);

            return element;
        }

        private static void ResolveEnumItemLabel<T>(DropdownItem item, int index, Func<T, string> enumDisplayResolver)
            where T: Enum
        {
            if (index < 0 || index >= EnumDef<T>.Count)
            {
                throw new ArgumentException("Value is not part of enum " + TypeDef<T>.Name);
            }

            item.label = enumDisplayResolver == null
                ? EnumDef<T>.Names[index]
                : enumDisplayResolver.Invoke(EnumDef<T>.Values[index]);
        }

        private static void ResolveEnumDropdownTitle<T>(DropdownItem selectedItem, IEnumerable<int> selectedValues, Func<T, string> enumDisplayResolver)
            where T: Enum
        {
            int index = selectedValues.FirstOrDefault();
            if (index < 0 || index >= EnumDef<T>.Count)
            {
                throw new ArgumentException("Value is not part of enum " + TypeDef<T>.Name);
            }

            selectedItem.label = enumDisplayResolver == null
                ? EnumDef<T>.Names[index]
                : enumDisplayResolver.Invoke(EnumDef<T>.Values[index]);
        }

        /// <summary>
        /// Helper method to register an element as session tracked, which means it's disable state is based on the muse session status
        /// </summary>
        /// <param name="element">The element to track</param>
        public static void SetSessionTracked(this VisualElement element, Model model)
        {
            if (UserSessionState.instance.DebugUIModeEnabled)
            {
                // We don't do session tracking in debug to have controls available
                return;
            }
            
            element.AddManipulator(new SessionStatusTracker(model));
        }
    }
}
