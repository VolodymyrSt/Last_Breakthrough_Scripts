using LastBreakthrought.CrashedShip;
using LastBreakthrought.Logic.ChargingPlace;
using LastBreakthrought.Logic.MaterialRecycler;
using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.Logic.ShipDetail;
using LastBreakthrought.Logic.ShipMaterial.ScriptableObjects;
using LastBreakthrought.NPC.Robot.States;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.NPC.Robot
{
    public class RobotTransporter : RobotBase
    {
        public ICrashedShip CrashedShip { get; private set; } = null;
        public List<ShipMaterialEntity> TransportedMaterials { get; private set; } = new();
        public bool HasLoadedMaterials { set; get; } = false;

        private RobotLoadingUpMaterialsState _robotLoadingUpMaterialsState;
        private RobotTransportingMaterialsState _robotTransportingMaterialsState;

        private RecycleMachine _recycleMachine;

        [Inject]
        private void Construct(RecycleMachine recycleMachine) =>
            _recycleMachine = recycleMachine;

        public override void OnCreated(BoxCollider wanderingZone, List<RobotChargingPlace> chargingPlaces, string id)
        {
            base.OnCreated(wanderingZone, chargingPlaces, id);

            _robotLoadingUpMaterialsState = new RobotLoadingUpMaterialsState(this, CoroutineRunner, Agent, Animator, Battary, EventBus, AudioService, RobotData.GeneralSpeed);
            _robotTransportingMaterialsState = new RobotTransportingMaterialsState(this, CoroutineRunner, Agent, Animator, Battary, _recycleMachine, EventBus, AudioService, RobotData.GeneralSpeed);

            StateMachine.AddTransition(_robotLoadingUpMaterialsState, RobotWanderingState, () => CrashedShip == null && IsWanderingState && !HasLoadedMaterials);
            StateMachine.AddTransition(_robotLoadingUpMaterialsState, RobotFollowingPlayerState, () => CrashedShip == null && IsFollowingState && !HasLoadedMaterials);
            StateMachine.AddTransition(_robotLoadingUpMaterialsState, RobotWanderingState, () => CrashedShip == null && !IsFollowingState && !IsWanderingState && !HasLoadedMaterials);

            StateMachine.AddTransition(_robotLoadingUpMaterialsState, _robotTransportingMaterialsState, () => CrashedShip == null && TransportedMaterials.Count > 0 && HasLoadedMaterials);

            StateMachine.AddTransition(_robotTransportingMaterialsState, RobotWanderingState, () => TransportedMaterials.Count <= 0 && IsWanderingState && !HasLoadedMaterials);
            StateMachine.AddTransition(_robotTransportingMaterialsState, RobotFollowingPlayerState, () => TransportedMaterials.Count <= 0 && IsFollowingState && !HasLoadedMaterials);
            StateMachine.AddTransition(_robotTransportingMaterialsState, _robotLoadingUpMaterialsState, () => TransportedMaterials.Count <= 0 && !HasLoadedMaterials);//
            StateMachine.AddTransition(_robotTransportingMaterialsState, RobotWanderingState, () => TransportedMaterials.Count <= 0 && !IsFollowingState && !IsWanderingState && !HasLoadedMaterials);

            StateMachine.AddTransition(_robotLoadingUpMaterialsState, RobotRechargingState, () => Battary.NeedToBeRecharged);
            StateMachine.AddTransition(_robotTransportingMaterialsState, RobotRechargingState, () => Battary.NeedToBeRecharged);

            StateMachine.AddTransition(RobotRechargingState, _robotLoadingUpMaterialsState, () => !Battary.NeedToBeRecharged && CrashedShip != null && !HasLoadedMaterials);
            StateMachine.AddTransition(RobotRechargingState, _robotTransportingMaterialsState, () => !Battary.NeedToBeRecharged && HasLoadedMaterials);
            StateMachine.AddTransition(RobotRechargingState, RobotWanderingState, () => !Battary.NeedToBeRecharged && CrashedShip == null && IsWanderingState);
            StateMachine.AddTransition(RobotRechargingState, RobotFollowingPlayerState, () => !Battary.NeedToBeRecharged && CrashedShip == null && IsFollowingState);
            StateMachine.AddTransition(RobotRechargingState, RobotWanderingState, () => !Battary.NeedToBeRecharged && CrashedShip == null && !IsFollowingState && !IsWanderingState);

            StateMachine.AddTransition(RobotFollowingPlayerState, RobotRechargingState, () => Battary.NeedToBeRecharged);
            StateMachine.AddTransition(RobotWanderingState, RobotRechargingState, () => Battary.NeedToBeRecharged);

            StateMachine.AddTransition(RobotWanderingState, RobotFollowingPlayerState, () => IsFollowingState);
            StateMachine.AddTransition(RobotFollowingPlayerState, RobotWanderingState, () => IsWanderingState);

            StateMachine.AddTransition(_robotLoadingUpMaterialsState, RobotDestroyedState, () => IsRobotDestroyed);
            StateMachine.AddTransition(_robotTransportingMaterialsState, RobotDestroyedState, () => IsRobotDestroyed);

            StateMachine.AddTransition(RobotDestroyedState, _robotLoadingUpMaterialsState, () => CrashedShip != null && !HasLoadedMaterials);
            StateMachine.AddTransition(RobotDestroyedState, _robotTransportingMaterialsState, () => !IsRobotDestroyed && HasLoadedMaterials);

            StateMachine.EnterInState(RobotWanderingState);
        }

        public override void DoWork()
        {
            if (CrashedShip != null)
            {
                if (IsRobotDestroyed)
                    MassageHandler.ShowMassage("Robot is destroyed");
                else if (CrashedShip.MinedMaterials.Count > 0)
                    MassageHandler.ShowMassage("Robot is already doing its duty");
            }

            CrashedShip = PlayerHandler.GetSeekedCrashedShip();

            if (!IsRobotReady()) return;

            StateMachine.EnterInState(_robotLoadingUpMaterialsState);
        }

        public override List<MechanismEntity> GetRequiredMechanismsToRepair() =>
            RequireMechanismsProvider.Holder.RepairRobotTransporter.GetRequiredShipDetails();

        public void ClearCrashedShip() => CrashedShip = null;

        private bool IsRobotReady()
        {
            if (CrashedShip != null && CrashedShip.MinedMaterials.Count <= 0)
            {
                MassageHandler.ShowMassage("Crashed ship doesn`t have mined materials");
                CrashedShip = null;
                return false;
            }
            if (Battary.NeedToBeRecharged)
            {
                MassageHandler.ShowMassage("Robot battary is too low");
                return false;
            }
            if (CrashedShip == null)
            {
                MassageHandler.ShowMassage("Robot don`t have a target crashed ship");
                return false;
            }
            if (HasLoadedMaterials)
            {
                MassageHandler.ShowMassage("Robot has transported material");
                return false;
            }
            return true;
        }
    }
}
