using LastBreakthrought.Logic.ShipDetail;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.Logic.Mechanisms
{
    [CreateAssetMenu(fileName = "New MechanismHolder", menuName = "MechanismHolder")]
    public class MechanismHolderSO : ScriptableObject
    {
        public List<MechanismSO> Mechanisms = new();
    }
}

