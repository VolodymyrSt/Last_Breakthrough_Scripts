using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.NPC.Base;
using LastBreakthrought.Player;
using LastBreakthrought.Util;
using UnityEngine;
using UnityEngine.AI;

namespace LastBreakthrought.NPC.Robot.States
{
    public class RobotFollowingPlayerState : INPCState
    {
        private const string IS_MOVING = "isMoving";

        private readonly RobotBase _robotBase;
        private readonly NavMeshAgent _agent;
        private readonly PlayerHandler _playerHandler;
        private readonly Animator _animator;
        private readonly RobotBattary _robotBattary;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IAudioService _audioService;
        private readonly IEventBus _eventBus;
        private readonly float _followingSpeed;

        private Coroutine _follovingSoundCoroutine;

        public RobotFollowingPlayerState(RobotBase robotBase, NavMeshAgent agent, PlayerHandler playerHandler, Animator animator, RobotBattary robotBattary
            , ICoroutineRunner coroutineRunner, IAudioService audioService, IEventBus eventBus, float followingSpeed)
        {
            _robotBase = robotBase;
            _agent = agent;
            _playerHandler = playerHandler;
            _animator = animator;
            _robotBattary = robotBattary;
            _coroutineRunner = coroutineRunner;
            _audioService = audioService;
            _eventBus = eventBus;
            _followingSpeed = followingSpeed;
        }

        public void Enter()
        {
            _agent.isStopped = false;
            _agent.speed = _followingSpeed;
            _agent.stoppingDistance = Constants.ROBOT_STOP_FOLLOWING_DISTANCE;
            _animator.SetBool(IS_MOVING, true);

            _eventBus.SubscribeEvent<OnGamePausedSignal>(StopFollowing);
            _eventBus.SubscribeEvent<OnGameResumedSignal>(ContinueFollowing);
        }

        public void Exit()
        {
            SetFollowingAnimation(false);
            ClearFollowingSound();
        }

        public void Update()
        {
            _agent.SetDestination(_playerHandler.GetPosition());

            if (_agent.remainingDistance < Constants.ROBOT_STOP_FOLLOWING_DISTANCE + 0.01f)
            {
                SetFollowingAnimation(false);
                ClearFollowingSound();
            }
            else
            {
                SetFollowingAnimation(true);
                PlayFollowingSound();
            }

            _robotBattary.DecreaseCapacity();
            _robotBattary.CheckIfCapacityIsRechedLimit();
        }

        private void StopFollowing(OnGamePausedSignal signal) => ClearFollowingSound();
        private void ContinueFollowing(OnGameResumedSignal signal) => PlayFollowingSound();

        private void SetFollowingAnimation(bool isMoving) =>
            _animator.SetBool(IS_MOVING, isMoving);

        private void PlayFollowingSound() =>
            _audioService.PlayOnObject(Configs.Sound.SoundType.RobotFollowing, _robotBase, true);

        private void ClearFollowingSound() =>
            _audioService.StopOnObject(_robotBase, Configs.Sound.SoundType.RobotFollowing);
    }
}
