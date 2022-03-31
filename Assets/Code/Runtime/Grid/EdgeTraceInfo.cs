namespace Vheos.Games.ShapeTracer
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;

    public class EdgeTraceInfo
    {
        // Publics
        public GridEdge Edge
        { get; private set; }
        public float Progress
        {
            get => _progress;
            private set
            {
                float clampedValue = value.Clamp01();
                if (_progress == clampedValue)
                    return;

                _progress = clampedValue;
                if (State == TraceState.Full)
                {
                    Grid.OnFullyTraceEdge.Invoke(Edge);
                    foreach (var triangle in Edge.NeighborTriangles)
                        if (triangle.IsFullyTraced())
                            Grid.OnFullyTraceTriangle.Invoke(triangle);
                }
            }
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
                VisualLine = VisualLinePool.Get();
            
            VisualLine.WorldFrom = tracer.transform.position;
            VisualLine.AlphaFrom = VisualLine.AlphaTo = 0f;

            Grid.Instance.Get<Updatable>().OnUpdate.SubDestroy(tracer, AssignTracerProgress);
            AssignTracerProgress();
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
            Progress = Tracer.ProgressAlongEdge.Clamp01();            
            VisualLine.WorldTo = Tracer.transform.position;
            VisualLine.AlphaFrom = VisualLine.AlphaTo = Progress / 2f;
        }

        // Initializers
        public EdgeTraceInfo(GridEdge edge)
        => Edge = edge;
    }
}