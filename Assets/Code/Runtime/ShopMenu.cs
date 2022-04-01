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
    using Tools.Utilities;
    using TMPro;

    public class ShopMenu : ABaseComponent
    {
        // Inspector
        [field: SerializeField] public TextMeshProUGUI  CoinsCountText { get; private set; }

        // Private
        private void UpdateCoinsCount(int from, int to)
        {
            CoinsCountText.text = $"Coins: {to}";
        }

        // Play
        protected override void PlayStart()
        {
            base.PlayStart();
            Player.OnChangeCoinsCount.SubEnableDisable(this, UpdateCoinsCount);
            UpdateCoinsCount(0, 0);
        }
    }
}