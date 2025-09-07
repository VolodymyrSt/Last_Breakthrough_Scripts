using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.Infrustructure.UI
{
    public class MainMenuSettingButton : MainMenuButton
    {
        [SerializeField] private MainMenuSettingPanel mainMenuSettingPanel;

        public override void OnAwake()
        {
            Button.onClick.AddListener(() => mainMenuSettingPanel.PerformShowing());
            mainMenuSettingPanel.Hide();
        }
    }
}
