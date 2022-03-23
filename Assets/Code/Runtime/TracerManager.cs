namespace Vheos.Games.Prototypes.ShapeTracer
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;

    [RequireComponent(typeof(Updatable))]
    public class TracerManager : AStaticManager<TracerManager, Tracer>
    { }
}