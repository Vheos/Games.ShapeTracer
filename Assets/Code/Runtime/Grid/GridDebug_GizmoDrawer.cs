#if UNITY_EDITOR
namespace Vheos.Games.ShapeTracer.Editor
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;
    using Grid = ShapeTracer.Grid;

    public static class GridDebug_GizmoDrawer
    {
        private const float LINE_HALF_LENGTH = 1e5f;

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected | GizmoType.Pickable)]
        static void Pickable(GridDebug gridDebug, GizmoType type)
        {
            if (!gridDebug.TryGet(out Grid grid)
            || !grid.isActiveAndEnabled)
                return;

            // Cache
            Vector3 horizontalFrom = LINE_HALF_LENGTH.Append(0, 0);
            Vector3 horizontalTo = LINE_HALF_LENGTH.Neg().Append(0, 0);
            Quaternion diagonalRotation = Quaternion.Euler(0, 0, 120f);
            Vector3 diagonalFrom = horizontalFrom.Rotate(diagonalRotation);
            Vector3 diagonalTo = horizontalTo.Rotate(diagonalRotation);
            float height = 3.Sqrt() / 2f;
            Color opacityColor = new(1f, 1f, 1f, gridDebug.Opacity);

            // Horizontal
            Gizmos.color = gridDebug.ColorX * opacityColor;
            for (int i = -grid.Radius; i <= +grid.Radius; i++)
                NewUtility.GizmosDrawLine(grid.transform, horizontalFrom.Add(0, i * height, 0), horizontalTo.Add(0, i * height, 0));

            // Diagonal 1
            Gizmos.color = gridDebug.ColorY * opacityColor;
            for (int i = -grid.Radius; i <= +grid.Radius; i++)
                NewUtility.GizmosDrawLine(grid.transform, diagonalFrom.Add(i, 0, 0), diagonalTo.Add(i, 0, 0));

            // Diagonal 2
            diagonalFrom = diagonalFrom.Rotate(diagonalRotation);
            diagonalTo = diagonalTo.Rotate(diagonalRotation);
            Gizmos.color = gridDebug.ColorZ * opacityColor;
            for (int i = -grid.Radius; i <= +grid.Radius; i++)
                NewUtility.GizmosDrawLine(grid.transform, diagonalFrom.Add(i, 0, 0), diagonalTo.Add(i, 0, 0));

            Gizmos.color = Color.green;
            for (int ix = -grid.Radius; ix <= +grid.Radius;  ix++)
            {
                int fromY = grid.Radius.Neg().ClampMin(-ix - grid.Radius);
                int toY = grid.Radius.ClampMax( -ix + grid.Radius);
                for (int iy = fromY; iy <= toY; iy++)
                {
                    Vector3 worldPosition = Grid.GridToWorldPosition(new(ix, iy));
                    if (!Application.isPlaying)
                        worldPosition = worldPosition.Transform(grid.transform);
                    //Gizmos.DrawWireSphere(worldPosition, 0.1f);
                }
            }
        }
    }
}
#endif