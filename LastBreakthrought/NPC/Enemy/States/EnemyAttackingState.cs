using LastBreakthrought.NPC.Base;
using LastBreakthrought.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LastBreakthrought.NPC.Enemy
{
    public class EnemyAttackingState : INPCState
    {
        private const string ATTACK = "Attack";

        private readonly EnemyBase _enemy;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;

        private Coroutine _isAttackEndedCoroutine;

        private float _animationTime;

        public EnemyAttackingState(EnemyBase enemy, ICoroutineRunner coroutineRunner, NavMeshAgent agent, Animator animator, float animationTime)
        {
            _enemy = enemy;
            _coroutineRunner = coroutineRunner;
            _agent = agent;
            _animator = animator;

            _animationTime = animationTime;
        }

        public void Enter()
        {
            _animator.SetTrigger(ATTACK);
            _agent.isStopped = true;
            _isAttackEndedCoroutine = _coroutineRunner.PerformCoroutine(ChackIfAttackIsEnded());
        }
        public void Exit() => 
            _coroutineRunner.HandleStopCoroutine(_isAttackEndedCoroutine);

        public void Update(){}

        private IEnumerator ChackIfAttackIsEnded()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(_animationTime);

                if (_enemy.TryToAttackTarget())
                    _animator.SetTrigger(ATTACK);
            }
        }
    }
}
