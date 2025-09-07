using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.NPC.Base;
using LastBreakthrought.NPC.Enemy;
using LastBreakthrought.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LastBreakthrought.NPC.Robot.States
{
    public class RobotWanderingState : INPCState
    {
        private const string IS_MOVING = "isMoving";

        private readonly RobotBase _robotBase;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly NavMeshAgent _agent;
        private readonly BoxCollider _wanderingZone;
        private readonly RobotBattary _robotBattary;
        private readonly Animator _animator;
        private readonly IAudioService _audioService;
        private readonly IEventBus _eventBus;
        private readonly float _wanderingSpeed;

        private Coroutine _wanderingCoroutine;

        public RobotWanderingState(RobotBase robotBase, ICoroutineRunner coroutineRunner, NavMeshAgent agent,
            Animator animator, BoxCollider wanderingZone, RobotBattary robotBattary, IAudioService audioService
            ,IEventBus eventBus, float wanderingSpeed)
        {
            _robotBase = robotBase;
            _coroutineRunner = coroutineRunner;
            _agent = agent;
            _animator = animator;
            _wanderingZone = wanderingZone;
            _robotBattary = robotBattary;
            _wanderingSpeed = wanderingSpeed;
            _audioService = audioService;
            _eventBus = eventBus;
        }

        public void Enter()
        {
            _agent.isStopped = false;
            _agent.speed = _wanderingSpeed;
            _wanderingCoroutine = _coroutineRunner.PerformCoroutine(StartWandering());

            _eventBus.SubscribeEvent<OnGamePausedSignal>(StopMoving);
            _eventBus.SubscribeEvent<OnGameResumedSignal>(ContinueMoving);
        }

        public void Update() 
        {
            _robotBattary.DecreaseCapacity();
            _robotBattary.CheckIfCapacityIsRechedLimit();
        }

        public void Exit()
        {
            if (_wanderingCoroutine != null)
                _coroutineRunner.HandleStopCoroutine(_wanderingCoroutine);

            SetMovingAnimation(false);
            ClearMovingSound();

            _eventBus.UnSubscribeEvent<OnGamePausedSignal>(StopMoving);
            _eventBus.UnSubscribeEvent<OnGameResumedSignal>(ContinueMoving);
        }

        private IEnumerator StartWandering()
        {
            while (true)
            {
                Vector3 destination = GetRandomPositionForNavMesh();
                if (destination != Vector3.negativeInfinity)
                {
                    _agent.SetDestination(destination);
                    SetMovingAnimation(true);
                    PlayMovingSound();

                    yield return WaitForDestinationReached();
                    SetMovingAnimation(false);
                    ClearMovingSound();
                }

                yield return WaitBeforeNext();
            }
        }

        private IEnumerator WaitForDestinationReached()
        {
            float elapsedTime = 0f;
            while (elapsedTime < Constants.ROBOT_MOVEMENT_TIME_OUT)
            {
                if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
                {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                        yield break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _agent.ResetPath();
        }

        private Vector3 GetRandomPositionForNavMesh()
        {
            Vector3 randomPoint = GetRandomPoint();

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, Constants.ROBOT_NAVMESH_SAMPLE_RANGE, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return Vector3.negativeInfinity;
        }

        private Vector3 GetRandomPoint()
        {
            Vector3 localPosition = new Vector3(
                Random.Range(-_wanderingZone.size.x / 2, _wanderingZone.size.x / 2),
                0,
                Random.Range(-_wanderingZone.size.z / 2, _wanderingZone.size.z / 2)
            );

            localPosition += _wanderingZone.center;
            return _wanderingZone.transform.TransformPoint(localPosition);
        }

        private IEnumerator WaitBeforeNext()
        {
            float waitTime = Random.Range(Constants.ROBOT_MIN_WAIT_TIME, Constants.ROBOT_MAX_WAIT_TIME);
            yield return new WaitForSeconds(waitTime);
        }

        private void StopMoving(OnGamePausedSignal signal) => ClearMovingSound();
        private void ContinueMoving(OnGameResumedSignal signal) => PlayMovingSound();

        private void SetMovingAnimation(bool isMoving) =>
            _animator.SetBool(IS_MOVING, isMoving);

        private void PlayMovingSound() =>
            _audioService.PlayOnObject(Configs.Sound.SoundType.RobotMoving, _robotBase, true);

        private void ClearMovingSound() => 
            _audioService.StopOnObject(_robotBase, Configs.Sound.SoundType.RobotMoving);
    }
}
