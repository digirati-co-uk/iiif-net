using System;
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
}