using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.NPC.Base;
using LastBreakthrought.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LastBreakthrought.NPC.Robot.States
{
    public class RobotLoadingUpMaterialsState : INPCState
    {
        private const string IS_Moving = "isMoving";
        private const string IS_TRANSPORTING = "isTransporting";

        private readonly RobotTransporter _robot;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;
        private readonly RobotBattary _robotBattary;
        private readonly IEventBus _eventBus;
        private readonly IAudioService _audioService;
        private readonly float _movingSpeed;

        private Coroutine _loadingUpCoroutine;
        private Coroutine _loadingUpAudioCoroutine;
        private bool _isTransporting = false;

        public RobotLoadingUpMaterialsState(RobotTransporter robot, ICoroutineRunner coroutineRunner, NavMeshAgent agent,
            Animator animator, RobotBattary robotBattary, IEventBus eventBus, IAudioService audioService, float followingSpeed)
        {
            _robot = robot;
            _coroutineRunner = coroutineRunner;
            _agent = agent;
            _animator = animator;
            _robotBattary = robotBattary;
            _eventBus = eventBus;
            _movingSpeed = followingSpeed;
            _audioService = audioService;
        }

        public void Enter()
        {
            _agent.isStopped = false;
            _agent.speed = _movingSpeed;
            _agent.stoppingDistance = Constants.LOADING_STOP_DISTANCE;
            _animator.SetBool(IS_Moving, true);

            _eventBus.SubscribeEvent<OnGamePausedSignal>(StopLoading);
            _eventBus.SubscribeEvent<OnGameResumedSignal>(ContinueLoading);
        }

        public void Exit()
        {
            _animator.SetBool(IS_TRANSPORTING, false);
            _animator.SetBool(IS_Moving, false);

            _isTransporting = false;

            if (_loadingUpCoroutine != null)
                _coroutineRunner.HandleStopCoroutine(_loadingUpCoroutine);

            ClearLoadingSound();

            _eventBus.UnSubscribeEvent<OnGamePausedSignal>(StopLoading);
            _eventBus.UnSubscribeEvent<OnGameResumedSignal>(ContinueLoading);
        }

        public void Update()
        {
            if (_robot.CrashedShip == null) return;

            _agent.SetDestination(_robot.CrashedShip.GetPosition());

            CheckForWhenToStartLoading();

            _robotBattary.DecreaseCapacity();
            _robotBattary.CheckIfCapacityIsRechedLimit();

            CheckIfLoadingIsDone();
        }

        private void CheckForWhenToStartLoading()
        {
            var isArrived = Vector3.Distance(_agent.transform.position, _robot.CrashedShip.GetPosition()) <= 0.1f + Constants.LOADING_STOP_DISTANCE;

            if (isArrived && !_isTransporting)
            {
                _isTransporting = true;
                _animator.SetBool(IS_Moving, false);
                _loadingUpCoroutine = _coroutineRunner.PerformCoroutine(StartLoadingUp());
            }
        }

        private IEnumerator StartLoadingUp()
        {
            if (_robot.CrashedShip == null) yield break;

            _animator.SetBool(IS_TRANSPORTING, true);

            PlayLoadingSound();

            while (_robot.CrashedShip.MinedMaterials.Count > 0)
            {
                yield return new WaitForSecondsRealtime(Constants.LOADING_ONE_METERIAL_TIME);

                HandleLoading();
            }
        }     

        private void HandleLoading()
        {
            _robot.TransportedMaterials.Add(_robot.CrashedShip.MinedMaterials[0]);
            _robot.CrashedShip.MinedMaterials.RemoveAt(0);
            _robot.CrashedShip.RemoveMinedMaterialView();
        }

        private void CheckIfLoadingIsDone()
        {
            if (_robot.CrashedShip.MinedMaterials.Count <= 0)
            {
                if (_robot.CrashedShip.Materials.Count <= 0 && _robot.CrashedShip.MinedMaterials.Count <= 0)
                {
                    _coroutineRunner.PerformCoroutine(_robot.CrashedShip.DestroySelf());
                    _robot.ClearCrashedShip();
                }
                else
                    _robot.ClearCrashedShip();

                if (_robot.TransportedMaterials.Count != 0)
                    _robot.HasLoadedMaterials = true;
            }
        }

        private void StopLoading(OnGamePausedSignal signal)
        {
            if (_loadingUpCoroutine != null)
                _coroutineRunner.HandleStopCoroutine(_loadingUpCoroutine);

            ClearLoadingSound();
        }

        private void ContinueLoading(OnGameResumedSignal signal)
        {
            if (_isTransporting)
            {
                _loadingUpCoroutine = _coroutineRunner.PerformCoroutine(StartLoadingUp());
                PlayLoadingSound();
            }
        }

        private void PlayLoadingSound() =>
            _audioService.PlayOnObject(Configs.Sound.SoundType.TransporterTransporting, _robot, true);

        private void ClearLoadingSound() => 
            _audioService.StopOnObject(_robot, Configs.Sound.SoundType.TransporterTransporting);
    }
}
