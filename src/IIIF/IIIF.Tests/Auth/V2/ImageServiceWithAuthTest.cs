using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Serialisation;

namespace IIIF.Tests.Auth.V2;

public class ImageServiceWithAuthTest
{
    [Fact]
    public void ImageService2_Can_Have_Auth_Services()
    {
        // Arrange
        var imgService2 = new ImageService2
        {
            Id = "https://example.com/image/service",
            Service = ReusableParts.Auth2Services
        };

        // Act
        var json = imgService2.AsJson();
        var jsonToken = Newtonsoft.Json.Linq.JToken.Parse(json);

        // Assert - validate structure via JSON tokens
        jsonToken["@id"]?.ToString().Should().Be("https://example.com/image/service");
        jsonToken["@type"]?.ToString().Should().Be("ImageService2");
        jsonToken["service"]?.Should().NotBeNull();
        jsonToken["service"]?["id"]?.ToString().Should().Be("https://example.com/image/service/probe");
        jsonToken["service"]?["type"]?.ToString().Should().Be("AuthProbeService2");
        // service property inside is an array
        jsonToken["service"]?["service"]?.Should().NotBeNull();
    }
    
    [Fact]
    public void ImageService2_Deserialise_With_Auth_Services()
    {
        // Arrange
        var imgService2 = new ImageService2
        {
            Id = "https://example.com/image/service",
            Service = ReusableParts.Auth2Services
        };

        var serialised = imgService2.AsJson();

        // Act
        var deserialised = serialised.FromJson<ImageService2>();
        deserialised.Should().BeEquivalentTo(imgService2);
    }

    [Fact]
    public void ImageService3_Can_Have_Auth_Services()
    {
        // Arrange
        var imgService3 = new ImageService3
        {
            Id = "https://example.com/image/service",
            Service = ReusableParts.Auth2Services
        };

        // Act
        var json = imgService3.AsJson();
        var jsonToken = Newtonsoft.Json.Linq.JToken.Parse(json);

        // Assert - validate structure via JSON tokens
        jsonToken["id"]?.ToString().Should().Be("https://example.com/image/service");
        jsonToken["type"]?.ToString().Should().Be("ImageService3");
        jsonToken["service"]?.Should().HaveCount(1);
        jsonToken["service"]?[0]?["id"]?.ToString().Should().Be("https://example.com/image/service/probe");
        jsonToken["service"]?[0]?["type"]?.ToString().Should().Be("AuthProbeService2");
    }
    
    [Fact]
    public void ImageService3_Deserialise_With_Auth_Services()
    {
        // Arrange
        var imgService3 = new ImageService3
        {
            Id = "https://example.com/image/service",
            Service = ReusableParts.Auth2Services
        };

        var serialised = imgService3.AsJson();

        // Act
        var deserialised = serialised.FromJson<ImageService3>();
        deserialised.Should().BeEquivalentTo(imgService3);
    }
}