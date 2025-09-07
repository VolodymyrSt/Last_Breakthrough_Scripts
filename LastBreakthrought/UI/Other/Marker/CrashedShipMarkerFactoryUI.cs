using LastBreakthrought.CrashedShip;
using LastBreakthrought.Factory;
using LastBreakthrought.Infrustructure.AssetManagment;
using UnityEngine;

namespace LastBreakthrought.UI.Other.Marker
{
    public class CrashedShipMarkerFactoryUI : BaseFactory<ICrashedShipMarker>
    {
        public CrashedShipMarkerFactoryUI(IAssetProvider assetProvider) : base(assetProvider){}

        public override ICrashedShipMarker Create(Vector3 at, Transform parent) => 
            AssetProvider.Instantiate<ICrashedShipMarker>(AssetPath.CrashedShipMarker, at, parent);

        public ICrashedShipMarker SpawnMarker(ICrashedShip crashedShip, RectTransform parent)
        {
            var marker = Create(Vector3.zero, parent);
            marker.Init(crashedShip);
            return marker;
        }
    }
}
