using System;

namespace LastBreakthrought.NPC.Robot
{
    public class RobotHealth
    {
        public event Action<float> OnHealthChanged;

        private readonly float _maxHealth;
        private float _currentHealth;

        public RobotHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
                OnHealthChanged?.Invoke(0);
            else
                OnHealthChanged?.Invoke(_currentHealth);
        }

        public bool IsHealthGone() =>
            _currentHealth <= 0;

        public void FullRecover()
        {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(_currentHealth);
        }
    }
}
