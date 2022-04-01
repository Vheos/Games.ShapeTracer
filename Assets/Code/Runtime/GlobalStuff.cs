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

    public class GlobalStuff : AStaticComponent<GlobalStuff>
    {
        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            Tween.DefaultCurve = Qurve.ValuesByProgress;
        }
    }
}