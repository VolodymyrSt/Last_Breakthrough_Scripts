using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.Logic.ShipDetail;
using System;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI.CraftingMachine.Crafts
{
    public class MechanismCraftHandler : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private MechanismCraftView _view;

        private ShipDetailsGeneratorUI _shipDetailsGeneratorUI;
        private MechanismsGeneratorUI _mechanismsGeneratorUI;

        [Inject]
        private void Construct(ShipDetailsGeneratorUI shipDetailsGeneratorUI, MechanismsGeneratorUI mechanismsGeneratorUI)
        {
            _shipDetailsGeneratorUI = shipDetailsGeneratorUI;
            _mechanismsGeneratorUI = mechanismsGeneratorUI;
        }

        public void Init(Action craftAction, MechanismSO mechanism)
        {
            _view.Init(craftAction);
            var mechanismEntity = new MechanismEntity(mechanism, 1);
            _mechanismsGeneratorUI.GenerateMechanism(mechanismEntity, _view.GetMechanismContainer());

            var requiredDetails = mechanism.RequireDetails.GetRequiredShipDetails();
            _shipDetailsGeneratorUI.GenerateRequireDetails(requiredDetails, _view.GetContainer());
        }
    }
}
