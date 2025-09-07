using LastBreakthrought.Configs.Robot;
using LastBreakthrought.Logic.ChargingPlace;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.NPC.Robot
{
    public interface IRobot
    {
        void OnCreated(BoxCollider wanderingZone, List<RobotChargingPlace> chargingPlaces, string id);
        void SetFollowingPlayerState();
        void SetWanderingState();
        RobotConfigSO GetRobotData();
        RobotBattary GetRobotBattary();
        void DoWork();
        RobotHealth GetRobotHealth();
        int GetRobotDistanceToPlayer();
    }
}
