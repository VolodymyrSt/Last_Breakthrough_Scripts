using LastBreakthrought.Configs.Game;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Logic.InteractionZone;
using LastBreakthrought.UI.PlayerStats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.Logic.OxygenSuppIier
{
    public class OxygenSupplierHandler : MonoBehaviour
    {
        private IAudioService _audioService;

        [Inject]
        private void Construct(IAudioService audioService) => 
            _audioService = audioService;

        private void OnEnable() =>
            GetComponentInChildren<InteractionZoneHandler>().Init();

        public void PlayOxygenSupplierSound() =>
            _audioService.PlayOnObject(Configs.Sound.SoundType.OxygenSupplierSound, this, true);

        public void StopOxygenSupplierSound() =>
            _audioService.StopOnObject(this, Configs.Sound.SoundType.OxygenSupplierSound);
    }
}
