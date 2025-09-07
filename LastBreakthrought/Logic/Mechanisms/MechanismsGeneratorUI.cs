using LastBreakthrought.UI.Inventory.Mechanism;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.Logic.Mechanisms
{
    public class MechanismsGeneratorUI
    {
        private readonly MechanismUIFactory _mechanismUIFactory;

        public MechanismsGeneratorUI(MechanismUIFactory mechanismUIFactory) =>
            _mechanismUIFactory = mechanismUIFactory;

        public void GenerateRequireMechanisms(List<MechanismEntity> requiredMechanisms, RectTransform parent)
        {
            foreach (var requiredMechanism in requiredMechanisms)
            {
                var neededMechanismView = _mechanismUIFactory.SpawnAt(parent);
                neededMechanismView.Init(requiredMechanism);
            }
        }

        public void GenerateMechanism(MechanismEntity requiredMechanism, RectTransform parent)
        {
            var neededMechanismView = _mechanismUIFactory.SpawnAt(parent);
            neededMechanismView.Init(requiredMechanism);
        }
    }
}
