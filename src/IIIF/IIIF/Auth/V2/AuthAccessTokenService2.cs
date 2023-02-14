using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Strings;
using Newtonsoft.Json;

namespace IIIF.Auth.V2
{
    public class AuthAccessTokenService2 : ResourceBase, IService
    {
        public override string Type => nameof(AuthAccessTokenService2);
        
        [JsonProperty(Order = 102, PropertyName = "errorHeading")]
        public LanguageMap? ErrorHeading { get; set; }
        
        [JsonProperty(Order = 103, PropertyName = "errorNote")]
        public LanguageMap? ErrorNote { get; set; }
    }
}