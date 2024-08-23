using System.Collections.Generic;
using System.IO;
using IIIF.Serialisation.Deserialisation;
using Newtonsoft.Json;

namespace IIIF.Serialisation;

/// <summary>
/// Extension methods to aid with serialisation and deserialisation
/// </summary>
public static class IIIFSerialiserX
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
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

    private static readonly JsonSerializerSettings DeserializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new PrettyIIIFContractResolver(),
        Formatting = Formatting.Indented,
        Converters = new List<JsonConverter>
        {
            new ImageService2Converter(), new AnnotationV3Converter(), new ResourceBaseV3Converter(),
            new StructuralLocationConverter(), new ExternalResourceConverter(), new PaintableConverter(),
            new SelectorConverter(), new ServiceConverter(), new ResourceConverter(), new CollectionItemConverter()
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
    public static TTarget FromJson<TTarget>(this string iiifResource)
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
    public static TTarget FromJsonStream<TTarget>(this Stream iiifResource)
        where TTarget : JsonLdBase
    {
        using var sr = new StreamReader(iiifResource);
        using var reader = new JsonTextReader(sr);
        var serializer = JsonSerializer.Create(DeserializerSettings);
        return serializer.Deserialize<TTarget>(reader);
    }
}