using Vheos.Games.Core;

namespace Vheos.Games.Prototypes.ShapeTracer
{
    public enum GridType
    {
        Triangle,
        Square,
    }

    public enum Space
    {
        World,
        Grid,
    }

    public enum GridDirection
    {
        RightDown = Axes.XY | AxisDirection.Positive,
        Right = Axes.XZ | AxisDirection.Positive,
        RightUp = Axes.YZ | AxisDirection.Positive,
        LeftUp = Axes.XY | AxisDirection.Negative,
        Left = Axes.XZ | AxisDirection.Negative,
        LeftDown = Axes.YZ | AxisDirection.Negative,

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