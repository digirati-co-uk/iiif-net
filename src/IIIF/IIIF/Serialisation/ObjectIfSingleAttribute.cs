using System;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Signifies that an object should be serialized as a single object if .Count == 1.
    /// Else, serialize as an array. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ObjectIfSingleAttribute : Attribute
    {
    }
}