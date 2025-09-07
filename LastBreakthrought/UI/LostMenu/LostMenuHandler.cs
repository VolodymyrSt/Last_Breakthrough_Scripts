using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Other;
using Zenject;
using System;

namespace LastBreakthrought.UI.LostMenu
{
    public class LostMenuHandler : IInitializable, IDisposable
    {
        private readonly LostMenuView _view;
        private readonly TimeHandler _timeHandler;
        private readonly IEventBus _eventBus;

        public LostMenuHandler(LostMenuView view, TimeHandler timeHandler, IEventBus eventBus)
        {
            _view = view;
            _timeHandler = timeHandler;
            _eventBus = eventBus;
        }

        public void Initialize()
        {
            _view.Init();

            _eventBus.SubscribeEvent<OnExploededStarVideoEndedSignal>(ShowPopup);
            _eventBus.SubscribeEvent<OnPlayerDiedSignal>(ShowPopup);
            _view.OnGoneToMenu += GoToMenu;
        }

        private void ShowPopup(OnExploededStarVideoEndedSignal signal) => _view.Show();
        private void ShowPopup(OnPlayerDiedSignal signal) => _view.Show();
        private void GoToMenu() => _timeHandler.ResetTime();

        public void Dispose()
        {
            _eventBus?.UnSubscribeEvent<OnExploededStarVideoEndedSignal>(ShowPopup);
            _eventBus?.UnSubscribeEvent<OnPlayerDiedSignal>(ShowPopup);
        }
    }
}
