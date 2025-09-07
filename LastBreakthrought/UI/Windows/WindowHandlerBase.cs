using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Logic.Camera;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI.Windows 
{
    public abstract class WindowHandlerBase : MonoBehaviour
    {
        private IAudioService _audioService;
        private FollowCamera _followCamera;

        [Inject]
        private void Construct(IAudioService audioService, FollowCamera followCamera)
        {
            _audioService = audioService;
            _followCamera = followCamera;
        }
        public virtual void ActivateWindow() => 
            _audioService.PlayOnObject(Configs.Sound.SoundType.WindowOpen, _followCamera);

        public abstract void DeactivateWindow();
        public virtual void UseDevice() { }
    }
}
