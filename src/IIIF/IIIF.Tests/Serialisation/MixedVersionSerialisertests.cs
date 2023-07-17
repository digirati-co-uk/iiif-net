using System.Collections.Generic;
using FluentAssertions;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3.Content;
using IIIF.Serialisation;
using Xunit;

namespace IIIF.Tests.Serialisation;

public class MixedVersionSerialiserTests
{
    [Fact]
    public void Image_Can_Have_V2_And_V3_Image_Services()
    {
        // Arrange
        var img = new Image
        {
            Id = $"https://example.org/images/my-image.jpg",
            Width = 1000,
            Height = 1000,
            Format = "image/jpeg",
            Service = new List<IService>
            {
                new ImageService2
                {
                    Id = "https://example.org/images/my-image.jpg/v2/service"
                },
                new ImageService3
                {
                    Id = "https://example.org/images/my-image.jpg/v2/service"
                }
            }
        };

        // Act
        var json = img.AsJson().Replace("\r\n", "\n");
        ;
        const string expected = @"{
  ""id"": ""https://example.org/images/my-image.jpg"",
  ""type"": ""Image"",
  ""width"": 1000,
  ""height"": 1000,
  ""format"": ""image/jpeg"",
  ""service"": [
    {
      ""@id"": ""https://example.org/images/my-image.jpg/v2/service"",
      ""@type"": ""ImageService2""
    },
    {
      ""id"": ""https://example.org/images/my-image.jpg/v2/service"",
      ""type"": ""ImageService3""
    }
  ]
}";
        // Assert
        json.Should().BeEquivalentTo(expected);
    }
}