using LastBreakthrought.Logic.RobotFactory;
using UnityEngine;

namespace LastBreakthrought.UI.Windows.RobotFactoryWindow 
{
    public class RobotFactoryWindowHandler : WindowHandler<RobotFactoryWindowView>
    {
        [field:SerializeField] public RobotFactoryMachine RobotFactoryMachine { get; private set; }

        public override void ActivateWindow() => View.ShowView();
        public override void DeactivateWindow() => View.HideView();

        public void CreateMiner() =>
            RobotFactoryMachine.CreateRobotMiner();
        public void CreateTransporter() =>
            RobotFactoryMachine.CreateRobotTransporter();
        public void CreateDefender() =>
            RobotFactoryMachine.CreateRobotDefender();
    }
}

