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

    public struct GridEdge : IEquatable<GridEdge>
    {
        // Publics
        public Vector3Int ID
        { get; private set; }
        public Axes Axis
        => Grid.AxesAndVectors[ID.PosMod(2)];
        public IEnumerable<GridVertex> Vertices
        {
            get
            {
                yield return new(ID.Sub(DirectionOffset) / 2);
                yield return new(ID.Add(DirectionOffset) / 2);
            }
        }
        public TraceInfo TraceInfo
        => Grid.GetTraceInfo(this);

        // Common
        public IEnumerable<GridVertex> VerticesSortedByDistanceFrom(Vector3 position, bool descending = false)
        => descending
         ? Vertices.OrderByDescending(t => t.Position.DistanceTo(position))
         : Vertices.OrderBy(t => t.Position.DistanceTo(position));
        public GridVertex VertexClosestTo(Vector3 position)
        => VerticesSortedByDistanceFrom(position).First();
        public GridVertex VertexFarthestFrom(Vector3 position)
        => VerticesSortedByDistanceFrom(position, true).First();

        // Privates
        private Vector3Int DirectionOffset
        => Axis.GridDirection().VectorInt();


        // Constructors
        public GridEdge(GridVertex a, GridVertex b)
        => ID = a.IsAdjacentTo(b)
            ? a.ID + b.ID
            : Grid.InvalidID;

        // IEquatable
        static public bool operator ==(GridEdge t, GridEdge a)
        => t.Equals(a);
        static public bool operator !=(GridEdge t, GridEdge a)
        => !t.Equals(a);
        public bool Equals(GridEdge a)
        => ID == a.ID;
        public override bool Equals(object a)
        => a is not null
        && a is GridEdge vertex
        && Equals(vertex);
        public override int GetHashCode()
        => ID.GetHashCode();
    }
}