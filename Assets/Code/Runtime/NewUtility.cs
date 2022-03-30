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
        static public float DecimalPart(this float t)
        => t % 1f;
        static public int IntegralPart(this float t)
        => (int)t;

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
        static public GridDirection ToGridDirection(this Axes t, AxisDirection a = AxisDirection.Positive)
        => t switch
        {
            Axes.XY when a == AxisDirection.Positive => GridDirection.RightDown,
            Axes.X when a == AxisDirection.Positive => GridDirection.Right,
            Axes.Y when a == AxisDirection.Positive => GridDirection.RightUp,
            Axes.XY when a == AxisDirection.Negative => GridDirection.LeftUp,
            Axes.X when a == AxisDirection.Negative => GridDirection.Left,
            Axes.Y when a == AxisDirection.Negative => GridDirection.LeftDown,
            _ => GridDirection.Invalid,
        };
        static public GridVectorInt ToGridVectorInt(this GridDirection t)
        => t switch
        {
            GridDirection.RightUp => new(0, +1),
            GridDirection.Right => new(+1, 0),
            GridDirection.RightDown => new(+1, -1),
            GridDirection.LeftDown => new(0, -1),
            GridDirection.Left => new(-1, 0),
            GridDirection.LeftUp => new(-1, +1),
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