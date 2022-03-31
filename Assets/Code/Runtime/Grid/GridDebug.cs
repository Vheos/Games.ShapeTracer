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
        [field: SerializeField] public TextMeshProUGUI DebugLog { get; private set; }
        [field: SerializeField] public TextMeshPro DebugTextPrefab { get; private set; }

        // Private
        private void Updatable_OnUpdate()
        {
            if (InstantiateTracer == true)
            {
                InstantiateTracer = false;
                for (int i = 0; i < InstantiateTracerCount; i++)
                    TracerManager.InstantiateComponent();
            }

            Vector3 mouseWorldPosition = GlobalCamera.UnityCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).NewZ(0);
            GridVector mouseGridPosition = Grid.WorldToGridPosition(mouseWorldPosition);
            GridVertex closestVertex = Grid.VertexAt(mouseGridPosition);
            GridEdge closestEdge = Grid.EdgeAt(mouseGridPosition);
            GridTriangle closestTriangle = Grid.TriangleAt(mouseGridPosition);

            if (DebugLog != null)
            {
                DebugLog.text = "";
                DebugLog.text += $"Position: {mouseGridPosition}\n";
                DebugLog.text += $"Vertex: {closestVertex.ID}\n";
                DebugLog.text += $"Edge: {closestEdge.ID}\n";
                DebugLog.text += $"Triangle: {closestTriangle.ID}\n";
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
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

                Debug.Log($"Triangle: {closestTriangle.ID}");
                Debug.Log($"Vertices:");
                foreach (var vertex in closestTriangle.Vertices)
                {
                    Debug.Log($" • {vertex.ID}");
                    Debug.DrawLine(mouseWorldPosition, vertex.WorldPosition, Color.red, 1f);
                }

                Debug.Log($"Edges:");
                foreach (var edge in closestTriangle.Edges)
                {
                    GridVertex[] edgeVertices = edge.Vertices.ToArray();
                    Debug.Log($" • {edge.ID}   =   {edgeVertices[0].ID}   +   {edgeVertices[1].ID}");
                    Debug.DrawLine(edgeVertices[0].WorldPosition, edgeVertices[1].WorldPosition, Color.green, 1f);
                }

                Debug.Log($"Triangles:");
                foreach (var triangle in closestTriangle.NeighborTriangles)
                {
                    Debug.Log($" • {triangle.ID}   /   {triangle.GridPosition.DistanceTo(closestTriangle.GridPosition)}");
                    Debug.DrawLine(mouseWorldPosition, triangle.WorldPosition, Color.blue, 1f);
                }
                Debug.Log($"");

                foreach (var triangle in closestTriangle.Edges.First().NeighborTriangles)
                {
                    Debug.DrawLine(mouseWorldPosition, triangle.WorldPosition, Color.magenta, 1f);
                }

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
        protected override void PlayStart()
        {
            base.PlayStart();
            Get<Updatable>().OnUpdate.SubEnableDisable(this, Updatable_OnUpdate);

            Grid grid = Get<Grid>();
            for (int ix = -grid.Radius; ix <= +grid.Radius; ix++)
            {
                int fromY = grid.Radius.Neg().ClampMin(-ix - grid.Radius);
                int toY = grid.Radius.ClampMax(-ix + grid.Radius);
                for (int iy = fromY; iy <= toY; iy++)
                {
                    TextMeshPro vertexText = Instantiate(DebugTextPrefab);
                    vertexText.transform.position = Grid.GridToWorldPosition(new(ix, iy));
                    vertexText.text = $"{ix} {iy}";
                    vertexText.color = Color.green;

                    TextMeshPro triangleText = Instantiate(DebugTextPrefab);
                    triangleText.transform.position = Grid.GridToWorldPosition(new(ix + 1 / 3f, iy + 1 / 3f));
                    triangleText.text = $"{ix.Add(1 / 3f).RoundUp()} {iy.Add(1 / 3f).RoundUp()} {(-ix - iy).Sub(2 / 3f).RoundUp()}";
                    triangleText.color = Color.gray;

                    triangleText = Instantiate(DebugTextPrefab);
                    triangleText.transform.position = Grid.GridToWorldPosition(new(ix + 2 / 3f, iy - 1 / 3f));
                    triangleText.text = $"{ix.Add(2 / 3f).RoundUp()} {iy.Sub(1 / 3f).RoundUp()} {(-ix - iy).Sub(1 / 3f).RoundUp()}";
                    triangleText.color = Color.gray;
                }
            }
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