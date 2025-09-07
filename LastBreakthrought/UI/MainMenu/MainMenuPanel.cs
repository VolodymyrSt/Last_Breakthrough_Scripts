using DG.Tweening;
using LastBreakthrought.Infrustructure.Services.EventBus;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Infrustructure.UI
{
    public abstract class MainMenuPanel : MonoBehaviour
    {
        protected IEventBus EventBus;

        private bool _isHidden = true;

        [Inject]
        private void Consturct(IEventBus eventBus) =>
            EventBus = eventBus;

        private void Awake() => Init();

        public abstract void Init();
        public abstract void OnPanelOpened();

        public void PerformShowing()
        {
            if (_isHidden)
            {
                Show();
                _isHidden = false;
            }
            else
            {
                Hide();
                _isHidden = true;
            }
        }

        public void Hide()
        {
            _isHidden = true;
            transform.DOScale(0f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.Linear)
                .Play().OnComplete(() =>
                    transform.gameObject.SetActive(false)
                );
        }

        private void Show()
        {
            transform.gameObject.SetActive(true);
            OnPanelOpened();

            transform.DOScale(1f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.Linear)
                .Play();
        }
    }
}
