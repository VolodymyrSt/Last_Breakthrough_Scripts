using LastBreakthrought.Logic.Mechanisms;
using LastBreakthrought.UI.ToolTip;
using UnityEngine;

namespace LastBreakthrought.UI.Inventory.Mechanism
{
    public class MechanismHandler : ToolTipTrigger
    {
        [SerializeField] private MechanismView _mechanismView;

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                _mechanismView.SetQuantity(_quantity);
            }
        }

        public MechanismEntity MechanismEntity { get; private set; }

        public void Init(MechanismEntity mechanismEntity)
        {
            Quantity = mechanismEntity.Quantity;
            MechanismEntity = mechanismEntity;

            _mechanismView.SetQuantity(Quantity);
            _mechanismView.SetImage(mechanismEntity.Data.Sprite);

            ConfigureToolTip(MechanismEntity.Data.Name, MechanismEntity.Data.Description, ToolTipPosition.BottomRight);
        }

        public void UpdateView(MechanismEntity mechanismEntity)
        {
            MechanismEntity = mechanismEntity;

            _mechanismView.SetImage(mechanismEntity.Data.Sprite);

            ConfigureToolTip(MechanismEntity.Data.Name, MechanismEntity.Data.Description, ToolTipPosition.BottomRight);
        }

        public void SelfDesctroy() =>
            Destroy(gameObject);
    }
}
