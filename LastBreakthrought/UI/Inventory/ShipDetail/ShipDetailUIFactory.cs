using LastBreakthrought.Factory;
using LastBreakthrought.Infrustructure.AssetManagment;
using LastBreakthrought.UI.Inventory.ShipDetail;
using UnityEngine;

namespace Assets.LastBreakthrought.UI.Inventory.ShipDetail
{
    public class ShipDetailUIFactory : BaseFactory<ShipDetailHandler>
    {
        public ShipDetailUIFactory(IAssetProvider assetProvider) : base(assetProvider) { }

        public override ShipDetailHandler Create(Vector3 at, Transform parent) =>
            AssetProvider.Instantiate<ShipDetailHandler>(AssetPath.ShipDetailViewPath, at, parent);
        public override ShipDetailHandler SpawnAt(RectTransform parent)
        {
            var shipMaterial = Create(Vector3.zero, parent);
            shipMaterial.transform.localScale = Vector3.one;
            return shipMaterial;
        }
    }
}
