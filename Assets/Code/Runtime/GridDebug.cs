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
    using TMPro;

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
        [field: SerializeField] public TextMeshProUGUI DebugText { get; private set; }

        // Private
        private void Updatable_OnUpdate()
        {
            if (InstantiateTracer == true)
            {
                InstantiateTracer = false;
                for (int i = 0; i < InstantiateTracerCount; i++)
                    TracerManager.InstantiateComponent();
            }

            if (DebugText != null)
            {
                Vector3 mouseWorldPosition = GlobalCamera.UnityCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).NewZ(0);
                GridVector mouseGridPosition = Grid.WorldToGridPosition(mouseWorldPosition);
                GridVertex closestVertex = Grid.VertexAt(mouseGridPosition);
                GridEdge closestEdge = Grid.EdgeAt(mouseGridPosition);
                GridTriangle closestTriangle = Grid.TriangleAt(mouseGridPosition);

                GridVector remainder = mouseGridPosition - mouseGridPosition.AxialRound();
                DebugText.text = "";
                DebugText.text += $"{mouseGridPosition}   ->   {mouseGridPosition.AxialRound()}\n\n";
                DebugText.text += $"{remainder}   ->   {remainder.X.Abs() / (remainder.X.Abs() + remainder.Y.Abs()):F2}\n\n";
                //DebugText.text += $"{mouseGridPosition * 3}   ->   {(mouseGridPosition + 0.5f).AxialRound()}\n\n";

                /*
                GridVector vertexSum = GridVector.Zero;
                Debug.Log($"Vertices:");
                foreach (var vertex in closestTriangle.Vertices)
                {
                    vertexSum += vertex.GridPosition;
                    Debug.Log($" • {vertex.ID}" + (vertex == closestVertex ? "   (closest)" : ""));
                }
                Debug.Log($"Vertex sum: {vertexSum}");
                */

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
            }

            /*
            if (Tracer != null && Anchor != FollowerAnchor.None)
            {
                GridVector followerGridPosition = Anchor switch
                {
                    FollowerAnchor.Cursor => mouseGridPosition,
                    FollowerAnchor.Center => closestTriangle.GridPosition,
                    FollowerAnchor.Vertex => closestVertex.GridPosition,
                    _ => default,
                };
                Tracer.transform.position = Grid.GridToWorldPosition(followerGridPosition);
            }
            */
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