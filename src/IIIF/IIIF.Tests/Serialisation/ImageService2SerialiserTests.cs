using System.Collections.Generic;
using FluentAssertions;
using IIIF.ImageApi.V2;
using IIIF.Serialisation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace IIIF.Tests.Serialisation
{
    public class ImageService2SerialiserTests
    {
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public ImageService2SerialiserTests()
        {
            // NOTE: Using JsonSerializerSettings to facilitate testing as it makes it a LOT easier
            jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> { new ImageService2Serialiser() }
            };
        }

        [Fact]
        public void WriteJson_OutputsExpected_IfNoProfileOrProfileDescription()
        {
            // Arrange
            var imageService = new ImageService2 { Id = "foo" };
            const string expected = "{\"@id\":\"foo\",\"@type\":\"ImageService2\",\"width\":0,\"height\":0}";
            
            // Act
            var result = JsonConvert.SerializeObject(imageService, jsonSerializerSettings);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Fact]
        public void WriteJson_OutputsExpected_ProfileOnly()
        {
            // Arrange
            var imageService = new ImageService2 { Id = "foo", Profile = "bar" };
            const string expected =
                "{\"@id\":\"foo\",\"@type\":\"ImageService2\",\"profile\":\"bar\",\"width\":0,\"height\":0}";
            
            // Act
            var result = JsonConvert.SerializeObject(imageService, jsonSerializerSettings);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Fact]
        public void WriteJson_OutputsExpected_ProfileDescriptionOnly()
        {
            // Arrange
            var imageService = new ImageService2
            {
                Id = "foo",
                ProfileDescription = new ProfileDescription { MaxHeight = 10, MaxWidth = 20 },
            };
            const string expected =
                "{\"@id\":\"foo\",\"@type\":\"ImageService2\",\"width\":0,\"height\":0,\"profile\":{\"maxHeight\":10,\"maxWidth\":20}}";
            
            // Act
            var result = JsonConvert.SerializeObject(imageService, jsonSerializerSettings);
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Fact]
        public void WriteJson_OutputsExpected_ProfileAndProfileDescription()
        {
            // Arrange
            var imageService = new ImageService2
            {
                Id = "foo",
                Profile = "bar",
                ProfileDescription = new ProfileDescription { MaxHeight = 10, MaxWidth = 20 },
            };
            const string expected =
                "{\"@id\":\"foo\",\"@type\":\"ImageService2\",\"profile\":[\"bar\",{\"maxHeight\":10,\"maxWidth\":20}],\"width\":0,\"height\":0}";
            
            // Act
            var result = JsonConvert.SerializeObject(imageService, jsonSerializerSettings);
            
            // Assert
            result.Should().Be(expected);
        }
    }
}