using System;
using UnityEngine;

namespace LastBreakthrought.Infrustructure.Services.AudioService
{
    public class AudioAutoCleanup : MonoBehaviour
    {
        private AudioSource _source;
        private Action _onComplete;

        public void Initialize(AudioSource source, Action onComplete)
        {
            _source = source;
            _onComplete = onComplete;
        }

        private void Update()
        {
            if (_source == null)
            {
                Cleanup();
                return;
            }

            if (!_source.isPlaying && _source.time >= _source.clip.length - 0.1f)
                Cleanup();
        }

        private void Cleanup()
        {
            _onComplete?.Invoke();
            if (this != null)
                Destroy(this);
        }

        private void OnDestroy()
        {
            _source = null;
            _onComplete = null;
        }
    }
}

