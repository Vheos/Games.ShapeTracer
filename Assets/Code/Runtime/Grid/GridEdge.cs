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

        public Axes Axis
        {
            get
            {
                Axes axis = 0;
                if (ID.X.IsOdd())
                    axis |= Axes.X;
                if (ID.Y.IsOdd())
                    axis |= Axes.Y;
                return axis;
            }
        }
        public IEnumerable<GridVertex> Vertices
        {
            get
            {
                GridVectorInt directionOffset = Axis.ToGridDirection().ToGridVectorInt();
                yield return new((ID - directionOffset) / 2);
                yield return new((ID + directionOffset) / 2);
            }
        }

        public IEnumerable<GridVertex> VerticesSortedByDistanceFrom(GridVector gridPosition, bool descending = false)
        => descending
         ? Vertices.OrderByDescending(t => t.ID.GridDistanceTo(gridPosition))
         : Vertices.OrderBy(t => t.ID.GridDistanceTo(gridPosition));
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

/*
// Common
public IEnumerable<GridVertex> VerticesSortedByDistanceFrom(Vector3 position, bool descending = false)
=> descending
 ? Vertices.OrderByDescending(t => t.ID.DistanceTo(position))
 : Vertices.OrderBy(t => t.ID.DistanceTo(position));
public GridVertex VertexClosestTo(Vector3 position)
=> VerticesSortedByDistanceFrom(position).First();
public GridVertex VertexFarthestFrom(Vector3 position)
=> VerticesSortedByDistanceFrom(position, true).First();
*/