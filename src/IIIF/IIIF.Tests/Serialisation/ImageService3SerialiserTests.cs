﻿using System.Collections.Generic;
using FluentAssertions;
using IIIF.ImageApi.V3;
using IIIF.Serialisation;
using Xunit;

namespace IIIF.Tests.Serialisation
{
    public class ImageService3SerialiserTests
    {
        [Fact]
        public void WriteJson_OutputsExpected_IfNoProfile()
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
        public void WriteJson_OutputsExpected_Profile()
        {
            // Arrange
            var imageService = new ImageService3 { Id = "foo", Profile = "bar" };
            const string expected = "{\n  \"id\": \"foo\",\n  \"type\": \"ImageService3\",\n  \"profile\": \"bar\"\n}";

            // Act
            var result = imageService.AsJson().Replace("\r\n", "\n");
            
            // Assert
            result.Should().Be(expected);
        }
        
        [Fact]
        public void CanDeserialise_ImageService3()
        {
            // Arrange
            var imageService = new ImageService3
            {
                Id = "foo",
                Profile = "level1",
                Height = 1234,
                Width = 2000,
                ExtraFeatures = new List<string> { Features.Cors, Features.RegionSquare },
                PreferredFormats = new List<string> { "webp", "png" },
                ExtraQualities = new List<string> { "bitonal", "gray" },
                ExtraFormats = new List<string> { "jpg" },
                Rights = "http://rightsstatements.org/vocab/InC-EDU/1.0/",
            };

            var serialised = imageService.AsJson();
            
            // Act
            var deserialised = serialised.FromJson<ImageService3>();
            
            // Assert
            deserialised.Should().BeEquivalentTo(imageService);
        } 
    }
}