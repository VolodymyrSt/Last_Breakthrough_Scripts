using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.Configs.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueConfig", menuName = "Configs/Dialogue")]
    public class DialogueConfigSO : ScriptableObject
    {
        [field: SerializeField] public List<string> DialogueLines { get; private set; }
        [field: SerializeField, Range(0, 1f)] public float LineWaitTime { get; private set; }
    }
}
