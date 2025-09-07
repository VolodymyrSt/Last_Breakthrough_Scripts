using LastBreakthrought.Configs.Robot;
using LastBreakthrought.Factory;
using LastBreakthrought.Infrustructure.AssetManagment;
using LastBreakthrought.NPC.Robot;
using System;
using UnityEngine;

namespace LastBreakthrought.UI.NPC.Robot.RobotsMenuPanel.RobotControls.Factory
{
    public class RobotDefenderControlUIFactory : BaseFactory<RobotControlHandlerUI>
    {
        public RobotDefenderControlUIFactory(IAssetProvider assetProvider) : base(assetProvider) { }

        public override RobotControlHandlerUI Create(Vector3 at, Transform parent) =>
            AssetProvider.Instantiate<RobotControlHandlerUI>(AssetPath.RobotDefenderControlPath, at, parent);

        public RobotControlHandlerUI Create(RectTransform parent, IRobot robot, RobotConfigSO robotData, RobotBattary battary,
           RobotHealth robotHealth, Action followAction, Action goHomeAction, Action defend)
        {
            var robotControl = Create(Vector3.zero, parent);
            robotControl.Init(robot, robotData, battary, robotHealth, followAction, goHomeAction, defend);
            return robotControl;
        }
    }
}
