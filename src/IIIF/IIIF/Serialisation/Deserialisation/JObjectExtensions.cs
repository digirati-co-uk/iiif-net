using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace IIIF.Serialisation.Deserialisation;

internal static class JObjectExtensions
{
    public static void RemoveEmptyArraysForObjectProperties(
        this JObject jsonObject,
        JsonSerializer serializer,
        Type targetType)
    {
        var contract = serializer.ContractResolver.ResolveContract(targetType) as JsonObjectContract;
        if (contract == null) return;

        var objectPropertyNames = contract.Properties
            .Where(p => !p.Ignored)
            .Where(p => p.PropertyType != null)
            .Where(p => p.PropertyType != typeof(string))
            .Where(p => !typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
            .Where(p => !p.PropertyType.IsValueType)
            .Select(p => p.PropertyName)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToHashSet(StringComparer.Ordinal);

        foreach (var prop in jsonObject.Properties()
                     .Where(p => objectPropertyNames.Contains(p.Name) && p.Value is JArray { Count: 0 })
                     .ToList())
        {
            prop.Remove();
        }
    }
}