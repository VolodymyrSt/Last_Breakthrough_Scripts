using LastBreakthrought.Logic.ShipDetail;
using NUnit.Framework;
using System;
using UnityEngine;

namespace LastBreakthrought.Logic.Mechanisms
{
    [CreateAssetMenu(fileName = "New Mechanism", menuName = "Mechanism")]
    public class MechanismSO : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public RequireDetailsSO RequireDetails { get; private set; }
    }

    [Serializable]
    public class MechanismEntity
    {
        public MechanismSO Data { get; }
        public int Quantity { get; set; }

        public MechanismEntity(MechanismSO data, int quantity)
        {
            Data = data;
            Quantity = quantity;
        }
    }
}

