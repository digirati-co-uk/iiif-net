using IIIF.Presentation.V2;

namespace IIIF.Auth.V1;

/// <summary>
/// Used to obtain an access token for accessing a Description Resource
/// </summary>
/// <remarks>https://iiif.io/api/auth/1.0/#access-token-service</remarks>
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