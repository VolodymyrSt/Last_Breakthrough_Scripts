using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Other;
using Zenject;
using System;
using LastBreakthrought.UI.Timer;

namespace LastBreakthrought.UI.VictoryMenu
{
    public class VictoryMenuHandler : IInitializable, IDisposable
    {
        private readonly VictoryMenuView _view;
        private readonly TimeHandler _timeHandler;
        private readonly IEventBus _eventBus;
        private readonly TimerController _timer;

        public VictoryMenuHandler(VictoryMenuView view, TimeHandler timeHandler, IEventBus eventBus, TimerController timerController)
        {
            _view = view;
            _timeHandler = timeHandler;
            _eventBus = eventBus;
            _timer = timerController;
        }

        public void Initialize()
        {
            _view.Init();

            _eventBus.SubscribeEvent<OnVictoryVideoEndedSignal>(ShowPopup);
            _eventBus.SubscribeEvent<OnGamePausedSignal>(Pause);
            _view.OnGoneToMenu += GoToMenu;

        }

        private void ShowPopup(OnVictoryVideoEndedSignal signal)
        {
            _view.Show();
            _view.SetVictoryTimer(_timer.GetDay(), _timer.GetMinutes(), _timer.GetSeconds());
        }

        private void Pause(OnGamePausedSignal signal) =>
            _timeHandler.StopTime();

        private void GoToMenu() => _timeHandler.ResetTime();

        public void Dispose()
        {
            _eventBus.UnSubscribeEvent<OnGamePausedSignal>(Pause);
            _eventBus.UnSubscribeEvent<OnVictoryVideoEndedSignal>(ShowPopup);
        }
    }
}
