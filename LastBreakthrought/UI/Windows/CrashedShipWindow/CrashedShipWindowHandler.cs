using LastBreakthrought.Logic.ShipMaterial;
using LastBreakthrought.Logic.ShipMaterial.ScriptableObjects;
using LastBreakthrought.UI.NPC.Robot.RobotsMenuPanel;
using LastBreakthrought.UI.ShipMaterial;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI.Windows.CrashedShipWindow
{
    public class CrashedShipWindowHandler : WindowHandler<CrashedShipWindowView>
    {
        [field: SerializeField] public CrashedShip.CrashedShip CrashedShip {  get; private set; }
        [field: SerializeField] public ShipMaterialsContainer UnminedShipMaterialsContainer { get; private set; }
        [field: SerializeField] public ShipMaterialsContainer MinedShipMaterialsContainer { get; private set; }

        private ShipMaterialUIFactory _shipMaterialUIFactory;
        private RobotMenuPanelHandler _robotMenuPanelHandler;

        private RectTransform _unminedShipMaterialsContainerTransform;
        private RectTransform _minedShipMaterialsContainerTransform;

        [Inject]
        private void Construct(ShipMaterialUIFactory shipMaterialUIFactory, RobotMenuPanelHandler robotMenuPanelHandler)
        {
            _shipMaterialUIFactory = shipMaterialUIFactory;
            _robotMenuPanelHandler = robotMenuPanelHandler; 
        }

        private void OnEnable()
        {
            _unminedShipMaterialsContainerTransform = UnminedShipMaterialsContainer.GetComponent<RectTransform>();
            _minedShipMaterialsContainerTransform = MinedShipMaterialsContainer.GetComponent<RectTransform>();
        }

        public override void ActivateWindow()
        {
            View.ShowView();
            _robotMenuPanelHandler.View.OpenPanel();
        }

        public override void DeactivateWindow() => View.HideView();

        public void CreateUnminedShipMaterialsView()
        {
            foreach (var unminedShipMaterial in CrashedShip.Materials)
                CreateUnminedShipMaterialAndInit(unminedShipMaterial);
        }

        public void UpdateEntireMaterial()
        {
            foreach (var unminedShipMaterial in UnminedShipMaterialsContainer.Materials)
            {
                if (UnminedShipMaterialsContainer.Materials.Count < 0)
                    break;

                var newShipMaterialUI = _shipMaterialUIFactory.SpawnAt(MinedShipMaterialsContainer.GetComponent<RectTransform>());
                newShipMaterialUI.InitMined(unminedShipMaterial.MaterialEntity);
                newShipMaterialUI.Quantity = unminedShipMaterial.Quantity;

                MinedShipMaterialsContainer.Materials.Add(newShipMaterialUI);
                UnminedShipMaterialsContainer.Materials.Remove(unminedShipMaterial);
                Destroy(unminedShipMaterial.gameObject);
                break;
            }
        }

        public void RemoveMinedMaterialFromWindow()
        {
            if (MinedShipMaterialsContainer.Materials.Count > 0)
            {
                foreach (var minedMaterial in MinedShipMaterialsContainer.Materials)
                {
                    MinedShipMaterialsContainer.Materials.Remove(minedMaterial);
                    Destroy(minedMaterial.gameObject);
                    break;
                }
            }
        }

        private void CreateUnminedShipMaterialAndInit(ShipMaterialEntity unminedShipMaterial)
        {
            var shipMaterialUI = _shipMaterialUIFactory.SpawnAt(_unminedShipMaterialsContainerTransform);
            UnminedShipMaterialsContainer.Materials.Add(shipMaterialUI);
            shipMaterialUI.Init(unminedShipMaterial);
        }
    }
}
