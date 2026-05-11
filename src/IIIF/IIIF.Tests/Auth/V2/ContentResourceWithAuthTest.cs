using IIIF.Presentation.V3.Content;
using IIIF.Serialisation;


namespace IIIF.Tests.Auth.V2;

public class ContentResourceWithAuthTest
{
    [Fact]
    public void ContentResource_Can_Have_Auth_Services()
    {
        // Arrange
        var res = new ExternalResource("Text")
        {
            Id = "https://example.com/documents/my.pdf",
            Service = ReusableParts.Auth2Services
        };

        // Act
        var json = res.AsJson();
        var jsonToken = Newtonsoft.Json.Linq.JToken.Parse(json);

        // Assert - validate structure via JSON tokens
        jsonToken["id"]?.ToString().Should().Be("https://example.com/documents/my.pdf");
        jsonToken["type"]?.ToString().Should().Be("Text");
        jsonToken["service"]?.Should().HaveCount(1);
        jsonToken["service"]?[0]?["id"]?.ToString().Should().Be("https://example.com/image/service/probe");
        jsonToken["service"]?[0]?["type"]?.ToString().Should().Be("AuthProbeService2");
    }
    
    [Fact]
    public void ContentResource_Deserialise_With_Auth_Services()
    {
        // Arrange
        var res = new ExternalResource("Text")
        {
            Id = "https://example.com/documents/my.pdf",
            Service = ReusableParts.Auth2Services
        };

        var serialised = res.AsJson();

        // Act
        var deserialised = serialised.FromJson<ExternalResource>();
        deserialised.Should().BeEquivalentTo(res);
    }
}