using System.Collections.Generic;
using UnityEngine;

namespace LastBreakthrought.UI.Inventory.Mechanism
{
    public class MechanismsContainerUI : MonoBehaviour
    {
        public List<MechanismHandler> Mechanisms { get; set; } = new();
    }
}
