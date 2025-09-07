using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.Infrustructure.UI
{
    public class MainMenuQuitButton : MainMenuButton
    {
        public override void OnAwake() => 
            Button.onClick.AddListener(() => Application.Quit());
    }
}
