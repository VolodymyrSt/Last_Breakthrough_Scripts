using LastBreakthrought.Infrustructure.Services.ConfigProvider;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Util;
using System.Collections;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Other
{
    public class DayLightHandler : IInitializable
    {
        private readonly Light _light;
        private readonly IEventBus _eventBus;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IConfigProviderService _configProviderService;
        private bool _isDay;

        private bool _isGamePaused = false;

        public DayLightHandler(Light light, IEventBus eventBus, ICoroutineRunner coroutineRunner, IConfigProviderService configProviderService)
        {
            _light = light;
            _eventBus = eventBus;
            _coroutineRunner = coroutineRunner;
            _configProviderService = configProviderService;
        }

        ~DayLightHandler() 
        {
            _eventBus?.UnSubscribeEvent((OnGamePausedSignal signal) => _isGamePaused = true);
            _eventBus?.UnSubscribeEvent((OnGameResumedSignal signal) => _isGamePaused = false);
        }

        public void Initialize()
        {
            _light.intensity = _configProviderService.GameConfigSO.CurrentLightIntensity;
            _isDay = true;

            _eventBus.SubscribeEvent((OnGamePausedSignal signal) => _isGamePaused = true);
            _eventBus.SubscribeEvent((OnGameResumedSignal signal) => _isGamePaused = false);

            _coroutineRunner.PerformCoroutine(StartUpdatingLight());
        }

        private IEnumerator StartUpdatingLight()
        {
            while (true)
            {
                if (_isGamePaused) yield return null;

                if (_isDay)
                {
                    _light.intensity -= Time.deltaTime * Constants.LIGHT_TIME_MULTIPLAYER;
                    if (_light.intensity <= Constants.MIN_LIGHT_INTENSITY)
                    {
                        _light.intensity = Constants.MIN_LIGHT_INTENSITY;
                        _isDay = false;
                        yield return new WaitForSeconds(Constants.WAITING_TIME_LIGHT);
                    }
                    yield return null;
                }
                else
                {
                    _light.intensity += Time.deltaTime * Constants.LIGHT_TIME_MULTIPLAYER;
                    if (_light.intensity >= Constants.MAX_LIGHT_INTENSITY)
                    {
                        _light.intensity = Constants.MAX_LIGHT_INTENSITY;
                        _isDay = true;
                        yield return new WaitForSeconds(Constants.WAITING_TIME_LIGHT);
                    }
                    yield return null;
                }
            }
        }
    }
}
