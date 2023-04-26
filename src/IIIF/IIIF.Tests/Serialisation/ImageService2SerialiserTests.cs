using System.Collections.Generic;
using FluentAssertions;
using IIIF.Auth.V0;
using IIIF.ImageApi;
using IIIF.ImageApi.V2;
using IIIF.Presentation.V2.Strings;
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
                Converters = new List<JsonConverter> { new ImageService2Converter() }
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

        [Fact]
        public void ReadJson_OutputsExpected_IfNoProfileOrProfileDescription()
        {
            // Arrange
            const string json = "{\"@id\":\"foo\",\"@type\":\"ImageService2\",\"width\":0,\"height\":0}";
            var expected = new ImageService2 { Id = "foo" };
            
            // Act
            var result = JsonConvert.DeserializeObject<ImageService2>(json, jsonSerializerSettings);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void ReadJson_OutputsExpected_ProfileOnly()
        {
            // Arrange
            var expected = new ImageService2 { Id = "foo", Profile = "bar" };
            const string json =
                "{\"@id\":\"foo\",\"@type\":\"ImageService2\",\"profile\":\"bar\",\"width\":0,\"height\":0}";
            
            // Act
            var result = JsonConvert.DeserializeObject<ImageService2>(json, jsonSerializerSettings);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void ReadJson_OutputsExpected_ProfileDescriptionOnly()
        {
            // Arrange
            var expected = new ImageService2
            {
                Id = "foo",
                ProfileDescription = new ProfileDescription { MaxHeight = 10, MaxWidth = 20 },
            };
            const string json =
                "{\"@id\":\"foo\",\"@type\":\"ImageService2\",\"width\":0,\"height\":0,\"profile\":{\"maxHeight\":10,\"maxWidth\":20}}";
            
            // Act
            var result = JsonConvert.DeserializeObject<ImageService2>(json, jsonSerializerSettings);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void ReadJson_OutputsExpected_ProfileAndProfileDescription()
        {
            // Arrange
            var expected = new ImageService2
            {
                Id = "foo",
                Profile = "bar",
                ProfileDescription = new ProfileDescription { MaxHeight = 10, MaxWidth = 20 },
            };
            const string json =
                "{\"@id\":\"foo\",\"@type\":\"ImageService2\",\"profile\":[\"bar\",{\"maxHeight\":10,\"maxWidth\":20}],\"width\":0,\"height\":0}";
            
            // Act
            var result = JsonConvert.DeserializeObject<ImageService2>(json, jsonSerializerSettings);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        } 
        
        [Fact]
        public void CanDeserialise_ImageService2()
        {
            // Arrange
            var imageService = new ImageService2
            {
                Id = "foo",
                Profile = "bar",
                ProfileDescription = new ProfileDescription { MaxHeight = 10, MaxWidth = 20 },
            };

            var serialised = imageService.AsJson();
            
            // Act
            var deserialised = serialised.FromJson<ImageService2>();
            
            // Assert
            deserialised.Should().BeEquivalentTo(imageService);
        } 
        
        [Fact]
        public void CanDeserialise_ImageService_WithAuth()
        {
            // Arrange
            var imageService = new ImageService2
            {
                Id = "foo",
                Profile = "bar",
                ProfileDescription = new ProfileDescription { MaxHeight = 10, MaxWidth = 20 },
                Sizes = new List<Size>
                {
                    new(100, 200),
                    new(400, 800),
                },
                Tiles = new List<Tile>
                {
                    new()
                    {
                        Height = 512, Width = 512, ScaleFactors = new[] { 1, 2, 4 }
                    }
                },
                Service = new List<IService>
                {
                    new AuthCookieService(AuthCookieService.ClickthroughProfile)
                    {
                        Id = "http://clickthrough",
                        Label = new MetaDataValue("This is the label"),
                        Description = new MetaDataValue("This is the description"),
                        Service = new List<IService>
                        {
                            new AuthLogoutService
                            {
                                Id = "http://clickthrough/logout",
                            },
                            new AuthTokenService
                            {
                                Id = "http://clickthrough/token",
                            }
                        }
                    }
                }
            };

            var serialised = imageService.AsJson();
            
            // Act
            var deserialised = serialised.FromJson<ImageService2>();
            
            // Assert
            deserialised.Should().BeEquivalentTo(imageService);
        } 
    }
}