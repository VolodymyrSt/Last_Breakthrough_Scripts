using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Other;
using System;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI.PausedMenu
{
    public class PausedMenuHandler : IInitializable, IDisposable, ITickable
    {
        private readonly PausedMenuView _view;
        private readonly TimeHandler _timeHandler;
        private readonly IEventBus _eventBus;

        public PausedMenuHandler(PausedMenuView view, TimeHandler timeHandler, IEventBus eventBus)
        {
            _view = view;
            _timeHandler = timeHandler;
            _eventBus = eventBus;
        }

        public void Initialize()
        {
            _view.Init();

            _eventBus.SubscribeEvent<OnGamePausedSignal>(Pause);
            _eventBus.SubscribeEvent<OnGameResumedSignal>(Resume);
            _view.OnGoneToMenu += GoToMenu;
        }

        public void Tick() => PerformEscapeInput();

        private void PerformEscapeInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_view.IsClicked)
                    _view.Hide();
                else
                    _view.Show();
            }
        }

        private void Pause(OnGamePausedSignal signal) =>
            _timeHandler.StopTime();

        private void Resume(OnGameResumedSignal signal) =>
            _timeHandler.ResetTime();

        private void GoToMenu() => _timeHandler.ResetTime();

        public void Dispose()
        {
            _eventBus.UnSubscribeEvent<OnGamePausedSignal>(Pause);
            _eventBus.UnSubscribeEvent<OnGameResumedSignal>(Resume);
        }
    }
}
