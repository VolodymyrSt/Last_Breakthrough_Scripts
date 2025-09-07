using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Logic.Camera;
using LastBreakthrought.UI.Windows;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Logic.InteractionZone
{
    public class InteractionZoneHandler : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private InterationZoneView _interationZoneView;
        [SerializeField] private WindowHandlerBase _windowHandler;

        [Header("Setting")]
        [SerializeField] private LayerMask _playerLayerMask;

        private IAudioService _audioService;
        private FollowCamera _followCamera;
        private IEventBus _eventBus;

        private readonly Collider[] _playerActivation = new Collider[1];
        private readonly Collider[] _playerInteraction = new Collider[1];

        private float _interactionRadious;
        private float _activationRadious;

        private bool _isPlayerInteracting = false;
        private bool _isInteractingZoneHidden = false;

        [Inject]
        private void Construct(IAudioService audioService, FollowCamera followCamera, IEventBus eventBus)
        {
            _audioService = audioService;
            _followCamera = followCamera;
            _eventBus = eventBus;
        }

        private void OnValidate()
        {
            _interactionRadious = _interationZoneView.transform.localScale.x / 2;
            _activationRadious = 1.25f * _interationZoneView.transform.localScale.x;
        }
        
        public void Init()
        {
            _interactionRadious = _interationZoneView.transform.localScale.x / 2;
            _activationRadious = 1.25f * _interationZoneView.transform.localScale.x;
            _interationZoneView.HideOnInit();
        }

        private void Start()
        {
            _eventBus.SubscribeEvent((OnGameEndedSignal signal) => Disactivate());
            _eventBus.SubscribeEvent((OnGameWonSignal signal) => Disactivate());
            _eventBus.SubscribeEvent((OnPlayerDiedSignal signal) => Disactivate());
        }

        private void Update()
        {
            if (TryToActivate())
                TryToInteract();
        }

        private bool TryToActivate()
        {
            Vector3 position = transform.position  + Vector3.up;
            var targets = Physics.OverlapSphereNonAlloc(position, _activationRadious, _playerActivation, _playerLayerMask);

            if (targets > 0)
            {
                if (!_isInteractingZoneHidden) return true;

                _interationZoneView.Show();
                _isInteractingZoneHidden = false;
                return true;
            }
            else
            {
                if (_isInteractingZoneHidden) return false;

                _interationZoneView.Hide();
                _isInteractingZoneHidden = true;
                return false;
            }
        }

        private void TryToInteract()
        {
            Vector3 position = transform.position + Vector3.up;
            var targets = Physics.OverlapSphereNonAlloc(position, _interactionRadious, _playerInteraction, _playerLayerMask);

            if (targets > 0)
            {
                if (_isPlayerInteracting) 
                    return;
                else
                {
                    _isPlayerInteracting = true;
                    ShowPopup();
                }
            }
            else
            {
                HidePopup();
                _isPlayerInteracting = false;
            }
        }

        public void Disactivate()
        {
            gameObject.SetActive(false);
            HidePopup();
        }

        public void Activate() => gameObject.SetActive(true);

        private void HidePopup() => _windowHandler.DeactivateWindow();
        private void ShowPopup()
        {
            _windowHandler.ActivateWindow();
            _audioService.PlayOnObject(Configs.Sound.SoundType.WindowOpen, _followCamera);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, _activationRadious);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, _interactionRadious);
        }

        private void OnDestroy()
        {
            _eventBus?.UnSubscribeEvent((OnGameEndedSignal signal) => Disactivate());
            _eventBus?.UnSubscribeEvent((OnGameWonSignal signal) => Disactivate());
            _eventBus?.UnSubscribeEvent((OnPlayerDiedSignal signal) => Disactivate());
        }
    }
}
