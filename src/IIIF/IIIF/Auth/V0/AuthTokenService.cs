using IIIF.Presentation.V2;
using Newtonsoft.Json;

namespace IIIF.Auth.V0
{
    /// <summary>
    /// Used to obtain an access token for accessing a Description Resource
    /// </summary>
    /// <remarks>https://iiif.io/api/auth/0.9/#access-token-service</remarks>
    public class AuthTokenService : ResourceBase, IService
    {
        public const string AuthToken0Profile = "http://iiif.io/api/auth/0/token";
        
        public AuthTokenService()
        {
            Profile = AuthToken0Profile;
        }
        
        [JsonProperty(PropertyName = "@type", Order = 3)]
        public override string? Type { get; set; } = "AuthTokenService0";
    }
}