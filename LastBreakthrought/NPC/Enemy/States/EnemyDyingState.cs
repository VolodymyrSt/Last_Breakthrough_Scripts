using LastBreakthrought.NPC.Base;
using LastBreakthrought.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LastBreakthrought.NPC.Enemy
{
    public class EnemyDyingState : INPCState
    {
        private const string DYING = "Dying";

        private readonly EnemyBase _enemy;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly NavMeshAgent _agent;
        private readonly Animator _animator;

        private Coroutine _dyingCoroutine;

        private readonly float _animationTime;

        public EnemyDyingState(EnemyBase enemyBase, ICoroutineRunner coroutineRunner, NavMeshAgent navMeshAgent, Animator animator, float animationTime)
        {
            _enemy = enemyBase;
            _coroutineRunner = coroutineRunner;
            _agent = navMeshAgent;
            _animator = animator;

            _animationTime = animationTime;
        }

        public void Enter()
        {
            _agent.isStopped = true;
            _dyingCoroutine = _coroutineRunner.PerformCoroutine(DieAndDestroySelf());
        }

        public void Exit() => 
            _coroutineRunner.HandleStopCoroutine(_dyingCoroutine);

        public void Update(){}

        private IEnumerator DieAndDestroySelf()
        {
            _animator.SetTrigger(DYING);

            yield return new WaitForSecondsRealtime(_animationTime);

            Object.Destroy(_enemy.gameObject);
        }
    }
}
