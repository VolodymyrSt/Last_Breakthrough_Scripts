using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _animator;

        private static readonly int MoveHash = Animator.StringToHash("Moving");
        private static readonly int MoveWithItemHash = Animator.StringToHash("MowingWithItem");
        private static readonly int IsUsingItemHash = Animator.StringToHash("IsUsingItem");
        private static readonly int DiedHash = Animator.StringToHash("Died");

        private IEventBus _eventBus;

        private bool _isUsingItem = false;

        [Inject]
        private void Construct(IEventBus eventBus) =>
            _eventBus = eventBus;

        private void Start() => 
            _eventBus.SubscribeEvent((OnPlayerDiedSignal signal) => SetDiedAnimation());

        private void Update()
        {
            if (!_isUsingItem)
                _animator.SetFloat(MoveHash, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);
            else
                _animator.SetFloat(MoveWithItemHash, _characterController.velocity.magnitude, 0.1f, Time.deltaTime);
        }

        public void SetMoving(bool withItem)
        {
            _isUsingItem = withItem;
            _animator.SetBool(IsUsingItemHash, withItem);
        }

        public void SetDiedAnimation() =>
            _animator.SetBool(DiedHash, true);

        private void OnDestroy() => 
            _eventBus?.UnSubscribeEvent((OnPlayerDiedSignal signal) => SetDiedAnimation());
    }
}

