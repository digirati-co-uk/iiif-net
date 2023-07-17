using FluentAssertions;
using IIIF.ImageApi;
using Xunit;

namespace IIIF.Tests.ImageApi;

public class SizeParameterTests
{
    [Fact]
    public void Parse_CorrectMax()
    {
        // Arrange
        const string size = "max";
        var expected = new SizeParameter
        {
            Max = true
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectMaxScaled()
    {
        // Arrange
        const string size = "^max";
        var expected = new SizeParameter
        {
            Max = true, Upscaled = true
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectWidthOnly()
    {
        // Arrange
        const string size = "100,";
        var expected = new SizeParameter
        {
            Width = 100
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectWidthOnlyScaled()
    {
        // Arrange
        const string size = "^100,";
        var expected = new SizeParameter
        {
            Width = 100, Upscaled = true
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectHeightOnly()
    {
        // Arrange
        const string size = ",40";
        var expected = new SizeParameter
        {
            Height = 40
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectHeightOnlyScaled()
    {
        // Arrange
        const string size = "^,40";
        var expected = new SizeParameter
        {
            Height = 40, Upscaled = true
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectWidthHeight()
    {
        // Arrange
        const string size = "90,40";
        var expected = new SizeParameter
        {
            Width = 90, Height = 40
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectWidthHeightScaled()
    {
        // Arrange
        const string size = "^90,40";
        var expected = new SizeParameter
        {
            Width = 90, Height = 40, Upscaled = true
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectWidthHeightConfined()
    {
        // Arrange
        const string size = "!90,40";
        var expected = new SizeParameter
        {
            Width = 90, Height = 40, Confined = true
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectWidthHeightScaledConfined()
    {
        // Arrange
        const string size = "^!90,40";
        var expected = new SizeParameter
        {
            Width = 90, Height = 40, Confined = true, Upscaled = true
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectPercentage()
    {
        // Arrange
        const string size = "pct:30";
        var expected = new SizeParameter
        {
            PercentScale = 30f
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }

    [Fact]
    public void Parse_CorrectPercentageScaled()
    {
        // Arrange
        const string size = "^pct:30";
        var expected = new SizeParameter
        {
            PercentScale = 30f, Upscaled = true
        };

        // Act
        var sizeParameter = SizeParameter.Parse(size);

        // Assert
        sizeParameter.Should().BeEquivalentTo(expected);
        sizeParameter.ToString().Should().Be(size);
    }
}