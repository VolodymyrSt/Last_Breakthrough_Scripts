using Assets.LastBreakthrought.UI.Inventory.ShipDetail;
using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.Logic.ShipDetail;
using LastBreakthrought.Logic.ShipMaterial.ScriptableObjects;
using LastBreakthrought.UI.Inventory.Mechanism;
using LastBreakthrought.UI.Inventory.ShipDetail;
using System.Collections.Generic;
using Zenject;

namespace LastBreakthrought.UI.Inventory
{
    public class InventoryMenuPanelHandler : IInitializable
    {
        public InventoryMenuPanelView View { get; }

        private readonly ShipDetailUIFactory _shipDetailUIFactory;
        private readonly MechanismUIFactory _mechanismUIFactory;

        public InventoryMenuPanelHandler(InventoryMenuPanelView view, ShipDetailUIFactory shipDetailUIFactory, MechanismUIFactory mechanismUIFactory)
        {
            View = view;
            _shipDetailUIFactory = shipDetailUIFactory;
            _mechanismUIFactory = mechanismUIFactory;
        }

        public void Initialize() => View.Init();

        public void UpdateInventoryDetails(ShipMaterialEntity shipMaterial)
        {
            foreach (var shipDetail in View.DetailsContainerUI.Details)
            {
                if (shipMaterial.Data.CraftDetail.Id == shipDetail.DetailEntity.Data.Id)
                {
                    shipDetail.Quantity += shipMaterial.Quantity;
                    shipDetail.UpdateView(shipDetail.DetailEntity);
                    break;
                }
            }        
        }

        public void UpdateInventoryMechanism(MechanismEntity mechanism)
        {
            foreach (var existedMechanism in View.MechanismsContainer.Mechanisms)
            {
                if (mechanism.Data.Id == existedMechanism.MechanismEntity.Data.Id)
                {
                    existedMechanism.Quantity += mechanism.Quantity;
                    existedMechanism.UpdateView(existedMechanism.MechanismEntity);
                    break;
                }
            }
        }

        public MechanismHandler CreateNewMechanismAndInit(MechanismEntity mechanism)
        {
            var newMechanismUI = _mechanismUIFactory.SpawnAt(View.GetMechanismContainer());
            View.MechanismsContainer.Mechanisms.Add(newMechanismUI);
            newMechanismUI.Init(mechanism);
            return newMechanismUI;
        }

        public ShipDetailHandler CreateNewShipDetailAndInit(ShipMaterialEntity shipMaterial)
        {
            var shipDetailUI = _shipDetailUIFactory.SpawnAt(View.GetDetailContainer());
            var shipDetailEntity = new ShipDetailEntity(shipMaterial.Data.CraftDetail, shipMaterial.Quantity);
            View.DetailsContainerUI.Details.Add(shipDetailUI);
            shipDetailUI.Init(shipDetailEntity);
            return shipDetailUI;
        }

        public void UpdateInventoryDetails(List<ShipDetailEntity> requiredDetails)
        {
            foreach (var requireDetail in requiredDetails)
            {
                for (var i = 0; i < View.DetailsContainerUI.Details.Count; i++)
                {
                    var existedDetailView = View.DetailsContainerUI.Details[i];

                    if (existedDetailView != null)
                    {
                        if (existedDetailView.DetailEntity.Data.Id == requireDetail.Data.Id)
                        {
                            existedDetailView.Quantity -= requireDetail.Quantity;

                            if (existedDetailView.Quantity <= 0)
                            {
                                existedDetailView.SelfDesctroy();
                                View.DetailsContainerUI.Details.Remove(existedDetailView);
                            }
                        }
                    }
                }
            }
        }

        public void UpdateInventoryMechanisms(List<MechanismEntity> requiredMechanisms)
        {
            foreach (var requireMechanism in requiredMechanisms)
            {
                for (var i = 0; i < View.MechanismsContainer.Mechanisms.Count; i++)
                {
                    var existedMechanismView = View.MechanismsContainer.Mechanisms[i];

                    if (existedMechanismView != null)
                    {
                        if (existedMechanismView.MechanismEntity.Data.Id == requireMechanism.Data.Id)
                        {
                            existedMechanismView.Quantity -= requireMechanism.Quantity;

                            if (existedMechanismView.Quantity <= 0)
                            {
                                existedMechanismView.SelfDesctroy();
                                View.MechanismsContainer.Mechanisms.Remove(existedMechanismView);
                            }
                        }
                    }
                }
            }
        }
    }
}
