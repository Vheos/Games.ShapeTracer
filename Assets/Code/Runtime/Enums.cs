using Vheos.Games.Core;

namespace Vheos.Games.ShapeTracer
{
    public enum GridDirection
    {
        RightDown = Axes.XY | AxisDirection.Positive,
        Right = Axes.X | AxisDirection.Positive,
        RightUp = Axes.Y | AxisDirection.Positive,
        LeftUp = Axes.XY | AxisDirection.Negative,
        Left = Axes.X | AxisDirection.Negative,
        LeftDown = Axes.Y | AxisDirection.Negative,

        DownRight = RightDown,
        UpRight = RightUp,
        UpLeft = LeftUp,
        DownLeft = LeftDown,

        Invalid = -1,
    }

    public enum TriangleDirection
    {
        RightDown,
        RightUp,
        Up,
        LeftUp,
        LeftDown,
        Down,

        DownRight = RightDown,
        UpRight = RightUp,
        UpLeft = LeftUp,
        DownLeft = LeftDown,

        Invalid = -1,
    }
}