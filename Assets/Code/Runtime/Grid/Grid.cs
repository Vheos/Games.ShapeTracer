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
    using Tools.Extensions.Collections;

    public class Grid : AStaticComponent<Grid>
    {
        // Inspector
        [field: SerializeField, Range(1, 50)] public int Radius { get; private set; }

        // Events
        static public AutoEvent<GridEdge> OnFullyTraceEdge;
        static public AutoEvent<GridTriangle> OnFullyTraceTriangle;

        // Publics
        static public IReadOnlyTwoWayDictionary<GridDirections, GridVectorInt> VertexDirectionsAndVectors
        => _vertexDirectionsAndVectors;
        static public IReadOnlyTwoWayDictionary<GridDirections, GridVectorInt> TriangleDirectionsAndVectors
        => _triangleDirectionsAndVectors;
        static public GridVector WorldToGridPosition(Vector3 worldPosition)
        {
            if (Instance != null)
                worldPosition = worldPosition.Untransform(Instance.transform);
            float radiusY = worldPosition.y / 3.Sqrt();
            GridVector gridPosition = new(worldPosition.x - radiusY, radiusY * 2f);
            return gridPosition;
        }
        static public Vector3 GridToWorldPosition(GridVector gridPosition)
        {
            Vector3 worldPosition = new(gridPosition.X + gridPosition.Y / 2f, 3.Sqrt() * gridPosition.Y / 2f, 0);
            if (Instance != null)
                worldPosition = worldPosition.Transform(Instance.transform);
            return worldPosition;
        }
        static public GridVertex VertexAt(GridVector gridPosition)
        => new(gridPosition.AxialRound());
        static public GridVertex VertexAt(Vector3 worldPosition)
        => VertexAt(WorldToGridPosition(worldPosition));
        static public GridEdge EdgeAt(GridVector gridPosition)
        => TriangleAt(gridPosition).EdgeClosestTo(gridPosition);
        static public GridEdge EdgeAt(Vector3 worldPosition)
        => EdgeAt(WorldToGridPosition(worldPosition));
        static public GridTriangle TriangleAt(GridVector gridPosition)
        {
            Vector3Int roundedGridPosition = gridPosition.XYZ.RoundUp();
            int isOdd = roundedGridPosition.CompSum().Mod(2);
            return new(roundedGridPosition.XY().Mul(3).Sub(2).Add(isOdd));
        }
        static public GridTriangle TriangleAt(Vector3 worldPosition)
        => TriangleAt(WorldToGridPosition(worldPosition));
        static public EdgeTraceInfo GetTraceInfo(GridEdge edge)
        {
            _traceInfosByEdge.TryAdd(edge, new(edge));
            return _traceInfosByEdge[edge];
        }

        // Privates
        static private TwoWayDictionary<GridDirections, GridVectorInt> _vertexDirectionsAndVectors;
        static private TwoWayDictionary<GridDirections, GridVectorInt> _triangleDirectionsAndVectors;
        static private Dictionary<GridEdge, EdgeTraceInfo> _traceInfosByEdge;
        static private void Tracer_OnStopTracingEdge(Tracer tracer, GridEdge edge)
        {
            edge.TraceInfo().Disconnect();
        }
        static private void Tracer_OnStartTracingEdge(Tracer tracer, GridEdge edge)
        {
            if (edge.TraceInfo().State == TraceState.None)
                edge.TraceInfo().ConnectTo(tracer);
        }
        static private void Grid_OnFullyTracedTriangle(GridTriangle triangle)
        {
            Coin coin = CoinPool.Get();
            coin.Initialize(triangle);
            foreach (var edge in triangle.Edges)
            {
                var visualLine = edge.TraceInfo().VisualLine;
                visualLine.RGBFrom = visualLine.RGBTo = Color.yellow;
            }
        }

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();

            _vertexDirectionsAndVectors = new();
            foreach (var positiveDirection in new[] { GridDirections.XY, GridDirections.YZ, GridDirections.ZX })
            {
                GridDirections negativeDirection = positiveDirection | GridDirections.Negative;
                _vertexDirectionsAndVectors.Add(positiveDirection, positiveDirection.ToOffset());
                _vertexDirectionsAndVectors.Add(negativeDirection, negativeDirection.ToOffset());
            }

            _triangleDirectionsAndVectors = new();
            foreach (var positiveDirection in new[] { GridDirections.XX, GridDirections.YY, GridDirections.ZZ })
            {
                GridDirections negativeDirection = positiveDirection | GridDirections.Negative;
                _triangleDirectionsAndVectors.Add(positiveDirection, positiveDirection.ToOffset());
                _triangleDirectionsAndVectors.Add(negativeDirection, negativeDirection.ToOffset());
            }

            _traceInfosByEdge = new();

            OnFullyTraceEdge = new();
            OnFullyTraceTriangle = new();

            OnFullyTraceTriangle.SubDestroy(this, Grid_OnFullyTracedTriangle);
        }
        protected override void PlayStart()
        {
            base.PlayStart();
            TracerManager.OnRegisterComponent.SubDestroy(this,
                newTracer => newTracer.OnStartTracingEdge.SubDestroy(newTracer, Tracer_OnStartTracingEdge),
                newTracer => newTracer.OnStopTracingEdge.SubDestroy(newTracer, Tracer_OnStopTracingEdge));
        }
    }
}