using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace LastBreakthrought.Configs.Sound
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Configs/Audio")]
    public class SoundConfigSO : ScriptableObject
    {
        public List<Sound> Sounds = new();

        private void OnValidate()
        {
            HashSet<SoundType> seenTypes = new();
            foreach (var sound in Sounds)
            {
                if (seenTypes.Contains(sound.SoundType))
                    Debug.LogWarning($"Duplicate sound type found in editor: {sound.SoundType}");

                seenTypes.Add(sound.SoundType);
            }
        }

        public AudioClip GetSoundByType(SoundType soundType)
        {
            foreach (var sound in Sounds)
            {
                if (soundType == sound.SoundType)
                    return sound.AudioClip;
            }

            throw new Exception($"The sound with soundType {soundType} was not foound");
        }
    }

    [Serializable]
    public struct Sound
    {
        public SoundType SoundType;
        public AudioClip AudioClip;
    }

    public enum SoundType
    {
        PlayerMoving, 
        PlayerUseDetector,

        RobotMoving,
        DefenderAttack,
        TransporterTransporting,
        MinerMining,
        RobotFollowing,
        RobotDestroyed,
        RobotFactoryCreating,
        WindTurbinSound,
        OxygenSupplierSound,
        RecyclerSound,
        CraftSound,
        WindowOpen,
        WarningMassage,
        PanelOpen,
        Selected,
        RobotTalking,

        GolemChassing,
        BatChassing,
        GolemAttack,
        BatAttack,
        None
    }
}
