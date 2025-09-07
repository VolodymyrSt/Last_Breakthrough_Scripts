using LastBreakthrought.Logic.Mechanisms;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LastBreakthrought.UI.Windows
{
    public class RocketVindowView : WindowView<RocketWindowHandler>
    {
        [Header("UI")]
        [SerializeField] private Button _repairButton;

        [Header("Container")]
        [SerializeField] private RectTransform _neededMechanismsForRocketContainer;

        private MechanismsGeneratorUI _mechanismsGeneratorUI;

        [Inject]
        private void Construct(MechanismsGeneratorUI mechanismsGeneratorUI) =>
            _mechanismsGeneratorUI = mechanismsGeneratorUI;

        public override void Initialize()
        {
            _repairButton.onClick.AddListener(() => Handler.Rocket.TryToRepair());

            var requiredMechanisms = Handler.Rocket.GetDMechanismsToRepairRocket();
            _mechanismsGeneratorUI.GenerateRequireMechanisms(requiredMechanisms, _neededMechanismsForRocketContainer);
        }

        public override void Dispose() => 
            _repairButton.onClick.RemoveListener(() => Handler.Rocket.TryToRepair());
    }
}
