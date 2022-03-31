namespace Vheos.Games.ShapeTracer
{
    using System;
    using UnityEngine;
    using Games.Core;
    using Tools.Extensions.Math;

    [RequireComponent(typeof(Selectable))]
    public class CommonSelectable : ABaseComponent
    {
        // Publics
        public void UpdateColorComponentType()
        => _colorComponentType = this.FindColorComponentType();
        public readonly Getter<float> HighlightScale = new();
        public readonly Getter<float> PressScale = new();

        // Privates
        private ColorComponent _colorComponentType;
        private void Selectable_OnGainHighlight(Selecter selecter, bool isFirst)
        {
            if (isFirst)
                this.NewTween()
                    .SetDuration(0.4f)
                    .LocalScaleRatio(HighlightScale)
                    .RGBRatio(_colorComponentType, 1.25f);
        }
        private void Selectable_OnLoseHighlight(Selecter selecter, bool isLast)
        {
            if (isLast)
                this.NewTween()
                    .SetDuration(0.4f)
                    .LocalScaleRatio(HighlightScale.Value.Inv())
                    .RGBRatio(_colorComponentType, 1.25f.Inv());
        }
        private void Selectable_OnPress(Selecter selecter)
        => this.NewTween()
            .SetDuration(0.1f)
            .LocalScaleRatio(PressScale)
            .RGBRatio(_colorComponentType, 0.75f);
        private void Selectable_OnRelease(Selecter selecter, bool isFullClick)
        => this.NewTween()
            .SetDuration(0.1f)
            .LocalScaleRatio(PressScale.Value.Inv())
            .RGBRatio(_colorComponentType, 0.75f.Inv());

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            HighlightScale.Set(() => 1.25f);
            PressScale.Set(() => 0.9f);
            Get<Selectable>().OnGainSelection.SubEnableDisable(this, Selectable_OnGainHighlight);
            Get<Selectable>().OnLoseSelection.SubEnableDisable(this, Selectable_OnLoseHighlight);
            Get<Selectable>().OnPress.SubEnableDisable(this, Selectable_OnPress);
            Get<Selectable>().OnRelease.SubEnableDisable(this, Selectable_OnRelease);
            UpdateColorComponentType();
        }
    }
}