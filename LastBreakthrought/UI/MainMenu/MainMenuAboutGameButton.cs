using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.Infrustructure.UI
{
    public class MainMenuAboutGameButton : MainMenuButton
    {
        [SerializeField] MainMenuAboutGamePanel _mainMenuAboutGamePanel;

        public override void OnAwake()
        {
            Button.onClick.AddListener(() => _mainMenuAboutGamePanel.PerformShowing());
            _mainMenuAboutGamePanel.Hide();
        }
    }
}
