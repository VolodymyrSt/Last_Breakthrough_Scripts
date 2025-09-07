using LastBreakthrought.CrashedShip;
using LastBreakthrought.UI.Other.Marker;
using Zenject;

namespace LastBreakthrought.UI.Map
{
    public class MapMenuPanelHandler : IInitializable
    {
        private readonly MapMenuPanelView _view;
        private readonly CrashedShipMarkerFactoryUI _shipMarkerFactoryUI;

        public MapMenuPanelHandler(MapMenuPanelView view, CrashedShipMarkerFactoryUI shipMarkerFactoryUI)
        {
            _view = view;
            _shipMarkerFactoryUI = shipMarkerFactoryUI;
        }

        public void Initialize() => _view.Init();

        public ICrashedShipMarker Add(ICrashedShip crashedShip)
        {
            var marker = _shipMarkerFactoryUI.SpawnMarker(crashedShip, _view.GetMarkerContainer());
            _view.OnNewItemAdded(marker.GetTransform());
            return marker;
        }
    }
}
