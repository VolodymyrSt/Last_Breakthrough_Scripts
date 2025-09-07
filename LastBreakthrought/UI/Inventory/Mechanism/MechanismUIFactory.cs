using LastBreakthrought.Factory;
using LastBreakthrought.Infrustructure.AssetManagment;
using UnityEngine;

namespace LastBreakthrought.UI.Inventory.Mechanism
{
    public class MechanismUIFactory : BaseFactory<MechanismHandler>
    {
        public MechanismUIFactory(IAssetProvider assetProvider) : base(assetProvider) { }

        public override MechanismHandler Create(Vector3 at, Transform parent) =>
            AssetProvider.Instantiate<MechanismHandler>(AssetPath.MechanismPath, at, parent);
        public override MechanismHandler SpawnAt(RectTransform parent)
        {
            var shipMaterial = Create(Vector3.zero, parent);
            shipMaterial.transform.localScale = Vector3.one;
            return shipMaterial;
        }
    }
}
