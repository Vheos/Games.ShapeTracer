namespace Vheos.Games.ShapeTracer
{
    using System;
    using Vheos.Games.Core;

    public enum TraceState
    {
        None,
        Partial,
        Full,
    }

    [Flags]
    public enum GridDirections
    {
        X = Axes.X,
        Y = Axes.Y,
        Z = Axes.Z,
        Negative = 1 << 3,

        XX = X | X,
        XY = X | Y,
        XZ = X | Z,
        YX = Y | X,
        YY = Y | Y,
        YZ = Y | Z,
        ZX = Z | X,
        ZY = Z | Y,
        ZZ = Z | Z,

        NegXX = XX | Negative,
        NegXY = XY | Negative,
        NegXZ = XZ | Negative,
        NegYX = YX | Negative,
        NegYY = YY | Negative,
        NegYZ = YZ | Negative,
        NegZX = ZX | Negative,
        NegZY = ZY | Negative,
        NegZZ = ZZ | Negative,
    }
}