using UnityEngine;

namespace LastBreakthrought.UI.Windows
{
    public abstract class WindowView<THandler> : WindowViewBase where THandler : WindowHandlerBase
    {
        [field: SerializeField] public THandler Handler { get; set; }
    }
}
