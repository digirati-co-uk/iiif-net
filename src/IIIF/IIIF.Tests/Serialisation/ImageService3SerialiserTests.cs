using FluentAssertions;
using IIIF.ImageApi.Service;
using IIIF.Serialisation;
using Xunit;

namespace IIIF.Tests.Serialisation
{
    public class ImageService3SerialiserTests
    {
        [Fact]
        public void WriteJson_OutputsExpected_IfNoProfileOrProfileDescription()
        {
            // Arrange
            var imageService = new ImageService3 { Id = "foo" };
            const string expected = "{\n  \"id\": \"foo\",\n  \"type\": \"ImageService3\"\n}";
            
            // Act
            var result = imageService.AsJson().Replace("\r\n", "\n");
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Fact]
        public void WriteJson_OutputsExpected_ProfileOnly()
        {
            // Arrange
            var imageService = new ImageService3 { Id = "foo", Profile = "bar" };
            const string expected = "{\n  \"id\": \"foo\",\n  \"type\": \"ImageService3\",\n  \"profile\": \"bar\"\n}";

            // Act
            var result = imageService.AsJson().Replace("\r\n", "\n");
            
            // Assert
            result.Should().Be(expected);
        }
    }
}