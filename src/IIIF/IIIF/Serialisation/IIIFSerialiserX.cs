using System.Collections.Generic;
using IIIF.Serialisation.Deserialisation;
using Newtonsoft.Json;

namespace IIIF.Serialisation
{
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
            Converters = new List<JsonConverter>
            {
                new SizeConverter(), new StringArrayConverter(), new ServiceReferenceConverter(),
                new ThumbnailConverter(), new ImageService2Serialiser()
            }
        };

        private static readonly JsonSerializerSettings DeserializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new PrettyIIIFContractResolver(),
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter>
            {
                new AnnotationV3Converter(), new ResourceBaseV3Converter(), new StructuralLocationConverter(),
                new ExternalResourceConverter(), new PaintableConverter(), new SelectorConverter(),
                new ServiceConverter()
            }
        };

        /// <summary>
        /// Serialise specified iiif resource to json string.
        /// </summary>
        /// <param name="iiifResource">IIIF resource to serialise.</param>
        /// <returns>JSON string representation of iiif resource.</returns>
        public static string AsJson(this JsonLdBase iiifResource)
            => JsonConvert.SerializeObject(iiifResource, SerializerSettings);

        /// <summary>
        /// Deserialize specified iiif resource from json string.
        /// </summary>
        /// <param name="iiifResource">IIIF resource to deserialize.</param>
        /// <typeparam name="TTarget">Type of object to deserialize to.</typeparam>
        /// <returns></returns>
        public static TTarget FromJson<TTarget>(this string iiifResource)
            where TTarget : JsonLdBase
            => JsonConvert.DeserializeObject<TTarget>(iiifResource, DeserializerSettings);
    }
}