using System;
using System.Globalization;
using IIIF.Exceptions;
using IIIF.ImageApi;

namespace IIIF.Tests.ImageApi;

public class SizeParameterTests
{
    [Fact]
    public void Parse_Throws_IfNull()
    {
        // Act
        Action action = () => SizeParameter.Parse(null!);

        // Assert
        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Parse_Full_IsMax()
    {
        // Act
        var sizeParameter = SizeParameter.Parse("full");

        // Assert
        sizeParameter.Should().BeEquivalentTo(new SizeParameter { Max = true });
    }

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
    [InlineData("", "Empty")]
    [InlineData("^", "Upscale only")]
    [InlineData("!", "Confined only")]
    [InlineData(",", "No dimensions")]
    [InlineData("^,", "Upscaled no dimensions")]
    [InlineData("100", "No comma")]
    [InlineData("!100", "Confined no comma")]
    [InlineData("1,2,3", "Too many commas")]
    [InlineData("a,100", "Non-numeric width")]
    [InlineData("100,b", "Non-numeric height")]
    [InlineData("!100,", "Confined width only")]
    [InlineData("!,100", "Confined height only")]
    [InlineData("^!100,", "Upscaled confined width only")]
    [InlineData("^!,100", "Upscaled confined height only")]
    [InlineData("!,", "Confined no dimensions")]
    [InlineData("pct:", "Percent no value")]
    [InlineData("pct:abc", "Percent non-numeric")]
    [InlineData("!pct:50", "Confined percent")]
    [InlineData("pc:50", "Invalid percent prefix")]
    [InlineData("!max", "Confined max")]
    [InlineData("^full", "Upscaled full")]
    [InlineData("!full", "Confined full")]
    [InlineData("0,", "Zero width")]
    [InlineData(",0", "Zero height")]
    [InlineData("!0,0", "Confined zero")]
    [InlineData("-100,", "Negative width")]
    [InlineData(",-100", "Negative height")]
    [InlineData("+100,", "Signed width")]
    [InlineData(" 100,", "Whitespace width")]
    [InlineData("1.5,", "Decimal width")]
    [InlineData("pct:0", "Zero percent")]
    [InlineData("pct:-5", "Negative percent")]
    [InlineData("pct:NaN", "NaN percent")]
    [InlineData("pct:1e2", "Exponent percent")]
    public void Parse_Throws_IfInvalid(string size, string reason)
    {
        // Act
        Action action = () => SizeParameter.Parse(size);

        // Assert
        action.Should().ThrowExactly<SizeException>(reason)
            .WithMessage($"Expected 'max', 'w,', ',h', 'pct:n', 'w,h' or '!w,h', optionally prefixed with '^'. Found: {size}");
    }

    [Theory]
    [InlineData("pct:12.5", null, null, 12.5f)]
    [InlineData("^pct:150.25", null, null, 150.25f)]
    [InlineData("1000,", 1000, null, null)]
    [InlineData(",2000", null, 2000, null)]
    [InlineData("1000,2000", 1000, 2000, null)]
    [InlineData("!1000,2000", 1000, 2000, null)]
    [InlineData("^1000,2000", 1000, 2000, null)]
    public void Parse_IsCultureInvariant(string size, int? expectedWidth, int? expectedHeight,
        float? expectedPercent)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            // Arrange - de-DE uses ',' as decimal separator and '.' as group separator
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

            // Act
            var sizeParameter = SizeParameter.Parse(size);

            // Assert
            sizeParameter.Width.Should().Be(expectedWidth);
            sizeParameter.Height.Should().Be(expectedHeight);
            sizeParameter.PercentScale.Should().Be(expectedPercent);
            sizeParameter.ToString().Should().Be(size);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Theory]
    [InlineData("1.000,")]
    [InlineData(",2.000")]
    [InlineData("1.000,2.000")]
    public void Parse_Throws_IfGroupSeparator_InNonInvariantCulture(string size)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            // Arrange - '.' is the de-DE group separator
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

            // Act
            Action action = () => SizeParameter.Parse(size);

            // Assert
            action.Should().ThrowExactly<SizeException>();
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
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