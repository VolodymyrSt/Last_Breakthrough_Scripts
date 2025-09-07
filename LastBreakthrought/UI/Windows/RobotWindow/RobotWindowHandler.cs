using LastBreakthrought.NPC.Robot;
using LastBreakthrought.UI.Windows;
using UnityEngine;

namespace LastBreakthrought.UI.RobotWindow
{
    public class RobotWindowHandler : WindowHandler<RobotWindowView>
    {
        [field:SerializeField] public RobotBase Robot { get; private set; }

        public override void ActivateWindow() => View.ShowView();

        public override void DeactivateWindow() => View.HideView();
    }

}

