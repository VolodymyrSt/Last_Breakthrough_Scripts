using UnityEngine;

namespace LastBreakthrought.NPC.Enemy.Golem
{
    public class GolemController : EnemyBase
    {
        public override void AddStates()
        {
            var wandering = new EnemyWanderingState(this, CoroutineRunner, Agent, WanderingZone, Animator, EnemyData.WandaringSpeed);
            var chassing = new EnemyChassingState(this, CoroutineRunner, Agent, Animator, AudioService, Configs.Sound.SoundType.GolemChassing, EnemyData.ChassingSpeed);
            var attacking = new EnemyAttackingState(this, CoroutineRunner, Agent, Animator, EnemyData.AttakAnimationTime);
            var dying = new EnemyDyingState(this, CoroutineRunner, Agent, Animator, EnemyData.DyingAnimationTime);

            StateMachine.AddTransition(wandering, chassing, () => IsTargetInVisionRange);
            StateMachine.AddTransition(chassing, wandering, () => !IsTargetInVisionRange);
            StateMachine.AddTransition(chassing, attacking, () => IsTargetInAttakingRange);
            StateMachine.AddTransition(attacking, chassing, () => !IsTargetInAttakingRange);
            StateMachine.AddAnyTransition(dying, () => IsEnemyDied());

            StateMachine.EnterInState(wandering);
        }
    }
}
