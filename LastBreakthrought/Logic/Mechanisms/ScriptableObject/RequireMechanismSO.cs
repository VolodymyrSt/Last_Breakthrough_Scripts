using LastBreakthrought.Logic.ShipDetail;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.Logic.Mechanisms
{
    [CreateAssetMenu(fileName = "New RequireMechanismForSmth", menuName = "RequireMechanismForSmth")]
    public class RequireMechanismSO : ScriptableObject
    {
        public List<RequireMechanismEntity> NeededMechanisms = new();

        public List<MechanismEntity> GetRequiredShipDetails()
        {
            List<MechanismEntity> neededMechanisms = new();

            foreach (var mechanism in NeededMechanisms)
                neededMechanisms.Add(new MechanismEntity(mechanism.Data, mechanism.Quantity));

            return neededMechanisms;
        }
    }

    [Serializable]
    public struct RequireMechanismEntity
    {
        public MechanismSO Data;
        public int Quantity;
    }
}

