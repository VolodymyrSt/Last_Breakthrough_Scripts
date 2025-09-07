using LastBreakthrought.CrashedShip;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using System;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Player
{
    public class PlayerHandler : MonoBehaviour, IDamagable, IEnemyTarget
    {
        public event Action<float> OnPlayerBeenAttacked;

        [SerializeField] private GameObject _wreckageDetectorItemPrefab;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private CrashedShipSeeker _crashedShipSeeker;

        private IAudioService _audioService;
        private IEventBus _eventBus;
        private Coroutine _audioCoroutine;

        [Inject]
        private void Construct(IAudioService audioService, IEventBus eventBus)
        {
            _audioService = audioService;
            _eventBus = eventBus;
        }

        private void OnEnable() => HideDetectorItem();

        public Vector3 GetPosition() => transform.position;

        public void ApplyDamage(float damage) => 
            OnPlayerBeenAttacked.Invoke(damage);

        public void SetMovingAnimation(bool withItem) =>
            _playerAnimator.SetMoving(withItem);

        public void ShowDetectorItem()
        {
            _audioService.PlayOnObject(Configs.Sound.SoundType.PlayerUseDetector, this, true);

            _wreckageDetectorItemPrefab.SetActive(true);
        }

        public void HideDetectorItem()
        {
            _audioService.StopOnObject(this, Configs.Sound.SoundType.PlayerUseDetector);

            _wreckageDetectorItemPrefab.SetActive(false);
        }

        public ICrashedShip GetSeekedCrashedShip() =>
            _crashedShipSeeker.FoundCrashedShip;
    }
}

