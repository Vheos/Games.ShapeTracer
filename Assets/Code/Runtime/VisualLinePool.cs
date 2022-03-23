namespace Vheos.Games.Prototypes.ShapeTracer
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

    public class VisualLinePool : AStaticComponent<VisualLinePool>
    {
        // Inspector
        [field: SerializeField] public VisualLine Prefab { get; private set; }

        // Publics
        static public VisualLine Get()
        => _pool.Get();
        static public void Release(VisualLine visualLine)
        => _pool.Release(visualLine);

        // Privates
        static private ObjectPool<VisualLine> _pool;
        static private VisualLine CreateFunc()
        => Grid.Instance.CreateChildComponent(Instance.Prefab, nameof(VisualLine));


        // Initializers
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static private void StaticInitialize()
        {
            _pool = new(
                createFunc: CreateFunc,
                actionOnGet: t => t.Activate(),
                actionOnRelease: t => t.Deactivate(),
                actionOnDestroy: t => t.DestroyObject(),
                defaultCapacity: 1000,
                maxSize: 1000,
                collectionCheck: true);
        }
    }
}