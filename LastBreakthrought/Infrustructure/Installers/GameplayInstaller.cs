using Zenject;
using UnityEngine;
using LastBreakthrought.Logic.Camera;
using LastBreakthrought.Player;
using LastBreakthrought.Other;
using LastBreakthrought.UI.Home;
using LastBreakthrought.UI.Timer;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.UI.PlayerStats;
using LastBreakthrought.Infrustructure.Services.ConfigProvider;
using LastBreakthrought.CrashedShip;
using LastBreakthrought.Infrustructure.AssetManagment;
using LastBreakthrought.Logic.ShipMaterial.ScriptableObjects;
using LastBreakthrought.Logic.ShipMaterial;
using LastBreakthrought.NPC.Enemy.Factory;
using Unity.AI.Navigation;
using LastBreakthrought.NPC.Robot.Factory;
using LastBreakthrought.UI.NPC.Robot.RobotsMenuPanel;
using LastBreakthrought.UI.NPC.Robot.RobotsMenuPanel.RobotControls.Factory;
using LastBreakthrought.Logic.ShipDetail;
using LastBreakthrought.Logic.MaterialRecycler;
using LastBreakthrought.UI.Inventory;
using Assets.LastBreakthrought.UI.Inventory.ShipDetail;
using LastBreakthrought.Logic.RobotFactory;
using LastBreakthrought.UI.Map;
using LastBreakthrought.UI.Other.Marker;
using LastBreakthrought.Infrustructure.Services.Massage;
using LastBreakthrought.UI.Inventory.Mechanism;
using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.UI.CraftingMachine.Crafts;
using LastBreakthrought.UI.PausedMenu;
using LastBreakthrought.UI.VictoryMenu;
using LastBreakthrought.UI.LostMenu;
using LastBreakthrought.UI.ToolTip;
using LastBreakthrought.Logic.FSX;
using LastBreakthrought.UI.Tutorial;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Logic.Video;
using LastBreakthrought.UI;
using LastBreakthrought.MiniMap;

