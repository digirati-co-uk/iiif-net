using System;

namespace IIIF.Exceptions;

/// <summary>
/// Exception thrown when a region is invalid.
/// </summary>
public class RegionException : ArgumentException
{
    public RegionException()
    {
    }

    public RegionException(string message) : base(message)
    {
    }

    public RegionException(string message, Exception inner) : base(message, inner)
    {
    }
}
