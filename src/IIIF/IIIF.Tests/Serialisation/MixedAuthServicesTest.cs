using System.Collections.Generic;
using FluentAssertions;
using IIIF.Auth.V1;
using IIIF.ImageApi.V2;
using IIIF.Presentation.V2.Strings;
using IIIF.Serialisation;
using IIIF.Tests.Auth.V2;
using Xunit;

namespace IIIF.Tests.Serialisation;

public class MixedAuthServicesTest
{
    [Fact]
    public void Image2_Can_Have_V1_And_V2_Auth_Services()
    {
        // Arrange
        var auth1CookieService = AuthCookieService.NewClickthroughInstance();
        auth1CookieService.Id = "https://example.com/login";
        auth1CookieService.Label = new MetaDataValue("auth1 - label");
        auth1CookieService.Header = new MetaDataValue("auth1 - header");
        auth1CookieService.Description = new MetaDataValue("auth1 - desc");
        auth1CookieService.ConfirmLabel = new MetaDataValue("auth1 - confirm");
        auth1CookieService.FailureHeader = new MetaDataValue("auth1 - fail");
        auth1CookieService.FailureDescription = new MetaDataValue("auth1 - fail-desc");
        auth1CookieService.Service = new List<IService>
        {
            new AuthTokenService
            {
                Id = "https://example.com/token"
            },
            new AuthLogoutService
            {
                Id = "https://example.com/logout",
                Label = new MetaDataValue("Log out")
            }
        };
        var imgService2 = new ImageService2
        {
            Id = "https://example.org/images/my-image.jpg/v2/service",
            Service = new List<IService> { auth1CookieService }
        };

        imgService2.Service.AddRange(ReusableParts.Auth2Services);

        // Act
        var json = imgService2.AsJson().Replace("\r\n", "\n");
        const string expected = @"{
  ""@id"": ""https://example.org/images/my-image.jpg/v2/service"",
  ""@type"": ""ImageService2"",
  ""service"": [
    {
      ""@id"": ""https://example.com/login"",
      ""@type"": ""AuthCookieService1"",
      ""profile"": ""http://iiif.io/api/auth/1/clickthrough"",
      ""label"": ""auth1 - label"",
      ""description"": ""auth1 - desc"",
      ""service"": [
        {
          ""@id"": ""https://example.com/token"",
          ""@type"": ""AuthTokenService1"",
          ""profile"": ""http://iiif.io/api/auth/1/token""
        },
        {
          ""@id"": ""https://example.com/logout"",
          ""@type"": ""AuthLogoutService1"",
          ""profile"": ""http://iiif.io/api/auth/1/logout"",
          ""label"": ""Log out""
        }
      ],
      ""confirmLabel"": ""auth1 - confirm"",
      ""header"": ""auth1 - header"",
      ""failureHeader"": ""auth1 - fail"",
      ""failureDescription"": ""auth1 - fail-desc""
    },
    {
      ""id"": ""https://example.com/image/service/probe"",
      ""type"": ""AuthProbeService2"",
      ""service"": [
        {
          ""id"": ""https://example.com/login"",
          ""type"": ""AuthAccessService2"",
          ""profile"": ""active"",
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
          ""confirmLabel"": {""en"":[""confirmLabel property""]},
          ""heading"": {""en"":[""heading property""]},
          ""note"": {""en"":[""note property""]}
        }
      ]
    }
  ]
}";

        // Assert
        json.Should().BeEquivalentTo(expected);
    }
}