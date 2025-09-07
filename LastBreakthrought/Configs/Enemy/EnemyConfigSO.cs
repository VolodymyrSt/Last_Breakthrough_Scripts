using UnityEngine;

namespace LastBreakthrought.Configs.Enemy 
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy")]
    public class EnemyConfigSO : ScriptableObject
    {
        [field: SerializeField, Range(0, 5)] public float WandaringSpeed { get; private set; }
        [field: SerializeField, Range(0, 5)] public float ChassingSpeed { get; private set; }
        [field: SerializeField, Range(0, 100)] public float AttackDamage { get; private set; }
        [field: SerializeField, Range(0, 150)] public float MaxHealth { get; private set; }


        [field: SerializeField, Range(0, 10), Space(20f)] public float VitionRadious { get; private set; }
        [field: SerializeField, Range(0, 5)] public float AttakingRadious { get; private set; }

        [field: SerializeField, Range(0, 5), Space(20f)] public float AttakAnimationTime{ get; private set; }
        [field: SerializeField, Range(0, 5)] public float DyingAnimationTime { get; private set; }
    }
}

