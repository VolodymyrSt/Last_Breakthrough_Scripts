using LastBreakthrought.Configs.Player;
using LastBreakthrought.Infrustructure.Services.EventBus;
using UnityEngine;

namespace LastBreakthrought.UI.PlayerStats
{
    public class PlayerStatsModel
    {
        private readonly PlayerConfigSO _playerConfig;
        private readonly PlayerStatsView _playerStatsView;
        private readonly IEventBus _eventBus;

        public float CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
        public float CurrentOxygen { get { return _currentOxygen; } set { _currentOxygen = value; } }

        private float _currentHealth;
        private float _currentOxygen;

        public PlayerStatsModel(PlayerConfigSO playerConfig, PlayerStatsView playerStatsView, IEventBus eventBus) 
        {
            _currentHealth = playerConfig.StartedHealth;
            _currentOxygen = playerConfig.StartedOxygen;
            _playerConfig = playerConfig;
            _playerStatsView = playerStatsView;

            _eventBus = eventBus;

            playerStatsView.SetHealthSliderMaxValueAndStartedValue(playerConfig.MaxHealth, playerConfig.StartedHealth);
            playerStatsView.SetOxygeSliderMaxValueAndStartedValue(playerConfig.MaxOxygen, playerConfig.StartedOxygen);
        }

        public void IncreaseOxygen(float value) =>
            CurrentOxygen = Mathf.Min(CurrentOxygen + value * Time.deltaTime, _playerConfig.MaxOxygen);
        public void IncreaseHealth() =>
            CurrentHealth = Mathf.Min(CurrentHealth + _playerConfig.HealthRegeneration * Time.deltaTime, _playerConfig.MaxHealth);
        public void DecreaseHealth() =>
            CurrentHealth = Mathf.Min(CurrentHealth - _playerConfig.HealthReductionIndex * Time.deltaTime, _playerConfig.MaxHealth);
        public void DecreaseOxygen() =>
            CurrentOxygen = Mathf.Min(CurrentOxygen - _playerConfig.OxygenSuppletion * Time.deltaTime, _playerConfig.MaxOxygen);

        public bool IsRunOutOfHealth() => CurrentHealth <= 0;
        public bool CanRegenerate() => CurrentHealth < _playerConfig.MaxHealth;

        public void UpdateHealth() => 
            _playerStatsView.SetHealthSliderValue(CurrentHealth);

        public void UpdateOxygen()
        {
            _playerStatsView.SetOxygeSliderValue(CurrentOxygen);

            if (CurrentOxygen <= 0)
                _playerStatsView.ShowWarningSigh();
            else
                _playerStatsView.HideWarningSigh();
        }
    }
}
