using LastBreakthrought.Logic.ShipDetail;
using System;
using UnityEngine;

namespace LastBreakthrought.Logic.ShipMaterial.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Material", menuName = "MaterialForShip")]
    public class ShipMaterialSO : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ShipDetailSO CraftDetail { get; private set; }
        [field: SerializeField, Range(2, 10)] public int MaxQuantity { get; private set; }
    }

    [Serializable]
    public class ShipMaterialEntity
    {
        public ShipMaterialSO Data { get; }
        public int Quantity { get; set; }

        public ShipMaterialEntity(ShipMaterialSO data, int quantity)
        {
            Data = data;
            Quantity = quantity;
        }
    }
}
