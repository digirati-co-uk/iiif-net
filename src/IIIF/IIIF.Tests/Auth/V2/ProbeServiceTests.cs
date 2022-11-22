using FluentAssertions;
using IIIF.Auth.V2;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;
using Xunit;

namespace IIIF.Tests.Auth.V2
{
    public class ProbeServiceTests
    {
        [Fact]
        public void ProbeService_Can_Be_For_External_Resource()
        {
            // Arrange
            var probe = new AuthProbeService2
            {
                Id = "https://example.org/resource1/probe",
                Label = new LanguageMap("en", "A probe Service"),
                For = new Image
                {
                    Id = "https://example.org/resource1"
                },
                Location = new Image
                {
                    Id = "https://example.org/resource1-open"
                }
            };
            
            // Act
            var json = probe.AsJson().Replace("\r\n", "\n");
            const string expected = @"{
  ""id"": ""https://example.org/resource1/probe"",
  ""type"": ""AuthProbeService2"",
  ""label"": {""en"":[""A probe Service""]},
  ""for"": {
    ""id"": ""https://example.org/resource1"",
    ""type"": ""Image""
  },
  ""location"": {
    ""id"": ""https://example.org/resource1-open"",
    ""type"": ""Image""
  }
}";
            
            // Assert
            json.Should().BeEquivalentTo(expected);
            
        }
        
        
        [Fact]
        public void ProbeService_Can_Be_For_ImageService2()
        {
            // Arrange
            var probe = new AuthProbeService2
            {
                Id = "https://example.org/resource1/probe",
                Label = new LanguageMap("en", "A probe Service"),
                For = new ImageService2
                {
                    Id = "https://example.org/imageService2"
                }
            };
            
            // Act
            var json = probe.AsJson().Replace("\r\n", "\n");
            const string expected = @"{
  ""id"": ""https://example.org/resource1/probe"",
  ""type"": ""AuthProbeService2"",
  ""label"": {""en"":[""A probe Service""]},
  ""for"": {
    ""@id"": ""https://example.org/imageService2"",
    ""@type"": ""ImageService2""
  }
}";
            
            // Assert
            json.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void ProbeService_Can_Be_For_ImageService3()
        {
            // Arrange
            var probe = new AuthProbeService2
            {
                Id = "https://example.org/resource1/probe",
                Label = new LanguageMap("en", "A probe Service"),
                For = new ImageService3
                {
                    Id = "https://example.org/imageService3"
                }
            };
            
            // Act
            var json = probe.AsJson().Replace("\r\n", "\n");
            const string expected = @"{
  ""id"": ""https://example.org/resource1/probe"",
  ""type"": ""AuthProbeService2"",
  ""label"": {""en"":[""A probe Service""]},
  ""for"": {
    ""id"": ""https://example.org/imageService3"",
    ""type"": ""ImageService3""
  }
}";
            
            // Assert
            json.Should().BeEquivalentTo(expected);
        }
    }
}