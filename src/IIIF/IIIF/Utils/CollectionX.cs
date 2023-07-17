﻿using System.Collections.Generic;

namespace IIIF.Utils;

internal static class CollectionX
{
    public static bool IsNullOrEmpty<T>(this List<T> collection)
    {
        return collection == null || collection.Count == 0;
    }
}