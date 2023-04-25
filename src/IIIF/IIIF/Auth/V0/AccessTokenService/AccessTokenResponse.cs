using Newtonsoft.Json;

namespace IIIF.Auth.V0.AccessTokenService
{
    /// <summary>
    /// Access Token response sent by access token service
    /// </summary>
    /// <remarks>
    /// See https://iiif.io/api/auth/0.9/#the-json-access-token-response
    /// </remarks>
    public class AccessTokenResponse : JsonLdBase
    {
        [JsonProperty(PropertyName = "messageId", Order = 1)]
        public string MessageId { get; set; }
        
        [JsonProperty(PropertyName = "accessToken", Order = 2)]
        public string AccessToken { get; set; }
        
        [JsonProperty(PropertyName = "expiresIn", Order = 3)]
        public int ExpiresIn { get; set; }
    }
}