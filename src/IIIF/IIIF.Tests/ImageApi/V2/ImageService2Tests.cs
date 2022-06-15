using FluentAssertions;
using IIIF.ImageApi.V2;
using Xunit;

namespace IIIF.Tests.ImageApi.V2
{
    public class ImageService2Tests
    {
        [Fact]
        public void Type_Default_IfNotSet()
        {
            // Arrange
            var imageService = new ImageService2();
            
            // Assert
            imageService.Type.Should().Be("ImageService2");
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("SomeType")]
        public void Type_ReturnsSetValue_IfSet(string type)
        {
            // Arrange
            var imageService = new ImageService2();
            imageService.Type = type;
            
            // Assert
            imageService.Type.Should().Be(type);
        }
    }
}