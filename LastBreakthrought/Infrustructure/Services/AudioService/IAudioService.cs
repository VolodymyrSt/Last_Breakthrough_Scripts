using LastBreakthrought.Configs.Sound;
using LastBreakthrought.Infrustructure.Services.EventBus;
using System.Collections;
using UnityEngine;

namespace LastBreakthrought.Infrustructure.Services.AudioService
{
    public interface IAudioService
    {
        void PlayOnObject(SoundType soundType, MonoBehaviour target, bool loop = false, float volumeMultiplier = 1f, float maxDistance = 8f);
        void StopOnObject(MonoBehaviour target, SoundType soundType);
        void SetZeroVolume();
        bool IsSoundPlaying(MonoBehaviour target, SoundType soundType);
        void SetMaxVolume();
        bool IsVolumeMax();
        void Initialize(IEventBus eventBus);
    }
}