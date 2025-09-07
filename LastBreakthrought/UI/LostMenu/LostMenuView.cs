using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Infrustructure.Services.EventBus;
using UnityEngine;
using Zenject;
using System;
using DG.Tweening;
using LastBreakthrought.Infrustructure;
using UnityEngine.UI;
using LastBreakthrought.Infrustructure.State;

namespace LastBreakthrought.UI.LostMenu
{
    public class LostMenuView : MonoBehaviour
    {
        public event Action OnGoneToMenu;

        [Header("UI")]
        [SerializeField] private RectTransform _generalBackground;
        [SerializeField] private RectTransform _menuRoot;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _goToMenuButton;

        private IEventBus _eventBus;
        private Game _game;

        [Inject]
        private void Construct(IEventBus eventBus, Game game)
        {
            _eventBus = eventBus;
            _game = game;
        }

        public void Init()
        {
            HideAtStart();

            _quitButton.onClick.AddListener(() => Application.Quit());

            _goToMenuButton.onClick.AddListener(() => {
                _game.StateMachine.Enter<LoadMenuState>();
                OnGoneToMenu?.Invoke();
            });
        }

        public void Show()
        {
            _generalBackground.gameObject.SetActive(true);

            _menuRoot.gameObject.SetActive(true);
            _menuRoot.DOScale(1f, Constants.ANIMATION_DURATION)
                .SetEase(Ease.Linear)
                .Play();
        }

        private void HideAtStart()
        {
            _generalBackground.gameObject.SetActive(false);
            _menuRoot.gameObject.SetActive(false);
            _menuRoot.localScale = Vector3.zero;
        }

        private void OnDestroy()
        {
            _quitButton.onClick.RemoveListener(() => Application.Quit());
            _goToMenuButton.onClick.RemoveListener(() => {
                _game.StateMachine.Enter<LoadMenuState>();
                OnGoneToMenu?.Invoke();
            });
        }
    }
}
