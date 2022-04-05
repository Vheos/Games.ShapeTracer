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

    public class EdgeInfo
    {
        // Publics
        public GridEdge Edge
        { get; private set; }
        public float Progress
        {
            get => _progress;
            set
            {
                var clampedValue = value.Clamp01();
                if (clampedValue == _progress)
                    return;

                var previousProgress = _progress;
                _progress = clampedValue;

                Grid.OnChangeEdgeTraceProgress.Invoke(this, previousProgress, _progress);
            }
        }
        public VisualLine VisualLine
        {
            get
            {
                if (_visualLine == null)
                    _visualLine = VisualLinePool.Get();
                return _visualLine;
            }
        }
        public Tracer Tracer
        { get; set; }

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

            VisualLine.WorldFrom = Tracer.VertexFrom.WorldPosition;
            VisualLine.RGBFrom = Color.red;

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
        private VisualLine _visualLine;
        private void AssignTracerProgress()
        {
            Progress = Tracer.ProgressAlongEdge.Clamp01();
            VisualLine.WorldTo = Tracer.VertexFrom.WorldPosition.Lerp(Tracer.VertexTo.WorldPosition, Progress);
            VisualLine.RGBFrom = Color.green * Progress;
        }

        // Initializers
        private EdgeInfo()
        { }
        public EdgeInfo(GridEdge edge)
        => Edge = edge;
    }
}