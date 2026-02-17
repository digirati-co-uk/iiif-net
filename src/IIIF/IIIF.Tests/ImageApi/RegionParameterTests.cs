using System;
using IIIF.Exceptions;
using IIIF.ImageApi;

namespace IIIF.Tests.ImageApi;

public class RegionParameterTests
{
    [Fact]
    public void Parse_Correct_Full()
    {
        const string full = "full";
        var expected = new RegionParameter
        {
            Full = true
        };

        // Act
        var rotationParameter = RegionParameter.Parse(full);

        // Assert
        rotationParameter.Should().BeEquivalentTo(expected);
        rotationParameter.ToString().Should().Be(full);
    }
    
    [Fact]
    public void Parse_Correct_Square()
    {
        const string square = "square";
        var expected = new RegionParameter
        {
            Square = true
        };

        // Act
        var rotationParameter = RegionParameter.Parse(square);

        // Assert
        rotationParameter.Should().BeEquivalentTo(expected);
        rotationParameter.ToString().Should().Be(square);
    }
    
    [Fact]
    public void Parse_Correct_XYWH()
    {
        const string xywh = "10,20,30,40";
        var expected = new RegionParameter
        {
            X = 10, Y = 20, W = 30, H = 40
        };

        // Act
        var rotationParameter = RegionParameter.Parse(xywh);

        // Assert
        rotationParameter.Should().BeEquivalentTo(expected);
        rotationParameter.ToString().Should().Be(xywh);
    }
    
    [Fact]
    public void Parse_Correct_Percent()
    {
        const string percent = "pct:10.1,20,30,40";
        var expected = new RegionParameter
        {
            Percent = true, X = 10.10F, Y = 20, W = 30, H = 40
        };

        // Act
        var rotationParameter = RegionParameter.Parse(percent);

        // Assert
        rotationParameter.Should().BeEquivalentTo(expected);
        rotationParameter.ToString().Should().Be(percent);
    }

    [Theory]
    [InlineData("pct:10,20,30")]
    [InlineData("all")]
    [InlineData("foo,bar")]
    public void Parse_Throws_IfUnableToParse(string input)
    {
        Action act = () => RegionParameter.Parse(input);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void GetExtractedRegionSize_ReturnsImageDimensions_IfFull()
    {
        var regionParameter = new RegionParameter { Full = true };
        var imageSize = new Size(100, 200);
        
        var result = regionParameter.GetExtractedRegionSize(imageSize);
        
        result.Should().Be(imageSize);
    }

    [Theory]
    [InlineData(100, 200)]
    [InlineData(200, 100)]
    public void GetExtractedRegionSize_ReturnsShorterDimension_IfSquare(int w, int h)
    {
        var regionParameter = new RegionParameter { Square = true };
        var imageSize = new Size(w, h);
        
        var result = regionParameter.GetExtractedRegionSize(imageSize);
        
        result.Width.Should().Be(result.Height).And.Be(Math.Min(w, h));
    }

    [Theory]
    [InlineData("0,0,128,199", 128, 199, "Start at top left")]
    [InlineData("125,15,120,140", 120, 140, "Start within image, size does not exceed")]
    [InlineData("125,15,200,200", 175, 185, "Image cropped as outside dimension")]
    public void GetExtractedRegionSize_Correct_XYWH(string xywh, int w, int h, string reason)
    {
        var regionParameter = RegionParameter.Parse(xywh);
        var imageSize = new Size(300, 200);
        
        var result = regionParameter.GetExtractedRegionSize(imageSize);
        
        result.Width.Should().Be(w, reason);
        result.Height.Should().Be(h, reason);
    }
    
    [Theory]
    [InlineData("pct:0,0,25,50", 75, 100, "Start at top left")]
    [InlineData("pct:41.6,7.5,40,70", 120, 140, "Start within image, size does not exceed")]
    [InlineData("pct:41.6,7.5,66.6,100", 175, 185, "Image cropped as outside dimension")]
    public void GetExtractedRegionSize_Correct_Pct(string xywh, int w, int h, string reason)
    {
        var regionParameter = RegionParameter.Parse(xywh);
        var imageSize = new Size(300, 200);
        
        var result = regionParameter.GetExtractedRegionSize(imageSize);
        
        result.Width.Should().Be(w, reason);
        result.Height.Should().Be(h, reason);
    }

    [Theory]
    [InlineData("301,0,25,25")]
    [InlineData("0,201,25,25")]
    [InlineData("301,201,25,25")]
    [InlineData("pct:101,0,25,25")]
    [InlineData("pct:0,101,25,25")]
    [InlineData("pct:101,101,25,25")]
    public void GetExtractedRegionSize_Throws_IfRegionOutsideBounds(string region)
    {
        var regionParameter = RegionParameter.Parse(region);
        var imageSize = new Size(300, 200);
        
        Action act = () => regionParameter.GetExtractedRegionSize(imageSize);
        act.Should().ThrowExactly<RegionException>().WithMessage("Region is outside image bounds");
    }
    
    [Theory]
    [InlineData("0,0,0,25")]
    [InlineData("0,0,25,0")]
    [InlineData("pct:0,0,0,25")]
    [InlineData("pct:0,0,25,0")]
    public void GetExtractedRegionSize_Throws_IfRegionWithOrHeight0(string region)
    {
        var regionParameter = RegionParameter.Parse(region);
        var imageSize = new Size(300, 200);
        
        Action act = () => regionParameter.GetExtractedRegionSize(imageSize);
        act.Should().ThrowExactly<RegionException>().WithMessage("Region * cannot be zero");
    }
}