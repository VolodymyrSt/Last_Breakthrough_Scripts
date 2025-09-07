using System;
using UnityEngine;

namespace LastBreakthrought.Configs.Robot
{
    [CreateAssetMenu(fileName = "RobotConfig", menuName = "Configs/Robot")]
    public class RobotConfigSO : ScriptableObject
    {
        [field: SerializeField, Range(0, 5)] public float WandaringSpeed { get; private set; }
        [field: SerializeField, Range(0, 5)] public float GeneralSpeed { get; private set; }
        [field: SerializeField, Range(0, 500)] public float MaxBattaryCapacity { get; private set; }
        [field: SerializeField, Range(0, 10)] public float CapacityLimit { get; private set; }
        [field: SerializeField, Range(1, 100)] public float MaxHealth { get; private set; }
    }
}

