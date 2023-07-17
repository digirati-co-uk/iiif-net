namespace IIIF.Auth.V2;

public class AuthAccessToken2 : JsonLdBase
{
    [JsonProperty(Order = 1, PropertyName = "@context")]
    public new string Context = Constants.IIIFAuth2Context;

    [JsonProperty(Order = 1, PropertyName = "type")]
    public string Type => nameof(AuthAccessToken2);

    [JsonProperty(Order = 10, PropertyName = "messageId")]
    public string? MessageId { get; set; }

    [JsonProperty(Order = 20, PropertyName = "accessToken")]
    public string? AccessToken { get; set; }

    [JsonProperty(Order = 30, PropertyName = "expiresIn")]
    public int? ExpiresIn { get; set; }
}