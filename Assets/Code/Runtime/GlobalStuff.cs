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

    [RequireComponent(typeof(Updatable))]
    [RequireComponent(typeof(Selecter))]
    public class GlobalStuff : AStaticComponent<GlobalStuff>
    {
        // Privates
        private Selectable SelectableUnderCursor
        => RaycastableManager.FindClosest<Selectable>(Mouse.current.position.ReadValue());
        private void Updatable_OnUpdate()
        {
            if (!Get<Selecter>().IsHolding)
                Get<Selecter>().Selectable = SelectableUnderCursor;

            if (Mouse.current.leftButton.wasPressedThisFrame)
                Get<Selecter>().TryPress();
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
                Get<Selecter>().TryRelease(Get<Selecter>().Selectable == SelectableUnderCursor);
        }

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            Tween.DefaultCurve = Qurve.ValuesByProgress;

            Get<Updatable>().OnUpdate.SubEnableDisable(this, Updatable_OnUpdate);
        }
    }
}