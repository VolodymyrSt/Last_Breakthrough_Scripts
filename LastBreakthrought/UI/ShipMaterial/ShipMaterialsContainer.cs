using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.UI.ShipMaterial
{
    public class ShipMaterialsContainer : MonoBehaviour
    {
        public List<ShipMaterialHandler> Materials { get; set; } = new();
    }
}
