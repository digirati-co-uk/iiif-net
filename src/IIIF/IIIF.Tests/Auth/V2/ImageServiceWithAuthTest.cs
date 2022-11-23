using System.Collections.Generic;
using FluentAssertions;
using IIIF.Auth.V2;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;
using Xunit;

namespace IIIF.Tests.Auth.V2
{
    public class ImageServiceWithAuthTest
    {
        const string ExpectedService = @"
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

        private readonly List<IService> auth2Services = new List<IService>
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


        [Fact]
        public void ImageService2_Can_Have_Auth_Services()
        {
            // Arrange
            var imgService2 = new ImageService2
            {
                Id = "https://example.com/image/service",
                Service = auth2Services
            };
            
            // Act
            var json = imgService2.AsJson().Replace("\r\n", "\n");
            const string expected = @"{
  ""@id"": ""https://example.com/image/service"",
  ""@type"": ""ImageService2""," + ExpectedService + @"
}";
            // Assert
            json.Should().BeEquivalentTo(expected);
        }
        
        
        [Fact]
        public void ImageService3_Can_Have_Auth_Services()
        {
            // Arrange
            var imgService3 = new ImageService3
            {
                Id = "https://example.com/image/service",
                Service = auth2Services
            };
            
            // Act
            var json = imgService3.AsJson().Replace("\r\n", "\n");
            const string expected = @"{
  ""id"": ""https://example.com/image/service"",
  ""type"": ""ImageService3""," + ExpectedService + @"
}";
            // Assert
            json.Should().BeEquivalentTo(expected);
        }
    }
}