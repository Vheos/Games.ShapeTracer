namespace Vheos.Games.ShapeTracer
{
    using System;
    using System.Linq;
    using UnityEngine;
    using Games.Core;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;

    [DisallowMultipleComponent]
    public class RaycastableManager : AStaticManager<RaycastableManager, Raycastable>
    {
        static public T FindClosest<T>(Vector2 screenPosition) where T : Component
        {
            T closestComponent = null;
            float minDistance = float.PositiveInfinity;
            foreach (var raycastable in ActiveComponents.Where(t => t.Has<T>()))
            {
                if (raycastable.Collider.Raycast(GlobalCamera.UnityCamera.ScreenPointToRay(screenPosition), out var hitInfo, float.PositiveInfinity)
                && raycastable.PerformRaycastTests(hitInfo.point))
                {
                    float distance = hitInfo.point.DistanceTo(GlobalCamera.Instance);
                    if (distance < minDistance)
                    {
                        closestComponent = raycastable.Get<T>();
                        minDistance = distance;
                    }
                }
            }
            return closestComponent;
        }
        static public T FindClosest<T>(GameObject pointer) where T : Component
        => FindClosest<T>(GlobalCamera.UnityCamera.WorldToScreenPoint(pointer.transform.position));
        static public T FindClosest<T>(Component pointer) where T : Component
        => FindClosest<T>(GlobalCamera.UnityCamera.WorldToScreenPoint(pointer.transform.position));
    }
}