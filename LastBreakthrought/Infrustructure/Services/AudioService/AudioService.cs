using LastBreakthrought.Configs.Sound;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Infrustructure.Services.AudioService
{
    public class AudioService : IAudioService, IDisposable
    {
        private SoundConfigSO _soundConfig;
        private IEventBus _eventBus;

        private readonly Dictionary<MonoBehaviour, Dictionary<SoundType, AudioSource>> _loopedSources = new();
        private readonly Dictionary<MonoBehaviour, List<AudioSource>> _activeSources = new();

        private readonly float _maxVolume = 1f;
        private float _currentVolume = 1f;

        [Inject]
        private void Construct(SoundConfigSO soundConfigSO) => 
            _soundConfig = soundConfigSO;     

        public void Initialize(IEventBus eventBus)
        {
            _eventBus = eventBus;

            _eventBus.SubscribeEvent((OnGamePausedSignal signal) => PauseAllAudio());
            _eventBus.SubscribeEvent((OnGameResumedSignal signal) => ResumeAllAudio());

            _eventBus.SubscribeEvent((OnVideoPlayedSignal signal) => PauseAllAudio());
            _eventBus.SubscribeEvent((OnBeginningVideoEndedSignal signal) => ResumeAllAudio());

            _eventBus.SubscribeEvent((OnGameWonSignal signal) => PauseAllAudio());
            _eventBus.SubscribeEvent((OnGameEndedSignal signal) => PauseAllAudio());
        }

        public void PlayOnObject(SoundType soundType, MonoBehaviour target, bool loop = false, float volumeMultiplier = 1f, float maxDistance = 8f)
        {
            if (loop)
            {
                if (_loopedSources.TryGetValue(target, out var sounds) && sounds.ContainsKey(soundType))
                    return;

                var audioSource = target.gameObject.AddComponent<AudioSource>();
                var clip = _soundConfig.GetSoundByType(soundType);

                ConfigurateAudioSource(loop, volumeMultiplier, maxDistance, audioSource, clip);
                audioSource.Play();

                if (!_loopedSources.ContainsKey(target))
                    _loopedSources[target] = new Dictionary<SoundType, AudioSource>();

                _loopedSources[target][soundType] = audioSource;
            }
            else
            {
                var audioSource = GetAvailableAudioSource(target);
                if (audioSource == null)
                    audioSource = target.gameObject.AddComponent<AudioSource>();

                var clip = _soundConfig.GetSoundByType(soundType);
                ConfigurateAudioSource(loop, volumeMultiplier, maxDistance, audioSource, clip);
                audioSource.Play();

                if (!_activeSources.TryGetValue(target, out var sources))
                {
                    sources = new List<AudioSource>();
                    _activeSources.Add(target, sources);
                }
                sources.Add(audioSource);

                target.gameObject.AddComponent<AudioAutoCleanup>().Initialize(audioSource, () =>
                {
                    if (_activeSources.TryGetValue(target, out var srcList))
                    {
                        srcList.Remove(audioSource);
                        if (srcList.Count == 0)
                            _activeSources.Remove(target);
                    }
                });
            }
        }

        public void StopOnObject(MonoBehaviour target, SoundType soundType)
        {
            if (_loopedSources.TryGetValue(target, out var loopedSounds))
            {
                if (loopedSounds.TryGetValue(soundType, out var loopedSource))
                {
                    if (loopedSource != null)
                    {
                        loopedSource.Stop();
                        GameObject.Destroy(loopedSource);
                    }
                    loopedSounds.Remove(soundType);
                    if (loopedSounds.Count == 0)
                        _loopedSources.Remove(target);
                }
            }

            if (_activeSources.TryGetValue(target, out var sources))
            {
                for (int i = sources.Count - 1; i >= 0; i--)
                {
                    var source = sources[i];
                    if (source == null)
                    {
                        sources.RemoveAt(i);
                        continue;
                    }

                    if (source.clip != null && _soundConfig.GetSoundByType(soundType) == source.clip)
                    {
                        source.Stop();
                        GameObject.Destroy(source);
                        sources.RemoveAt(i);
                    }
                }

                if (sources.Count == 0)
                    _activeSources.Remove(target);
            }
        }

        public bool IsSoundPlaying(MonoBehaviour target, SoundType soundType)
        {
            if (_loopedSources.TryGetValue(target, out var sounds))
            {
                if (sounds.ContainsKey(soundType))
                    return true;
            }

            if (_activeSources.TryGetValue(target, out var sources))
            {
                foreach (var source in sources)
                {
                    if (source != null && source.isPlaying &&
                        _soundConfig.GetSoundByType(soundType) == source.clip)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void SetZeroVolume()
        {
            _currentVolume = 0f;
            UpdateVolumeListeners();
        }

        public void SetMaxVolume()
        {
            _currentVolume = _maxVolume;
            UpdateVolumeListeners();
        }

        public bool IsVolumeMax() => _currentVolume == 1f;

        private AudioSource GetAvailableAudioSource(MonoBehaviour target)
        {
            if (_activeSources.TryGetValue(target, out var sources))
            {
                foreach (var source in sources)
                {
                    if (source != null && !source.isPlaying)
                        return source;
                }
            }
            return null;
        }

        private void PauseAllAudio()
        {
            foreach (var loopedPair in _loopedSources)
            {
                foreach (var sourcePair in loopedPair.Value)
                {
                    if (sourcePair.Value != null)
                        sourcePair.Value.Pause();
                }
            }

            foreach (var sourcesPair in _activeSources)
            {
                for (int i = sourcesPair.Value.Count - 1; i >= 0; i--)
                {
                    var source = sourcesPair.Value[i];
                    if (source != null)
                    {
                        source.Stop();
                        GameObject.Destroy(source);
                    }
                    sourcesPair.Value.RemoveAt(i);
                }
            }
            _activeSources.Clear();
        }

        private void ResumeAllAudio()
        {
            foreach (var loopedPair in _loopedSources)
            {
                foreach (var sourcePair in loopedPair.Value)
                {
                    if (sourcePair.Value != null)
                        sourcePair.Value.Play();
                }
            }
        }

        private void ConfigurateAudioSource(bool loop, float volumeMultiplier, float maxDistance, AudioSource audioSource, AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.volume = _currentVolume * Mathf.Clamp01(volumeMultiplier);
            audioSource.loop = loop;
            audioSource.minDistance = 1;
            audioSource.maxDistance = maxDistance;
            audioSource.spatialBlend = 1f;
        }

        private void UpdateVolumeListeners()
        {
            foreach (var sources in _activeSources.Values)
            {
                foreach (var source in sources)
                {
                    if (source != null)
                        source.volume = _currentVolume;
                }
            }
        }

        public void Dispose()
        {
            _eventBus?.UnSubscribeEvent<OnGamePausedSignal>(PauseAllAudio);
            _eventBus?.UnSubscribeEvent<OnGameResumedSignal>(ResumeAllAudio);
            _eventBus?.UnSubscribeEvent((OnVideoPlayedSignal signal) => SetZeroVolume());
            _eventBus?.UnSubscribeEvent((OnBeginningVideoEndedSignal signal) => SetMaxVolume());
            _eventBus?.UnSubscribeEvent((OnGameWonSignal signal) => PauseAllAudio());
            _eventBus?.UnSubscribeEvent((OnGameEndedSignal signal) => PauseAllAudio());
        }
    }
}

