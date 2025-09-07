using LastBreakthrought.Configs.Robot;
using LastBreakthrought.NPC.Robot;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.NPC.Robot.RobotsMenuPanel.RobotControls
{
    public class RobotControlHandlerUI : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private RobotControlViewUI _view;
        [SerializeField] private TextMeshProUGUI _distanceText;

        private IRobot _robot;
        private RobotBattary _robotBattary;
        private RobotHealth _robotHealth;

        public void Init(IRobot robot, RobotConfigSO robotData, RobotBattary battary, RobotHealth robotHealth, Action followAction, Action goHomeAction, Action doWorkAction)
        {
            _view.Init(robotData, followAction, goHomeAction, doWorkAction);
            _robot = robot;
            _robotBattary = battary;
            _robotHealth = robotHealth;

            _robotHealth.OnHealthChanged += UpdateHealthSlider;
        }

        private void UpdateHealthSlider(float obj) => 
            _view.SetHealthSliderValue(obj);

        public void UpdateSlider() =>
            _view.SetBattarySliderValue(_robotBattary.Capacity);
        public void UpdateDistanceToPlayer() =>
            _distanceText.text = _robot.GetRobotDistanceToPlayer().ToString() + "m";

        private void OnDestroy() => 
            _robotHealth.OnHealthChanged -= UpdateHealthSlider;
    }
}
