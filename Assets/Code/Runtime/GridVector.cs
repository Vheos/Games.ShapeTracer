namespace Vheos.Games.ShapeTracer
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;
    using Vheos.Tools.Utilities;
    using Vheos.Tools.Extensions.Collections;



    public struct GridVector
    {
        // Fields
        public float X;
        public float Y;
        public float Z
        => -X - Y;

        // Math
        public float SumXY()
        => X + Y;
        public GridVector Abs()
        => new(X.Abs(), Y.Abs());
        public GridVector PosMod(int a)
        => new(X.PosMod(a), Y.PosMod(a));
        public GridVector PosMod(float a)
        => new(X.PosMod(a), Y.PosMod(a));
        public float DistanceTo(GridVector t)
        => (X.DistanceTo(t.X) + Y.DistanceTo(t.Y) + Z.DistanceTo(t.Z)) / 2f;
        public float DistanceTo(GridVectorInt t)
        => DistanceTo((GridVector)t);
        public GridVectorInt AxialRound()
        => AxialRound_Internal(t => t.Round());
        public GridVectorInt AxialRoundUp()
        => AxialRound_Internal(t => t.RoundUp());
        public GridVectorInt AxialRoundDown()
        => AxialRound_Internal(t => t.RoundDown());
        public GridVectorInt AxialRoundAwayFromZero()
        => AxialRound_Internal(t => t.RoundAwayFromZero());
        public GridVectorInt AxialRoundTowardsZero()
        => AxialRound_Internal(t => t.RoundTowardsZero());

        // Privates
        public GridVectorInt AxialRound_Internal(Func<float, int> roundingFunc)
        {
            Vector3Int rounded = new(roundingFunc(X), roundingFunc(Y), roundingFunc(Z));
            return ((Vector3)this - rounded).Abs().MaxCompAxis() switch
            {
                Axes.X => FromYZ(rounded.y, rounded.z),
                Axes.Y => FromXZ(rounded.x, rounded.z),
                Axes.Z => FromXY(rounded.x, rounded.y),
                _ => Invalid,
            };
        }

        // Initializers
        public GridVector(float x, float y)
        {
            X = x;
            Y = y;
        }
        static public GridVector FromXY(float x, float y)
        => new() { X = x, Y = y };
        static public GridVector FromXZ(float x, float z)
        => new() { X = x, Y = -x - z };
        static public GridVector FromYZ(float y, float z)
        => new() { X = -y - z, Y = y };
        static public GridVector Zero
        => new() { X = 0f, Y = 0f };
        static public GridVector Invalid
        => new() { X = float.NaN, Y = float.NaN };

        // Overrides
        public override string ToString()
        => $"(X: {X:F2},  Y: {Y:F2})";

        // Operators
        public static GridVector operator +(GridVector a, GridVectorInt b)
        => new(a.X + b.X, a.Y + b.Y);
        public static GridVector operator -(GridVector a, GridVectorInt b)
        => new(a.X - b.X, a.Y - b.Y);
        public static GridVector operator *(GridVector a, GridVectorInt b)
        => new(a.X * b.X, a.Y * b.Y);
        public static GridVector operator /(GridVector a, GridVectorInt b)
        => new(a.X / b.X, a.Y / b.Y);
        public static GridVector operator %(GridVector a, GridVectorInt b)
        => new(a.X % b.X, a.Y % b.Y);

        public static GridVector operator +(GridVector a, int b)
        => new(a.X + b, a.Y + b);
        public static GridVector operator -(GridVector a, int b)
        => new(a.X - b, a.Y - b);
        public static GridVector operator *(GridVector a, int b)
        => new(a.X * b, a.Y * b);
        public static GridVector operator /(GridVector a, int b)
        => new(a.X / b, a.Y / b);
        public static GridVector operator %(GridVector a, int b)
        => new(a.X % b, a.Y % b);

        public static GridVector operator +(GridVector a, GridVector b)
        => new(a.X + b.X, a.Y + b.Y);
        public static GridVector operator -(GridVector a, GridVector b)
        => new(a.X - b.X, a.Y - b.Y);
        public static GridVector operator *(GridVector a, GridVector b)
        => new(a.X * b.X, a.Y * b.Y);
        public static GridVector operator /(GridVector a, GridVector b)
        => new(a.X / b.X, a.Y / b.Y);
        public static GridVector operator %(GridVector a, GridVector b)
        => new(a.X % b.X, a.Y % b.Y);

        public static GridVector operator +(GridVector a, float b)
        => new(a.X + b, a.Y + b);
        public static GridVector operator -(GridVector a, float b)
        => new(a.X - b, a.Y - b);
        public static GridVector operator *(GridVector a, float b)
        => new(a.X * b, a.Y * b);
        public static GridVector operator /(GridVector a, float b)
        => new(a.X / b, a.Y / b);
        public static GridVector operator %(GridVector a, float b)
        => new(a.X % b, a.Y % b);


        static public implicit operator GridVectorInt(GridVector t)
        => new((int)t.X, (int)t.Y);
        static public implicit operator Vector3(GridVector t)
        => new(t.X, t.Y, t.Z);
    }
}