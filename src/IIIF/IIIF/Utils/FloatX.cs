using System;

namespace IIIF.Utils;

public static class FloatX
{
    internal static int PercentOf(this float percentage, int value)
        => Convert.ToInt32(value * (percentage / 100));
}