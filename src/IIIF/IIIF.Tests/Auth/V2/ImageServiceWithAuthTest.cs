using FluentAssertions;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Serialisation;
using Xunit;

namespace IIIF.Tests.Auth.V2
{
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
            var json = imgService2.AsJson().Replace("\r\n", "\n");
            const string expected = @"{
  ""@id"": ""https://example.com/image/service"",
  ""@type"": ""ImageService2""," + ReusableParts.ExpectedService + @"
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
                Service = ReusableParts.Auth2Services
            };
            
            // Act
            var json = imgService3.AsJson().Replace("\r\n", "\n");
            const string expected = @"{
  ""id"": ""https://example.com/image/service"",
  ""type"": ""ImageService3""," + ReusableParts.ExpectedService + @"
}";
            // Assert
            json.Should().BeEquivalentTo(expected);
        }
    }
}