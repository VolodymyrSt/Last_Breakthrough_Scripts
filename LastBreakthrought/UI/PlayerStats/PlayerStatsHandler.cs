using LastBreakthrought.Configs.Player;
using LastBreakthrought.Infrustructure.Services.EventBus;
using LastBreakthrought.Infrustructure.Services.EventBus.Signals;
using LastBreakthrought.Player;
using System;
using UnityEngine;
using Zenject;

namespace LastBreakthrought.UI.PlayerStats
{
    public class PlayerStatsHandler : ITickable, IDisposable, IInitializable
    {
        private readonly PlayerStatsModel _playerStatsModel;
        private readonly PlayerHandler _playerHandler;
        private PlayerConfigSO _playerConfig;
        private readonly IEventBus _eventBus;

        private bool _isGameOver = false;
        private bool _isOxygenCharging = false;

        public bool IsOxygenCharging {  get { return _isOxygenCharging; } set { _isOxygenCharging = value; } }

        public PlayerStatsHandler(PlayerStatsModel statsModel, IEventBus eventBus, PlayerConfigSO playerConfig, PlayerHandler playerHandler)
        {
            _playerStatsModel = statsModel;
            _playerHandler = playerHandler;
            _playerConfig = playerConfig;
            _eventBus = eventBus;
        }

        public void Initialize() => 
            _playerHandler.OnPlayerBeenAttacked += UpdateStatsAfterAttack;

        public void Tick()
        {
            if (_isGameOver) return;

            UpdateStats();
            CheckForEnd();
        }

        private void UpdateStats()
        {
            if (_isOxygenCharging)
            {
                _playerStatsModel.UpdateOxygen();
                RegenerateHealth();
                return;
            }

            if (_playerStatsModel.CurrentOxygen > 0)
            {
                _playerStatsModel.DecreaseOxygen();
                _playerStatsModel.UpdateOxygen();

                RegenerateHealth();
            }
            else
            {
                _playerStatsModel.DecreaseHealth();
                _playerStatsModel.UpdateHealth();
            }
        }

        private void CheckForEnd()
        {
            if (_playerStatsModel.IsRunOutOfHealth())
            {
                _isGameOver = true;
                _eventBus.Invoke(new OnPlayerDiedSignal());
            }
        }

        private void RegenerateHealth()
        {
            if (!_playerStatsModel.CanRegenerate()) return;

            _playerStatsModel.IncreaseHealth();

            _playerStatsModel.UpdateHealth();
        }

        private void UpdateStatsAfterAttack(float arg1)
        {
            _playerStatsModel.CurrentHealth = Mathf.Max(_playerStatsModel.CurrentHealth - arg1, 0);

            _playerStatsModel.UpdateHealth();
            CheckForEnd();
        }

        public void Dispose() => 
            _playerHandler.OnPlayerBeenAttacked -= UpdateStatsAfterAttack;
    }
}

