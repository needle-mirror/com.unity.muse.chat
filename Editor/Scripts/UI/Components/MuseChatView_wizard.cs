using Unity.Muse.Common.Account;
using UnityEditor;
using UnityEngine.UIElements;

namespace Unity.Muse.Chat.UI.Components
{
    partial class MuseChatView
    {
        WizardPanelBase m_WizardPanel;

        /// <summary>
        /// Shows any new wizard the user has not seen yet.
        /// </summary>
        private void ShowRequiredWizard()
        {
            // Make sure this check is done again if the account info changes:
            AccountInfo.Instance.OnLegalConsentChanged -= ShowRequiredWizard;
            AccountInfo.Instance.OnLegalConsentChanged += ShowRequiredWizard;

            AccountInfo.Instance.OnOrganizationChanged -= ShowRequiredWizard;
            AccountInfo.Instance.OnOrganizationChanged += ShowRequiredWizard;

            if (!AccountInfo.Instance.IsReady)
            {
                // There is no callback for IsReady, just try again later:
                EditorApplication.delayCall += ShowRequiredWizard;
                return;
            }

            if (AgentWizard.ShowIfNeeded(this))
            {
                return;
            }

            // TODO Add any future wizard checks here:
        }

        void CloseWizard()
        {
            m_WizardPanel?.RemoveFromHierarchy();
        }

        internal void ShowWizard(WizardPanelBase wizardPanel)
        {
            CloseWizard();

            m_WizardPanel = wizardPanel;

            m_WizardPanel.Initialize();
            m_WizardPanel.OnClose += WizardPanelClosed;
            m_RootPanel.Add(m_WizardPanel);
        }

        private void WizardPanelClosed(WizardPanelBase obj)
        {
            // Check if another wizard has not been seen:
            ShowRequiredWizard();
        }
    }
}