namespace LastBreakthrought.Infrustructure.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [Header("Player")]
        [SerializeField] private PlayerHandler _playerPrefab;
        [SerializeField] private Transform _playerSpawnPoint;

        [Header("Camera")]
        [SerializeField] private FollowCamera _cameraPrefab;
        [SerializeField] private MiniMapCameraHandler _miniMapCameraHandlerPrefab;

        [Header("UI")]
        [SerializeField] private GameplayHub _gameplayHubPrefab;
        [SerializeField] private GameObject _joyStickPrefab;

        [Header("HomePoint")]
        [SerializeField] private HomePoint _homePointPrefab;

        [Header("RecyceMachine")]
        [SerializeField] private RecycleMachine _recycleMachine;

        [Header("RecyceMachine")]
        [SerializeField] private RobotFactoryMachine _robotFactoryMachine;

        [Header("Materials")]
        [SerializeField] private ShipMaterialHolderSO _shipMaterialHolder;

        [Header("Mechanism")]
        [SerializeField] private MechanismHolderSO _mechanismHolderSO;
        [SerializeField] private RequireMechanismHolderSO _requireMechanismHolderSO;

        [Header("Videos")]
        [SerializeField] private VideoPlayerHandler _videoPlayerHandler;

        [Header("Other")]
        [SerializeField] private NavMeshSurface _navMeshSurface;
        [SerializeField] private Light _light;

        private GameplayHub _gameplayHub;

        public override void InstallBindings()
        {
            BindEventBus();
            BindTimeHandler();
            BindAudioService();

            BindAssetProvider();

            BindSpawnersContainer();
            BindNavMeshSurface();

            BindShipMaterialGenerator();
            BindCrashedShipsContainer();

            BindEffectCreator();

            BindFactories();

            BindPlayer();
            BindCamera();

            BindVideoPlayerHandler();

            BindRecycleMachine();

            BindUI();
            BindInventory();

            BindLight();
            BindDayLightHandler();
            BindTimer();
        }

        private void BindInventory()
        {
            BindDetailsContainer();
            BindMechanismsContainer();

            BindShipDetailsGeneratorUI();
            BindRequireMechanismsProvider();

            BindMechanismHolder();
            BindMechanismsGeneratorUI();
        }

        private void BindFactories()
        {
            BindRobotFactoryMachine();
            BindCrashedShipFactory();
            BindShipMaterialViewFactory();
            BindShipDetailViewFactory();
            BindRobotControlsUIFactory();
            BindEnemyFactory();
            BindRobotsFactory();
            BindCrashedShipMarkerFactory();
            BindMechanismCraftUIFactory();
            BindMechanismUIFactory();
        }

        private void BindUI()
        {
            BindGamePlayHub();
            BindJoyStick();
            BindHomePoint();
            BindHomeDistanceInformer();
            BindRobotMenuPanel();
            BindDetailInventoryMenuPanel();
            BindMassageHandler();
            BindMapMenuPanel();
            BindPausedMenu();
            BindVictoryMenu();
            BindLostMenu();
            BindPlayerStats();
            BindToolTip();
            BindTutorial();
        }

        private void BindLight() => 
            Container.Bind<Light>().FromInstance(_light).AsSingle();

        private void BindDayLightHandler() => 
            Container.BindInterfacesAndSelfTo<DayLightHandler>().AsSingle().NonLazy();
        
        private void BindVideoPlayerHandler() => 
            Container.Bind<VideoPlayerHandler>().FromInstance(_videoPlayerHandler).AsSingle().NonLazy();

        private void BindSpawnersContainer()
        {
            var spawnersContainer = new SpawnersContainer();
            Container.Resolve<Game>().SpawnersContainer = spawnersContainer;
            Container.Bind<SpawnersContainer>().FromInstance(spawnersContainer).AsSingle();
        }

        private void BindRobotFactoryMachine()
        {
            Container.Resolve<Game>().RobotFactoryMachine = _robotFactoryMachine;
            Container.Bind<RobotFactoryMachine>().FromInstance(_robotFactoryMachine).AsSingle();
        }

        private void BindRecycleMachine() => 
            Container.Bind<RecycleMachine>().FromInstance(_recycleMachine).AsSingle();
        
        private void BindEffectCreator() => 
            Container.Bind<EffectCreator>().AsSingle();

        private void BindMechanismHolder() =>
            Container.Bind<MechanismHolderSO>().FromInstance(_mechanismHolderSO).AsSingle();

        private void BindDetailsContainer() => 
            Container.Bind<DetailsContainer>().AsSingle();

        private void BindMechanismsContainer() =>
            Container.Bind<MechanismsContainer>().AsSingle();

        private void BindRequireMechanismsProvider() =>
            Container.Bind<RequireMechanismsProvider>().AsSingle().WithArguments(_requireMechanismHolderSO);

        private void BindShipDetailsGeneratorUI() =>
            Container.Bind<ShipDetailsGeneratorUI>().AsSingle();

        private void BindMechanismsGeneratorUI() =>
            Container.Bind<MechanismsGeneratorUI>().AsSingle();

        private void BindNavMeshSurface()
        {
            Container.Resolve<Game>().NavMeshSurface = _navMeshSurface;
            Container.Bind<NavMeshSurface>().FromInstance(_navMeshSurface);
        }

        private void BindEnemyFactory() => 
            Container.Bind<EnemyFactory>().AsSingle();

        private void BindTimeHandler() => 
            Container.Bind<TimeHandler>().AsSingle();

        private void BindMechanismCraftUIFactory() =>
            Container.Bind<MechanismCraftUIFactory>().AsSingle();

        private void BindMechanismUIFactory() =>
            Container.Bind<MechanismUIFactory>().AsSingle();

        private void BindCrashedShipMarkerFactory() =>
            Container.Bind<CrashedShipMarkerFactoryUI>().AsSingle();

        private void BindMassageHandler()
        {
            var massageView = _gameplayHub.GetComponentInChildren<MassageView>();
            Container.Bind<MassageView>().FromInstance(massageView).AsSingle();

            Container.Bind<IMassageHandlerService>().To<MassageHandlerService>().AsSingle();
        }

        private void BindPausedMenu()
        {
            var pausedMenu = _gameplayHub.GetComponentInChildren<PausedMenuView>();
            Container.Bind<PausedMenuView>().FromInstance(pausedMenu).AsSingle();

            Container.BindInterfacesAndSelfTo<PausedMenuHandler>().AsSingle().NonLazy();
        }

        private void BindToolTip()
        {
            var toolTip = _gameplayHub.GetComponentInChildren<ToolTipView>();
            var canvas = _gameplayHub.GetComponent<Canvas>();

            Container.Bind<ToolTipView>().FromInstance(toolTip).AsSingle();
            Container.BindInterfacesAndSelfTo<ToolTipHandler>().AsSingle().WithArguments(toolTip, canvas).NonLazy();
        }

        private void BindVictoryMenu()
        {
            var victoryMenu = _gameplayHub.GetComponentInChildren<VictoryMenuView>();
            Container.Bind<VictoryMenuView>().FromInstance(victoryMenu).AsSingle();

            Container.BindInterfacesAndSelfTo<VictoryMenuHandler>().AsSingle().NonLazy();
        }

        private void BindTutorial()
        {
            var tutorialView = _gameplayHub.GetComponentInChildren<TutorialView>();
            Container.Bind<TutorialView>().FromInstance(tutorialView).AsSingle();

            Container.BindInterfacesAndSelfTo<TutorialHandler>().AsSingle().NonLazy();
        }

        private void BindLostMenu()
        {
            var lostMenu = _gameplayHub.GetComponentInChildren<LostMenuView>();
            Container.Bind<LostMenuView>().FromInstance(lostMenu).AsSingle();

            Container.BindInterfacesAndSelfTo<LostMenuHandler>().AsSingle().NonLazy();
        }

        private void BindRobotsFactory()
        {
            Container.Bind<RobotMinerFactory>().AsSingle();
            Container.Bind<RobotTransporterFactory>().AsSingle();
            Container.Bind<RobotDefenderFactory>().AsSingle();
        }

        private void BindShipMaterialViewFactory() => 
            Container.Bind<ShipMaterialUIFactory>().AsSingle();
        private void BindShipDetailViewFactory() =>
            Container.Bind<ShipDetailUIFactory>().AsSingle();


        private void BindRobotControlsUIFactory()
        {
            Container.Bind<RobotMinerControlUIFactory>().AsSingle();
            Container.Bind<RobotTransporterControlUIFactory>().AsSingle();
            Container.Bind<RobotDefenderControlUIFactory>().AsSingle();
        }

        private void BindShipMaterialGenerator()
        {
            var materialGenerator = new ShipMaterialGenerator(_shipMaterialHolder.AllMaterials);
            Container.Bind<ShipMaterialGenerator>().FromInstance(materialGenerator);
        }

        private void BindAssetProvider() =>
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();

        private void BindCrashedShipsContainer() => 
            Container.Bind<CrashedShipsContainer>().AsSingle();

        private void BindCrashedShipFactory() => 
            Container.Bind<CrashedShipFactory>().AsSingle();

        private void BindPlayerStats()
        {
            var playerStatsView = _gameplayHub.GetComponentInChildren<PlayerStatsView>();
            Container.Bind<PlayerStatsView>().FromInstance(playerStatsView).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerStatsModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerStatsHandler>().AsSingle().NonLazy();
        }

        private void BindEventBus() => 
            Container.Bind<IEventBus>().To<EventBus>().AsSingle();

        private void BindAudioService()
        {
            var eventBus = Container.TryResolve<IEventBus>();
            Container.TryResolve<IAudioService>().Initialize(eventBus);
        }

        private void BindTimer()
        {
            var timerView = _gameplayHub.GetComponentInChildren<TimerView>();
            Container.Bind<TimerView>().FromInstance(timerView).AsSingle();

            Container.BindInterfacesAndSelfTo<TimerController>().AsSingle().NonLazy();
        }

        private void BindMapMenuPanel()
        {
            var mapView = _gameplayHub.GetComponentInChildren<MapMenuPanelView>();
            Container.Bind<MapMenuPanelView>().FromInstance(mapView).AsSingle();

            Container.BindInterfacesAndSelfTo<MapMenuPanelHandler>().AsSingle().NonLazy();
        }

        private void BindHomeDistanceInformer()
        {
            var homeDistanceView = _gameplayHub.GetComponentInChildren<HomeDistanceView>();
            Container.Bind<HomeDistanceView>().FromInstance(homeDistanceView).AsSingle();

            Container.BindInterfacesAndSelfTo<HomeDistanceCounter>().AsSingle().NonLazy();
        }

        private void BindRobotMenuPanel()
        {
            var robotMenuPanelView = _gameplayHub.GetComponentInChildren<RobotMenuPanelView>();
            Container.Bind<RobotMenuPanelView>().FromInstance(robotMenuPanelView).AsSingle();

            Container.BindInterfacesAndSelfTo<RobotMenuPanelHandler>().AsSingle().NonLazy();
        }

        private void BindDetailInventoryMenuPanel()
        {
            var view = _gameplayHub.GetComponentInChildren<InventoryMenuPanelView>();
            Container.Bind<InventoryMenuPanelView>().FromInstance(view).AsSingle();

            Container.BindInterfacesAndSelfTo<InventoryMenuPanelHandler>().AsSingle().NonLazy();
        }

        private void BindJoyStick()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
                Container.InstantiatePrefab(_joyStickPrefab, _gameplayHub.transform);
        }

        private void BindGamePlayHub()
        {
            var hub = Container.InstantiatePrefab(_gameplayHubPrefab);
            _gameplayHub = hub.GetComponent<GameplayHub>();
            _gameplayHub.Init();
        }

        private void BindCamera()
        {
            var camera = Container.InstantiatePrefabForComponent<FollowCamera>(_cameraPrefab);
            Container.Bind<FollowCamera>().FromInstance(camera).AsSingle();
            Container.Bind<Camera>().FromInstance(camera.GetComponentInChildren<Camera>()).AsSingle();

            var miniMapCamera = Container.InstantiatePrefabForComponent<MiniMapCameraHandler>(_miniMapCameraHandlerPrefab);
            Container.Bind<MiniMapCameraHandler>().FromInstance(miniMapCamera).AsSingle();
        }
        private void BindHomePoint() => 
            Container.Bind<HomePoint>().FromInstance(_homePointPrefab).AsSingle();

        private void BindPlayer()
        {
            var player = Container.InstantiatePrefabForComponent<PlayerHandler>(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity, null);
            Container.Bind<PlayerHandler>().FromInstance(player).AsSingle();
        }
    }
}

