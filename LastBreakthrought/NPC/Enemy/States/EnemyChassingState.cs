using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.NPC.Base;
using LastBreakthrought.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LastBreakthrought.NPC.Enemy
{
    public class EnemyChassingState : INPCState
    {
        private const string IS_CHASSING = "isWalking";

        private readonly EnemyBase _enemy;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;
        private readonly IAudioService _audioService;

        private readonly Configs.Sound.SoundType _chassingSound;
        private Coroutine _isTargetEscapeCoroutine;

        private float _chassingSpeed;

        public EnemyChassingState(EnemyBase enemy, ICoroutineRunner  coroutineRunner,NavMeshAgent agent, Animator animator
            , IAudioService audioService, Configs.Sound.SoundType chassingSound,  float chassingSpeed)
        {
            _enemy = enemy;
            _coroutineRunner = coroutineRunner;
            _agent = agent;
            _animator = animator;
            _audioService = audioService;
            _chassingSound = chassingSound;
            _chassingSpeed = chassingSpeed;
        }

        public void Enter()
        {
            _agent.isStopped = false;
            _agent.speed = _chassingSpeed;

            _isTargetEscapeCoroutine = _coroutineRunner.PerformCoroutine(CheckIfTargetEscaped());
            SetChassingAnimation(true);
        }
        public void Update()
        {
            _agent.SetDestination(_enemy.Target.GetPosition());

            if (_enemy.TryToAttackTarget())
                ClearChassingSound();
            else
                PlayChassingSound();
        }

        public void Exit()
        {
            if (_isTargetEscapeCoroutine != null)
                _coroutineRunner.HandleStopCoroutine(_isTargetEscapeCoroutine);

            SetChassingAnimation(false);
            ClearChassingSound();
        }

        private IEnumerator CheckIfTargetEscaped()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(Constants.CHASSING_TIME_AFTER_WHICH_CHECK_TARGET);

                _enemy.TryToFindTarget();
            }
        }

        private void SetChassingAnimation(bool isChassing) =>
            _animator.SetBool(IS_CHASSING, isChassing);

        private void PlayChassingSound()
        {
            if (_chassingSound != Configs.Sound.SoundType.None)
                _audioService.PlayOnObject(_chassingSound, _enemy, true);
        }

        private void ClearChassingSound()
        {
            if (_chassingSound != Configs.Sound.SoundType.None)
                _audioService.StopOnObject(_enemy, _chassingSound);
        }
    }
}
