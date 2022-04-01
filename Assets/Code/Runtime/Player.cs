namespace Vheos.Games.ShapeTracer
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Games.Core;
    using Tools.Extensions.General;
    using Tools.Extensions.UnityObjects;
    using Tools.Extensions.Math;

    [RequireComponent(typeof(Updatable))]
    [RequireComponent(typeof(Selecter))]
    public class Player : AStaticComponent<GlobalStuff>
    {
        // Publics
        static public int CoinsCount
        {
            get => _coinsCount;
            set
            {
                int previousCoinsCount = _coinsCount;
                _coinsCount = value.ClampMin(0);

                if (_coinsCount != previousCoinsCount)
                    OnChangeCoinsCount.Invoke(previousCoinsCount, _coinsCount);
            }
        }

        // Events
        static public AutoEvent<int, int> OnChangeCoinsCount;

        // Privates
        static private int _coinsCount;
        static private Selectable SelectableUnderCursor
        => RaycastableManager.FindClosest<Selectable>(Mouse.current.position.ReadValue());
        private void HandleCursorEvents()
        {
            if (!Get<Selecter>().IsHolding)
                Get<Selecter>().Selectable = SelectableUnderCursor;

            if (Mouse.current.leftButton.wasPressedThisFrame)
                Get<Selecter>().TryPress();
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
                Get<Selecter>().TryRelease(Get<Selecter>().Selectable == SelectableUnderCursor);
        }

        // Play
        protected override void PlayAwake()
        {
            base.PlayAwake();
            Get<Updatable>().OnUpdate.SubEnableDisable(this, HandleCursorEvents);
            OnChangeCoinsCount = new();
            _coinsCount = 0;
        }
    }
}