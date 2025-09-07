using System;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.Configs.Robot
{
    [CreateAssetMenu(fileName = "RobotConfigHolder", menuName = "Configs/RobotHolder")]
    public class RobotConfigHolderSO : ScriptableObject
    {
        public List<RobotType> RobotConfigs = new();

        public RobotConfigSO GetRobotDataById(string robotId)
        {
            foreach (var robot in RobotConfigs)
            {
                if (robot.Id == robotId)
                    return robot.RobotData;
            }

            throw new Exception($"The RobotConfig with id: {robotId} was not found");
        }
    }

    [Serializable]
    public struct RobotType
    {
        public string Id;
        public RobotConfigSO RobotData;
    }
}

