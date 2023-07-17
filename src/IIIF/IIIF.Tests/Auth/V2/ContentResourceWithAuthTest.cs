using FluentAssertions;
using IIIF.Presentation.V3.Content;
using IIIF.Serialisation;
using Xunit;

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
        var json = res.AsJson().Replace("\r\n", "\n");
        const string expected = @"{
  ""id"": ""https://example.com/documents/my.pdf"",
  ""type"": ""Text""," + ReusableParts.ExpectedServiceAsArray + @"
}";
        // Assert
        json.Should().BeEquivalentTo(expected);
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