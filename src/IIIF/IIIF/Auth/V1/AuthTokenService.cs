using IIIF.Presentation.V2;
using Newtonsoft.Json;

namespace IIIF.Auth.V1
{
    public class AuthTokenService : ResourceBase, IService
    {
        public const string AuthToken1Profile = "http://iiif.io/api/auth/1/token";
        
        public AuthTokenService()
        {
            Profile = AuthToken1Profile;
        }
        
        [JsonProperty(PropertyName = "@type", Order = 3)]
        public override string? Type { get; set; } = "AuthTokenService1";
    }
}