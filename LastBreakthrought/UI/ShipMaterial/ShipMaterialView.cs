using DG.Tweening;
using LastBreakthrought.UI.ToolTip;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LastBreakthrought.UI.ShipMaterial
{
    public class ShipMaterialView : MonoBehaviour
    {
        [Header("Base")]
        [SerializeField] private Image _shipMaterialImage;
        [SerializeField] private TextMeshProUGUI _quantityText;

        public void SetImage(Sprite sprite) => 
            _shipMaterialImage.sprite = sprite;
        public void SetQuantity(int quantity) => 
            _quantityText.text = quantity.ToString();
    }
}
