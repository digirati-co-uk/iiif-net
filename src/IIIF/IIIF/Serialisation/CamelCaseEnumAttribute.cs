using System;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Any enum property decorated with this attribute will have a serialised value of it's string representation
    /// in camelCase (e.g. InvalidRequest => invalidRequest)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CamelCaseEnumAttribute : Attribute
    {
    }
    
    /// <summary>
    /// Any enum property decorated with this attribute will have a serialised value of it's string representation
    /// in camelCase (e.g. InvalidRequest => invalidRequest)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EnumAsStringAttribute : Attribute
    {
    }
}