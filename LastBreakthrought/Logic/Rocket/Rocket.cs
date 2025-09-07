using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Infrustructure.Services.Massage;
using LastBreakthrought.Logic.InteractionZone;
using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.UI.Inventory;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Logic.Rocket
{
    public class Rocket : MonoBehaviour
    {
        private MechanismsContainer _mechanismsContainer;
        private InventoryMenuPanelHandler _inventory;
        private IMassageHandlerService _massageHandler;
        private RequireMechanismsProvider _requireMechanismsProvider;
        private IEventBus _eventBus;

        [Inject]
        private void Construct(MechanismsContainer mechanismsContainer, InventoryMenuPanelHandler detailInventory
            , IEventBus eventBus, IMassageHandlerService massageHandler, RequireMechanismsProvider mechanismsProvider)
        {
            _mechanismsContainer = mechanismsContainer;
            _inventory = detailInventory;
            _eventBus = eventBus;
            _massageHandler = massageHandler;
            _requireMechanismsProvider = mechanismsProvider;
        }

        private void OnEnable() =>
            GetComponentInChildren<InteractionZoneHandler>().Init();

        public void TryToRepair()
        {
            if (_mechanismsContainer.IsSearchedMechanismsAllFound(GetDMechanismsToRepairRocket()))
                Repair();
            else
                _massageHandler.ShowMassage("You don`t have the right mechanisms");
        }

        public List<MechanismEntity> GetDMechanismsToRepairRocket() =>
            _requireMechanismsProvider.Holder.RepairRocket.GetRequiredShipDetails();

        private void Repair()
        {
            _mechanismsContainer.GiveMechanisms(GetDMechanismsToRepairRocket());
            _inventory.UpdateInventoryMechanisms(GetDMechanismsToRepairRocket());
            _eventBus.Invoke(new OnGameWonSignal());
        }
    }
}
