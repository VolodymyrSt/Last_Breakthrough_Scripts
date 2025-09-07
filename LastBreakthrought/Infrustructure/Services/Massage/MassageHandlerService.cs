using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Logic.Camera;
using LastBreakthrought.Player;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Infrustructure.Services.Massage
{
    public class MassageHandlerService : IMassageHandlerService
    {
        private MassageView _massageView;
        private IAudioService _audioService;
        private FollowCamera _followCamera;

        [Inject]
        private void Construct(MassageView massageView, IAudioService audioService, FollowCamera followCamera)
        {
            _massageView = massageView;
            _audioService = audioService;
            _followCamera = followCamera;
        }

        public void ShowMassage(string massage)
        {
            _massageView.Show(massage);
            _audioService.PlayOnObject(Configs.Sound.SoundType.WarningMassage, _followCamera);
        }
    }
}
