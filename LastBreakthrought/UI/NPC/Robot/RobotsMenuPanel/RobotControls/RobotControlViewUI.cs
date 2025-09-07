using LastBreakthrought.Configs.Robot;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.NPC.Robot.RobotsMenuPanel.RobotControls
{
    public class RobotControlViewUI : MonoBehaviour
    {
        [Header("UI:")]
        [SerializeField] private Slider _battarySlider;
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Button _followButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _doWorkButton;

        public void Init(RobotConfigSO RobotData, Action followAction, Action goHomeAction, Action doWorkAction)
        {
            var maxBattaryCapacity = RobotData.MaxBattaryCapacity;
            _battarySlider.maxValue = maxBattaryCapacity;
            _battarySlider.value = maxBattaryCapacity;

            var maxHealth = RobotData.MaxHealth;
            _healthSlider.maxValue = maxHealth;
            _healthSlider.value = maxHealth;

            _followButton.onClick.AddListener(() => {
                followAction?.Invoke();
            });

            _homeButton.onClick.AddListener(() => {
                goHomeAction?.Invoke();
            });

            _doWorkButton.onClick.AddListener(() => {
                doWorkAction?.Invoke();
            });
        }

        public void SetBattarySliderValue(float value) => 
            _battarySlider.value = value;

        public void SetHealthSliderValue(float value) =>
            _healthSlider.value = value;
    }
}
