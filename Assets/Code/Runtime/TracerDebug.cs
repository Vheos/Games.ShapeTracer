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
    public class TracerDebug : ABaseComponent
    {
        // Inspector
        [field: SerializeField] public TextMeshProUGUI DebugLog { get; private set; }

        // Private
        private void Updatable_OnUpdate()
        {

            if (DebugLog == null)
                return;

            Tracer tracer = Get<Tracer>();
            GridVector tracerOffset = tracer.GridPosition - tracer.VertexFrom.GridPosition;
            GridVector toOffset = tracer.VertexTo.GridPosition - tracer.VertexFrom.GridPosition;
            DebugLog.text = "";
            DebugLog.text += $"Vertices:   {tracer.VertexFrom.ID}   ->   {tracer.VertexTo.ID}\n";
            DebugLog.text += $"TracerOffset:   {tracerOffset}\n";
            DebugLog.text += $"ToOffset:   {toOffset.XYZ}   /   {toOffset.XYZ.magnitude}   =   {toOffset.XYZ.normalized}\n";
            DebugLog.text += $"Dot:    {tracerOffset.XYZ.Dot(toOffset.XYZ.normalized)}\n";
        }

        // Play
        protected override void PlayStart()
        {
            base.PlayStart();
            Get<Updatable>().OnUpdate.SubEnableDisable(this, Updatable_OnUpdate);
        }
    }
}