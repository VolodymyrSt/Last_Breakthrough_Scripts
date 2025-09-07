using UnityEngine;

namespace LastBreakthrought.Configs.Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game")]
    public class GameConfigSO : ScriptableObject
    {
        [field: SerializeField, Range(0, 4)] public int StartedDay { get; private set; }
        [field: SerializeField, Range(0, 23)] public int StartedMinute { get; private set; }
        [field: SerializeField, Range(0, 59)] public int StartedSecond { get; private set; }


        [field: SerializeField, Range(0f, 10f)] public float OxygenIncreasingIndex { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float CurrentLightIntensity { get; private set; }
    }
}
