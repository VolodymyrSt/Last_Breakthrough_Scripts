using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.NPC.Enemy
{
    public class EnemyHealthView : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;

        public void InitSlider(float maxHealth) =>
            _healthSlider.maxValue = maxHealth;

        public void UpdateHealthView(float currentHealth) =>
            _healthSlider.value = currentHealth;
    }
}
