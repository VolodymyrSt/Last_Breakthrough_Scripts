using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Player;
using UnityEngine;

namespace LastBreakthrought.NPC.Enemy
{
    public class EnemyAttackIndecator : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private float _attackRadious = 1f;
        [SerializeField] private LayerMask _layerMask;

        [Header("Setting")]
        [SerializeField] private float _upOffset = 1f;
        [SerializeField] private float _forwardOffset = 1f;

        private Collider[] _attackedTargets = new Collider[3];
        private IAudioService _audioService;
        private float _attackDamage;

        public void Init(float attackDamage, IAudioService audioService)
        {
            _attackDamage = attackDamage;
            _audioService = audioService;
        }

        public void Attack() 
        {
            Vector3 position = transform.position + transform.forward * _forwardOffset + Vector3.up * _upOffset;
            Physics.OverlapSphereNonAlloc(position, _attackRadious, _attackedTargets, _layerMask);

            if (_attackedTargets.Length > 0)
            {
                for (int i = 0; i < _attackedTargets.Length; i++)
                {
                    if (_attackedTargets[i] != null)
                    {
                        if (_attackedTargets[i].TryGetComponent(out IDamagable damagable))
                        {
                            damagable.ApplyDamage(_attackDamage);
                            _attackedTargets[i] = null;
                        }
                    }
                }
            }
        }

        public void PlayGolemAttackSound() =>
            _audioService.PlayOnObject(Configs.Sound.SoundType.GolemAttack, this);
        
        public void PlayBatAttackSound() =>
            _audioService.PlayOnObject(Configs.Sound.SoundType.BatAttack, this);

        private void OnDrawGizmosSelected()
        {
            Vector3 position = transform.position + transform.forward * _forwardOffset + Vector3.up * _upOffset;
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(position, _attackRadious);
        }
    }
}
