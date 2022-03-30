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

        public bool IsAdjacentTo(GridVertex vertex)
        => ID.GridDistanceTo(vertex.ID) == 1;

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



/*
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

public GridEdge GetEdge(TriangleDirection direction)
=> default;
public GridTriangle GetTriangle(TriangleDirection direction)
=> default;

public GridDirection DirectionTowards(GridVertex vertex)
{
    return default; // WIP
}
public TriangleDirection DirectionTowards(GridTriangle triangle)
{
    return default; // WIP
}
*/