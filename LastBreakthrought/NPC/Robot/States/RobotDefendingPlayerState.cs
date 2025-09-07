using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Logic.FSX;
using LastBreakthrought.NPC.Base;
using LastBreakthrought.NPC.Enemy;
using LastBreakthrought.Player;
using LastBreakthrought.Util;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LastBreakthrought.NPC.Robot.States
{
    public class RobotDefendingPlayerState : INPCState
    {
        private const string IS_MOVING = "isMoving";
        private const string IS_Defending = "isDefending";

        private readonly RobotDefender _robot;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;
        private readonly RobotBattary _robotBattary;
        private readonly EffectCreator _effectCreator;
        private readonly IEventBus _eventBus;
        private readonly IAudioService _audioService;
        private readonly float _followingSpeed;

        private Coroutine _defendingCoroutine;

        public RobotDefendingPlayerState(RobotDefender robot, ICoroutineRunner coroutineRunner, NavMeshAgent agent,  Animator animator,
            RobotBattary robotBattary, EffectCreator effectCreator, IEventBus eventBus, IAudioService audioService, float followingSpeed)
        {
            _agent = agent;
            _robot = robot;
            _coroutineRunner = coroutineRunner;
            _animator = animator;
            _robotBattary = robotBattary;
            _effectCreator = effectCreator;
            _eventBus = eventBus;
            _audioService = audioService;
            _followingSpeed = followingSpeed;
        }

        public void Enter()
        {
            _agent.isStopped = false;
            _agent.speed = _followingSpeed;
            _agent.stoppingDistance = Constants.ROBOT_STOP_DEFENDING_DISTANCE;
            _animator.SetBool(IS_MOVING, true);
            _defendingCoroutine = _coroutineRunner.PerformCoroutine(PerformDefending());

            _eventBus.SubscribeEvent<OnGamePausedSignal>(StopDefending);
            _eventBus.SubscribeEvent<OnGameResumedSignal>(ContinueDefending);
        }

        public void Exit()
        {
            _animator.SetBool(IS_MOVING, false);
            _animator.SetBool(IS_Defending, false);
            _coroutineRunner.HandleStopCoroutine(_defendingCoroutine);

            _eventBus.UnSubscribeEvent<OnGamePausedSignal>(StopDefending);
            _eventBus.UnSubscribeEvent<OnGameResumedSignal>(ContinueDefending);
        }

        public void Update()
        {
            _robotBattary.DecreaseCapacity();
            _robotBattary.CheckIfCapacityIsRechedLimit();
        }

        private IEnumerator PerformDefending()
        {
            while (true)
            {
                if (_robot.Target == null) yield return null;

                _agent.SetDestination(_robot.Target.GetPosition());

                if (_agent.remainingDistance < Constants.ROBOT_STOP_DEFENDING_DISTANCE + 0.01f)
                {
                    PerformAttack();
                    yield return new WaitForSecondsRealtime(Constants.ROBOT_ATTACK_COOLDOWN);

                    if (CheckIfTargetDied())
                        yield break;
                }
                else
                    ResetAnimation();

                yield return null;
            }
        }

        private bool CheckIfTargetDied()
        {
            if (_robot.Target.IsEnemyDied())
            {
                _robot.ClearTarget();
                return true;
            }
            return false;
        }

        private void PerformAttack()
        {
            var enemy = _robot.Target as EnemyBase;
            enemy?.ApplyDamage(35f);

            _effectCreator.CreateLightningEffect(_robot.GetRootForEffect());
            _audioService.PlayOnObject(Configs.Sound.SoundType.DefenderAttack, _robot);

            _animator.SetBool(IS_Defending, true);
            _animator.SetBool(IS_MOVING, false);
        }
        private void ResetAnimation()
        {
            _animator.SetBool(IS_MOVING, true);
            _animator.SetBool(IS_Defending, false);
        }

        private void StopDefending(OnGamePausedSignal signal) =>
            _coroutineRunner.HandleStopCoroutine(_defendingCoroutine);

        private void ContinueDefending(OnGameResumedSignal signal) =>
            _defendingCoroutine = _coroutineRunner.PerformCoroutine(PerformDefending());
    }
}
