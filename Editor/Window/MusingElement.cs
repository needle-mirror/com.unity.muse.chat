using Unity.Muse.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat
{
    internal class MusingElement : ManagedTemplate
    {
        const int k_RotationSpeed = 360;

        private VisualElement m_Spinner;
        private Text m_Message;
        private double m_LastRotationTime;
        private bool m_Running;
        private int m_Rotation;

        public MusingElement()
            : base(MuseChatConstants.UIModulePath)
        {
        }

        protected override void InitializeView(TemplateContainer view)
        {
            m_Spinner = view.Q<VisualElement>("musingSpinner");
            m_Message = view.Q<Text>("musingSpinnerMessage");

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

            Start();
        }

        public void Start()
        {
            if (m_Running)
            {
                return;
            }

            m_Running = true;
            m_LastRotationTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += UpdateProgress;
        }

        public void Stop()
        {
            if (!m_Running)
            {
                return;
            }

            m_Running = false;
            EditorApplication.update -= UpdateProgress;
        }

        public void SetMessage(string message)
        {
            m_Message.text = message;
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            Stop();
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
        }

        private void UpdateProgress()
        {
            double currentTime = EditorApplication.timeSinceStartup;
            double deltaTime = currentTime - m_LastRotationTime;

            m_Spinner.transform.rotation = Quaternion.Euler(0, 0, m_Rotation);

            m_Rotation += (int)(k_RotationSpeed * deltaTime);
            m_Rotation %= 360;
            if (m_Rotation < 0)
            {
                m_Rotation += 360;
            }

            m_LastRotationTime = currentTime;
        }
    }
}
