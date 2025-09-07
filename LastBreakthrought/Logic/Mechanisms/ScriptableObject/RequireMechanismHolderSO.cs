using UnityEngine;

namespace LastBreakthrought.Logic.Mechanisms
{
    [CreateAssetMenu(fileName = "New RequireMechanismHolder", menuName = "RequireMechanismHolder")]
    public class RequireMechanismHolderSO : ScriptableObject
    {
        public RequireMechanismSO CreateRobotMiner;
        public RequireMechanismSO CreateRobotTransporter;
        public RequireMechanismSO CreateRobotDefender;
        public RequireMechanismSO RepairRocket;
        public RequireMechanismSO RepairRobotMiner;
        public RequireMechanismSO RepairRobotTransporter;
        public RequireMechanismSO RepairRobotDefender;
    }
}

