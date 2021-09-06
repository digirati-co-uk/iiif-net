using IIIF.Presentation.V2.Strings;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2
{
    public class ExternalResource
    {
        [JsonProperty(Order = 1, PropertyName = "@id")]
        public string? Id { get; set; }
        
        [JsonProperty(Order = 2, PropertyName = "label")]
        public MetaDataValue? Label { get; set; }
        
        [JsonProperty(Order = 3, PropertyName = "format")]
        public string? Format { get; set; }
    }
}