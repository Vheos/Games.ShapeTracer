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

    [RequireComponent(typeof(Selectable))]
    public class Coin : ABaseComponent
    {
        // Publics
        public GridTriangle Triangle
        { get; private set; }
        public void Expand( bool instantly = false)
        {

            Get<Selectable>().Enable();
            this.NewTween(ConflictResolution.Interrupt)
                .SetDuration(0.8f)
                .LocalRotation(transform.localRotation.Add(new Vector3(0f, 0f,-180f)))
                .LocalScaleRatio(2f)
                .Alpha(ColorComponent.SpriteRenderer, 1f)
                .FinishIf(instantly);
        }
        public void Hide(bool instantly = false)
        {
            Get<Selectable>().Disable();
            this.NewTween(ConflictResolution.Interrupt)
                .SetDuration(0.8f)
                .LocalRotation(transform.localRotation.Add(new Vector3(0f, 0f, 180f)))
                .LocalScaleRatio(0.5f)
                .Alpha(ColorComponent.SpriteRenderer, 0f)
                .AddEventsOnFinish(() => CoinPool.Release(this))
                .FinishIf(instantly);
        }

        // Privates
        private void Selectable_OnRelease(Selecter selecter, bool isFullClick)
        {
            if (!isFullClick)
                return;

            Hide();
        }

        // Play
        public void Initialize(GridTriangle triangle)
        {
            Triangle = triangle;

            Quaternion startingRotation = Quaternion.Euler(0f, 0f, 90f * triangle.IsEven.ToSign().Neg());
            transform.SetPositionAndRotation(Triangle.WorldPosition, startingRotation);
            transform.localScale = CoinPool.Instance.Prefab.transform.localScale * 0.5f;
            Get<SpriteRenderer>().color = Get<SpriteRenderer>().color.NewA(0f);

            Expand();
        }
        protected override void PlayAwake()
        {
            base.PlayAwake();
            Get<Selectable>().OnRelease.SubEnableDisable(this, Selectable_OnRelease);
        }
    }
}