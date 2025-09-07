using LastBreakthrought.Logic.ChargingPlace;
using LastBreakthrought.Logic.FSX;
using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.NPC.Enemy;
using LastBreakthrought.NPC.Robot.States;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.NPC.Robot
{
    public class RobotDefender : RobotBase 
    {
        [Header("Base:")]
        [SerializeField] private LayerMask _targetLayerMask;
        [SerializeField] private float _detectionRadious;

        [Header("Effect:")]
        [SerializeField] private Transform _effectContainer;

        public IEnemy Target { get; private set; } = null;

        private RobotDefendingPlayerState _robotDefendingPlayerState;

        private readonly Collider[] _targetCollider = new Collider[1];
            
        public override void OnCreated(BoxCollider wanderingZone, List<RobotChargingPlace> chargingPlaces, string id)
        {
            base.OnCreated(wanderingZone, chargingPlaces, id);

            _robotDefendingPlayerState = new RobotDefendingPlayerState(this, CoroutineRunner, Agent, Animator, Battary, EffectCreator, EventBus, AudioService, RobotData.GeneralSpeed);

            StateMachine.AddTransition(_robotDefendingPlayerState, RobotWanderingState, () => Target == null && IsWanderingState);
            StateMachine.AddTransition(_robotDefendingPlayerState, RobotFollowingPlayerState, () => Target == null && IsFollowingState);
            StateMachine.AddTransition(_robotDefendingPlayerState, RobotWanderingState, () => Target == null && !IsFollowingState && !IsWanderingState);

            StateMachine.AddTransition(RobotRechargingState, _robotDefendingPlayerState, () => !Battary.NeedToBeRecharged && Target != null);
            StateMachine.AddTransition(RobotRechargingState, RobotWanderingState, () => !Battary.NeedToBeRecharged && IsWanderingState && Target == null);
            StateMachine.AddTransition(RobotRechargingState, RobotFollowingPlayerState, () => !Battary.NeedToBeRecharged && IsFollowingState && Target == null);
            StateMachine.AddTransition(RobotRechargingState, RobotWanderingState, () => !Battary.NeedToBeRecharged && !IsFollowingState && !IsWanderingState && Target == null);

            StateMachine.AddTransition(RobotFollowingPlayerState, RobotRechargingState, () => Battary.NeedToBeRecharged);
            StateMachine.AddTransition(RobotWanderingState, RobotRechargingState, () => Battary.NeedToBeRecharged);
            StateMachine.AddTransition(_robotDefendingPlayerState, RobotRechargingState, () => Battary.NeedToBeRecharged);

            StateMachine.AddTransition(RobotWanderingState, RobotFollowingPlayerState, () => IsFollowingState);
            StateMachine.AddTransition(RobotFollowingPlayerState, RobotWanderingState, () => IsWanderingState);

            StateMachine.AddTransition(_robotDefendingPlayerState, RobotDestroyedState, () => IsRobotDestroyed);
            StateMachine.AddTransition(RobotDestroyedState, _robotDefendingPlayerState, () => !IsRobotDestroyed && Target != null);

            StateMachine.EnterInState(RobotWanderingState);
        }
        public override void DoWork()
        {
            var target = Physics.OverlapSphereNonAlloc(transform.position + Vector3.up, _detectionRadious, _targetCollider, _targetLayerMask);

            if (target > 0 && _targetCollider[0].TryGetComponent(out IEnemy enemy))
            {
                Target = enemy;
                StateMachine.EnterInState(_robotDefendingPlayerState);
                _targetCollider[0] = null;
            }
            else
                MassageHandler.ShowMassage("Not enemy detected");
        }

        public void ClearTarget() => 
            Target = null;

        public Transform GetRootForEffect() =>
            _effectContainer;

        public override List<MechanismEntity> GetRequiredMechanismsToRepair() =>
            RequireMechanismsProvider.Holder.RepairRobotDefender.GetRequiredShipDetails();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + Vector3.up, _detectionRadious);
        }
    }
}
