using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LastBreakthrought.UI.RobotWindow
{
    public class RobotWindowView : WindowView<RobotWindowHandler>
    {
        [Header("UI")]
        [SerializeField] private RectTransform _mechanismsContainerForRepair;
        [SerializeField] private Button _repairButton;

        private MechanismsGeneratorUI _mechanismsGeneratorUI;

        [Inject]
        private void Construct(MechanismsGeneratorUI mechanismsGeneratorUI) =>
            _mechanismsGeneratorUI = mechanismsGeneratorUI;

        public override void Initialize()
        {
            var mechanimesToRepairRobot = Handler.Robot.GetRequiredMechanismsToRepair();
            _mechanismsGeneratorUI.GenerateRequireMechanisms(mechanimesToRepairRobot, _mechanismsContainerForRepair);

            _repairButton.onClick.AddListener(() => Handler.Robot.TryToRepair());
        }   

        public override void Dispose() =>
            _repairButton.onClick.RemoveListener(() => Handler.Robot.TryToRepair());
    }
}

