using LastBreakthrought.CrashedShip;
using LastBreakthrought.Logic.ShipMaterial;
using LastBreakthrought.Logic.ShipMaterial.ScriptableObjects;
using LastBreakthrought.Player;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI.Other.Marker
{
    public class CrashedShipMarkerHandlerUI : MonoBehaviour, ICrashedShipMarker
    {
        [SerializeField] private CrashedShipMarkerViewUI _view;

        private PlayerHandler _player;
        private ICrashedShip _crashedShip;

        private ShipMaterialUIFactory _shipMaterialUIFactory;

        [Inject]
        private void Construct(PlayerHandler playerHandler, ShipMaterialUIFactory shipMaterialUIFactory)
        {
            _player = playerHandler;
            _shipMaterialUIFactory = shipMaterialUIFactory;
        }

        public void Init(ICrashedShip crashedShip)
        {
            _crashedShip = crashedShip;

            var materialsForMarker = _crashedShip.GetMaterialsForMarker();
            AddCrashedShipMaterialsToMarkerContainer(materialsForMarker);
        }

        private void Update() => UpdateDistance();

        public void SelfDestroy() =>
            Destroy(gameObject);

        public RectTransform GetTransform() =>
            transform.GetComponent<RectTransform>();

        private void UpdateDistance()
        {
            var distance = (int)(_player.GetPosition() - _crashedShip.GetPosition()).magnitude;
            _view.SetDistanceText(distance.ToString());
        }

        private void AddCrashedShipMaterialsToMarkerContainer(List<ShipMaterialEntity> materials)
        {
            foreach (var unminedShipMaterial in materials)
            {
                var shipMaterialUI = _shipMaterialUIFactory.SpawnAt(_view.GetContainer());
                shipMaterialUI.Init(unminedShipMaterial);
            }
        }
    }
}
