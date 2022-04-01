namespace Vheos.Games.ShapeTracer
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;

    public struct GridVectorInt : IEquatable<GridVectorInt>
    {
        // Fields
        public int X;
        public int Y;
        public int Z
        => -X - Y;

        // Math
        public Vector2Int XY
        => this;
        public Vector3Int XYZ
        => this;
        public int Length
        => XYZ.Abs().CompSum() / 2;
        public GridVector Normalized
        => this / Length;
        public int Dot(GridVectorInt a)
        => XYZ.Mul(a.XYZ).CompSum();
        public float Dot(GridVector a)
        => XYZ.Mul(a.XYZ).CompSum();
        public GridVectorInt PosMod(int a)
        => new(X.PosMod(a), Y.PosMod(a));
        public GridVector PosMod(float a)
        => new(X.PosMod(a), Y.PosMod(a));
        public int DistanceTo(GridVectorInt a)
        => (this - a).Length;
        public float DistanceTo(GridVector a)
        => (this - a).Length;
        public GridVectorInt RotateCW()
        => new(-Y, -Z);
        public GridVectorInt RotateCW(GridVectorInt around)
        => (this - around).RotateCW() + around;
        public GridVector RotateCW(GridVector around)
        => (this - around).RotateCW() + around;
        public GridVectorInt RotateCCW()
        => new(-Z, -X);
        public GridVectorInt RotateCCW(GridVectorInt around)
        => (this - around).RotateCCW() + around;
        public GridVector RotateCCW(GridVector around)
        => (this - around).RotateCCW() + around;

        // Initializers
        public GridVectorInt(int x, int y)
        {
            X = x;
            Y = y;
        }
        static public GridVectorInt FromXY(int x, int y)
        => new() { X = x, Y = y };
        static public GridVectorInt FromXZ(int x, int z)
        => new() { X = x, Y = -x - z };
        static public GridVectorInt FromYX(int y, int x)
        => new() { X = x, Y = y };
        static public GridVectorInt FromYZ(int y, int z)
        => new() { X = -y - z, Y = y };
        static public GridVectorInt FromZX(int z, int x)
        => new() { X = x, Y = -x - z };
        static public GridVectorInt FromZY(int z, int y)
        => new() { X = -y - z, Y = y };
        static public GridVectorInt Zero
        => new() { X = 0, Y = 0 };
        static public GridVectorInt Invalid
        => new() { X = int.MinValue, Y = int.MinValue };

        // Overrides
        public override string ToString()
        => $"(X: {X},  Y: {Y})";

        // Operators
        public static GridVectorInt operator +(GridVectorInt a, GridVectorInt b)
        => new(a.X + b.X, a.Y + b.Y);
        public static GridVectorInt operator -(GridVectorInt a, GridVectorInt b)
        => new(a.X - b.X, a.Y - b.Y);
        public static GridVectorInt operator *(GridVectorInt a, GridVectorInt b)
        => new(a.X * b.X, a.Y * b.Y);
        public static GridVectorInt operator /(GridVectorInt a, GridVectorInt b)
        => new(a.X / b.X, a.Y / b.Y);
        public static GridVectorInt operator %(GridVectorInt a, GridVectorInt b)
        => new(a.X % b.X, a.Y % b.Y);

        public static GridVectorInt operator +(GridVectorInt a, int b)
        => new(a.X + b, a.Y + b);
        public static GridVectorInt operator -(GridVectorInt a, int b)
        => new(a.X - b, a.Y - b);
        public static GridVectorInt operator *(GridVectorInt a, int b)
        => new(a.X * b, a.Y * b);
        public static GridVectorInt operator /(GridVectorInt a, int b)
        => new(a.X / b, a.Y / b);
        public static GridVectorInt operator %(GridVectorInt a, int b)
        => new(a.X % b, a.Y % b);

        public static GridVectorInt operator +(int a, GridVectorInt b)
        => new(b.X + a, b.Y + a);
        public static GridVectorInt operator -(int a, GridVectorInt b)
        => new(b.X - a, b.Y - a);
        public static GridVectorInt operator *(int a, GridVectorInt b)
        => new(b.X * a, b.Y * a);
        public static GridVectorInt operator /(int a, GridVectorInt b)
        => new(b.X / a, b.Y / a);
        public static GridVectorInt operator %(int a, GridVectorInt b)
        => new(b.X % a, b.Y % a);

        public static GridVector operator +(GridVectorInt a, GridVector b)
        => new(a.X + b.X, a.Y + b.Y);
        public static GridVector operator -(GridVectorInt a, GridVector b)
        => new(a.X - b.X, a.Y - b.Y);
        public static GridVector operator *(GridVectorInt a, GridVector b)
        => new(a.X * b.X, a.Y * b.Y);
        public static GridVector operator /(GridVectorInt a, GridVector b)
        => new(a.X / b.X, a.Y / b.Y);
        public static GridVector operator %(GridVectorInt a, GridVector b)
        => new(a.X % b.X, a.Y % b.Y);

        public static GridVector operator +(GridVectorInt a, float b)
        => new(a.X + b, a.Y + b);
        public static GridVector operator -(GridVectorInt a, float b)
        => new(a.X - b, a.Y - b);
        public static GridVector operator *(GridVectorInt a, float b)
        => new(a.X * b, a.Y * b);
        public static GridVector operator /(GridVectorInt a, float b)
        => new(a.X / b, a.Y / b);
        public static GridVector operator %(GridVectorInt a, float b)
        => new(a.X % b, a.Y % b);

        public static GridVector operator +(float a, GridVectorInt b)
        => new(b.X + a, b.Y + a);
        public static GridVector operator -(float a, GridVectorInt b)
        => new(b.X - a, b.Y - a);
        public static GridVector operator *(float a, GridVectorInt b)
        => new(b.X * a, b.Y * a);
        public static GridVector operator /(float a, GridVectorInt b)
        => new(b.X / a, b.Y / a);
        public static GridVector operator %(float a, GridVectorInt b)
        => new(b.X % a, b.Y % a);

        static public implicit operator GridVector(GridVectorInt t)
        => new(t.X, t.Y);
        static public implicit operator Vector2Int(GridVectorInt t)
        => new(t.X, t.Y);
        static public implicit operator Vector3Int(GridVectorInt t)
        => new(t.X, t.Y, t.Z);
        static public implicit operator GridVectorInt(Vector2Int t)
        => new(t.x, t.y);
        static public implicit operator GridVectorInt(Vector3Int t)
        => new(t.x, t.y);

        // IEquatable
        static public bool operator ==(GridVectorInt t, GridVectorInt a)
        => t.Equals(a);
        static public bool operator !=(GridVectorInt t, GridVectorInt a)
        => !t.Equals(a);
        public bool Equals(GridVectorInt a)
        => X == a.X
        && Y == a.Y;
        public override bool Equals(object a)
        => a is not null
        && a is GridVectorInt vertex
        && Equals(vertex);
        public override int GetHashCode()
        => X.GetHashCode() ^ X.GetHashCode() << 2;
    }
}