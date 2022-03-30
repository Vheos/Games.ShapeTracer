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

        public bool IsEven
        => ID.XY.CompSum().PosMod(3) == 2;
        public IEnumerable<GridVertex> Vertices
        {
            get
            {
                foreach (var offset in GetRawTriangleOffsets(IsEven))
                    yield return new((ID + offset) / 3);
            }
        }
        public IEnumerable<GridEdge> Edges
        {
            get
            {
                GridVertex[] vertices = Vertices.ToArray();
                yield return new(vertices[0].ID + vertices[1].ID);
                yield return new(vertices[1].ID + vertices[2].ID);
                yield return new(vertices[2].ID + vertices[0].ID);
            }
        }
        public IEnumerable<GridTriangle> NeighborTriangles
        {
            get
            {
                foreach (var offset in GetRawTriangleOffsets(!IsEven))
                    yield return new(ID + offset);
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

        public bool IsAdjacent(GridTriangle triangle)
        => ID.DistanceTo(triangle.ID) == 2;

        // Privates
        static private IEnumerable<GridVectorInt> GetRawTriangleOffsets(bool isEven)
        {
            int sign = isEven.ToSign();
            yield return new GridVectorInt(-1, -1) * sign;
            yield return new GridVectorInt(+2, -1) * sign;
            yield return new GridVectorInt(-1, +2) * sign;
        }

        // Constructors
        internal GridTriangle(GridVectorInt id)
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