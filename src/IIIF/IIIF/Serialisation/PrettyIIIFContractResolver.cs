using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Resolves mappings for IIIF objects.
    /// </summary>
    public class PrettyIIIFContractResolver : CamelCasePropertyNamesContractResolver
    {
        // adapted from https://stackoverflow.com/a/34903827
        private static readonly ObjectIfSingleConverter ObjectIfSingleConverter = new();

        private static readonly EnumCamelCaseValueConverter EnumCamelCaseValueConverter = new();
        private static readonly EnumStringValueConverter EnumStringValueConverter = new();

        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var pType = property.PropertyType;
            if (pType == null) return property;
            
            // Don't serialise Width or Height if they have a zero value
            if (member.Name is "Width" or "Height")
            {
                property.ShouldSerialize = instance =>
                {
                    var o = instance
                        .GetType()
                        .GetProperty(member.Name)
                        ?.GetValue(instance);
                    return o != null && (int) o != 0;
                };
            }
            
            if (member.GetCustomAttribute<CamelCaseEnumAttribute>() != null)
            {
                property.Converter = EnumCamelCaseValueConverter;
            }
            
            if (member.GetCustomAttribute<EnumAsStringAttribute>() != null)
            {
                property.Converter = EnumStringValueConverter;
            }
            
            // Don't serialise empty lists, unless they have the [RequiredOutput] attribute
            if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(List<>))
            {
                property.ShouldSerialize = instance =>
                {
                    IList? list = null;
                    if (member.MemberType == MemberTypes.Property)
                    {
                        list = instance
                            .GetType()
                            .GetProperty(member.Name)
                            ?.GetValue(instance, null) as IList;
                    }
                    var hasContent = list != null && list.Count > 0;
                    var requiredOutputAttr = member.GetCustomAttribute<RequiredOutputAttribute>();
                    return hasContent || requiredOutputAttr != null;
                };

                if (member.GetCustomAttribute<ObjectIfSingleAttribute>() != null)
                {
                    property.Converter = ObjectIfSingleConverter;
                }
            }
            return property;
        }
    }
}