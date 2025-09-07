using UnityEngine;

namespace LastBreakthrought.Infrustructure.Services.Input
{
    public abstract class InputService : IInputService
    {
        protected const string HORIZONTAL = "Horizontal";
        protected const string VERTICAL = "Vertical";
        public abstract Vector2 Axis {  get; }

        protected Vector2 GetSimpleInputAxis() =>
            new Vector2(SimpleInput.GetAxis(HORIZONTAL), SimpleInput.GetAxis(VERTICAL));
    }
}
