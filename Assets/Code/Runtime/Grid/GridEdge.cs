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

    public struct GridEdge : IEquatable<GridEdge>
    {
        // Publics
        public GridVectorInt ID
        { get; private set; }
        public GridVector GridPosition
        => ID / 2f;
        public Vector3 WorldPosition
        => Grid.GridToWorldPosition(GridPosition);

        public GridDirections Axis
        {
            get
            {
                GridDirections axis = 0;
                if (ID.X.IsOdd())
                    axis |= GridDirections.X;
                if (ID.Y.IsOdd())
                    axis |= GridDirections.Y;
                if (ID.Z.IsOdd())
                    axis |= GridDirections.Z;
                return axis;
            }
        }
        public IEnumerable<GridVertex> Vertices
        {
            get
            {
                GridVectorInt directionOffset = Axis.ToOffset();
                yield return new((ID - directionOffset) / 2);
                yield return new((ID + directionOffset) / 2);
            }
        }
        public IEnumerable<GridTriangle> NeighborTriangles
        {
            get
            {
                GridVectorInt directionOffset = Axis.ToOffset();
                GridVectorInt firstVertexID = (ID - directionOffset) / 2;
                yield return new(ID + firstVertexID + directionOffset.RotateCW());
                yield return new(ID + firstVertexID + directionOffset.RotateCCW());
            }
        }

        public IEnumerable<GridVertex> VerticesSortedByDistanceFrom(GridVector gridPosition, bool descending = false)
        => descending
         ? Vertices.OrderByDescending(t => t.ID.DistanceTo(gridPosition))
         : Vertices.OrderBy(t => t.ID.DistanceTo(gridPosition));
        public GridVertex VertexClosestTo(GridVector gridPosition)
        => VerticesSortedByDistanceFrom(gridPosition).First();
        public GridVertex VertexFarthestFrom(GridVector gridPosition)
        => VerticesSortedByDistanceFrom(gridPosition, true).First();

        // Constructors
        internal GridEdge(GridVectorInt id)
        => ID = id;
        public GridEdge(GridVertex a, GridVertex b)
        => ID = a.IsAdjacentTo(b)
            ? a.ID + b.ID
            : GridVectorInt.Invalid;
        static public GridEdge Invalid
        => new() { ID = GridVectorInt.Invalid };

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