using DG.Tweening;
using LastBreakthrought.Logic.ShipMaterial.ScriptableObjects;
using LastBreakthrought.UI.ToolTip;
using UnityEngine;

namespace LastBreakthrought.UI.ShipMaterial
{
    public class ShipMaterialHandler : ToolTipTrigger
    {
        [SerializeField] private ShipMaterialView _shipMaterialView;

        public int Quantity { get { return _quantity; } set { ChangeQuantity(value); }}
        public ShipMaterialEntity MaterialEntity { get; private set; }

        private int _quantity;

        public void Init(ShipMaterialEntity materialEntity)
        {
            Quantity = materialEntity.Quantity;
            MaterialEntity = materialEntity;

            _shipMaterialView.SetQuantity(Quantity);
            _shipMaterialView.SetImage(materialEntity.Data.Sprite);

            ConfigureToolTip(MaterialEntity.Data.Name, MaterialEntity.Data.Description, ToolTipPosition.BottomLeft);
        }
        
        public void InitMined(ShipMaterialEntity materialEntity)
        {
            MaterialEntity = materialEntity;
            _shipMaterialView.SetImage(materialEntity.Data.Sprite);

            ConfigureToolTip(MaterialEntity.Data.Name, MaterialEntity.Data.Description, ToolTipPosition.BottomLeft);
        }

        private void ChangeQuantity(int value)
        {
            _quantity = value;
            _shipMaterialView.SetQuantity(_quantity);
        }
    }
}
