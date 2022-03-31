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

    public struct GridVertex : IEquatable<GridVertex>
    {
        // Publics
        public GridVectorInt ID
        { get; private set; }
        public GridVector GridPosition
        => ID;
        public Vector3 WorldPosition
        => Grid.GridToWorldPosition(ID);

        public IEnumerable<GridVertex> NeighborVertices
        {
            get
            {
                foreach (var offset in Grid.GridDirectionsAndVectors.Values)
                    yield return new(ID + offset);
            }
        }
        public IEnumerable<GridEdge> NeighborEdges
        {
            get
            {
                foreach (var vertex in NeighborVertices)
                    yield return new(ID + vertex.ID);
            }
        }
        public IEnumerable<GridTriangle> NeighborTriangles
        {
            get
            {
                GridVertex[] vertices = NeighborVertices.ToArray();
                yield return new(ID + vertices[0].ID + vertices[1].ID);
                yield return new(ID + vertices[1].ID + vertices[2].ID);
                yield return new(ID + vertices[2].ID + vertices[3].ID);
                yield return new(ID + vertices[3].ID + vertices[4].ID);
                yield return new(ID + vertices[4].ID + vertices[5].ID);
                yield return new(ID + vertices[5].ID + vertices[0].ID);
            }
        }
        public bool IsAdjacentTo(GridVertex vertex)
        => ID.DistanceTo(vertex.ID) == 1;

        // Constructors
        public GridVertex(GridVectorInt id)
        => ID = id;
        static public GridVertex Zero
        => new() { ID = GridVectorInt.Zero };
        static public GridVertex Invalid
        => new() { ID = GridVectorInt.Invalid };

        // IEquatable
        static public bool operator ==(GridVertex t, GridVertex a)
        => t.Equals(a);
        static public bool operator !=(GridVertex t, GridVertex a)
        => !t.Equals(a);
        public bool Equals(GridVertex a)
        => ID == a.ID;
        public override bool Equals(object a)
        => a is not null
        && a is GridVectorInt vertex
        && Equals(vertex);
        public override int GetHashCode()
        => ID.GetHashCode();
    }
}