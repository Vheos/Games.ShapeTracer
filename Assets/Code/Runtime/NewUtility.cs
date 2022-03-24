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
    using Vheos.Tools.Utilities;

    static public class NewUtility
    {
        // Extensions
        static public Vector3Int Add(this Vector3Int t, Vector3Int a)
        => new(t.x.Add(a.x), t.y.Add(a.y), t.z.Add(a.z));
        static public Vector3Int Add(this Vector3Int t, int x, int y, int z)
        => new(t.x.Add(x), t.y.Add(y), t.z.Add(z));
        static public Vector3Int Add(this Vector3Int t, int a)
        => new(t.x.Add(a), t.y.Add(a), t.z.Add(a));
        static public Vector3Int Sub(this Vector3Int t, Vector3Int a)
        => new(t.x.Sub(a.x), t.y.Sub(a.y), t.z.Sub(a.z));
        static public Vector3Int Sub(this Vector3Int t, int x, int y, int z)
        => new(t.x.Sub(x), t.y.Sub(y), t.z.Sub(z));
        static public Vector3Int Sub(this Vector3Int t, int a)
        => new(t.x.Sub(a), t.y.Sub(a), t.z.Sub(a));
        static public Vector3Int Mul(this Vector3Int t, Vector3Int a)
        => new(t.x.Mul(a.x), t.y.Mul(a.y), t.z.Mul(a.z));
        static public Vector3Int Mul(this Vector3Int t, int x, int y, int z)
        => new(t.x.Mul(x), t.y.Mul(y), t.z.Mul(z));
        static public Vector3Int Mul(this Vector3Int t, int a)
        => new(t.x.Mul(a), t.y.Mul(a), t.z.Mul(a));
        static public Vector3 Div(this Vector3Int t, Vector3Int a)
        => new(t.x.Div(a.x), t.y.Div(a.y), t.z.Div(a.z));
        static public Vector3 Div(this Vector3Int t, int x, int y, int z)
        => new(t.x.Div(x), t.y.Div(y), t.z.Div(z));
        static public Vector3 Div(this Vector3Int t, int a)
        => new(t.x.Div(a), t.y.Div(a), t.z.Div(a));
        static public Vector3Int Mod(this Vector3Int t, Vector3Int a)
        => new(t.x.Mod(a.x), t.y.Mod(a.y), t.z.Mod(a.z));
        static public Vector3Int Mod(this Vector3Int t, int x, int y, int z)
        => new(t.x.Mod(x), t.y.Mod(y), t.z.Mod(z));
        static public Vector3Int Mod(this Vector3Int t, int a)
        => new(t.x.Mod(a), t.y.Mod(a), t.z.Mod(a));
        static public Vector3Int PosMod(this Vector3Int t, Vector3Int a)
        => new(t.x.PosMod(a.x), t.y.PosMod(a.y), t.z.PosMod(a.z));
        static public Vector3Int PosMod(this Vector3Int t, int x, int y, int z)
        => new(t.x.PosMod(x), t.y.PosMod(y), t.z.PosMod(z));
        static public Vector3Int PosMod(this Vector3Int t, int a)
        => new(t.x.PosMod(a), t.y.PosMod(a), t.z.PosMod(a));
        static public Vector3Int Min(this Vector3Int t, Vector3Int a)
        => new(t.x.Min(a.x), t.y.Min(a.y), t.z.Min(a.z));
        static public Vector3Int Min(this Vector3Int t, int x, int y, int z)
        => new(t.x.Min(x), t.y.Min(y), t.z.Min(z));
        static public Vector3Int Min(this Vector3Int t, int a)
        => new(t.x.Min(a), t.y.Min(a), t.z.Min(a));
        static public Vector3Int Max(this Vector3Int t, Vector3Int a)
        => new(t.x.Max(a.x), t.y.Max(a.y), t.z.Max(a.z));
        static public Vector3Int Max(this Vector3Int t, int x, int y, int z)
        => new(t.x.Max(x), t.y.Max(y), t.z.Max(z));
        static public Vector3Int Max(this Vector3Int t, int a)
        => new(t.x.Max(a), t.y.Max(a), t.z.Max(a));

        static public Vector3 Add(this Vector3Int t, Vector3 a)
        => new(t.x.Add(a.x), t.y.Add(a.y), t.z.Add(a.z));
        static public Vector3 Add(this Vector3Int t, float x, float y, float z)
        => new(t.x.Add(x), t.y.Add(y), t.z.Add(z));
        static public Vector3 Add(this Vector3Int t, float a)
        => new(t.x.Add(a), t.y.Add(a), t.z.Add(a));
        static public Vector3 Sub(this Vector3Int t, Vector3 a)
        => new(t.x.Sub(a.x), t.y.Sub(a.y), t.z.Sub(a.z));
        static public Vector3 Sub(this Vector3Int t, float x, float y, float z)
        => new(t.x.Sub(x), t.y.Sub(y), t.z.Sub(z));
        static public Vector3 Sub(this Vector3Int t, float a)
        => new(t.x.Sub(a), t.y.Sub(a), t.z.Sub(a));
        static public Vector3 Mul(this Vector3Int t, Vector3 a)
        => new(t.x.Mul(a.x), t.y.Mul(a.y), t.z.Mul(a.z));
        static public Vector3 Mul(this Vector3Int t, float x, float y, float z)
        => new(t.x.Mul(x), t.y.Mul(y), t.z.Mul(z));
        static public Vector3 Mul(this Vector3Int t, float a)
        => new(t.x.Mul(a), t.y.Mul(a), t.z.Mul(a));
        static public Vector3 Div(this Vector3Int t, Vector3 a)
        => new(t.x.Div(a.x), t.y.Div(a.y), t.z.Div(a.z));
        static public Vector3 Div(this Vector3Int t, float x, float y, float z)
        => new(t.x.Div(x), t.y.Div(y), t.z.Div(z));
        static public Vector3 Div(this Vector3Int t, float a)
        => new(t.x.Div(a), t.y.Div(a), t.z.Div(a));
        static public Vector3 Mod(this Vector3Int t, Vector3 a)
        => new(t.x.Mod(a.x), t.y.Mod(a.y), t.z.Mod(a.z));
        static public Vector3 Mod(this Vector3Int t, float x, float y, float z)
        => new(t.x.Mod(x), t.y.Mod(y), t.z.Mod(z));
        static public Vector3 Mod(this Vector3Int t, float a)
        => new(t.x.Mod(a), t.y.Mod(a), t.z.Mod(a));
        static public Vector3 PosMod(this Vector3Int t, Vector3 a)
        => new(t.x.PosMod(a.x), t.y.PosMod(a.y), t.z.PosMod(a.z));
        static public Vector3 PosMod(this Vector3Int t, float x, float y, float z)
        => new(t.x.PosMod(x), t.y.PosMod(y), t.z.PosMod(z));
        static public Vector3 PosMod(this Vector3Int t, float a)
        => new(t.x.PosMod(a), t.y.PosMod(a), t.z.PosMod(a));
        static public Vector3 Min(this Vector3Int t, Vector3 a)
        => new(t.x.Min(a.x), t.y.Min(a.y), t.z.Min(a.z));
        static public Vector3 Min(this Vector3Int t, float x, float y, float z)
        => new(t.x.Min(x), t.y.Min(y), t.z.Min(z));
        static public Vector3 Min(this Vector3Int t, float a)
        => new(t.x.Min(a), t.y.Min(a), t.z.Min(a));
        static public Vector3 Max(this Vector3Int t, Vector3 a)
        => new(t.x.Max(a.x), t.y.Max(a.y), t.z.Max(a.z));
        static public Vector3 Max(this Vector3Int t, float x, float y, float z)
        => new(t.x.Max(x), t.y.Max(y), t.z.Max(z));
        static public Vector3 Max(this Vector3Int t, float a)
        => new(t.x.Max(a), t.y.Max(a), t.z.Max(a));

        static public Vector2Int Abs(this Vector2Int t)
        => new(t.x.Abs(), t.y.Abs());
        static public int SumComp(this Vector2Int t)
        => t.x + t.y;
        static public Vector3Int Append(this Vector2Int t, int z = 0)
        => new(t.x, t.y, z);
        static public Vector3 Append(this Vector2Int t, float z)
        => new(t.x, t.y, z);

        static public Vector3Int Abs(this Vector3Int t)
        => new(t.x.Abs(), t.y.Abs(), t.z.Abs());
        static public int SumComp(this Vector3Int t)
        => t.x + t.y + t.z;

        static public Vector3Int AsInt(this Vector3 t)
        => new((int)t.x, (int)t.y, (int)t.z);
        static public Vector2Int ToVector2Int(this int a)
        => new(a, a);
        static public Vector3Int ToVector3Int(this int a)
        => new(a, a, a);
        static public Vector3Int MaxComps<T>(this IEnumerable<T> t, Func<T, Vector3Int> vectorGetter)
        {
            var enumerator = t.GetEnumerator();
            if (!enumerator.MoveNext())
                return default;

            Vector3Int r = vectorGetter(enumerator.Current);
            while (enumerator.MoveNext())
                r = r.Max(vectorGetter(enumerator.Current));
            return r;
        }

        static public float SumComp(this Vector3 t)
        => t.x + t.y + t.z;
        static public Axes MaxCompAxis(this Vector3 t)
        {
            if (t.x >= t.y && t.x >= t.z)
                return Axes.X;
            if (t.y >= t.z)
                return Axes.Y;
            return Axes.Z;
        }
        static public T Random<T>(this IEnumerable<T> t)
        {
            T r = default;
            int count = 0;
            foreach (T element in t)
                if (UnityEngine.Random.Range(0, ++count) == 0)
                    r = element;
            return r;
        }
        static public T Random<T>(this IList<T> t)
        => t[UnityEngine.Random.Range(0, t.Count)];
        static public bool IsEither<T>(this T t, params T[] elements) where T : IEquatable<T>
        {
            foreach (var element in elements)
                if (t.Equals(element))
                    return true;
            return false;
        }
        static public IEnumerable<T> LoopFirstElement<T>(this IEnumerable<T> t)
        => t.Any() ? t.Append(t.First()) : t;
        static public IEnumerable<T> Shift<T>(this IEnumerable<T> t, int a, bool wrap = false)
        {
            IEnumerator<T> enumerator = t.GetEnumerator();
            if (!t.Any())
                yield break;
            if (a < 1)
                while (enumerator.MoveNext())
                    yield return enumerator.Current;

            // Skip
            for (int i = 0; i < a; i++)
                enumerator.MoveNext();

            // Yield
            while (enumerator.MoveNext())
                yield return enumerator.Current;

            if (!wrap)
                yield break;

            // Wrap
            enumerator.Reset();
            for (int i = 0; i < a; i++)
            {
                enumerator.MoveNext();
                yield return enumerator.Current;
            }
        }
        static public IEnumerable<T> GetEnumValues<T>(bool removeDuplicates = false, bool removeNegatives = false) where T : Enum
        {
            IEnumerable<T> filteredValues = Utility.GetEnumValues<T>();
            if (removeDuplicates)
                filteredValues = filteredValues.Distinct();
            if (removeNegatives)
                filteredValues = filteredValues.Where(t => Convert.ToInt32(t) >= 0);
            return filteredValues;
        }

        // Editor
#if UNITY_EDITOR
        static public void GizmosDrawLine(Transform transform, Vector3 from, Vector3 to)
        {
            from = from.Transform(transform);
            to = to.Transform(transform);
            Gizmos.DrawLine(from, to);
        }
#endif
    }



    static public class GridVertex_Extensions
    {
        static public GridDirection GridDirection(this Axes t, AxisDirection a = AxisDirection.Positive)
        => t switch
        {
            Axes.XY when a == AxisDirection.Positive => ShapeTracer.GridDirection.RightDown,
            Axes.X when a == AxisDirection.Positive => ShapeTracer.GridDirection.Right,
            Axes.Y when a == AxisDirection.Positive => ShapeTracer.GridDirection.RightUp,
            Axes.XY when a == AxisDirection.Negative => ShapeTracer.GridDirection.LeftUp,
            Axes.X when a == AxisDirection.Negative => ShapeTracer.GridDirection.Left,
            Axes.Y when a == AxisDirection.Negative => ShapeTracer.GridDirection.LeftDown,
            _ => ShapeTracer.GridDirection.Invalid,
        };
        static public GridVectorInt Vector(this GridDirection t)
        => t switch
        {
            ShapeTracer.GridDirection.RightUp => new(0, +1),
            ShapeTracer.GridDirection.Right => new(+1, 0),
            ShapeTracer.GridDirection.RightDown => new(+1, -1),
            ShapeTracer.GridDirection.LeftDown => new(0, -1),
            ShapeTracer.GridDirection.Left => new(-1, 0),
            ShapeTracer.GridDirection.LeftUp => new(-1, +1),
            _ => GridVectorInt.Invalid,
        };
        static public Vector3Int VectorInt(this TriangleDirection t)
        => t switch
        {
            TriangleDirection.RightDown => new(+1, 00, 00),
            TriangleDirection.RightUp => new(+1, +1, 00),
            TriangleDirection.Up => new(00, +1, 00),
            TriangleDirection.LeftUp => new(00, +1, +1),
            TriangleDirection.LeftDown => new(00, 00, +1),
            TriangleDirection.Down => new(+1, 00, +1),
            _ => default,
        };
        static public Vector3 Vector(this TriangleDirection t)
        => t.VectorInt();
    }

    static public class Axis_Extensions
    {
        static public Vector3Int Vector3Int(this Axes t)
        => new(t.HasFlag(Axes.X).To01(),
               t.HasFlag(Axes.Y).To01(),
               t.HasFlag(Axes.Z).To01());
        static public Vector3 Vector3(this Axes t)
        => t.Vector3Int();
        static public Axes SetDirection(ref Axes t, AxisDirection a)
        => t |= (Axes)a;
        static public AxisDirection Direction(Axes t)
        => (AxisDirection)t & AxisDirection.Positive;
    }
}