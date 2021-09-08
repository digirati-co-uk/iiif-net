using System.Collections.Generic;
using Newtonsoft.Json;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Extension methods to aid with serialisation
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

        /// <summary>
        /// Serialise specified iiif resource to json string.
        /// </summary>
        /// <param name="iiifResource">IIIF resource to serialise.</param>
        /// <returns>JSON string representation of iiif resource.</returns>
        public static string AsJson(this JsonLdBase iiifResource)
            => JsonConvert.SerializeObject(iiifResource, SerializerSettings);
    }
}