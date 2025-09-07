using LastBreakthrought.Factory;
using LastBreakthrought.Infrustructure.AssetManagment;
using LastBreakthrought.UI.ShipMaterial;
using UnityEngine;

namespace LastBreakthrought.Logic.ShipMaterial
{
    public class ShipMaterialUIFactory : BaseFactory<ShipMaterialHandler>
    {
        public ShipMaterialUIFactory(IAssetProvider assetProvider) : base(assetProvider) { }

        public override ShipMaterialHandler Create(Vector3 at, Transform parent) =>
            AssetProvider.Instantiate<ShipMaterialHandler>(AssetPath.ShipMaterialViewPath, at, parent);
        public override ShipMaterialHandler SpawnAt(RectTransform parent)
        {
            var shipMaterial = Create(Vector3.zero, parent);
            shipMaterial.transform.localScale = Vector3.one;
            return shipMaterial;
        }
    }
}