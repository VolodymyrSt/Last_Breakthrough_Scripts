using LastBreakthrought.Configs.Robot;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.ConfigProvider;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Infrustructure.Services.Massage;
using LastBreakthrought.Logic.ChargingPlace;
using LastBreakthrought.Logic.FSX;
using LastBreakthrought.Logic.InteractionZone;
using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.NPC.Base;
using LastBreakthrought.NPC.Robot.States;
using LastBreakthrought.Player;
using LastBreakthrought.UI.Inventory;
using LastBreakthrought.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace LastBreakthrought.NPC.Robot
{
    public abstract class RobotBase : MonoBehaviour, IRobot, IDamagable
    {
        [Header("Base:")]
        [SerializeField] protected NavMeshAgent Agent;
        [SerializeField] protected Animator Animator;
        [SerializeField] private InteractionZoneHandler _zoneHandler;
        [SerializeField] private BoxCollider _collider;

        protected RobotBattary Battary;
        protected RobotHealth Health;
        protected NPCStateMachine StateMachine;
        protected PlayerHandler PlayerHandler;
        protected ICoroutineRunner CoroutineRunner;
        protected RobotConfigSO RobotData;
        protected IEventBus EventBus;
        protected IMassageHandlerService MassageHandler;
        protected RequireMechanismsProvider RequireMechanismsProvider;
        protected EffectCreator EffectCreator;

        protected RobotWanderingState RobotWanderingState;
        protected RobotFollowingPlayerState RobotFollowingPlayerState;
        protected RobotRechargingState RobotRechargingState;
        protected RobotDestroyedState RobotDestroyedState;
        protected IAudioService AudioService;

        private IConfigProviderService _configProvider;
        private MechanismsContainer _mechanismsContainer;
        private InventoryMenuPanelHandler _inventory;
        private List<RobotChargingPlace> _chargingPlaces;

        protected bool IsWanderingState { get; set; }
        protected bool IsFollowingState { get; set; }
        protected bool IsRobotDestroyed { get; set; }

        [Inject]
        private void Construct(PlayerHandler playerHandler, ICoroutineRunner coroutineRunner, IConfigProviderService configProviderService,
            IEventBus eventBus, IMassageHandlerService massageHandler, MechanismsContainer mechanismsContainer, InventoryMenuPanelHandler detailInventory,
            RequireMechanismsProvider requireMechanismsProvider, EffectCreator effectCreator, IAudioService audioService)
        {
            PlayerHandler = playerHandler;
            CoroutineRunner = coroutineRunner;
            EventBus = eventBus;
            _configProvider = configProviderService;
            MassageHandler = massageHandler;
            _mechanismsContainer = mechanismsContainer;
            _inventory = detailInventory;
            RequireMechanismsProvider = requireMechanismsProvider;
            EffectCreator = effectCreator;
            AudioService = audioService;
        }

        public virtual void OnCreated(BoxCollider wanderingZone, List<RobotChargingPlace> chargingPlaces, string id)
        {
            _chargingPlaces = chargingPlaces;
            RobotData = _configProvider.RobotConfigHolderSO.GetRobotDataById(id);

            Battary = new RobotBattary(RobotData.MaxBattaryCapacity, RobotData.CapacityLimit);
            Health = new RobotHealth(RobotData.MaxHealth);
            StateMachine = new NPCStateMachine();

            RobotWanderingState = new RobotWanderingState(this, CoroutineRunner, Agent, Animator, wanderingZone, Battary, AudioService, EventBus, RobotData.WandaringSpeed);
            RobotFollowingPlayerState = new RobotFollowingPlayerState(this, Agent, PlayerHandler, Animator, Battary, CoroutineRunner, AudioService, EventBus, RobotData.GeneralSpeed);
            RobotRechargingState = new RobotRechargingState(this, Agent, Animator, Battary, RobotData.GeneralSpeed);
            RobotDestroyedState = new RobotDestroyedState(this, Agent, Animator, _collider, _zoneHandler, EffectCreator, AudioService);

            StateMachine.AddTransition(RobotWanderingState, RobotDestroyedState, () => IsRobotDestroyed);
            StateMachine.AddTransition(RobotFollowingPlayerState, RobotDestroyedState, () => IsRobotDestroyed);
            StateMachine.AddTransition(RobotRechargingState, RobotDestroyedState, () => IsRobotDestroyed);

            StateMachine.AddTransition(RobotDestroyedState, RobotWanderingState, () => !IsRobotDestroyed && IsWanderingState);
            StateMachine.AddTransition(RobotDestroyedState, RobotFollowingPlayerState, () => !IsRobotDestroyed && IsFollowingState);
            StateMachine.AddTransition(RobotDestroyedState, RobotRechargingState, () => !IsRobotDestroyed && Battary.NeedToBeRecharged);
            StateMachine.AddTransition(RobotDestroyedState, RobotWanderingState, () => !IsRobotDestroyed && !IsWanderingState && !IsFollowingState);

            _zoneHandler.Init();
            _zoneHandler.Disactivate();
        }

        private void Update() => StateMachine.Tick();

        public virtual void ApplyDamage(float damage)
        {
            Health.TakeDamage(damage);

            if (Health.IsHealthGone())
                IsRobotDestroyed = true;
        }

        public void SetFollowingPlayerState()
        {
            if (!CanFunction()) return;

            IsFollowingState = true;
            IsWanderingState = false;
        }

        public void SetWanderingState()
        {
            if (!CanFunction()) return;

            IsWanderingState = true;
            IsFollowingState = false;
        }

        public virtual void DoWork() { }

        public RobotConfigSO GetRobotData() => RobotData;

        public RobotBattary GetRobotBattary() => Battary;

        public RobotHealth GetRobotHealth() => Health;

        public int GetRobotDistanceToPlayer() =>
            (int)(transform.position - PlayerHandler.GetPosition()).magnitude;

        public RobotChargingPlace FindAvelableCharingPlace()
        {
            foreach (var chargingPlace in _chargingPlaces)
                if (!chargingPlace.IsOccupiad)
                    return chargingPlace;

            return null;
        }

        public abstract List<MechanismEntity> GetRequiredMechanismsToRepair();

        public void TryToRepair()
        {
            if (_mechanismsContainer.IsSearchedMechanismsAllFound(GetRequiredMechanismsToRepair()))
            {
                IsRobotDestroyed = false;
                Health.FullRecover();
                _mechanismsContainer.GiveMechanisms(GetRequiredMechanismsToRepair());
                _inventory.UpdateInventoryMechanisms(GetRequiredMechanismsToRepair());
            }
            else
                MassageHandler.ShowMassage("You don`t have the right mechanisms");
        }

        private bool CanFunction()
        {
            if (IsRobotDestroyed)
            {
                MassageHandler.ShowMassage("Robot is destroyed");
                return false;
            }
            if (Battary.NeedToBeRecharged)
            {
                MassageHandler.ShowMassage("Robot battary is too low");
                return false;
            }
            return true;
        }
    }
}
