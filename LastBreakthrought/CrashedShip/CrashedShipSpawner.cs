using LastBreakthrought.Infrustructure;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.CrashedShip 
{
    public class CrashedShipSpawner : MonoBehaviour
    {
        private CrashedShipFactory _crashedShipFactory;
        private ICrashedShip _spawnedCrashedShip;
        private IEventBus _eventBus;

        private Coroutine _spawningCoroutine;
        private bool _isShipDestroyed = false;

        [Inject]
        private void Construct(CrashedShipFactory crashedShipFactory, Game game, IEventBus eventBus)
        {
            _crashedShipFactory = crashedShipFactory;
            _eventBus = eventBus;
            game.SpawnersContainer.AddCrashedShipSpawner(this);
        }

        private void OnEnable()
        {
            _eventBus.SubscribeEvent((OnGamePausedSignal signal) => {
                if (_isShipDestroyed)
                    if (_spawningCoroutine != null)
                        StopCoroutine(_spawningCoroutine);
            });
            _eventBus.SubscribeEvent((OnGameResumedSignal signal) => {
                if (_isShipDestroyed)
                    Respawn();
            });
        }
        public void SpawnCrashedShip()
        {
            _spawnedCrashedShip = _crashedShipFactory.SpawnAt(transform.position, transform);
            _spawnedCrashedShip.OnDestroyed += SetDestroyed;
            _spawnedCrashedShip.OnDestroyed += Respawn;
        }

        private void Respawn() => 
            _spawningCoroutine = StartCoroutine(SpawnShipAfterTime());

        private void SetDestroyed() => _isShipDestroyed = true;

        private IEnumerator SpawnShipAfterTime()
        {
            yield return new WaitForSeconds(Constants.CRASHED_SHIP_SPAWNING_TIME_AFTER_DESTRUCTION);
            SpawnCrashedShip();
            _isShipDestroyed = false;
        }
    }
}

