using DG.Tweening;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI
{
    public class GameplayHub : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _uiRootsForHide = new();

        private IEventBus _eventBus;

        [Inject]
        private void Construct(IEventBus eventBus) => 
            _eventBus = eventBus;

        public void Init()
        {
            _eventBus.SubscribeEvent((OnBeginningVideoEndedSignal signal) => Show());

            _eventBus.SubscribeEvent((OnGameEndedSignal signal) => HideUiRoots());

            _eventBus.SubscribeEvent((OnGameWonSignal signal) => HideUiRoots());

            _eventBus.SubscribeEvent((OnPlayerDiedSignal signal) => HideUiRoots());
        }

        private void Awake() => Hide();

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

        private void HideUiRoots()
        {
            foreach (var root in _uiRootsForHide)
                root.SetActive(false);
        }

        private void OnDestroy()
        {
            _eventBus?.UnSubscribeEvent((OnBeginningVideoEndedSignal signal) => Show());
            _eventBus?.UnSubscribeEvent((OnGameEndedSignal signal) => HideUiRoots());
            _eventBus?.UnSubscribeEvent((OnGameWonSignal signal) => HideUiRoots());
            _eventBus?.UnSubscribeEvent((OnPlayerDiedSignal signal) => HideUiRoots());
        }
    }
}
