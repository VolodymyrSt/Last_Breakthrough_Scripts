using DG.Tweening;
using LastBreakthrought.Infrustructure;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Infrustructure.State;
using LastBreakthrought.Logic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LastBreakthrought.UI.PausedMenu
{
    public class PausedMenuView : MonoBehaviour
    {
        public event Action OnGoneToMenu;

        [Header("UI")]
        [SerializeField] private RectTransform _generalBackground;
        [SerializeField] private RectTransform _menuRoot;
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _goToMenuButton;

        private IEventBus _eventBus;
        private Game _game;
        private IAudioService _audioService;

        public bool IsClicked { get; set; } = false;

        [Inject]
        private void Construct(IEventBus eventBus, Game game, IAudioService audioService)
        {
            _eventBus = eventBus;
            _game = game;
            _audioService = audioService;
        }

        public void Init()
        {
            HideAtStart();

            _openButton.onClick.AddListener(() => Show());
            _closeButton.onClick.AddListener(() => Hide());

            _goToMenuButton.onClick.AddListener(() => { 
                _game.StateMachine.Enter<LoadMenuState>();
                OnGoneToMenu?.Invoke();
            });
        }

        public void Show()
        {
            IsClicked = true;
            _generalBackground.gameObject.SetActive(true);
            _openButton.gameObject.SetActive(false);

            _menuRoot.gameObject.SetActive(true);
            _menuRoot.DOScale(1f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() => 
                    _eventBus.Invoke(new OnGamePausedSignal()));
        }

        public void Hide()
        {
            IsClicked = false;
            _eventBus.Invoke(new OnGameResumedSignal());

            _menuRoot.DOScale(0, Constants.ANIMATION_DURATION)
                .SetEase(Ease.Linear)
                .Play()
                .OnComplete(() =>
                {
                    _generalBackground.gameObject.SetActive(false);
                    _menuRoot.gameObject.SetActive(false);
                    _openButton.gameObject.SetActive(true);
                });
        }

        public void ToggleSound()
        {
            if (_audioService.IsVolumeMax())
                _audioService.SetZeroVolume();
            else
                _audioService.SetMaxVolume();
        }

        private void HideAtStart()
        {
            _generalBackground.gameObject.SetActive(false);
            _menuRoot.gameObject.SetActive(false);
            _menuRoot.localScale = Vector3.zero;
        }

        private void OnDestroy()
        {
            _openButton.onClick.RemoveListener(() => Show());
            _closeButton.onClick.RemoveListener(() => Hide());
        }
    }
}
