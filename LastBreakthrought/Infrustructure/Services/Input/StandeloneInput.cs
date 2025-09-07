﻿using UnityEngine;

namespace LastBreakthrought.Infrustructure.Services.Input
{
    public class StandeloneInput : InputService
    {
        public override Vector2 Axis 
        {
            get
            {
                var axis = GetSimpleInputAxis();

                if (axis == Vector2.zero)
                    axis = GetUnityAxis();

                return axis;
            }
        }

        private Vector2 GetUnityAxis() =>
            new Vector2(UnityEngine.Input.GetAxis(HORIZONTAL), UnityEngine.Input.GetAxis(VERTICAL));
    }
}
