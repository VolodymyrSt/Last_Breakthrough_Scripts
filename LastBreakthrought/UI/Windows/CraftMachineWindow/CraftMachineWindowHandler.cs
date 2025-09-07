using LastBreakthrought.Logic.CraftingMachine;
using LastBreakthrought.UI.Inventory;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI.Windows.CraftMachineWindow
{
    public class CraftMachineWindowHandler : WindowHandler<CraftMachineWindowView>
    {
        [field:SerializeField] public CraftMachine CraftMachine {  get; private set; }

        private InventoryMenuPanelHandler _inventoryMenuPanel;

        [Inject]
        private void Construct(InventoryMenuPanelHandler inventoryMenuPanel) =>
            _inventoryMenuPanel = inventoryMenuPanel;

        public override void ActivateWindow()
        {
            View.ShowView();
            _inventoryMenuPanel.View.OpenInventory();
        }

        public override void DeactivateWindow() => View.HideView();
    }
}
