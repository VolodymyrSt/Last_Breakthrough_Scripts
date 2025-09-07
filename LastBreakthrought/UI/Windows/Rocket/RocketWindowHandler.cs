using LastBreakthrought.Logic.Rocket;
using UnityEngine;

namespace LastBreakthrought.UI.Windows
{
    public class RocketWindowHandler : WindowHandler<RocketVindowView>
    {
        [field:SerializeField] public Rocket Rocket {  get; private set; }

        public override void ActivateWindow() => View.ShowView();

        public override void DeactivateWindow() => View.HideView();
    }
}
