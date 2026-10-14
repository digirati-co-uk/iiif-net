using System;

namespace IIIF.Exceptions;

/// <summary>
/// Exception thrown when a rotation is invalid.
/// </summary>
public class RotationException : ArgumentException
{
    public RotationException()
    {
    }

    public RotationException(string message) : base(message)
    {
    }

    public RotationException(string message, Exception inner) : base(message, inner)
    {
    }
}
