using LastBreakthrought.UI.Inventory.ShipDetail;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.LastBreakthrought.UI.Inventory.ShipDetail
{
    public class DetailContainerUI : MonoBehaviour
    {
        public List<ShipDetailHandler> Details { get; set; } = new();
    }
}
