using LastBreakthrought.Util;
using System.Collections;
using Zenject;
using UnityEngine;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Infrustructure.Services.ConfigProvider;
using System;

namespace LastBreakthrought.UI.Timer
{
    public class TimerController : IInitializable, IDisposable
    {
        private readonly TimerView _timerView;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IEventBus _eventBus;

        private bool _isTimeRunUp;
        private int _currentDays;
        private int _currentMinutes;
        private int _currentSeconds;

        private bool _isGamePaused;
        private bool _isTimerPaused;

        public TimerController(TimerView timerView, ICoroutineRunner coroutineRunner, IEventBus eventBus, IConfigProviderService configProvider)
        {
            _timerView = timerView;
            _coroutineRunner = coroutineRunner;
            _eventBus = eventBus;

            _currentDays = configProvider.GameConfigSO.StartedDay;
            _currentMinutes = configProvider.GameConfigSO.StartedMinute;
            _currentSeconds = configProvider.GameConfigSO.StartedSecond;

            _timerView.UpdateDay(_currentDays);
            _timerView.UpdateClock(_currentMinutes, _currentSeconds);
        }

        public void Initialize()
        {
            _isTimeRunUp = false;
            _isGamePaused = false;
            _isTimerPaused = false;

            _eventBus.SubscribeEvent((OnGamePausedSignal signal) => StopTimer());
            _eventBus.SubscribeEvent((OnGameResumedSignal signal) => ResumeTimer());

            _eventBus.SubscribeEvent((OnGameEndedSignal signal) => SetTimerOnPause());
            _eventBus.SubscribeEvent((OnGameWonSignal signal) => SetTimerOnPause());
            _eventBus.SubscribeEvent((OnPlayerDiedSignal signal) => SetTimerOnPause());

            _coroutineRunner.PerformCoroutine(StartTimer());
        }

        public int GetDay() => _currentDays;
        public int GetMinutes() => _currentMinutes;
        public int GetSeconds() => _currentSeconds;

        private IEnumerator StartTimer()
        {
            while (true)
            {
                if (!_isGamePaused)
                {
                    if (!_isTimerPaused)
                    {
                        yield return new WaitForSecondsRealtime(1);

                        _currentSeconds++;

                        if (_currentSeconds == 60)
                        {
                            _currentSeconds = 0;
                            ChangeTime();

                        }

                        _timerView.UpdateClock(_currentMinutes, _currentSeconds);

                        if (_isTimeRunUp) yield break;
                    }
                }
                yield return null;
            }
        }

        private void ChangeTime()
        {
            _currentMinutes++;

            if (_currentMinutes == 24)
            {
                _currentMinutes = 0;

                ChangeDay();
            }
        }

        private void ChangeDay()
        {
            _currentDays++;

            if (_currentDays == 5)
            {
                _isTimeRunUp = true;
                _eventBus.Invoke(new OnGameEndedSignal());
            }

            _timerView.UpdateDay(_currentDays);
        }

        private void StopTimer() =>
            _isGamePaused = true;

        private void ResumeTimer() =>
            _isGamePaused = false;

        private void SetTimerOnPause() =>
            _isTimerPaused = true;

        public void Dispose()
        {
            _eventBus?.UnSubscribeEvent<OnGamePausedSignal>(StopTimer);
            _eventBus?.UnSubscribeEvent<OnGameResumedSignal>(ResumeTimer);
            _eventBus?.UnSubscribeEvent<OnGameEndedSignal>(SetTimerOnPause);
            _eventBus?.UnSubscribeEvent<OnGameWonSignal>(SetTimerOnPause);
            _eventBus?.UnSubscribeEvent<OnPlayerDiedSignal>(SetTimerOnPause);
        }
    }
}
