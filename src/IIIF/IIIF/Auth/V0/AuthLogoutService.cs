using IIIF.Presentation.V2;

namespace IIIF.Auth.V0;

/// <summary>
/// Used to logout of session
/// </summary>
/// <remarks>https://iiif.io/api/auth/0.9/#logout-service</remarks>
public class AuthLogoutService : ResourceBase, IService
{
    public const string AuthLogout0Profile = "http://iiif.io/api/auth/0/logout";

    public AuthLogoutService()
    {
        Profile = AuthLogout0Profile;
    }

    [JsonProperty(PropertyName = "@type", Order = 3)]
    public override string? Type { get; set; } = "AuthLogoutService0";
}