using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace IIIF.Utils;

internal static class CollectionX
{
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this List<T>? collection)
    {
        return collection == null || collection.Count == 0;
    }
}