using LastBreakthrought.CrashedShip;
using UnityEngine;

namespace LastBreakthrought.UI.Other.Marker
{
    public interface ICrashedShipMarker
    {
        void Init(ICrashedShip crashedShip);
        void SelfDestroy();
        RectTransform GetTransform();
    }
}