using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.Inventory.ShipDetail
{
    public class ShipDetailView : MonoBehaviour
    {
        [SerializeField] private Image _shipMaterialImage;
        [SerializeField] private TextMeshProUGUI _quantityText;

        public void SetImage(Sprite sprite) =>
            _shipMaterialImage.sprite = sprite;
        public void SetQuantity(int quantity) =>
            _quantityText.text = quantity.ToString();
    }
}
