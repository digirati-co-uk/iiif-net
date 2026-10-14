using System;

namespace IIIF.Exceptions;

/// <summary>
/// Exception thrown when a size is invalid.
/// </summary>
public class SizeException : ArgumentException
{
    public SizeException()
    {
    }

    public SizeException(string message) : base(message)
    {
    }

    public SizeException(string message, Exception inner) : base(message, inner)
    {
    }
}
