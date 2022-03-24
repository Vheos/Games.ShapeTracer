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

    public struct GridTriangle : IEquatable<GridTriangle>
    {
        // Publics
        public GridVectorInt ID
        { get; private set; }
        public GridVector GridPosition
        => ID / 3f;
        public Vector3 WorldPosition
        => Grid.GridToWorldPosition(GridPosition);

        public IEnumerable<GridVertex> Vertices
        {
            get
            {
                yield break;

                /*
                GridVectorInt directionOffset = Axis.GridDirection().Vector();
                yield return new((ID - directionOffset) / 2);
                yield return new((ID + directionOffset) / 2);

                bool isEven = ID.SumXY.IsEven();
                if (isEven)
                {
                    yield return new(ID - new GridVectorInt(1, 1));
                    yield return new(ID.Sub(1, 0, 1));
                    yield return new(ID.Sub(0, 1, 1));
                }
                else
                {
                    yield return new(ID.Sub(1, 0, 0));
                    yield return new(ID.Sub(0, 1, 0));
                    yield return new(ID.Sub(0, 0, 1));
                }
                */
            }

        }

        public IEnumerable<GridEdge> Edges
        {
            get
            {
                GridVertex[] vertices = Vertices.ToArray();
                yield return new(vertices[0], vertices[1]);
                yield return new(vertices[1], vertices[2]);
                yield return new(vertices[2], vertices[0]);
            }
        }
        public IEnumerable<GridTriangle> NeighborTriangles
        {
            get
            {
                yield break;
            }
        }

        // Constructors
        public GridTriangle(GridVectorInt id)
        => ID = id;
        public GridTriangle(GridVertex a, GridVertex b, GridVertex c)
        => ID = a.IsAdjacentTo(b) && a.IsAdjacentTo(c)
            ? a.ID + b.ID + c.ID
            : GridVectorInt.Invalid;

        // IEquatable
        static public bool operator ==(GridTriangle t, GridTriangle a)
        => t.Equals(a);
        static public bool operator !=(GridTriangle t, GridTriangle a)
        => !t.Equals(a);
        public bool Equals(GridTriangle a)
        => ID == a.ID;
        public override bool Equals(object a)
        => a is not null
        && a is GridTriangle vertex
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
 
public GridTriangle(Vector3Int id)
=> ID = IsIDValid(id) ? id : Grid.InvalidID;
public GridTriangle(int idX, int idY, int idZ) : this(new Vector3Int(idX, idY, idZ))
{ }
public GridTriangle(GridVertex a, GridVertex b, GridVertex c)
=> ID = a.IsAdjacentTo(b) && a.IsAdjacentTo(c)
    ? new[] { a, b, c }.MaxComps(t => t.ID)
    : Grid.InvalidID;
*/