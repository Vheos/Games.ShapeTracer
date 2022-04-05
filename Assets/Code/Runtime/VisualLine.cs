namespace Vheos.Games.ShapeTracer
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;

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
        public Color ColorFrom
        {
            get => _lineRenderer.startColor;
            set => _lineRenderer.startColor = value;
        }
        public Color ColorTo
        {
            get => _lineRenderer.endColor;
            set => _lineRenderer.endColor = value;
        }
        public Color RGBFrom
        {
            get => _lineRenderer.startColor.NewA(1f);
            set => _lineRenderer.startColor = value.NewA(AlphaFrom);
        }
        public Color RGBTo
        {
            get => _lineRenderer.endColor.NewA(1f);
            set => _lineRenderer.endColor = value.NewA(AlphaTo);
        }
        public float AlphaFrom
        {
            get => _lineRenderer.startColor.a;
            set => _lineRenderer.startColor = _lineRenderer.startColor.NewA(value);
        }
        public float AlphaTo
        {
            get => _lineRenderer.endColor.a;
            set => _lineRenderer.endColor = _lineRenderer.startColor.NewA(value);
        }

        // Privates
        private LineRenderer _lineRenderer;

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            this.BecomeChildOf(Grid.Instance);
            _lineRenderer = Get<LineRenderer>();
        }
    }
}