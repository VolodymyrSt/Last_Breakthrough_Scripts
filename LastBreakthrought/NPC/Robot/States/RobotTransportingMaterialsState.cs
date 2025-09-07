using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Logic.MaterialRecycler;
using LastBreakthrought.NPC.Base;
using LastBreakthrought.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LastBreakthrought.NPC.Robot.States
{
    public class RobotTransportingMaterialsState : INPCState
    {
        private const string IS_Moving = "isMoving";
        private const string IS_TRANSPORTING = "isTransporting";

        private readonly RobotTransporter _robot;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;
        private readonly RobotBattary _robotBattary;
        private readonly RecycleMachine _recycleMachine;
        private readonly IEventBus _eventBus;
        private readonly IAudioService _audioService;
        private readonly float _movingSpeed;

        private Coroutine _transportingAudioCoroutine;
        private Coroutine _transportingCoroutine;
        private bool _isCarring = false;

        public RobotTransportingMaterialsState(RobotTransporter robot, ICoroutineRunner coroutineRunner
            , NavMeshAgent agent, Animator animator, RobotBattary robotBattary, RecycleMachine recycleMachine
            , IEventBus eventBus, IAudioService audioService, float followingSpeed)
        {
            _robot = robot;
            _coroutineRunner = coroutineRunner;
            _agent = agent;
            _animator = animator;
            _robotBattary = robotBattary;
            _recycleMachine = recycleMachine;
            _eventBus = eventBus;
            _audioService = audioService;
            _movingSpeed = followingSpeed;
        }

        public void Enter()
        {
            _agent.isStopped = false;
            _agent.speed = _movingSpeed;
            _agent.stoppingDistance = Constants.TRANSPORTING_STOP_DISTANCE;
            _animator.SetBool(IS_Moving, true);

            _eventBus.SubscribeEvent<OnGamePausedSignal>(StopTransporting);
            _eventBus.SubscribeEvent<OnGameResumedSignal>(ContinueTransporting);
        }

        public void Exit()
        {
            _animator.SetBool(IS_TRANSPORTING, false);
            _animator.SetBool(IS_Moving, false);

            _isCarring = false;

            if (_transportingCoroutine != null)
                _coroutineRunner.HandleStopCoroutine(_transportingCoroutine);

            ClearTransportingSound();

            _eventBus.UnSubscribeEvent<OnGamePausedSignal>(StopTransporting);
            _eventBus.UnSubscribeEvent<OnGameResumedSignal>(ContinueTransporting);
        }

        public void Update()
        {
            _agent.SetDestination(_recycleMachine.GetMachinePosition());

            var isArrived = Vector3.Distance(_agent.transform.position, _recycleMachine.GetMachinePosition()) <= Constants.TRANSPORTING_STOP_DISTANCE + 1f;

            if (isArrived && !_isCarring)
            {
                _isCarring = true;
                _animator.SetBool(IS_Moving, false);
                _transportingCoroutine = _coroutineRunner.PerformCoroutine(StartTransporting());
            }

            _robotBattary.DecreaseCapacity();
            _robotBattary.CheckIfCapacityIsRechedLimit();
        }

        private IEnumerator StartTransporting()
        {
            _animator.SetBool(IS_TRANSPORTING, true);
            PlayTransportingSound();

            while (_robot.TransportedMaterials.Count > 0)
            {
                for (int i = 0; i < _robot.TransportedMaterials.Count; i++)
                {
                    var transportedMaterial = _robot.TransportedMaterials[i];

                    if (transportedMaterial != null)
                    {
                        yield return new WaitForSecondsRealtime(Constants.TRANSPORTING_TIME);
                        _recycleMachine.RecycleEntireMaterial(transportedMaterial);
                        _robot.TransportedMaterials.Remove(transportedMaterial);
                    }
                }
            }

            _robot.ClearCrashedShip();
            _robot.HasLoadedMaterials = false;
        }

        private void StopTransporting(OnGamePausedSignal signal)
        {
            _coroutineRunner.HandleStopCoroutine(_transportingCoroutine);
            ClearTransportingSound();
        }

        private void ContinueTransporting(OnGameResumedSignal signal)
        {
            if (_isCarring)
            {
                _transportingCoroutine = _coroutineRunner.PerformCoroutine(StartTransporting());
                PlayTransportingSound();
            }
        }

        private void PlayTransportingSound() =>
             _audioService.PlayOnObject(Configs.Sound.SoundType.TransporterTransporting, _robot, true);

        private void ClearTransportingSound() => 
            _audioService.StopOnObject(_robot, Configs.Sound.SoundType.TransporterTransporting);
    }
}
