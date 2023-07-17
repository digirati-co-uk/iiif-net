using System.Collections.Generic;
using IIIF.Auth.V2;
using IIIF.Presentation.V3.Strings;

namespace IIIF.Tests.Auth.V2;

public class ReusableParts
{
    public const string ExpectedService = @"{
      ""id"": ""https://example.com/image/service/probe"",
      ""type"": ""AuthProbeService2"",
      ""service"": [
        {
          ""id"": ""https://example.com/login"",
          ""type"": ""AuthAccessService2"",
          ""profile"": ""interactive"",
          ""label"": {""en"":[""label property""]},
          ""service"": [
            {
              ""id"": ""https://example.com/token"",
              ""type"": ""AuthAccessTokenService2""
            },
            {
              ""id"": ""https://example.com/logout"",
              ""type"": ""AuthLogoutService2"",
              ""label"": {""en"":[""Logout from Example Institution""]}
            }
          ],
          ""confirmLabel"": {""en"":[""ConfirmLabel property""]},
          ""heading"": {""en"":[""heading property""]},
          ""note"": {""en"":[""note property""]}
        }
      ]
    }
";

    public const string ExpectedServiceAsArray = @"
  ""service"": [
    " + ExpectedService + @"  ]";

    public static string GetExpectedServiceAsSingle()
    {
        return @"
  ""service"": " + ExpectedService.Replace("\n  ", "\n");
    }

    public static readonly List<IService> Auth2Services = new()
    {
        new AuthProbeService2
        {
            Id = "https://example.com/image/service/probe",
            Service = new List<IService>
            {
                new AuthAccessService2
                {
                    Id = "https://example.com/login",
                    Profile = AuthAccessService2.ActiveProfile,
                    Label = new LanguageMap("en", "label property"),
                    ConfirmLabel = new LanguageMap("en", "confirmLabel property"),
                    Heading = new LanguageMap("en", "heading property"),
                    Note = new LanguageMap("en", "note property"),
                    Service = new List<IService>
                    {
                        new AuthAccessTokenService2
                        {
                            Id = "https://example.com/token"
                        },
                        new AuthLogoutService2
                        {
                            Id = "https://example.com/logout",
                            Label = new LanguageMap("en", "Logout from Example Institution")
                        }
                    }
                }
            }
        }
    };
}