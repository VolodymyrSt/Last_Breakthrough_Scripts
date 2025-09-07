using LastBreakthrought.Logic.ShipDetail;
using LastBreakthrought.UI.ToolTip;
using UnityEngine;

namespace LastBreakthrought.UI.Inventory.ShipDetail
{
    public class ShipDetailHandler : ToolTipTrigger
    {
        [SerializeField] private ShipDetailView _shipDetailView;

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                _shipDetailView.SetQuantity(_quantity);
            }
        }

        public ShipDetailEntity DetailEntity { get; private set; }

        public void Init(ShipDetailEntity shipDetailEntity)
        {
            Quantity = shipDetailEntity.Quantity;
            DetailEntity = shipDetailEntity;

            _shipDetailView.SetQuantity(Quantity);
            _shipDetailView.SetImage(shipDetailEntity.Data.Sprite);

            ConfigureToolTip(DetailEntity.Data.Name, DetailEntity.Data.Description, ToolTipPosition.BottomRight);
        }

        public void UpdateView(ShipDetailEntity shipDetail)
        {
            DetailEntity = shipDetail;
            _shipDetailView.SetImage(shipDetail.Data.Sprite);

            ConfigureToolTip(DetailEntity.Data.Name, DetailEntity.Data.Description, ToolTipPosition.BottomRight);
        }

        public void SelfDesctroy() =>
            Destroy(gameObject);
    }
}
