using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Content;
using Newtonsoft.Json;

namespace IIIF.Auth.V2
{
    public class AuthProbeService2 : ResourceBase
    {
        public override string Type => nameof(AuthProbeService2);
        
        [JsonProperty(Order = 101, PropertyName = "for")]
        public JsonLdBase? For { get; set; }
        
        [JsonProperty(Order = 102, PropertyName = "location")]
        public JsonLdBase? Location { get; set; }
    }
}