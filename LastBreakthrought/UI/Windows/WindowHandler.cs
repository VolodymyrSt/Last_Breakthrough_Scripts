using UnityEngine;

namespace LastBreakthrought.UI.Windows
{
    public abstract class WindowHandler<TView> : WindowHandlerBase where TView : WindowViewBase
    {
        [field: SerializeField] public TView View { get; set; }
    }
}
