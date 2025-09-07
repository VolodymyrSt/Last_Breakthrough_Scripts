using UnityEngine;

namespace LastBreakthrought.Infrustructure.Services.Input
{
    public class ComputerInput : InputService
    {
        public override Vector2 Axis => GetUnityAxis();

        private Vector2 GetUnityAxis() =>
            new Vector2(UnityEngine.Input.GetAxis(HORIZONTAL), UnityEngine.Input.GetAxis(VERTICAL));
    }
}
