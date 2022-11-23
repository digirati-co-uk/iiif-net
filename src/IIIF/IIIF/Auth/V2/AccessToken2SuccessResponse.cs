using Newtonsoft.Json;

namespace IIIF.Auth.V2
{
    public class AccessToken2SuccessResponse : JsonLdBase
    {
        [JsonProperty(Order = 10, PropertyName = "accessToken")]
        public string? AccessToken { get; set; }
        
        [JsonProperty(Order = 20, PropertyName = "expiresIn")]
        public int? ExpiresIn { get; set; }
        
        [JsonProperty(Order = 30, PropertyName = "messageId")]
        public string? MessageId { get; set; }
    }
}