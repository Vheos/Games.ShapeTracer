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
        public bool IsOdd
        => ID.XY.CompSum().PosMod(3) == 1;
        public IEnumerable<GridVertex> Vertices
        {
            get
            {
                foreach (var offset in GetTriangleOffsets(IsEven))
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
                foreach (var offset in GetTriangleOffsets(IsOdd))
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
        public GridEdge EdgeClosestTo(GridVector gridPosition)
        {
            GridVertex[] sortedVertices = VerticesSortedByDistanceFrom(gridPosition).ToArray();
            return new(sortedVertices[0].ID + sortedVertices[1].ID);
        }
        public GridEdge EdgeFarthestFrom(GridVector gridPosition)
        {
            GridVertex[] sortedVertices = VerticesSortedByDistanceFrom(gridPosition, true).ToArray();
            return new(sortedVertices[0].ID + sortedVertices[1].ID);
        }

        public bool IsAdjacent(GridTriangle triangle)
        => ID.DistanceTo(triangle.ID) == 2;

        // Privates
        static private IEnumerable<GridVectorInt> GetTriangleOffsets(bool positive)
        {
            foreach (var directionAndVector in Grid.TriangleDirectionsAndVectors.KeyValuePairs)
                if (directionAndVector.Key.HasFlag(GridDirections.Negative) != positive)
                    yield return directionAndVector.Value;
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