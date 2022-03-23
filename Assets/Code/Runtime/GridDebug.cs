namespace Vheos.Games.Prototypes.ShapeTracer
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
    public class GridDebug : ABaseComponent
    {
        // Inspector
        [field: SerializeField] public Tracer Tracer { get; private set; }
        [field: SerializeField] public FollowerAnchor Anchor { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float Opacity { get; private set; } = 1 / 3f;
        [field: SerializeField] public Color ColorX { get; private set; } = Color.red;
        [field: SerializeField] public Color ColorY { get; private set; } = Color.green;
        [field: SerializeField] public Color ColorZ { get; private set; } = Color.blue;
        [field: SerializeField, Range(1, 10)] public int InstantiateTracerCount { get; private set; }
        [field: SerializeField] public bool InstantiateTracer { get; private set; }

        // Private
        private void Updatable_OnUpdate()
        {
            if(InstantiateTracer == true)
            {
                InstantiateTracer = false;
                for (int i = 0; i < InstantiateTracerCount; i++)
                    TracerManager.InstantiateComponent();
            }

            Vector3 worldPosition = GlobalCamera.UnityCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).NewZ(0);
            Vector3 gridPosition = Grid.WorldToGridPosition(worldPosition);
            GridTriangle triangle = Grid.TriangleAt(Space.Grid, gridPosition);
            GridVertex closestVertex = triangle.VertexClosestTo(gridPosition);

            if (Tracer != null && Anchor != FollowerAnchor.None)
            {
                Vector3 followerGridPosition = Anchor switch
                {
                    FollowerAnchor.None => gridPosition,
                    FollowerAnchor.Center => triangle.Center,
                    FollowerAnchor.Vertex => closestVertex.Position,
                    _ => default,
                };
                Tracer.transform.position = Grid.GridToWorldPosition(followerGridPosition);
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Debug.Log($"GridPosition: {gridPosition}");
                Debug.Log($"ID: {triangle.ID}");
                Debug.Log($"Center: {triangle.Center}");
                Debug.Log($"Vertices:");
                foreach (var vertex in triangle.Vertices)
                    Debug.Log($" • {vertex.ID}" + (vertex.Equals(closestVertex) ? "   (closest)" : ""));

                /*
                Debug.Log($"");
                GridEdge[] edges = triangle.Edges.ToArray();
                Debug.DrawLine(edges[0].VertexA.WorldPosition, edges[1].VertexB.WorldPosition, Color.red, 1f);
                Debug.DrawLine(edges[1].VertexA.WorldPosition, edges[2].VertexB.WorldPosition, Color.yellow, 1f);
                Debug.DrawLine(edges[2].VertexA.WorldPosition, edges[0].VertexB.WorldPosition, Color.green, 1f);

                Debug.Log($"Edges:");
                foreach (var edge in edges)
                    Debug.Log($" • {edge.VertexA.ID} -> {edge.VertexB.ID}   /   {edge.ID}");
                */

                /* triangle from 3x vertex
                Debug.Log($"");
                var vertices = triangle.Vertices.ToArray();
                var newTriangle = new GridTriangle(vertices[0], vertices[1], vertices[2]);
                Debug.Log($"{triangle.ID}   vs   {newTriangle.ID}   =   {triangle.ID == newTriangle.ID}");
                */

                /* distance
                Vector3Int absDiff = closestVertex.ID.Sub(_previousVertex.ID).Abs();
                Debug.Log($"{closestVertex.ID}   ->   {_previousVertex.ID}   =   {absDiff} / {absDiff.SumComp()}");
                _previousVertex = closestVertex;
                */

                /* neighbors
                Debug.Log($"");
                Debug.Log($"Closest vertex: {closestVertex.Position}");
                Debug.Log($"Neighbor vertices:");
                foreach (var neighborVertex in closestVertex.NeighborVertices)
                    Debug.Log($" • {neighborVertex.Position}");
                Debug.Log($"Neighbor triangles:");
                foreach (var neighborTriangle in closestVertex.NeighborTriangles)
                    Debug.Log($" • {neighborTriangle.ID}");
                */

                Debug.Log($"");
            }
        }

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            Get<Updatable>().OnUpdate.SubEnableDisable(this, Updatable_OnUpdate);
        }

        // Defines
        public enum FollowerAnchor
        {
            None,
            Cursor,
            Center,
            Vertex,
        }
    }
}