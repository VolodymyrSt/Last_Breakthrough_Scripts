using Zenject;
using UnityEngine;
using LastBreakthrought.UI.CraftingMachine.Crafts;
using LastBreakthrought.Logic.Mechanisms;

namespace LastBreakthrought.UI.Windows.CraftMachineWindow
{
    public class CraftMachineWindowView : WindowView<CraftMachineWindowHandler>
    {
        [SerializeField] private RectTransform _craftsContent;

        private MechanismCraftUIFactory _mechanismCraftUIFactory;
        private MechanismHolderSO _mechanismHolder;

        [Inject]
        private void Construct(MechanismCraftUIFactory mechanismCraftUIFactory, MechanismHolderSO mechanismHolderSO)
        {
            _mechanismCraftUIFactory = mechanismCraftUIFactory;
            _mechanismHolder = mechanismHolderSO;
        }

        public override void Initialize()
        {
            SpawnCrafts();

            UpdateChildrenScale();
        }

        private void SpawnCrafts()
        {
            foreach (var mechanism in _mechanismHolder.Mechanisms)
            {
                _mechanismCraftUIFactory.SpawnCraft(_craftsContent, mechanism, () =>
                    Handler.CraftMachine.TryToCraft(mechanism.RequireDetails.GetRequiredShipDetails(), mechanism));
            }
        }

        public void UpdateChildrenScale()
        {
            foreach (Transform item in _craftsContent)
                item.localScale = Vector3.one;
        }

        public override void Dispose() { }
    }
}
