namespace Vheos.Games.ShapeTracer
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;

    public struct GridVector
    {
        // Fields
        public float X;
        public float Y;
        public float Z
        => -X - Y;

        // Math
        public Vector2 XY
        => this;
        public Vector3 XYZ
        => this;
        public float Length
        => XYZ.Abs().CompSum() / 2f;
        public GridVector Normalized
        => this / Length;
        public float Dot(GridVectorInt a)
        => XYZ.Mul(a.XYZ).CompSum();
        public float Dot(GridVector a)
        => XYZ.Mul(a.XYZ).CompSum();
        public GridVector PosMod(int a)
        => new(X.PosMod(a), Y.PosMod(a));
        public GridVector PosMod(float a)
        => new(X.PosMod(a), Y.PosMod(a));
        public float DistanceTo(GridVectorInt a)
        => (this - a).Length;
        public float DistanceTo(GridVector a)
        => (this - a).Length;
        public GridVectorInt AxialRound()
        {
            Vector3Int rounded = XYZ.Round();
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

        public static GridVector operator +(int a, GridVector b)
        => new(b.X + a, b.Y + a);
        public static GridVector operator -(int a, GridVector b)
        => new(b.X - a, b.Y - a);
        public static GridVector operator *(int a, GridVector b)
        => new(b.X * a, b.Y * a);
        public static GridVector operator /(int a, GridVector b)
        => new(b.X / a, b.Y / a);
        public static GridVector operator %(int a, GridVector b)
        => new(b.X % a, b.Y % a);

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

        public static GridVector operator +(float a, GridVector b)
        => new(b.X + a, b.Y + a);
        public static GridVector operator -(float a, GridVector b)
        => new(b.X - a, b.Y - a);
        public static GridVector operator *(float a, GridVector b)
        => new(b.X * a, b.Y * a);
        public static GridVector operator /(float a, GridVector b)
        => new(b.X / a, b.Y / a);
        public static GridVector operator %(float a, GridVector b)
        => new(b.X % a, b.Y % a);

        static public implicit operator GridVectorInt(GridVector t)
        => new((int)t.X, (int)t.Y);
        static public implicit operator Vector2(GridVector t)
        => new(t.X, t.Y);
        static public implicit operator Vector3(GridVector t)
        => new(t.X, t.Y, t.Z);
        static public implicit operator GridVector(Vector2 t)
        => new(t.x, t.y);
        static public implicit operator GridVector(Vector3 t)
        => new(t.x, t.y);
    }
}