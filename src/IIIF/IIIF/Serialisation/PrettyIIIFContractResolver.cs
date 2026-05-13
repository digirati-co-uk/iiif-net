using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace IIIF.Serialisation;

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
            property.ShouldSerialize = instance =>
            {
                var o = instance
                    .GetType()
                    .GetProperty(member.Name)
                    ?.GetValue(instance);
                return o != null && (int)o != 0;
            };

        if (member.GetCustomAttribute<CamelCaseEnumAttribute>() != null)
            property.Converter = EnumCamelCaseValueConverter;

        if (member.GetCustomAttribute<EnumAsStringAttribute>() != null) property.Converter = EnumStringValueConverter;

        // Don't serialise empty lists, unless they have the [RequiredOutput] attribute
        if (pType.IsGenericType && pType.GetGenericTypeDefinition() == typeof(List<>))
        {
            property.ShouldSerialize = instance =>
            {
                IList? list = null;
                if (member.MemberType == MemberTypes.Property)
                    list = instance
                        .GetType()
                        .GetProperty(member.Name)
                        ?.GetValue(instance, null) as IList;
                var hasContent = list != null && list.Count > 0;
                var requiredOutputAttr = member.GetCustomAttribute<RequiredOutputAttribute>();
                return hasContent || requiredOutputAttr != null;
            };

            if (member.GetCustomAttribute<ObjectIfSingleAttribute>() != null)
                property.Converter = ObjectIfSingleConverter;
        }

        return property;
    }

    // Wraps the [JsonExtensionData] setter so that JSON keys which already correspond to a known property on the type
    // are not also stored in AdditionalProperties.
    // Without this, getter-only overrides (e.g. PaintingAnnotation.Motivation) can't be set so end up in
    // AdditionalProperties during deserialisation, causing the value to be emitted twice on re-serialisation.
    // Newtonsoft caches the contract per type, so this runs once per type, not per instance.
    protected override JsonObjectContract CreateObjectContract(Type objectType)
    {
        var contract = base.CreateObjectContract(objectType);

        // Nothing to do if the type has no [JsonExtensionData] member.
        if (contract.ExtensionDataSetter == null) return contract;

        // Get every JSON property name the contract already knows about (already camelCased by the base resolver)
        var knownPropertyNames = new HashSet<string>(
            contract.Properties.Select(p => p.PropertyName!),
            StringComparer.OrdinalIgnoreCase);

        // Update [JsonExtensionData] to drop keys that correspond to a real prop. Meaning only real 'additional' props
        // are stored in AdditionalProperties.
        var originalSetter = contract.ExtensionDataSetter;
        contract.ExtensionDataSetter = (obj, key, value) =>
        {
            if (!knownPropertyNames.Contains(key)) originalSetter(obj, key, value);
        };

        return contract;
    }
}