namespace Vheos.Games.Prototypes.ShapeTracer
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;
    using Vheos.Tools.Utilities;
    using Vheos.Tools.Extensions.Collections;

    public enum TraceState
    {
        None,
        Partial,
        Full,
    }

    public class TraceInfo
    {
        // Publics
        public float Progress
        {
            get => _progress;
            set => _progress = value.Clamp01();
        }
        public VisualLine VisualLine
        { get; private set; }
        public Tracer Tracer
        { get; private set; }
        public TraceState State
        => _progress switch
        {
            0f => TraceState.None,
            1f => TraceState.Full,
            _ => TraceState.Partial,
        };
        public void ConnectTo(Tracer tracer)
        {
            Tracer = tracer;
            if (VisualLine == null)
            {
                VisualLine = VisualLinePool.Get();
                VisualLine.WorldFrom = tracer.transform.position;
            }
            AssignTracerProgress();
            Grid.Instance.Get<Updatable>().OnUpdate.Sub(AssignTracerProgress);
        }
        public void Disconnect()
        {
            Grid.Instance.Get<Updatable>().OnUpdate.Unsub(AssignTracerProgress);
            Tracer = null;
        }

        // Privates
        private float _progress;
        private void AssignTracerProgress()
        {
            Progress = Tracer.ProgressAlongEdge;
            VisualLine.WorldTo = Tracer.transform.position;
        }
    }

    public class Grid : AStaticComponent<Grid>
    {
        // Inspector
        [field: SerializeField] public GridType Type { get; private set; }
        [field: SerializeField, Range(1, 1000)] public int SizeFactor { get; private set; }

        // Publics
        static public Vector3Int InvalidID
        => int.MaxValue.ToVector3Int();
        static public IReadOnlyTwoWayDictionary<Axes, Vector3Int> AxesAndVectors
        => _axesAndVectors;
        static public IReadOnlyTwoWayDictionary<GridDirection, Vector3Int> GridDirectionsAndVectors
        => _vertexDirectionsAndVectors;
        static public Vector3 WorldToGridPosition(Vector3 worldPosition)
        {
            Vector3 localPosition = worldPosition.Untransform(Instance.transform);
            float radiusY = localPosition.y / 3.Sqrt();
            return new(+localPosition.x - radiusY, radiusY * 2f, -localPosition.x - radiusY);
        }
        static public Vector3 GridToWorldPosition(Vector3 gridPosition)
        {
            Vector3 localPosition = new(gridPosition.x + gridPosition.y / 2f, 3.Sqrt() * gridPosition.y / 2f, 0);
            return localPosition.Transform(Instance.transform);
        }
        static public GridTriangle TriangleAt(Space space, Vector3 position)
        {
            if (space == Space.World)
                position = WorldToGridPosition(position);

            return position == Vector3.zero
                 ? new GridVertex(Vector3Int.zero).NeighborTriangles.Random()
                 : new(position.x.RoundUp(), position.y.RoundUp(), position.z.RoundUp());
        }
        static public GridVertex VertexClosestTo(Space space, Vector3 position)
        {
            if (space == Space.World)
                position = WorldToGridPosition(position);
            return TriangleAt(Space.Grid, position).VertexClosestTo(position);
        }
        static public TraceInfo GetTraceInfo(GridEdge edge)
        {
            _infosByEdge.TryAdd(edge, new());
            return _infosByEdge[edge];
        }


        // Privates
        static private TwoWayDictionary<Axes, Vector3Int> _axesAndVectors;
        static private TwoWayDictionary<GridDirection, Vector3Int> _vertexDirectionsAndVectors;
        static private Dictionary<GridEdge, TraceInfo> _infosByEdge;
        static private void Tracer_OnFinishTracingEdge(Tracer tracer, GridEdge edge)
        {
            GetTraceInfo(edge).Disconnect();
        }



        static private void Tracer_OnStartTracingEdge(Tracer tracer, GridEdge edge)
        {
            if (GetTraceInfo(edge).State == TraceState.None)
                GetTraceInfo(edge).ConnectTo(tracer);
        }

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();

            _axesAndVectors = new();
            foreach (var axis in NewUtility.GetEnumValues<Axes>(true, true))
                _axesAndVectors.Add(axis, axis.Vector3Int());

            _vertexDirectionsAndVectors = new();
            foreach (var direction in NewUtility.GetEnumValues<GridDirection>(true, true))
                _vertexDirectionsAndVectors.Add(direction, direction.VectorInt());

            _infosByEdge = new();
        }
        protected override void PlayStart()
        {
            base.PlayStart();
            TracerManager.OnRegisterComponent.SubDestroy(this,
                newTracer => newTracer.OnStartTracingEdge.SubDestroy(newTracer, Tracer_OnStartTracingEdge),
                newTracer => newTracer.OnFinishTracingEdge.SubDestroy(newTracer, Tracer_OnFinishTracingEdge));
        }
    }


    static public class GridVertex_Extensions
    {
        static public GridDirection GridDirection(this Axes t, AxisDirection a = AxisDirection.Positive)
        => t switch
        {
            Axes.XY when a == AxisDirection.Positive => ShapeTracer.GridDirection.RightDown,
            Axes.XZ when a == AxisDirection.Positive => ShapeTracer.GridDirection.Right,
            Axes.YZ when a == AxisDirection.Positive => ShapeTracer.GridDirection.RightUp,
            Axes.XY when a == AxisDirection.Negative => ShapeTracer.GridDirection.LeftUp,
            Axes.XZ when a == AxisDirection.Negative => ShapeTracer.GridDirection.Left,
            Axes.YZ when a == AxisDirection.Negative => ShapeTracer.GridDirection.LeftDown,
            _ => ShapeTracer.GridDirection.Invalid,
        };
        static public Vector3Int VectorInt(this GridDirection t)
        => t switch
        {
            ShapeTracer.GridDirection.RightDown => new(+1, -1, 00),
            ShapeTracer.GridDirection.Right => new(+1, 00, -1),
            ShapeTracer.GridDirection.RightUp => new(00, +1, -1),
            ShapeTracer.GridDirection.LeftUp => new(-1, +1, 00),
            ShapeTracer.GridDirection.Left => new(-1, 00, +1),
            ShapeTracer.GridDirection.LeftDown => new(00, -1, +1),
            _ => Grid.InvalidID,
        };
        static public Vector3 Vector(this GridDirection t)
        => t.VectorInt();
        static public Vector3Int VectorInt(this TriangleDirection t)
        => t switch
        {
            TriangleDirection.RightDown => new(+1, 00, 00),
            TriangleDirection.RightUp => new(+1, +1, 00),
            TriangleDirection.Up => new(00, +1, 00),
            TriangleDirection.LeftUp => new(00, +1, +1),
            TriangleDirection.LeftDown => new(00, 00, +1),
            TriangleDirection.Down => new(+1, 00, +1),
            _ => default,
        };
        static public Vector3 Vector(this TriangleDirection t)
        => t.VectorInt();
    }

}