using LastBreakthrought.Factory;
using LastBreakthrought.Infrustructure.AssetManagment;
using LastBreakthrought.Logic.ChargingPlace;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.NPC.Robot.Factory
{
    public class RobotDefenderFactory : BaseFactory<IRobot>
    {
        public string RobotID { get; private set; } = AssetPath.RobotDefenderPath;
        public RobotDefenderFactory(IAssetProvider assetProvider) : base(assetProvider) { }

        public override IRobot Create(Vector3 at, Transform parent) =>
            AssetProvider.Instantiate<IRobot>(AssetPath.RobotDefenderPath, at, parent);

        public IRobot CreateRobot(Vector3 at, Transform parent, BoxCollider zone, List<RobotChargingPlace> chargingPlaces)
        {
            var robot = Create(at, parent);
            robot.OnCreated(zone, chargingPlaces, RobotID);
            return robot;
        }
    }
}
