using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation.Deserialisation;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation;

/// <summary>
/// Extension methods to aid with serialisation and deserialisation
/// </summary>
public static class IIIFSerialiserX
{
    /// <summary>
    /// Cache of all properties on a type, avoids reflecting the same type multiple times
    /// </summary>
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypePropertyCache = new();
    
    public static JsonSerializerSettings SerializerSettings { get; set; } = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new PrettyIIIFContractResolver(),
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
        Converters = new List<JsonConverter>
        {
            new ImageService2Converter(), new SizeConverter(), new StringArrayConverter(),
            new ServiceReferenceConverter(), new ThumbnailConverter()
        }
    };

    public static JsonSerializerSettings DeserializerSettings { get; set; } = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new PrettyIIIFContractResolver(),
        Formatting = Formatting.Indented,
        Converters = new List<JsonConverter>
        {
            new ExternalResourceConverter(), new ImageService2Converter(), new AnnotationV3Converter(),
            new StructuralLocationConverter(), new PaintableConverter(),
            new SelectorConverter(), new ResourceBaseV3Converter(),  new ServiceConverter(),
            new CollectionItemConverter(), new ResourceConverter(), new GeometryConverter()
        }
    };

    /// <summary>
    /// Serialise specified iiif resource to json string.
    /// </summary>
    /// <param name="iiifResource">IIIF resource to serialise.</param>
    /// <returns>JSON string representation of iiif resource.</returns>
    public static string AsJson(this JsonLdBase iiifResource)
    {
        return JsonConvert.SerializeObject(iiifResource, SerializerSettings);
    }

    /// <summary>
    /// Serialise specified iiif resource to stream.
    /// </summary>
    /// <param name="iiifResource">IIIF resource to serialise.</param>
    /// <param name="stream">Stream to serialise object to</param>
    /// <returns>Stream representation of iiif resource json.</returns>
    public static void AsJsonStream(this JsonLdBase iiifResource, Stream stream)
    {
        using var sw = new StreamWriter(stream, leaveOpen: true);
        using var writer = new JsonTextWriter(sw);
        var serializer = JsonSerializer.Create(SerializerSettings);
        serializer.Serialize(writer, iiifResource);
        writer.Flush();
    }

    /// <summary>
    /// Deserialize specified iiif resource from json string.
    /// </summary>
    /// <param name="iiifResource">IIIF resource to deserialize.</param>
    /// <typeparam name="TTarget">Type of object to deserialize to.</typeparam>
    /// <returns></returns>
    public static TTarget? FromJson<TTarget>(this string iiifResource)
        where TTarget : JsonLdBase
    {
        return JsonConvert.DeserializeObject<TTarget>(iiifResource, DeserializerSettings);
    }

    /// <summary>
    /// Deserialize specified iiif resource from stream containing json
    /// </summary>
    /// <param name="iiifResource">IIIF resource to deserialize.</param>
    /// <typeparam name="TTarget">Type of object to deserialize to.</typeparam>
    /// <returns></returns>
    public static TTarget? FromJsonStream<TTarget>(this Stream iiifResource)
        where TTarget : JsonLdBase
    {
        using var sr = new StreamReader(iiifResource);
        using var reader = new JsonTextReader(sr);
        var serializer = JsonSerializer.Create(DeserializerSettings);
        return serializer.Deserialize<TTarget>(reader);
    }

    /// <summary>
    /// Removes ALL entries from <see cref="JsonLdBase.AdditionalProperties"/> on this resource and every
    /// descendant <see cref="JsonLdBase"/> in the object graph, producing output containing only
    /// known, spec-modelled properties when serialised.
    /// </summary>
    /// <remarks>Mutates the object in place and returns it for chaining.</remarks>
    public static T WithoutAdditionalProperties<T>(this T resource)
        where T : JsonLdBase
    {
        StripAdditionalProperties(resource, null, new HashSet<object>(ReferenceEqualityComparer.Instance));
        return resource;
    }

    /// <summary>
    /// Removes the specified <paramref name="keys"/> from <see cref="JsonLdBase.AdditionalProperties"/> on this
    /// resource and every descendant <see cref="JsonLdBase"/> in the object graph.
    /// </summary>
    /// <remarks>Mutates the object in place and returns it for chaining.</remarks>
    public static T WithoutAdditionalProperties<T>(this T resource, params string[] keys)
        where T : JsonLdBase
    {
        if (keys.Length == 0) return resource;
        StripAdditionalProperties(resource, new HashSet<string>(keys, StringComparer.Ordinal),
            new HashSet<object>(ReferenceEqualityComparer.Instance));
        return resource;
    }

    private static void StripAdditionalProperties(
        JsonLdBase node,
        HashSet<string>? keys,
        HashSet<object> visited)
    {
        if (!visited.Add(node)) return;

        if (keys is null)
            node.AdditionalProperties.Clear();
        else
            foreach (var key in keys)
                node.AdditionalProperties.Remove(key);

        foreach (var prop in GetCachedProperties(node.GetType()))
        {
            var val = prop.GetValue(node);

            switch (val)
            {
                case JsonLdBase child:
                    StripAdditionalProperties(child, keys, visited);
                    break;

                case IEnumerable enumerable when val is not IDictionary<string, JToken>:
                    foreach (var element in enumerable)
                    {
                        // Collections are often typed as interfaces (e.g. List<ICollectionItem>,
                        // List<IService>, List<IPaintable>) where the concrete elements are
                        // JsonLdBase subclasses. The cast here handles those — non-JsonLdBase
                        // elements (strings, ints, etc.) are silently skipped.
                        if (element is JsonLdBase childNode)
                            StripAdditionalProperties(childNode, keys, visited);
                    }
                    break;
            }
        }
    }
    
    private static PropertyInfo[] GetCachedProperties(Type type) =>
        TypePropertyCache.GetOrAdd(type, static t =>
            t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                .ToArray());
}