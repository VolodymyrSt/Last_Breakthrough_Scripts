using LastBreakthrought.CrashedShip;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Logic.ChargingPlace;
using LastBreakthrought.Logic.InteractionZone;
using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.Logic.ShipDetail;
using LastBreakthrought.NPC.Robot.States;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.NPC.Robot
{
    public class RobotMiner : RobotBase
    {
        public ICrashedShip CrashedShip { get; private set; } = null;

        private RobotMiningState _robotMiningState;

        public override void OnCreated(BoxCollider wanderingZone, List<RobotChargingPlace> chargingPlaces, string id)
        {
            base.OnCreated(wanderingZone, chargingPlaces, id);

            _robotMiningState = new RobotMiningState(this, CoroutineRunner, Agent, Animator, Battary, EventBus, AudioService, RobotData.GeneralSpeed);

            StateMachine.AddTransition(_robotMiningState, RobotWanderingState, () => CrashedShip == null && IsWanderingState);
            StateMachine.AddTransition(_robotMiningState, RobotFollowingPlayerState, () => CrashedShip == null && IsFollowingState);
            StateMachine.AddTransition(_robotMiningState, RobotWanderingState, () => CrashedShip == null && !IsFollowingState && !IsWanderingState);

            StateMachine.AddTransition(RobotRechargingState, _robotMiningState, () => !Battary.NeedToBeRecharged && CrashedShip != null);
            StateMachine.AddTransition(RobotRechargingState, RobotWanderingState, () => !Battary.NeedToBeRecharged && CrashedShip == null && IsWanderingState);
            StateMachine.AddTransition(RobotRechargingState, RobotFollowingPlayerState, () => !Battary.NeedToBeRecharged && CrashedShip == null && IsFollowingState);
            StateMachine.AddTransition(RobotRechargingState, RobotWanderingState, () => !Battary.NeedToBeRecharged && CrashedShip == null && !IsFollowingState && !IsWanderingState);

            StateMachine.AddTransition(_robotMiningState, RobotRechargingState, () => Battary.NeedToBeRecharged);
            StateMachine.AddTransition(RobotFollowingPlayerState, RobotRechargingState, () => Battary.NeedToBeRecharged);
            StateMachine.AddTransition(RobotWanderingState, RobotRechargingState, () => Battary.NeedToBeRecharged);

            StateMachine.AddTransition(RobotWanderingState, RobotFollowingPlayerState, () => IsFollowingState);
            StateMachine.AddTransition(RobotFollowingPlayerState, RobotWanderingState, () => IsWanderingState);

            StateMachine.AddTransition(_robotMiningState, RobotDestroyedState, () => IsRobotDestroyed);
            StateMachine.AddTransition(RobotDestroyedState, _robotMiningState, () => !IsRobotDestroyed && CrashedShip != null);

            StateMachine.EnterInState(RobotWanderingState);
        }
        public override void DoWork()
        {
            if (IsRobotDestroyed)
            {
                MassageHandler.ShowMassage("Robot is destroyed");
                return;
            }
            if (CrashedShip != null)
            {
                if (CrashedShip.Materials.Count > 0)
                    MassageHandler.ShowMassage("Robot is already doing its duty");
                return;
            }   

            CrashedShip = PlayerHandler.GetSeekedCrashedShip();

            if (!IsRobotReady()) return;

            StateMachine.EnterInState(_robotMiningState);
        }

        public override List<MechanismEntity> GetRequiredMechanismsToRepair() =>
            RequireMechanismsProvider.Holder.RepairRobotMiner.GetRequiredShipDetails();

        public void ClearCrashedShip() => 
            CrashedShip = null;

        private bool IsRobotReady()
        {
            if (CrashedShip != null && CrashedShip.Materials.Count <= 0)
            {
                MassageHandler.ShowMassage("Crashed ship doesn`t have materials to mine");
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
            return true;
        }
    }
}
