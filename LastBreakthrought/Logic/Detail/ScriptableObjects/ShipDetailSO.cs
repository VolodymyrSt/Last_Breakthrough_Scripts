using System;
using UnityEngine;

namespace LastBreakthrought.Logic.ShipDetail
{
    [CreateAssetMenu(fileName = "New Detail", menuName = "DetailForShip")]
    public class ShipDetailSO : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }

    [Serializable]
    public class ShipDetailEntity
    {
        public ShipDetailSO Data { get; }
        public int Quantity { get; set; }

        public ShipDetailEntity(ShipDetailSO data, int quantity)
        {
            Data = data;
            Quantity = quantity;
        }
    }
}
