using LastBreakthrought.Logic.Mechanisms;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LastBreakthrought.UI.Windows.RobotFactoryWindow
{
    public class RobotFactoryWindowView : WindowView<RobotFactoryWindowHandler>
    {
        [Header("UI")]
        [SerializeField] private Button _createRobotMinerButton;
        [SerializeField] private Button _createRobotTransporterButton;
        [SerializeField] private Button _createRobotDefenderButton;

        [Space(20f)]
        [SerializeField] private TextMeshProUGUI _minersCountText;
        [SerializeField] private TextMeshProUGUI _transportersCountText;
        [SerializeField] private TextMeshProUGUI _defendersCountText;

        [Header("Containers")]
        [SerializeField] private RectTransform _neededMechanismsForMinerCreateContainer;
        [SerializeField] private RectTransform _neededMechanismsForTransporterCreateContainer;
        [SerializeField] private RectTransform _neededMechanismsForDefenderCreateContainer;

        private MechanismsGeneratorUI _mechanismsGeneratorUI;

        [Inject]
        private void Construct(MechanismsGeneratorUI mechanismsGeneratorUI) =>
            _mechanismsGeneratorUI = mechanismsGeneratorUI;

        //I use OnEnable because Initialize is called on start which means i OnMinersCountChanged is called +- on awake and it will be late
        private void OnEnable()
        {
            Handler.RobotFactoryMachine.OnMinersCountChanged += ChangedMinersCount;
            Handler.RobotFactoryMachine.OnTransportersCountChanged += ChangedTransportersCount;
            Handler.RobotFactoryMachine.OnDefendersCountChanged += ChangedDefendersCount;
        }

        public override void Initialize()
        {
            _createRobotMinerButton.onClick.AddListener(() =>
                Handler.CreateMiner());

            _createRobotTransporterButton.onClick.AddListener(() =>
                Handler.CreateTransporter());

            _createRobotDefenderButton.onClick.AddListener(() =>
                Handler.CreateDefender());

            GenerateRequiredMechanismsForCreatingRobots();
        }


        private void GenerateRequiredMechanismsForCreatingRobots()
        {
            var requiredMechanismsForMiner = Handler.RobotFactoryMachine.GetMechanismsToCreateMiner();
            _mechanismsGeneratorUI.GenerateRequireMechanisms(requiredMechanismsForMiner, _neededMechanismsForMinerCreateContainer);

            var requiredMechanismsForTransporter = Handler.RobotFactoryMachine.GetMechanismsToCreateTransporter();
            _mechanismsGeneratorUI.GenerateRequireMechanisms(requiredMechanismsForTransporter, _neededMechanismsForTransporterCreateContainer);

            var requiredMechanismsForDefender = Handler.RobotFactoryMachine.GetMechanismsToCreateDefender();
            _mechanismsGeneratorUI.GenerateRequireMechanisms(requiredMechanismsForDefender, _neededMechanismsForDefenderCreateContainer);
        }

        private void ChangedMinersCount(int obj) => _minersCountText.text = $"{obj}/3";
        private void ChangedDefendersCount(int obj) => _defendersCountText.text = $"{obj}/3";
        private void ChangedTransportersCount(int obj) => _transportersCountText.text = $"{obj}/3";

        public override void Dispose()
        {
            _createRobotMinerButton.onClick.RemoveListener(() => Handler.CreateMiner());
            _createRobotTransporterButton.onClick.RemoveListener(() => Handler.CreateTransporter());
            _createRobotTransporterButton.onClick.RemoveListener(() => Handler.CreateDefender());

            Handler.RobotFactoryMachine.OnMinersCountChanged -= ChangedMinersCount;
            Handler.RobotFactoryMachine.OnTransportersCountChanged -= ChangedTransportersCount;
            Handler.RobotFactoryMachine.OnDefendersCountChanged -= ChangedDefendersCount;
        }
    }
}

