namespace Vheos.Games.Prototypes.ShapeTracer
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;

    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(Updatable))]
    public class GlobalCamera : AStaticComponent<GlobalCamera>
    {
        // Inspector
        [field: SerializeField] public int Threshold { get; private set; }
        [field: SerializeField] public ScreenEdgeThresholdUnit ThresholdUnit { get; private set; }

        // Publics
        static public Camera UnityCamera { get; private set; }

        // Privates
        private void HandleScreenEdgeMovement()
        {
            Vector2 screenSize = new(Screen.width, Screen.height);
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            switch (ThresholdUnit)
            {
                case ScreenEdgeThresholdUnit.Percent:
                    //mousePosition = mousePosition.MapTo01(Threshold.Div(100).ToVector2(), screenSize.Mul(1 -Threshold.Div(100).ToVector2());
                    break;
                case ScreenEdgeThresholdUnit.Pixels:
                    break;
            }

            Vector2 movementInput = Vector2.zero;
            if (mousePosition.x <= 0)
                movementInput.x = -1;
            else if(mousePosition.x >= 1)
                movementInput.x = +1;
            if (mousePosition.y <= 0)
                movementInput.y = -1;
            else if (mousePosition.y >= 1)
                movementInput.y = +1;

            Debug.Log($"{movementInput}");
        }

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            UnityCamera = Get<Camera>();

            Get<Updatable>().OnUpdate.SubEnableDisable(this, HandleScreenEdgeMovement);

        }

        // Defines
        public enum ScreenEdgeThresholdUnit
        {
            Percent,
            Pixels,
        }
    }
}