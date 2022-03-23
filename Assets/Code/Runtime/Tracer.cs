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
        public GridEdge CurrentEdge
        => new(VertexFrom, VertexTo);
        public float ProgressAlongEdge
        => VertexFrom.DistanceTo(Grid.WorldToGridPosition(transform.position));

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
            Vector3 worldDirection = transform.position.DirectionTowards(Grid.GridToWorldPosition(VertexTo.Position));
            transform.position += Speed * Time.deltaTime * worldDirection.Mul(Grid.Instance.transform.localScale);

            // Rotation
            float angle = Vector3.SignedAngle(Vector3.right, worldDirection, Vector3.forward);
            transform.rotation = transform.rotation.Lerp(Quaternion.Euler(0, 0, angle), Utility.HalfTimeToLerpAlpha(0.1f));
        }
        private void UpdateTargetVertex()
        {
            VertexFrom = VertexTo;
            IEnumerable<GridEdge> potentialEdges = VertexFrom.NeighborEdges.Where(t => t.TraceInfo.State == TraceState.None);
            if (!potentialEdges.Any())
                potentialEdges = VertexFrom.NeighborEdges;

            GridEdge targetEdge = potentialEdges.Random();        
            VertexTo = targetEdge.VertexFarthestFrom(VertexFrom.Position);
            OnStartTracingEdge.Invoke(this, CurrentEdge);
        }

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            Get<Updatable>().OnUpdate.SubEnableDisable(this, Updatable_OnUpdate);
            VertexFrom = Grid.VertexClosestTo(Space.World, transform.position);
            UpdateTargetVertex();
        }
    }
}