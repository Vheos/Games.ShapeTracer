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

    public struct GridTriangle : IEquatable<GridTriangle>
    {
        // Publics
        public Vector3Int ID
        { get; private set; }
        public Vector3 FloatID
        => ID;
        public Vector3 Center
        => FloatID.Sub(IsEven.Map(2, 1) / 3f);
        public Vector3 WorldCenter
        => Grid.GridToWorldPosition(Center);
        public bool IsEven
        => ID.SumComp().IsEven();
        public bool IsOdd
        => ID.SumComp().IsOdd();
        public IEnumerable<GridVertex> Vertices
        {
            get
            {
                if (IsEven)
                {
                    yield return new(ID.Sub(1, 1, 0));
                    yield return new(ID.Sub(1, 0, 1));
                    yield return new(ID.Sub(0, 1, 1));
                }
                else
                {
                    yield return new(ID.Sub(1, 0, 0));
                    yield return new(ID.Sub(0, 1, 0));
                    yield return new(ID.Sub(0, 0, 1));
                }
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
        static private bool IsIDValid(Vector3Int id)
        => id.SumComp().IsEither(1, 2);

        // Constructors
        public GridTriangle(Vector3Int id)
        => ID = IsIDValid(id) ? id : Grid.InvalidID;
        public GridTriangle(int idX, int idY, int idZ) : this(new Vector3Int(idX, idY, idZ))
        { }
        public GridTriangle(GridVertex a, GridVertex b, GridVertex c)
        => ID = a.IsAdjacentTo(b) && a.IsAdjacentTo(c)
            ? new[] { a, b, c }.MaxComps(t => t.ID)
            : Grid.InvalidID;

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