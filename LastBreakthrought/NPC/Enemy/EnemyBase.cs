using LastBreakthrought.Configs.Enemy;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.ConfigProvider;
using LastBreakthrought.NPC.Base;
using LastBreakthrought.Player;
using LastBreakthrought.UI.NPC.Enemy;
using LastBreakthrought.Util;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace LastBreakthrought.NPC.Enemy
{
    public abstract class EnemyBase : MonoBehaviour, IEnemy, IDamagable
    {
        [Header("Base:")]
        [SerializeField] protected NavMeshAgent Agent;
        [SerializeField] protected Animator Animator;
        [SerializeField] private EnemyAttackIndecator _attackIndecator;
        [SerializeField] private EnemyHealthHandler _healthHandler;

        [Header("Setting:")]
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _attackUpOffset = 1f;
        [SerializeField] private float _attackForwardOffset = 1f;

        public IEnemyTarget Target { get; private set; }

        private readonly Collider[] _attackingTargetColliders = new Collider[2];
        private readonly Collider[] _targetColliders = new Collider[2];

        private float _visionRadious;
        private float _attakingRadious;

        protected bool IsTargetInVisionRange = false;
        protected bool IsTargetInAttakingRange = false;
        protected bool IsDied = false;

        protected NPCStateMachine StateMachine;
        protected PlayerHandler PlayerHandler;
        protected ICoroutineRunner CoroutineRunner;
        protected BoxCollider WanderingZone;
        protected IConfigProviderService ConfigProvider;
        protected IAudioService AudioService;
        protected EnemyConfigSO EnemyData;

        [Inject]
        private void Construct(PlayerHandler playerHandler, ICoroutineRunner coroutineRunner
            , IConfigProviderService configProviderService, IAudioService audioService)
        {
            PlayerHandler = playerHandler;
            CoroutineRunner = coroutineRunner;
            ConfigProvider = configProviderService;
            AudioService = audioService;
        }

        public void OnSpawned(BoxCollider wanderingZone, string id)
        {
            WanderingZone = wanderingZone;

            EnemyData = ConfigProvider.EnemyConfigHolderSO.GetEnemyDataById(id);
            ConfigurateEnemy(EnemyData);

            _healthHandler.OnHealthGone += Died;

            StateMachine = new NPCStateMachine();

            AddStates();
        }

        public Vector3 GetPosition() =>
            transform.position;

        public void ApplyDamage(float damage) => 
            _healthHandler.Health -= damage;

        private void Update() => StateMachine.Tick();

        public bool TryToFindTarget()
        {
            Physics.OverlapSphereNonAlloc(transform.position + Vector3.up, _visionRadious, _targetColliders, _layerMask);

            if (_targetColliders.Length > 0)
            {
                for (int i = 0; i < _targetColliders.Length; i++)
                {
                    if (_targetColliders[i] != null)
                    {
                        if (_targetColliders[i].TryGetComponent(out IEnemyTarget enemyTarget))
                        {
                            Target = enemyTarget;
                            _targetColliders[i] = null;
                            IsTargetInVisionRange = true;
                            break;
                        }
                    }
                    else
                        IsTargetInVisionRange = false;
                }
            }
            else
            {
                IsTargetInVisionRange = false;
                Target = null;
            }

            return IsTargetInVisionRange;
        }
        
        public bool TryToAttackTarget()
        {
            Vector3 position = transform.position + transform.forward * _attackForwardOffset + Vector3.up * _attackUpOffset;
            var targets = Physics.OverlapSphereNonAlloc(position, _attakingRadious, _attackingTargetColliders, _layerMask);

            if (targets > 0)
                IsTargetInAttakingRange = true;
            else
                IsTargetInAttakingRange = false;

            return IsTargetInAttakingRange;
        }

        public bool IsEnemyDied() => IsDied;

        public abstract void AddStates();

        private void ConfigurateEnemy(EnemyConfigSO enemyData)
        {
            _visionRadious = enemyData.VitionRadious;
            _attakingRadious = enemyData.AttakingRadious;

            _attackIndecator.Init(enemyData.AttackDamage, AudioService);
            _healthHandler.Init(enemyData.MaxHealth);
        }

        private void Died() => IsDied = true;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + Vector3.up, _visionRadious);

            Vector3 position = transform.position + transform.forward * _attackForwardOffset + Vector3.up * _attackUpOffset;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position * 1f, _attakingRadious);
        }
    }
}
