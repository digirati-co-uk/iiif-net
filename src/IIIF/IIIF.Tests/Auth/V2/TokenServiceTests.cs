using FluentAssertions;
using IIIF.Auth.V2;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;
using Xunit;

namespace IIIF.Tests.Auth.V2
{
    public class TokenServiceTests
    {
        [Fact]
        public void Token_Service_Success_Response()
        {
            // Arrange
            var tokenResp = new AuthAccessToken2
            {
                AccessToken = "TOKEN_HERE",
                ExpiresIn = 999,
                MessageId = "100"
            };
            
            // Act
            var json = tokenResp.AsJson().Replace("\r\n", "\n");
            var expected = @"{
  ""@context"": ""http://iiif.io/api/auth/2/context.json"",
  ""type"": ""AuthAccessToken2"",
  ""messageId"": ""100"",
  ""accessToken"": ""TOKEN_HERE"",
  ""expiresIn"": 999
}";
            // Assert
            json.Should().BeEquivalentTo(expected);

        }
        
        [Fact]
        public void Token_Service_Error_Response()
        {
            // Arrange
            var tokenResp = new AuthAccessTokenError2(
                AuthAccessTokenError2.InvalidAspect,
                new LanguageMap("en", "Your credentials are wrong"));
            
            // Act
            var json = tokenResp.AsJson().Replace("\r\n", "\n");
            var expected = @"{
  ""@context"": ""http://iiif.io/api/auth/2/context.json"",
  ""type"": ""AuthAccessTokenError2"",
  ""profile"": ""invalidAspect"",
  ""note"": {""en"":[""Your credentials are wrong""]}
}";
            // Assert
            json.Should().BeEquivalentTo(expected);

        }
    }
}