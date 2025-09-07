using LastBreakthrought.Logic.ShipMaterial.ScriptableObjects;
using LastBreakthrought.Logic.ShipMaterial;
using TMPro;
using UnityEngine;

namespace LastBreakthrought.UI.Other.Marker
{
    public class CrashedShipMarkerViewUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _distance;
        [SerializeField] private RectTransform _materialsContainer;

        public void SetDistanceText(string distance) => 
            _distance.text = distance + "m";

        public RectTransform GetContainer() =>
            _materialsContainer;
    }
}
