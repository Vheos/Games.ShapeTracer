namespace Vheos.Games.ShapeTracer
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;
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
        [field: SerializeField, Range(1, 50)] public int SizeFactor { get; private set; }

        // Publics
        static public Vector2Int InvalidID
        => int.MaxValue.ToVector2Int();
        static public IReadOnlyTwoWayDictionary<GridDirection, GridVector> GridDirectionsAndVectors
        => _GridDirectionsAndVectors;
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
        static public GridVector TestVertexAt(Vector3 worldPosition)
        {
            if (Instance != null)
                worldPosition = worldPosition.Untransform(Instance.transform);

            Vector3 rounded = worldPosition.Round();
            Vector3 reminder = worldPosition - rounded;

            return reminder.x.Abs() >= reminder.y.Abs()
                 ? new(rounded.x + (reminder.x + 0.5f * reminder.y).Round(), rounded.y)
                 : new(rounded.x, rounded.y + (reminder.y + 0.5f * reminder.x).Round());
        }
        static public GridVertex VertexAt(GridVector gridPosition)
        => new(gridPosition.AxialRound());
        static public GridVertex VertexAt(Vector3 worldPosition)
        => VertexAt(WorldToGridPosition(worldPosition));
        static public GridEdge EdgeAt(GridVector gridPosition)
        => new((gridPosition * 2 ).AxialRound());
        static public GridEdge EdgeAt(Vector3 worldPosition)
        => EdgeAt(WorldToGridPosition(worldPosition));
        static public GridTriangle TriangleAt(GridVector gridPosition)
        => new((gridPosition * 3).AxialRound());
        static public GridTriangle TriangleAt(Vector3 worldPosition)
        => TriangleAt(WorldToGridPosition(worldPosition));

        static public TraceInfo GetTraceInfo(GridEdge edge)
        {
            _infosByEdge.TryAdd(edge, new());
            return _infosByEdge[edge];
        }


        // Privates
        static private TwoWayDictionary<GridDirection, GridVector> _GridDirectionsAndVectors;
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
            _GridDirectionsAndVectors = new();
            foreach (var direction in NewUtility.GetEnumValues<GridDirection>(true, true))
                _GridDirectionsAndVectors.Add(direction, direction.Vector());

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
}