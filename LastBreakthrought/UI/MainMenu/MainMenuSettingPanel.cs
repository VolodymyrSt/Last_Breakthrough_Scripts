using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using UnityEngine;

namespace LastBreakthrought.Infrustructure.UI
{
    public class MainMenuSettingPanel : MainMenuPanel
    {
        [SerializeField] private AudioSource _audioSource;

        private bool _isPaused = false;

        public override void Init() =>
            EventBus.SubscribeEvent((OnMainMenuAboutGamePanelOpenedSignal signal) => Hide());

        public override void OnPanelOpened() =>
            EventBus.Invoke(new OnMainMenuSettingPanelOpenedSignal());

        public void ToggleSound()
        {
            if (_isPaused)
            {
                _audioSource.Play();
                _isPaused = false;
            }
            else
            {
                _audioSource.Pause();
                _isPaused = true;
            }
        }
    }
}
