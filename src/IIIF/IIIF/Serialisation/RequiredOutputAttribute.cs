using System;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Any list property decorated with this attribute will be serialized to json as '[]' if empty.
    /// Default behaviour is to not output empty lists.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredOutputAttribute : Attribute
    {
    }
}