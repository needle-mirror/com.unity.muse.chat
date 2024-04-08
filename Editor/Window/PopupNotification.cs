using System;
using Unity.Muse.AppUI.UI;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    class PopupNotification : ManagedTemplate
    {
        const int k_AnimationDuration = 100;

        Text m_TextField;

        Icon m_Icon;

        bool m_Dismissed;

        public Action<PopupNotification> OnDismissed;

        public class PopupNotificationContext
        {
            public string message;
            public string icon;
        }

        public PopupNotification()
            : base(MuseChatConstants.UIModulePath)
        { }

        public void SetData(PopupNotificationContext messageContext)
        {
            m_TextField.text = messageContext.message;
            m_Icon.iconName = messageContext.icon;
        }

        protected override void InitializeView(TemplateContainer view)
        {
            view.SetupButton("dismissButton", OnDismissClicked);

            m_Icon = view.Q<Icon>("icon");

            m_TextField = view.Q<Text>("messageField");

            contentContainer.style.opacity = 0;
            contentContainer.experimental.animation.Start(0, 1, k_AnimationDuration, (element, f) =>
            {
                element.style.opacity = f;
            });
        }

        void OnDismissClicked(PointerUpEvent evt)
        {
            Dismiss();
        }

        public void Dismiss(bool animated = true)
        {
            if (m_Dismissed)
            {
                return;
            }

            m_Dismissed = true;

            OnDismissed?.Invoke(this);

            if (animated)
            {
                contentContainer.experimental.animation.Start(1, 0, k_AnimationDuration, (element, f) =>
                {
                    element.style.opacity = f;
                }).OnCompleted(() =>
                {
                    parent.Remove(this);
                });
            }
            else
            {
                parent.Remove(this);
            }
        }
    }
}
