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
    using UnityEngine.Pool;

    [RequireComponent(typeof(LineRenderer))]
    public class VisualLine : ABaseComponent
    {
        // Publics
        public Vector3 From
        {
            get => _lineRenderer.GetPosition(0);
            set => _lineRenderer.SetPosition(0, value);
        }
        public Vector3 To
        {
            get => _lineRenderer.GetPosition(1);
            set => _lineRenderer.SetPosition(1, value);
        }
        public Vector3 WorldFrom
        {
            get => From.Transform(transform);
            set => From = value.Untransform(transform);
        }
        public Vector3 WorldTo
        {
            get => To.Transform(transform);
            set => To = value.Untransform(transform);
        }

        // Privates
        private LineRenderer _lineRenderer;

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            _lineRenderer = Get<LineRenderer>();

        }
    }
}