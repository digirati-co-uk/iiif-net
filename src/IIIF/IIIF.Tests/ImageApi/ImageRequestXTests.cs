using FluentAssertions;
using IIIF.ImageApi;
using Xunit;

namespace IIIF.Tests.ImageApi
{
    public class ImageRequestXTests
    {
        [Fact]
        public void Parse_CorrectMax()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                Max = true
            };
            var originalSize = new Size(400, 400);
            var expected = new Size(400, 400);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectMaxScaled()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                Max = true, Upscaled = true
            };
            var originalSize = new Size(400, 400);
            var expected = new Size(400, 400);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectWidthOnly()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                Width = 100
            };
            var originalSize = new Size(400, 800);
            var expected = new Size(100, 200);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectWidthOnlyScaled()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                Width = 100, Upscaled = true
            };
            var originalSize = new Size(400, 800);
            var expected = new Size(100, 200);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectHeightOnly()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                Height = 40
            };
            var originalSize = new Size(400, 800);
            var expected = new Size(20, 40);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectHeightOnlyScaled()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                Height = 40, Upscaled = true
            };
            var originalSize = new Size(400, 800);
            var expected = new Size(20, 40);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectWidthHeight()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                Width = 90, Height = 40
            };
            var originalSize = new Size(400, 800);
            var expected = new Size(90, 40);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectWidthHeightScaled()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                Width = 90, Height = 40, Upscaled = true
            };
            var originalSize = new Size(400, 800);
            var expected = new Size(90, 40);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectWidthHeightConfined()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                Width = 90, Height = 40, Confined = true
            };
            var originalSize = new Size(400, 800);
            var expected = new Size(20, 40);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectWidthHeightScaledConfined()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                Width = 90, Height = 40, Confined = true, Upscaled = true
            };
            var originalSize = new Size(400, 800);
            var expected = new Size(20, 40);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectPercentage()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                PercentScale = 30f
            };
            var originalSize = new Size(400, 800);
            var expected = new Size(120, 240);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Parse_CorrectPercentageScaled()
        {
            // Arrange
            var sizeParameter = new SizeParameter
            {
                PercentScale = 30f, Upscaled = true
            };
            var originalSize = new Size(400, 800);
            var expected = new Size(120, 240);
            
            // Act
            var result = sizeParameter.GetResultingSize(originalSize);
            
            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}