using System;
using FluentAssertions;
using IIIF.ImageApi;

namespace IIIF.Tests.ImageApi;

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
    
    [Theory]
    [InlineData("my-asset")]
    [InlineData("my-asset/")]
    public void Parse_IsBase(string path)
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var result = ImageRequest.Parse($"{prefix}{path}", prefix);
        
        // Assert
        result.IsBase.Should().BeTrue();
    }

    [Theory]
    [InlineData("iiif-img/27/1/")]
    [InlineData("/iiif-img/27/1/")]
    [InlineData("/iiif-img/27/1")]
    [InlineData("iiif-img/27/1")]
    public void Parse_Validate_Succeeds(string prefix)
    {
        // Arrange and Act
        const string request = $"iiif-img/27/1/my-asset/full/800,/0/default.jpg";
        var action = () => ImageRequest.Parse(request, prefix, true);
        
        // Assert
        action.Should().NotThrow();
    }
    
    [Theory]
    [InlineData("my-asset//full/800,/0/default.jpg")]
    [InlineData("my-asset/full//800,/0/default.jpg")]
    [InlineData("my-asset/full/800,//0/default.jpg")]
    [InlineData("my-asset/full/800,/0//default.jpg")]
    public void Parse_Validate_Fails_WhenGivenExtraSegments(string path)
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var action = () => ImageRequest.Parse($"{prefix}{path}", prefix, true);
        
        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }
    
    [Theory]
    [InlineData("my-asset//800,/0/default.jpg")]
    [InlineData("my-asset/full//0/default.jpg")]
    [InlineData("my-asset/full/800,//default.jpg")]
    [InlineData("my-asset/full/800,/0/")]
    [InlineData("my-asset////default.jpg")]
    [InlineData("my-asset////")]
    public void Parse_TryParse_Fails_WhenGivenEmptyParameters(string path)
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var action = () => ImageRequest.Parse($"{prefix}{path}", prefix, true);
        
        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }
}