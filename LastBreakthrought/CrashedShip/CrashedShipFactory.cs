using LastBreakthrought.Factory;
using LastBreakthrought.Infrustructure.AssetManagment;
using UnityEngine;

namespace LastBreakthrought.CrashedShip
{
    public class CrashedShipFactory : BaseFactory<ICrashedShip>
    {
        public CrashedShipFactory(IAssetProvider assetProvider) : base(assetProvider){}

        public override ICrashedShip Create(Vector3 at, Transform parent) => 
            AssetProvider.Instantiate<CrashedShip>(AssetPath.GetRandomCrashedShipPath(), at, parent);

        public override ICrashedShip SpawnAt(Vector3 at, Transform parent)
        {
            var crashedShip = Create(at, parent);
            crashedShip.OnInitialized();
            return crashedShip;
        }
    }
}
