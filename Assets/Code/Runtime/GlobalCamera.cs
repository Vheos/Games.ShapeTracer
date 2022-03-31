namespace Vheos.Games.ShapeTracer
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
        [field: SerializeField, Range(0f, 0.25f)] public float Threshold { get; private set; }
        [field: SerializeField, Range(1f, 10f)] public float Speed { get; private set; }

        // Publics
        static public Camera UnityCamera { get; private set; }

        // Privates
        private void HandleScreenEdgeMovement()
        {
            Vector2 screenSize = new(Screen.width, Screen.height);
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 mappedMousePosition = mousePosition.Div(screenSize).MapTo01(Threshold.ToVector2(), 1.Sub(Threshold).ToVector2());

            Vector2 movementInput = Vector2.zero;
            if (mappedMousePosition.x <= 0)
                movementInput.x = -1;
            else if(mappedMousePosition.x >= 1)
                movementInput.x = +1;
            if (mappedMousePosition.y <= 0)
                movementInput.y = -1;
            else if (mappedMousePosition.y >= 1)
                movementInput.y = +1;

            transform.position += movementInput.Mul(Time.deltaTime * Speed).Append();
        }

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            UnityCamera = Get<Camera>();

            Get<Updatable>().OnUpdate.SubEnableDisable(this, HandleScreenEdgeMovement);
        }
    }
}