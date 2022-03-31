namespace Vheos.Games.ShapeTracer
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Pool;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;

    public class VisualLinePool : AComponentPool<VisualLinePool, VisualLine>
    {
        // Privates
        protected override VisualLine CreateComponent()
        {
            VisualLine newLine = base.CreateComponent();
            newLine.BecomeChildOf(Grid.Instance);
            return newLine;
        }
    }
}