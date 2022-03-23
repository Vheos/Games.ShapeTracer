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
    using Vheos.Tools.Utilities;

    public struct GridVertex : IEquatable<GridVertex>
    {
        // Publics
        public Vector3Int ID
        { get; private set; }
        public Vector3 Position
        => ID;
        public Vector3 WorldPosition
        => Grid.GridToWorldPosition(Position);
        public IEnumerable<GridVertex> NeighborVertices
        {
            get
            {
                foreach (var direction in Grid.GridDirectionsAndVectors.Keys)
                    yield return GetVertex(direction);
            }
        }
        public IEnumerable<GridEdge> NeighborEdges
        {
            get
            {
                foreach (var vertex in NeighborVertices)
                    yield return new(this, vertex);
            }
        }
        public IEnumerable<GridTriangle> NeighborTriangles
        {
            get
            {
                foreach (var direction in NewUtility.GetEnumValues<TriangleDirection>(true, true))
                    yield return GetTriangle(direction);
            }
        }
        public GridVertex GetVertex(GridDirection direction)
        => new(ID.Add(direction.VectorInt()));
        public GridEdge GetEdge(TriangleDirection direction)
        => default; // WIP
        public GridTriangle GetTriangle(TriangleDirection direction)
        => new(ID.Add(direction.VectorInt()));

        public float DistanceTo(Vector3 position)
        => position.Sub(ID).Abs().SumComp() / 2f;
        public int DistanceTo(Vector3Int position)
        => position.Sub(ID).Abs().SumComp() / 2;
        public bool IsAdjacentTo(GridVertex vertex)
        => DistanceTo(vertex.ID) == 1;
        public GridDirection DirectionTowards(GridVertex vertex)
        {
            return default; // WIP
        }
        public TriangleDirection DirectionTowards(GridTriangle triangle)
        {
            return default; // WIP
        }

        // Privates
        static private bool IsIDValid(Vector3Int id)
        => id.SumComp() == 0;

        // Constructors
        public GridVertex(Vector3Int id)
        => ID = IsIDValid(id) ? id : Grid.InvalidID;
        public GridVertex(int idX, int idY, int idZ) : this(new Vector3Int(idX, idY, idZ))
        { }
        static public GridVertex Zero
        => new(Vector3Int.zero);

        // IEquatable
        static public bool operator ==(GridVertex t, GridVertex a)
        => t.Equals(a);
        static public bool operator !=(GridVertex t, GridVertex a)
        => !t.Equals(a);
        public bool Equals(GridVertex a)
        => ID == a.ID;
        public override bool Equals(object a)
        => a is not null
        && a is GridVertex vertex
        && Equals(vertex);
        public override int GetHashCode()
        => ID.GetHashCode();
    }
}