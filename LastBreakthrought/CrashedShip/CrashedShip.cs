using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Infrustructure.Services.Massage;
using LastBreakthrought.Logic.InteractionZone;
using LastBreakthrought.Logic.ShipMaterial;
using LastBreakthrought.Logic.ShipMaterial.ScriptableObjects;
using LastBreakthrought.UI.Map;
using LastBreakthrought.UI.Other.Marker;
using LastBreakthrought.UI.Windows.CrashedShipWindow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.CrashedShip
{
    public class CrashedShip : MonoBehaviour, ICrashedShip
    {
        public event Action OnDestroyed;

        [Header("Setting:")]
        [SerializeField] private ShipRarity _rarity;
        [SerializeField] private int _maxNumberOfMaterialDiversity;
        [SerializeField] private CrashedShipWindowHandler _crashedShipWindowHandler;

        private CrashedShipsContainer _shipsContainer;
        private ShipMaterialGenerator _shipMaterialGenerator;
        private MapMenuPanelHandler _mapMenuPanel;
        private ICrashedShipMarker _marker;
        private IMassageHandlerService _massageHandler;

        public List<ShipMaterialEntity> Materials { get; private set; } = new ();
        public List<ShipMaterialEntity> MinedMaterials { get; private set; } = new ();

        private List<ShipMaterialEntity> _materials;
        private bool _isMarked = false;

        private void OnValidate()
        {
            if (_maxNumberOfMaterialDiversity < (int)_rarity)
                _maxNumberOfMaterialDiversity = (int)_rarity;
            if (_maxNumberOfMaterialDiversity > Constants.MaxNumberOfShipMaterialsInOneWindow)
                _maxNumberOfMaterialDiversity = Constants.MaxNumberOfShipMaterialsInOneWindow;
        }

        [Inject]
        private void Construct(CrashedShipsContainer shipsContainer, ShipMaterialGenerator materialGenerator
            , MapMenuPanelHandler mapMenuPanel, IMassageHandlerService massageHandler)
        {
            _shipsContainer = shipsContainer;
            _shipMaterialGenerator = materialGenerator;
            _mapMenuPanel = mapMenuPanel;
            _massageHandler = massageHandler;
        }

        public void OnInitialized()
        {
            _shipsContainer.CrashedShips.Add(this);
            Materials = _shipMaterialGenerator.GenerateShipMaterials(_rarity, _maxNumberOfMaterialDiversity);
            GetComponentInChildren<InteractionZoneHandler>().Init();

            _materials = new List<ShipMaterialEntity>(Materials);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
                MineEntireMaterial();
        }

        public Vector3 GetPosition() => 
            transform.position;

        public ShipMaterialEntity MineEntireMaterial()
        {
            ShipMaterialEntity minedMaterial = null;
            foreach (var unminedShipMaterial in Materials)
            {
                if (Materials.Count < 0)
                    break;
                else
                {
                    minedMaterial = unminedShipMaterial;
                    _crashedShipWindowHandler.UpdateEntireMaterial();
                    MinedMaterials.Add(unminedShipMaterial);
                    Materials.Remove(unminedShipMaterial);
                    break;
                }
            }
            return minedMaterial;
        }

        public void RemoveMinedMaterialView() => 
            _crashedShipWindowHandler.RemoveMinedMaterialFromWindow();

        public void AddItselfAsMarker()
        {
            if (!_isMarked)
            {
                _isMarked = true;
                _marker = _mapMenuPanel.Add(this);
            }
            else
                _massageHandler.ShowMassage("This ship is already marked");
        }

        public IEnumerator DestroySelf()
        {
            yield return new WaitForSecondsRealtime(Constants.CRASHED_SHIP_DESCTRUCTION_TIME);
            _shipsContainer.CrashedShips.Remove(this);
            _marker?.SelfDestroy();
            Destroy(gameObject);
            OnDestroyed?.Invoke();
        }

        public List<ShipMaterialEntity> GetMaterialsForMarker() => _materials;
    }
}
