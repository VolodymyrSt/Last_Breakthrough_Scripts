using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.UI.Windows.CrashedShipWindow
{
    public class CrashedShipWindowView : WindowView<CrashedShipWindowHandler>
    {
        [SerializeField] private Button _setMarkerButton;

        public override void Initialize()
        {
            Handler.CreateUnminedShipMaterialsView();
            _setMarkerButton.onClick.AddListener(() => Handler.CrashedShip.AddItselfAsMarker());
        }

        public override void Dispose() =>
            _setMarkerButton.onClick.RemoveListener(() => Handler.CrashedShip.AddItselfAsMarker());
    }
}
