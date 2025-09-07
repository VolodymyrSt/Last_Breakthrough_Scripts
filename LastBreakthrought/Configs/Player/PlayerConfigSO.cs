using UnityEngine;

namespace LastBreakthrought.Configs.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player")]
    public class PlayerConfigSO : ScriptableObject
    {
        [field: SerializeField, Range(0, 100)] public float MoveSpeed { get; private set; }
        [field: SerializeField, Range(0, 100)] public float RotationSpeed { get; private set; }
        [field: SerializeField, Range(0, 100)] public float GravityMultiplier { get; private set; }
        [field: SerializeField, Range(0, 100)] public float Acceleration { get; private set; }
        [field: SerializeField, Range(0, 100)] public float Deceleration { get; private set; }

        [field: SerializeField, Range(0, 100), Space(20)] public float StartedHealth { get; private set; }
        [field: SerializeField, Range(0, 100)] public float StartedOxygen { get; private set; }
        [field: SerializeField, Range(0, 100)] public float MaxHealth{ get; private set; }
        [field: SerializeField, Range(0, 100)] public float MaxOxygen { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float HealthRegeneration { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float OxygenSuppletion { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float HealthReductionIndex { get; private set; }
    }

}