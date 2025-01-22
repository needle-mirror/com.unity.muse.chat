using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Muse.Chat.UI.Components;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal class RoutesPopup : ManagedTemplate
    {
        VisualElement m_Root;
        VisualElement m_RunItem;
        VisualElement m_CodeItem;
        VisualElement m_MatchThreeItem;
        const string k_FirstPopupItem = "mui-first-popup-item";
        const string k_LastPopupItem = "mui-last-popup-item";


        List<string> m_RouteLabels;

        public Action OnSelectionChanged;

        public RoutesPopup()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        public void DisplayRoutes(string routeFilter = "", bool initialCreation = false)
        {
            int firstIndex = 0;
            int lastIndex = -1;
            for (int i = 0; i < MuseChatConstants.Routes.Count; i++)
            {
                RouteCommand route = MuseChatConstants.Routes[i];

                if (initialCreation)
                {
                    RoutesPopupEntry newPopupItem = new RoutesPopupEntry(route.PopupLabel, route.Description);
                    newPopupItem.Initialize();
                    m_Root.Add(newPopupItem);
                    newPopupItem.name = route.Route;
                    newPopupItem.RegisterCallback<ClickEvent>(_ => ChangeRoute(route.Type));
                }

                var popupItem = m_Root.Q<VisualElement>(route.Route);
                popupItem.visible = route.Label.StartsWith(routeFilter);
                popupItem.RemoveFromClassList(k_FirstPopupItem);
                popupItem.RemoveFromClassList(k_LastPopupItem);

                if (route.Label.StartsWith(routeFilter))
                {
                    popupItem.style.display = DisplayStyle.Flex;
                    lastIndex += 1;
                }
                else
                {
                    popupItem.style.display = DisplayStyle.None;
                }
            }

            List<VisualElement> popupItems = m_Root.Children().ToList();
            popupItems[firstIndex].AddToClassList(k_FirstPopupItem);
            if (lastIndex >= 0)
            {
                popupItems[lastIndex].AddToClassList(k_LastPopupItem);
            }
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Root = view.Q<VisualElement>("popupRoot");
            DisplayRoutes("", true);
        }

        void ChangeRoute (ChatCommandType type)
        {
            UserSessionState.instance.SelectedCommandMode = type;
            OnSelectionChanged.Invoke();
        }
    }
}
