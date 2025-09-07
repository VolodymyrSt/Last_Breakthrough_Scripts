using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI.Other.Joystick
{
    public class JoystickHandler : MonoBehaviour
    {
        private IEventBus _eventBus;

        [Inject]
        private void Construct(IEventBus eventBus) =>
            _eventBus = eventBus;

        private void OnEnable()
        {
            _eventBus.SubscribeEvent((OnGameEndedSignal signal) => gameObject.SetActive(false));
            _eventBus.SubscribeEvent((OnGameWonSignal signal) => gameObject.SetActive(false));
            _eventBus.SubscribeEvent((OnPlayerDiedSignal signal) => gameObject.SetActive(false));
        }

        private void OnDestroy()
        {
            _eventBus?.UnSubscribeEvent((OnBeginningVideoEndedSignal signal) => gameObject.SetActive(true));
            _eventBus?.UnSubscribeEvent((OnGameEndedSignal signal) => gameObject.SetActive(false));
            _eventBus?.UnSubscribeEvent((OnGameWonSignal signal) => gameObject.SetActive(false));
            _eventBus?.UnSubscribeEvent((OnPlayerDiedSignal signal) => gameObject.SetActive(false));
        }
    }
}
