using IIIF.Auth.V2;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;

namespace IIIF.Tests.Auth.V2;

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
        var json = tokenResp.AsJson();
        var jsonToken = Newtonsoft.Json.Linq.JToken.Parse(json);

        // Assert - validate structure via JSON tokens
        jsonToken["@context"]?.ToString().Should().Be(Constants.IIIFAuth2Context);
        jsonToken["type"]?.ToString().Should().Be("AuthAccessToken2");
        jsonToken["messageId"]?.ToString().Should().Be("100");
        jsonToken["accessToken"]?.ToString().Should().Be("TOKEN_HERE");
        ((int?)jsonToken["expiresIn"]).Should().Be(999);
    }

    [Fact]
    public void Token_Service_Deserialise_Success_Response()
    {
        // Arrange
        var tokenResp = new AuthAccessToken2
        {
            AccessToken = "TOKEN_HERE",
            ExpiresIn = 999,
            MessageId = "100"
        };

        var serialised = tokenResp.AsJson();

        // Act
        var deserialised = serialised.FromJson<AuthAccessToken2>();

        // Assert - explicit field validation
        deserialised.AccessToken.Should().Be(tokenResp.AccessToken);
        deserialised.ExpiresIn.Should().Be(tokenResp.ExpiresIn);
        deserialised.MessageId.Should().Be(tokenResp.MessageId);

        // Verify context was serialized correctly
        var json = Newtonsoft.Json.Linq.JToken.Parse(serialised);
        json["@context"]?.ToString().Should().Be(Constants.IIIFAuth2Context);
        json["type"]?.ToString().Should().Be("AuthAccessToken2");
    }

    [Fact]
    public void Token_Service_Error_Response()
    {
        // Arrange
        var tokenResp = new AuthAccessTokenError2(
            AuthAccessTokenError2.InvalidAspect,
            new LanguageMap("en", "Your credentials are wrong"))
        {
            MessageId = "1010"
        };

        // Act
        var json = tokenResp.AsJson();
        var jsonToken = Newtonsoft.Json.Linq.JToken.Parse(json);

        // Assert - validate structure via JSON tokens
        jsonToken["@context"]?.ToString().Should().Be(Constants.IIIFAuth2Context);
        jsonToken["type"]?.ToString().Should().Be("AuthAccessTokenError2");
        jsonToken["profile"]?.ToString().Should().Be("invalidAspect");
        jsonToken["messageId"]?.ToString().Should().Be("1010");
        jsonToken["note"]?["en"]?.Values<string>().Should().Contain("Your credentials are wrong");
    }
    
    [Fact]
    public void Token_Service_Deserialise_Error_Response()
    {
        // Arrange
        var tokenResp = new AuthAccessTokenError2(
            AuthAccessTokenError2.InvalidAspect,
            new LanguageMap("en", "Your credentials are wrong"));

        var serialised = tokenResp.AsJson();

        // Act
        var deserialised = serialised.FromJson<AuthAccessTokenError2>();
        deserialised.Should().BeEquivalentTo(tokenResp);
    }
}