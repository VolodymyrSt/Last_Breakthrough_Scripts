using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.Massage;
using LastBreakthrought.Logic.InteractionZone;
using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.Logic.ShipDetail;
using LastBreakthrought.UI.Inventory;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Logic.CraftingMachine
{
    public class CraftMachine : MonoBehaviour
    {
        private DetailsContainer _detailsContainer;
        private InventoryMenuPanelHandler _detailInventory;
        private IMassageHandlerService _massageHandler;
        private MechanismsContainer _mechanismsContainer;
        private IAudioService _audioService;

        [Inject]
        private void Construct(DetailsContainer detailsContainer , InventoryMenuPanelHandler detailInventory
            , IMassageHandlerService massage, MechanismsContainer mechanismsContainer, IAudioService audioService)
        {
            _detailsContainer = detailsContainer;
            _detailInventory = detailInventory;
            _massageHandler = massage;
            _mechanismsContainer = mechanismsContainer;
            _audioService = audioService;
        }

        private void OnEnable() => 
            GetComponentInChildren<InteractionZoneHandler>().Init();

        public void TryToCraft(List<ShipDetailEntity> requiredDetails, MechanismSO mechanism)
        {
            if (_detailsContainer.IsSearchedDetailsAllFound(requiredDetails))
            {
                var mechanismEntity = new MechanismEntity(mechanism, quantity: 1); 

                _detailsContainer.GiveDetails(requiredDetails);
                _detailInventory.UpdateInventoryDetails(requiredDetails);
                CreateOrAddMechanism(mechanismEntity);
            }
            else
                _massageHandler.ShowMassage("You don`t have right details");
        }

        private void CreateOrAddMechanism(MechanismEntity mechanismEntity)
        {
            _audioService.PlayOnObject(Configs.Sound.SoundType.CraftSound, this);

            bool isNewMechansim = true;

            foreach (var existedMechanism in _mechanismsContainer.Mechanisms)
            {
                if (mechanismEntity.Data.Id == existedMechanism.Data.Id)
                {
                    existedMechanism.Quantity += mechanismEntity.Quantity;
                    _detailInventory.UpdateInventoryMechanism(mechanismEntity);
                    isNewMechansim = false;
                    break;
                }
            }

            if (isNewMechansim)
            {
                var newMechanism = _detailInventory.CreateNewMechanismAndInit(mechanismEntity);
                _mechanismsContainer.Mechanisms.Add(newMechanism.MechanismEntity);
            }
        }
    }
}
