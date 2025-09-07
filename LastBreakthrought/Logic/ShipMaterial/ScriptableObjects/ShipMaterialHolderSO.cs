using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.Logic.ShipMaterial.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New MaterialHolder", menuName = "MaterialHolder")]
    public class ShipMaterialHolderSO : ScriptableObject 
    {
        public List<ShipMaterialSO> AllMaterials = new();
    }
}
