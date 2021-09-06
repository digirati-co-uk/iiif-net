using IIIF.Presentation.V2.Strings;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2
{
    /// <summary>
    /// Common base for all legacy/pre-v3 IIIF models.
    /// </summary>
    public abstract class ResourceBase : JsonLdBase
    {
        [JsonProperty(PropertyName = "@id", Order = 2)]
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "@type", Order = 3)]
        public abstract string? Type { get; set; }
        
        [JsonProperty(Order = 4)]
        public string? Profile { get; set; } 
        
        [JsonProperty(Order = 11, PropertyName = "label")]
        public MetaDataValue? Label { get; set; }
        
        [JsonProperty(Order = 13, PropertyName = "description")]
        public MetaDataValue? Description { get; set; }
    }
}