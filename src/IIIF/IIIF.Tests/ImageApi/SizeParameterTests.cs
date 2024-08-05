using System;
using IIIF.ImageApi;

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

    [Theory]
    [InlineData(InvalidUpscaleBehaviour.Throw)]
    [InlineData(InvalidUpscaleBehaviour.ReturnOriginal)]
    public void Resize_UpscaleMax(InvalidUpscaleBehaviour invalidUpscaleBehaviour)
    {
        // Arrange
        var sp = SizeParameter.Parse("^max");
        var size = new Size(1, 1);
        
        // Act
        Action action = () => sp.Resize(size, invalidUpscaleBehaviour);
        
        // Assert
        action.Should().Throw<NotSupportedException>()
            .WithMessage("^max is not supported as maxWidth, maxHeight or maxArea unknown");
    }
    
    [Theory]
    [InlineData(100, 100, "max", 100, 100, "Square")]
    [InlineData(50, 100, "max", 50, 100, "Portrait")]
    [InlineData(100, 50, "max", 100, 50, "Landscape")]
    public void Resize_Max_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);

        // Act
        var actual = sp.Resize(size);

        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "50,", 50, 50, "Square")]
    [InlineData(50, 100, "50,", 50, 100, "Portrait")]
    [InlineData(100, 50, "50,", 50, 25, "Landscape")]
    public void Resize_Width_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "^50,", 50, 50, "Square")]
    [InlineData(50, 100, "^50,", 50, 100, "Portrait")]
    [InlineData(100, 50, "^50,", 50, 25, "Landscape")]
    [InlineData(100, 100, "^200,", 200, 200, "Square larger")]
    [InlineData(50, 100, "^200,", 200, 400, "Portrait larger")]
    [InlineData(100, 50, "^200,", 200, 100, "Landscape larger")]
    public void Resize_UpscaleWidth_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, ",50", 50, 50, "Square")]
    [InlineData(50, 100, ",50", 25, 50, "Portrait")]
    [InlineData(100, 50, ",50", 100, 50, "Landscape")]
    public void Resize_Height_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "^,50", 50, 50, "Square")]
    [InlineData(50, 100, "^,50", 25, 50, "Portrait")]
    [InlineData(100, 50, "^,50", 100, 50, "Landscape")]
    [InlineData(100, 100, "^,200", 200, 200, "Square larger")]
    [InlineData(50, 100, "^,200", 100, 200, "Portrait larger")]
    [InlineData(100, 50, "^,200", 400, 200, "Landscape larger")]
    public void Resize_UpscaleHeight_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "pct:50", 50, 50, "Square")]
    [InlineData(50, 100, "pct:50", 25, 50, "Portrait")]
    [InlineData(100, 50, "pct:50", 50, 25, "Landscape")]
    public void Resize_Percent_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "^pct:50", 50, 50, "Square")]
    [InlineData(50, 100, "^pct:50", 25, 50, "Portrait")]
    [InlineData(100, 50, "^pct:50", 50, 25, "Landscape")]
    [InlineData(100, 100, "^pct:200", 200, 200, "Square larger")]
    [InlineData(50, 100, "^pct:200", 100, 200, "Portrait larger")]
    [InlineData(100, 50, "^pct:200", 200, 100, "Landscape larger")]
    public void Resize_UpscalePercent_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "50,50", 50, 50, "Square")]
    [InlineData(50, 100, "50,50", 50, 50, "Portrait")]
    [InlineData(100, 50, "50,50", 50, 50, "Landscape")]
    public void Resize_WidthHeight_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "^50,50", 50, 50, "Square")]
    [InlineData(50, 100, "^50,50", 50, 50, "Portrait")]
    [InlineData(100, 50, "^50,50", 50, 50, "Landscape")]
    [InlineData(100, 100, "^200,200", 200, 200, "Square larger")]
    [InlineData(50, 100, "^200,200", 200, 200, "Portrait larger")]
    [InlineData(100, 50, "^200,200", 200, 200, "Landscape larger")]
    public void Resize_UpscaleWidthHeight_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "!50,50", 50, 50, "Square")]
    [InlineData(50, 100, "!50,50", 25, 50, "Portrait")]
    [InlineData(100, 50, "!50,50", 50, 25, "Landscape")]
    public void Resize_ConfinedWidthHeight_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "^!50,50", 50, 50, "Square")]
    [InlineData(50, 100, "^!50,50", 25, 50, "Portrait")]
    [InlineData(100, 50, "^!50,50", 50, 25, "Landscape")]
    [InlineData(100, 100, "^!200,200", 200, 200, "Square larger")]
    [InlineData(50, 100, "^!200,200", 100, 200, "Portrait larger")]
    [InlineData(100, 50, "^!200,200", 200, 100, "Landscape larger")]
    public void Resize_UpscaleConfinedWidthHeight_ThrowBehaviour(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "200,", "Square width")]
    [InlineData(50, 100, "200,", "Portrait width")]
    [InlineData(100, 50, "200,", "Landscape width")]
    [InlineData(100, 100, ",200", "Square height")]
    [InlineData(50, 100, ",200", "Portrait height")]
    [InlineData(100, 50, ",200", "Landscape height")]
    [InlineData(100, 100, "pct:200", "Square pct")]
    [InlineData(50, 100, "pct:200", "Portrait pct")]
    [InlineData(100, 50, "pct:200", "Landscape pct")]
    [InlineData(100, 100, "200,200", "Square width,height")]
    [InlineData(50, 100, "200,200", "Portrait width,height")]
    [InlineData(100, 50, "200,200", "Landscape width,height")]
    [InlineData(100, 100, "!200,200", "Square confined")]
    [InlineData(50, 100, "!200,200", "Portrait confined")]
    [InlineData(100, 50, "!200,200", "Landscape confined")]
    public void Resize_ThrowBehaviour_ThrowsIfWouldUpscale(int w, int h, string sizeParam, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        
        // Act
        Action action = () => sp.Resize(size, InvalidUpscaleBehaviour.Throw);
        
        // Assert
        action.Should().ThrowExactly<InvalidOperationException>(test)
            .WithMessage($"SizeParameter /{sizeParam}/ cannot upscale image size '{w},{h}'");
    }

    [Theory]
    [InlineData(100, 100, "max", 100, 100, "Square")]
    [InlineData(50, 100, "max", 50, 100, "Portrait")]
    [InlineData(100, 50, "max", 100, 50, "Landscape")]
    public void Resize_Max_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);

        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);

        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "50,", 50, 50, "Square")]
    [InlineData(50, 100, "50,", 50, 100, "Portrait")]
    [InlineData(100, 50, "50,", 50, 25, "Landscape")]
    [InlineData(100, 100, "200,", 100, 100, "Square larger")]
    [InlineData(50, 100, "200,", 50, 100, "Portrait larger")]
    [InlineData(100, 50, "200,", 100, 50, "Landscape larger")]
    public void Resize_Width_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "^50,", 50, 50, "Square")]
    [InlineData(50, 100, "^50,", 50, 100, "Portrait")]
    [InlineData(100, 50, "^50,", 50, 25, "Landscape")]
    [InlineData(100, 100, "^200,", 200, 200, "Square larger")]
    [InlineData(50, 100, "^200,", 200, 400, "Portrait larger")]
    [InlineData(100, 50, "^200,", 200, 100, "Landscape larger")]
    public void Resize_UpscaleWidth_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, ",50", 50, 50, "Square")]
    [InlineData(50, 100, ",50", 25, 50, "Portrait")]
    [InlineData(100, 50, ",50", 100, 50, "Landscape")]
    [InlineData(100, 100, ",200", 100, 100, "Square larger")]
    [InlineData(50, 100, ",200", 50, 100, "Portrait larger")]
    [InlineData(100, 50, ",200", 100, 50, "Landscape larger")]
    public void Resize_Height_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "^,50", 50, 50, "Square")]
    [InlineData(50, 100, "^,50", 25, 50, "Portrait")]
    [InlineData(100, 50, "^,50", 100, 50, "Landscape")]
    [InlineData(100, 100, "^,200", 200, 200, "Square larger")]
    [InlineData(50, 100, "^,200", 100, 200, "Portrait larger")]
    [InlineData(100, 50, "^,200", 400, 200, "Landscape larger")]
    public void Resize_UpscaleHeight_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "pct:50", 50, 50, "Square")]
    [InlineData(50, 100, "pct:50", 25, 50, "Portrait")]
    [InlineData(100, 50, "pct:50", 50, 25, "Landscape")]
    [InlineData(100, 100, "pct:200", 100, 100, "Square larger")]
    [InlineData(50, 100, "pct:200", 50, 100, "Portrait larger")]
    [InlineData(100, 50, "pct:200", 100, 50, "Landscape larger")]
    public void Resize_Percent_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "^pct:50", 50, 50, "Square")]
    [InlineData(50, 100, "^pct:50", 25, 50, "Portrait")]
    [InlineData(100, 50, "^pct:50", 50, 25, "Landscape")]
    [InlineData(100, 100, "^pct:200", 200, 200, "Square larger")]
    [InlineData(50, 100, "^pct:200", 100, 200, "Portrait larger")]
    [InlineData(100, 50, "^pct:200", 200, 100, "Landscape larger")]
    public void Resize_UpscalePercent_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "50,50", 50, 50, "Square")]
    [InlineData(50, 100, "50,50", 50, 50, "Portrait")]
    [InlineData(100, 50, "50,50", 50, 50, "Landscape")]
    [InlineData(100, 100, "200,200", 100, 100, "Square larger")]
    [InlineData(50, 100, "200,200", 50, 100, "Portrait larger")]
    [InlineData(100, 50, "200,200", 100, 50, "Landscape larger")]
    public void Resize_WidthHeight_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "^50,50", 50, 50, "Square")]
    [InlineData(50, 100, "^50,50", 50, 50, "Portrait")]
    [InlineData(100, 50, "^50,50", 50, 50, "Landscape")]
    [InlineData(100, 100, "^200,200", 200, 200, "Square larger")]
    [InlineData(50, 100, "^200,200", 200, 200, "Portrait larger")]
    [InlineData(100, 50, "^200,200", 200, 200, "Landscape larger")]
    public void Resize_UpscaleWidthHeight_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "!50,50", 50, 50, "Square")]
    [InlineData(50, 100, "!50,50", 25, 50, "Portrait")]
    [InlineData(100, 50, "!50,50", 50, 25, "Landscape")]
    [InlineData(100, 100, "!200,200", 100, 100, "Square larger")]
    [InlineData(50, 100, "!200,200", 50, 100, "Portrait larger")]
    [InlineData(100, 50, "!200,200", 100, 50, "Landscape larger")]
    public void Resize_ConfinedWidthHeight_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
    
    [Theory]
    [InlineData(100, 100, "^!50,50", 50, 50, "Square")]
    [InlineData(50, 100, "^!50,50", 25, 50, "Portrait")]
    [InlineData(100, 50, "^!50,50", 50, 25, "Landscape")]
    [InlineData(100, 100, "^!200,200", 200, 200, "Square larger")]
    [InlineData(50, 100, "^!200,200", 100, 200, "Portrait larger")]
    [InlineData(100, 50, "^!200,200", 200, 100, "Landscape larger")]
    public void Resize_UpscaleConfinedWidthHeight_ReturnOriginal(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
    {
        // Arrange
        var sp = SizeParameter.Parse(sizeParam);
        var size = new Size(w, h);
        var expected = new Size(expectedW, expectedH);
        
        // Act
        var actual = sp.Resize(size, InvalidUpscaleBehaviour.ReturnOriginal);
        
        // Assert
        actual.Should().BeEquivalentTo(expected, test);
    }
}