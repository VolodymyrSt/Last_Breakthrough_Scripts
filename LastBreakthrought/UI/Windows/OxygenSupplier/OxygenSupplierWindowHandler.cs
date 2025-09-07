using DG.Tweening;
using LastBreakthrought.Infrustructure.Services.AudioService;
using LastBreakthrought.Infrustructure.Services.ConfigProvider;
using LastBreakthrought.Logic.OxygenSuppIier;
using LastBreakthrought.UI.PlayerStats;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI.Windows.OxygenSupplier
{
    public class OxygenSupplierWindowHandler : WindowHandler<OxygenSupplierWindowView>
    {
        [SerializeField] private OxygenSupplierHandler _oxygenSupplier;

        private PlayerStatsHandler _playerStats;
        private PlayerStatsModel _playerStatsModel;
        private IConfigProviderService _configProviderService;

        [Inject]
        private void Construct(PlayerStatsHandler playerStats, PlayerStatsModel playerStatsModel, IConfigProviderService configProviderService, IAudioService audioService)
        {
            _playerStats = playerStats;
            _playerStatsModel = playerStatsModel;
            _configProviderService = configProviderService;
        }

        private void Update()
        {
            if (_playerStats.IsOxygenCharging)
                _playerStatsModel.IncreaseOxygen(_configProviderService.GameConfigSO.OxygenIncreasingIndex);
        }

        public override void ActivateWindow() => View.ShowView();

        public override void DeactivateWindow()
        {
            _playerStats.IsOxygenCharging = false;
            _oxygenSupplier.StopOxygenSupplierSound();
            View.ProccesingAnimation.Rewind();
            View.HideView();
        }

        public override void UseDevice()
        {
            _playerStats.IsOxygenCharging = true;
            _oxygenSupplier.PlayOxygenSupplierSound();
        }
    }
}
