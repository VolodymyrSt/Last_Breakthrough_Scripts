using UnityEngine;

namespace LastBreakthrought.Infrustructure.Services.Input
{
    public class MobileInput : InputService
    {
        public override Vector2 Axis =>
            GetSimpleInputAxis();
    }
}
