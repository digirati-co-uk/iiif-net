using System;
using System.Collections.Generic;
using FluentAssertions;
using IIIF.ImageApi;

namespace IIIF.Tests.ImageApi;

public class ImageRequestXTests
{
    [Fact]
    public void GetResultingSize_CorrectMax()
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
    public void GetResultingSize_CorrectMaxScaled()
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
    public void GetResultingSize_CorrectWidthOnly()
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
    public void GetResultingSize_CorrectWidthOnlyScaled()
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
    public void GetResultingSize_CorrectHeightOnly()
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
    public void GetResultingSize_CorrectHeightOnlyScaled()
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
    public void GetResultingSize_CorrectWidthHeight()
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
    public void GetResultingSize_CorrectWidthHeightScaled()
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
    public void GetResultingSize_CorrectWidthHeightConfined()
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
    public void GetResultingSize_CorrectWidthHeightScaledConfined()
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
    public void GetResultingSize_CorrectPercentage()
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
    public void GetResultingSize_CorrectPercentageScaled()
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
        result.ToString().Should().Be("iiif-img/27/1/my-asset");
    }
    
    [Fact]
    public void Parse_InfoJson()
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var result = ImageRequest.Parse($"{prefix}my-asset/info.json", prefix);
        
        // Assert
        result.IsInformationRequest.Should().BeTrue();
        result.ToString().Should().Be("iiif-img/27/1/my-asset/info.json");
    }
    
    [Fact]
    public void Parse_Fails_WhenInfoHasInvalidExtension()
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var action = () => ImageRequest.Parse($"{prefix}my-asset/info.jsonll", prefix);
        
        // Assert
        action.Should().ThrowExactly<ArgumentException>();
    }

    [Theory]
    [InlineData("iiif-img/27/1/")]
    [InlineData("/iiif-img/27/1/")]
    [InlineData("/iiif-img/27/1")]
    [InlineData("iiif-img/27/1")]
    public void Parse_Validate_HandlesPrefixFormats(string prefix)
    {
        // Arrange and Act
        const string request = $"iiif-img/27/1/my-asset/full/800,/0/default.jpg";
        var action = () => ImageRequest.Parse(request, prefix, true);
        
        // Assert
        action.Should().NotThrow<ArgumentException>();
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
        action.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Path contains empty or an invalid number of segments");
    }
    
    [Theory]
    [InlineData("my-asset//800,/0/default.jpg")]
    [InlineData("my-asset/full//0/default.jpg")]
    [InlineData("my-asset/full/800,//default.jpg")]
    [InlineData("my-asset/full/800,/0/")]
    [InlineData("my-asset////default.jpg")]
    [InlineData("my-asset////")]
    public void Parse_Validate_Fails_WhenGivenEmptyParameters(string path)
    {
        // Arrange and Act
        const string prefix = "iiif-img/27/1/";
        var action = () => ImageRequest.Parse($"{prefix}{path}", prefix, true);
        
        // Assert
        action.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Path contains empty or an invalid number of segments");
    }
    
    public static IEnumerable<object[]> ImageRequests =>
        new List<object[]>
        {
            new object[] { "iiif-img/27/1/my-asset", new ImageRequest { IsBase = true } },
            new object[] { "iiif-img/27/1/my-asset/info.json", new ImageRequest { IsInformationRequest = true } },
            new object[]
            {
                "iiif-img/27/1/my-asset/full/max/0/default.jpg",
                new ImageRequest
                {
                    Region = new RegionParameter { Full = true },
                    Format = "jpg",
                    Size = new SizeParameter { Max = true },
                    Quality = "default",
                    Rotation = new RotationParameter { Angle = 0f },
                    OriginalPath = "my-asset/full/max/0/default.jpg",
                }
            },
            new object[]
            {
                "iiif-img/27/1/my-asset/0,0,512,1024/!500,250/!180/bitonal.png",
                new ImageRequest
                {
                    Region = new RegionParameter { X = 0, Y = 0, W = 512, H = 1024 },
                    Format = "png",
                    Size = new SizeParameter { Confined = true, Width = 500, Height = 250 },
                    Quality = "bitonal",
                    Rotation = new RotationParameter { Angle = 180f, Mirror = true },
                    OriginalPath = "my-asset/0,0,512,1024/!500,250/!180/bitonal.png",
                }
            },
        };

    [Theory]
    [MemberData(nameof(ImageRequests))]
    public void Parse_ValidRequest_CanRoundTripToString(string input, ImageRequest expected)
    {
        const string prefix = "iiif-img/27/1/";
        expected.Prefix = prefix;
        expected.Identifier = "my-asset";
        
        var result = ImageRequest.Parse(input, prefix);
        
        // Assert
        result.Should().BeEquivalentTo(expected);
        result.ToString().Should().Be(input);
    }

    [Fact]
    public void ToString_ReflectsUpdates()
    {
        var expected = "iiif-img/27/1/my-asset/0,0,512,1024/!500,250/!180/bitonal.png";
        var imageRequest = new ImageRequest
        {
            Prefix = "iiif-img/27/1/",
            Identifier = "my-asset", 
            Format = "jpg",
            Quality = "default",
            Size = new SizeParameter { PercentScale = 75 },
            Region = new RegionParameter { Square = true },
            Rotation = new RotationParameter { Angle = 0f },
        };

        imageRequest.Format = "png";
        imageRequest.Quality = "bitonal";
        imageRequest.Size = new SizeParameter { Confined = true, Width = 500, Height = 250 };
        imageRequest.Region = new RegionParameter { X = 0, Y = 0, W = 512, H = 1024 };
        imageRequest.Rotation = new RotationParameter { Angle = 180f, Mirror = true };
        
        imageRequest.ToString().Should().Be(expected);
    }
    
    [Fact]
    public void ToString_Correct_NoPrefixReflectsUpdates()
    {
        var expected = "my-asset/0,0,512,1024/!500,250/!180/bitonal.png";
        var imageRequest = new ImageRequest
        {
            Format = "png",
            Identifier = "my-asset",
            Quality = "bitonal",
            Size = new SizeParameter { Confined = true, Width = 500, Height = 250 },
            Region = new RegionParameter { X = 0, Y = 0, W = 512, H = 1024 },
            Rotation = new RotationParameter { Angle = 180f, Mirror = true },
        };
        
        imageRequest.ToString().Should().Be(expected);
    }
}