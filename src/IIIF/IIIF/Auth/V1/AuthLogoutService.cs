using IIIF.Presentation.V2;

namespace IIIF.Auth.V1;

/// <summary>
/// Used to logout of session
/// </summary>
/// <remarks>https://iiif.io/api/auth/1.0/#logout-service</remarks>
public class AuthLogoutService : ResourceBase, IService
{
    public const string AuthLogout1Profile = "http://iiif.io/api/auth/1/logout";

    public AuthLogoutService()
    {
        Profile = AuthLogout1Profile;
    }

    [JsonProperty(PropertyName = "@type", Order = 3)]
    public override string? Type { get; set; } = "AuthLogoutService1";
}