using System.Collections.Generic;
using IIIF.Auth.V2;
using IIIF.Presentation.V3.Strings;

namespace IIIF.Tests.Auth.V2
{
    public class ReusableParts
    {
        public const string ExpectedService = @"
  ""service"": [
    {
      ""id"": ""https://example.com/image/service/probe"",
      ""type"": ""AuthProbeService2""
    },
    {
      ""id"": ""https://example.com/login"",
      ""type"": ""AuthAccessService2"",
      ""profile"": ""interactive"",
      ""service"": [
        {
          ""id"": ""https://example.com/token"",
          ""type"": ""AuthTokenService2""
        },
        {
          ""id"": ""https://example.com/logout"",
          ""type"": ""AuthLogoutService2"",
          ""label"": {""en"":[""Logout from Example Institution""]}
        }
      ],
      ""confirmLabel"": {""en"":[""ConfirmLabel property""]},
      ""header"": {""en"":[""Header property""]},
      ""description"": {""en"":[""Description property""]},
      ""failureHeader"": {""en"":[""FailureHeader property""]},
      ""failureDescription"": {""en"":[""FailureDescription property""]}
    }
  ]";

        public static readonly List<IService> Auth2Services = new()
        {
            new AuthProbeService2
            {
                Id = "https://example.com/image/service/probe"
            },
            new AuthAccessService2
            {
                Id = "https://example.com/login",
                Profile = AuthAccessService2.InteractiveProfile,
                ConfirmLabel = new LanguageMap("en", "ConfirmLabel property"),
                Header = new LanguageMap("en", "Header property"),
                Description = new LanguageMap("en", "Description property"),
                FailureHeader = new LanguageMap("en", "FailureHeader property"),
                FailureDescription = new LanguageMap("en", "FailureDescription property"),
                Service = new List<IService>
                {
                    new AuthTokenService2
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
        };
    }
}