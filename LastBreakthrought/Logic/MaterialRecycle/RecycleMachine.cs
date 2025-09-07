using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Logic.InteractionZone;
using LastBreakthrought.Logic.ShipDetail;
using LastBreakthrought.Logic.ShipMaterial.ScriptableObjects;
using LastBreakthrought.UI.Inventory;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Logic.MaterialRecycler
{
    public class RecycleMachine : MonoBehaviour
    {
        private DetailsContainer _detailsContainer;
        private InventoryMenuPanelHandler _detailInventory;
        private IAudioService _audioService;

        [Inject]
        private void Construct(DetailsContainer shipDetailsContainer
            , InventoryMenuPanelHandler detailInventoryMenuPanelHandler, IAudioService audioService)
        {
            _detailsContainer = shipDetailsContainer;
            _detailInventory = detailInventoryMenuPanelHandler;
            _audioService = audioService;
        }

        private void OnEnable() =>
            GetComponentInChildren<InteractionZoneHandler>().Init();

        public void RecycleEntireMaterial(ShipMaterialEntity shipMaterialEntity)
        {
            bool isNewDetail = true;
            PlayRecycleSound();

            foreach (var detail in _detailsContainer.Details)
            {
                if (shipMaterialEntity.Data.CraftDetail.Id == detail.Data.Id)
                {
                    detail.Quantity += shipMaterialEntity.Quantity;
                    _detailInventory.UpdateInventoryDetails(shipMaterialEntity);
                    isNewDetail = false;
                    break;
                }
            }

            if (isNewDetail)
            {
                var detail = _detailInventory.CreateNewShipDetailAndInit(shipMaterialEntity);
                _detailsContainer.Details.Add(detail.DetailEntity);
            }
        }

        public Vector3 GetMachinePosition() =>
            transform.position;

        public void PlayRecycleSound() =>
            _audioService.PlayOnObject(Configs.Sound.SoundType.RecyclerSound, this, false, 0.2f, 2f);
    }
}
