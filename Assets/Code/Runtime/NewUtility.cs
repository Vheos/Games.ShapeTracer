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
        static public void Activate(this Component t)
        => t.gameObject.SetActive(true);
        static public void Deactivate(this Component t)
        => t.gameObject.SetActive(false);
        static public void Enable(this MonoBehaviour t)
        => t.enabled = true;
        static public void Disable(this MonoBehaviour t)
        => t.enabled = false;

        static public bool IsFullyTraced(this GridEdge t)
        => t.TraceInfo().State == TraceState.Full;
        static public bool IsFullyTraced(this GridTriangle t)
        {
            foreach (var edge in t.Edges)
                if (!edge.IsFullyTraced())
                    return false;
            return true;
        }
        static public EdgeTraceInfo TraceInfo(this GridEdge t)
        => Grid.GetTraceInfo(t);

        static public float InverseLerp(this Vector2 t, Vector2 a, Vector2 b)
        => (t - a).Dot((b - a).normalized);
        static public float InverseLerp(this Vector3 t, Vector3 a, Vector3 b)
        => (t - a).Dot((b - a).normalized);

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

    static public class GridDirections_Extensions
    {
        static public GridVectorInt ToOffset(this GridDirections t)
        => t switch
        {
            // Vertex directions
            GridDirections.XY => GridVectorInt.FromXY(+1, -1),
            GridDirections.YZ => GridVectorInt.FromYZ(+1, -1),
            GridDirections.ZX => GridVectorInt.FromZX(+1, -1),
            GridDirections.NegXY => GridVectorInt.FromXY(-1, +1),
            GridDirections.NegYZ => GridVectorInt.FromYZ(-1, +1),
            GridDirections.NegZX => GridVectorInt.FromZX(-1, +1),
            // Triangle directions
            GridDirections.XX => GridVectorInt.FromXY(+2, -1),
            GridDirections.YY => GridVectorInt.FromYZ(+2, -1),
            GridDirections.ZZ => GridVectorInt.FromZX(+2, -1),
            GridDirections.NegXX => GridVectorInt.FromXY(-2, +1),
            GridDirections.NegYY => GridVectorInt.FromYZ(-2, +1),
            GridDirections.NegZZ => GridVectorInt.FromZX(-2, +1),

            _ => GridVectorInt.Invalid,
        };
        static public GridDirections RotateAxisCW(this GridDirections t)
        => (t & ~GridDirections.Negative) switch
        {
            // Vertex directions
            GridDirections.XY => GridDirections.YZ,
            GridDirections.YZ => GridDirections.ZX,
            GridDirections.ZX => GridDirections.XY,
            // Triangle directions
            GridDirections.XX => GridDirections.YY,
            GridDirections.YY => GridDirections.ZZ,
            GridDirections.ZZ => GridDirections.XX,
            _ => default,
        };
        static public GridDirections RotateAxisCCW(this GridDirections t)
        => (t & ~GridDirections.Negative) switch
        {
            // Vertex directions
            GridDirections.XY => GridDirections.ZX,
            GridDirections.YZ => GridDirections.XY,
            GridDirections.ZX => GridDirections.YZ,
            // Triangle directions
            GridDirections.XX => GridDirections.ZZ,
            GridDirections.YY => GridDirections.XX,
            GridDirections.ZZ => GridDirections.YY,
            _ => default,
        };
        static public GridDirections RotateCW(this GridDirections t)
        => t.RotateAxisCW() | ~(t & GridDirections.Negative);
        static public GridDirections RotateCCW(this GridDirections t)
        => t.RotateAxisCCW() | ~(t & GridDirections.Negative);
        static public IEnumerable<GridDirections> NeighborDirections(this GridDirections t)
        {
            yield return t.RotateCCW();
            yield return t.RotateCW();
        }
    }
}