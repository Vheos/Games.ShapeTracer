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
    using Random = UnityEngine.Random;
    using Vheos.Tools.Utilities;

    [RequireComponent(typeof(Updatable))]
    public class Tracer : ABaseComponent
    {
        // Inspector
        [field: SerializeField, Range(0, 2f)] public float Speed { get; private set; }

        // Events
        public AutoEvent<Tracer, GridEdge> OnStartTracingEdge = new();
        public AutoEvent<Tracer, GridEdge> OnFinishTracingEdge = new();

        // Publics
        public GridVertex VertexFrom;
        public GridVertex VertexTo;
        public GridVector GridPosition
        {
            set => transform.position = Grid.GridToWorldPosition(value);
            get => Grid.WorldToGridPosition(transform.position);
        }
        public GridVector EdgeOffset
        => (VertexTo.GridPosition - VertexFrom.GridPosition);
        public GridEdge CurrentEdge
        => new(VertexFrom, VertexTo);
        public float ProgressAlongEdge
        => GridPosition.XYZ.InverseLerp(VertexFrom.GridPosition.XYZ, VertexTo.GridPosition.XYZ) / 2.Sqrt();

        // Privates
        private void Updatable_OnUpdate()
        {
            // Check if arrived at target vertex
            if (ProgressAlongEdge >= 1f)
            {
                OnFinishTracingEdge.Invoke(this, CurrentEdge);
                UpdateTargetVertex();
            }

            // Position
            GridPosition += Speed * Time.deltaTime * EdgeOffset.Normalized;

            // Rotation
            Vector2 worldDirection = transform.position.DirectionTowards(Grid.GridToWorldPosition(VertexTo.ID));
            float angle = Vector3.SignedAngle(Vector3.right, worldDirection, Vector3.forward);
            transform.rotation = transform.rotation.Lerp(Quaternion.Euler(0, 0, angle), Utility.HalfTimeToLerpAlpha(0.1f));
        }
        private void UpdateTargetVertex()
        {
            VertexFrom = VertexTo;
            IEnumerable<GridEdge> potentialEdges = VertexFrom.NeighborEdges.Where(t => Grid.GetTraceInfo(t).State == TraceState.None);
            if (!potentialEdges.Any())
                potentialEdges = VertexFrom.NeighborEdges;

            GridEdge targetEdge = potentialEdges.Random();
            VertexTo = targetEdge.VertexFarthestFrom(VertexFrom.ID);
            OnStartTracingEdge.Invoke(this, CurrentEdge);
        }

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            Get<Updatable>().OnUpdate.SubEnableDisable(this, Updatable_OnUpdate);
            VertexFrom = Grid.VertexAt(transform.position);
            UpdateTargetVertex();
        }
    }
}