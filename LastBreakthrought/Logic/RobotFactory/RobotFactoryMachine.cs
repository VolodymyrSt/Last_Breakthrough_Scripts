using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.Massage;
using LastBreakthrought.Logic.ChargingPlace;
using LastBreakthrought.Logic.FSX;
using LastBreakthrought.Logic.InteractionZone;
using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.Logic.ShipDetail;
using LastBreakthrought.NPC.Robot.Factory;
using LastBreakthrought.UI.Inventory;
using LastBreakthrought.UI.NPC.Robot.RobotsMenuPanel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Logic.RobotFactory
{
    public class RobotFactoryMachine : MonoBehaviour
    {
        public event Action<int> OnMinersCountChanged;
        public event Action<int> OnTransportersCountChanged;
        public event Action<int> OnDefendersCountChanged;

        [Header("Base:")]
        [SerializeField] private Transform _robotSpawnPoint;
        [SerializeField] private BoxCollider _robotWanderingZone;
        [SerializeField] private List<RobotChargingPlace> _chargingPlaces;
        [SerializeField] private Transform _effectRoot;

        private RobotMinerFactory _robotMinerFactory;
        private RobotTransporterFactory _robotTransporterFactory;
        private RobotDefenderFactory _robotDefenderFactory;

        private RobotMenuPanelHandler _robotMenuPanelHandler;
        private MechanismsContainer _mechanismsContainer;
        private InventoryMenuPanelHandler _inventory;
        private IMassageHandlerService _massageHandler;
        private RequireMechanismsProvider _requireMechanismsProvider;
        private IAudioService _audioService;
        private EffectCreator _effectCreator;

        private int _currentMinersCount = 0;
        private int _currentTransportersCount = 0;
        private int _currentDefendersCount = 0;

        private bool _isRobotCreating = false;

        [Inject]
        private void Construct(RobotMinerFactory robotFactory, RobotTransporterFactory robotTransporterFactory
            , RobotDefenderFactory robotDefenderFactory, RobotMenuPanelHandler robotMenuPanelHandler, MechanismsContainer mechanismsContainer
            , InventoryMenuPanelHandler detailInventory, IMassageHandlerService massage, RequireMechanismsProvider requireMechanismsProvider,
            IAudioService audioService, EffectCreator effectCreator)
        {
            _robotMinerFactory = robotFactory;
            _robotTransporterFactory = robotTransporterFactory;
            _robotDefenderFactory = robotDefenderFactory;

            _robotMenuPanelHandler = robotMenuPanelHandler;
            _mechanismsContainer = mechanismsContainer;
            _inventory = detailInventory;
            _massageHandler = massage;
            _requireMechanismsProvider = requireMechanismsProvider;
            _audioService = audioService;
            _effectCreator = effectCreator;
        }

        private void OnEnable() =>
            GetComponentInChildren<InteractionZoneHandler>().Init();

        public void CreateStartedRobotsAtTheBeginning()
        {
            CreateMiner();
            CreateTransporter();
            CreateDefender();
        }

        public void CreateRobotMiner()
        {
            if (_currentMinersCount < Constants.MAX_MINERS_COUNT)
            {
                if (_mechanismsContainer.IsSearchedMechanismsAllFound(GetMechanismsToCreateMiner()))
                {
                    if (_isRobotCreating)
                        _massageHandler.ShowMassage("Wait a bit, machine can`t create robots soo fast");
                    else
                    {
                        _isRobotCreating = true;
                        PlaySoundAndEffect();
                        _mechanismsContainer.GiveMechanisms(GetMechanismsToCreateMiner());
                        _inventory.UpdateInventoryMechanisms(GetMechanismsToCreateMiner());
                        CreateMiner();
                        StartCoroutine(CheckIfRobotIsCreated());
                    }
                }
                else
                    _massageHandler.ShowMassage("You can`t create because you don`t have right mechanisms");
            }
            else
                _massageHandler.ShowMassage("You can only have three miners");
        }

        public void CreateRobotTransporter()
        {
            if (_currentTransportersCount < Constants.MAX_TRANSPORTERS_COUNT)
            {
                if (_mechanismsContainer.IsSearchedMechanismsAllFound(GetMechanismsToCreateTransporter()))
                {
                    if (_isRobotCreating)
                        _massageHandler.ShowMassage("Wait a bit, machine can`t create robots soo fast");
                    else
                    {
                        _isRobotCreating = true;
                        PlaySoundAndEffect();
                        _mechanismsContainer.GiveMechanisms(GetMechanismsToCreateTransporter());
                        _inventory.UpdateInventoryMechanisms(GetMechanismsToCreateTransporter());
                        CreateTransporter();
                        StartCoroutine(CheckIfRobotIsCreated());
                    }
                }
                else
                    _massageHandler.ShowMassage("You can`t create because you don`t have right mechanisms");
            }
            else
                _massageHandler.ShowMassage("You can only have three transporters");
        }

        public void CreateRobotDefender()
        {
            if (_currentDefendersCount < Constants.MAX_DEFENDERS_COUNT)
            {
                if (_mechanismsContainer.IsSearchedMechanismsAllFound(GetMechanismsToCreateDefender()))
                {
                    if (_isRobotCreating)
                        _massageHandler.ShowMassage("Wait a bit, machine can`t create robots soo fast");
                    else
                    {
                        _isRobotCreating = true;
                        PlaySoundAndEffect();
                        _mechanismsContainer.GiveMechanisms(GetMechanismsToCreateDefender());
                        _inventory.UpdateInventoryMechanisms(GetMechanismsToCreateDefender());
                        CreateDefender();
                        StartCoroutine(CheckIfRobotIsCreated());
                    }
                }
                else
                    _massageHandler.ShowMassage("You can`t create because you don`t have right mechanisms");
            }
            else
                _massageHandler.ShowMassage("You can only have three defenders");
        }

        public List<MechanismEntity> GetMechanismsToCreateMiner() =>
            _requireMechanismsProvider.Holder.CreateRobotMiner.GetRequiredShipDetails();

        public List<MechanismEntity> GetMechanismsToCreateTransporter() =>
            _requireMechanismsProvider.Holder.CreateRobotTransporter.GetRequiredShipDetails();

        public List<MechanismEntity> GetMechanismsToCreateDefender() =>
            _requireMechanismsProvider.Holder.CreateRobotDefender.GetRequiredShipDetails();

        private void CreateMiner()
        {
            var robotMiner = _robotMinerFactory.CreateRobot(_robotSpawnPoint.position, _robotSpawnPoint,
                                    _robotWanderingZone, _chargingPlaces);

            _robotMenuPanelHandler.AddRobotMinerControlUI(robotMiner, robotMiner.GetRobotData(), 
                robotMiner.GetRobotBattary(), robotMiner.GetRobotHealth(), robotMiner.SetFollowingPlayerState
                , robotMiner.SetWanderingState, mineAction: robotMiner.DoWork);

            _currentMinersCount++;

            OnMinersCountChanged?.Invoke(_currentMinersCount);
        }

        private void CreateTransporter()
        {
            var robotTransporter = _robotTransporterFactory.CreateRobot(_robotSpawnPoint.position,
                                    _robotSpawnPoint, _robotWanderingZone, _chargingPlaces);

            _robotMenuPanelHandler.AddRobotTransporterControlUI(robotTransporter, robotTransporter.GetRobotData(),
                robotTransporter.GetRobotBattary(), robotTransporter.GetRobotHealth(), robotTransporter.SetFollowingPlayerState,
                robotTransporter.SetWanderingState, transportAction: robotTransporter.DoWork);

            _currentTransportersCount++;

            OnTransportersCountChanged?.Invoke(_currentTransportersCount);
        }

        private void CreateDefender()
        {
            var robotDefender = _robotDefenderFactory.CreateRobot(_robotSpawnPoint.position,
                                    _robotSpawnPoint, _robotWanderingZone, _chargingPlaces);

            _robotMenuPanelHandler.AddRobotDefenderControlUI(robotDefender, robotDefender.GetRobotData(),
                robotDefender.GetRobotBattary(), robotDefender.GetRobotHealth(), robotDefender.SetFollowingPlayerState,
                robotDefender.SetWanderingState, defend: robotDefender.DoWork);

            _currentDefendersCount++;

            OnDefendersCountChanged?.Invoke(_currentDefendersCount);
        }

        private void PlaySoundAndEffect()
        {
            _audioService.PlayOnObject(Configs.Sound.SoundType.RobotFactoryCreating, this);
            _effectCreator.CreateExplosionEffect(_effectRoot);
        }

        private IEnumerator CheckIfRobotIsCreated()
        {
            yield return new WaitForSeconds(1.5f);

            _isRobotCreating = false;
        }
    }
}
