using System;
using UnityEngine;

namespace LastBreakthrought.UI.NPC.Enemy
{
    public class EnemyHealthHandler : MonoBehaviour 
    {
        public event Action OnHealthGone;

        [SerializeField] private EnemyHealthView _enemyHealthView;
        private float _maxHealth;
        private float _currentHealth;

        public float Health
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                _enemyHealthView.UpdateHealthView(_currentHealth);

                if (_currentHealth <= 0)
                    OnHealthGone?.Invoke();
            }
        }

        public void Init(float maxHealth)
        {
            _maxHealth = maxHealth;
            _enemyHealthView.InitSlider(maxHealth);
            Health = _maxHealth;
        }
    }
}
