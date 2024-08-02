using System;
using IIIF.ImageApi;

namespace IIIF.Tests.ImageApi;

public class SizeParameterX
{
    [Theory]
    [InlineData(100, 100, "max", 100, 100, "Square")]
    [InlineData(50, 100, "max", 50, 100, "Portrait")]
    [InlineData(100, 50, "max", 100, 50, "Landscape")]
    public void Resize_Max(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
    
    [Fact]
    public void Resize_UpscaleMax()
    {
        // Arrange
        var sp = SizeParameter.Parse("^max");
        var size = new Size(1, 1);
        
        // Act
        Action action = () => sp.Resize(size);
        
        // Assert
        action.Should().Throw<NotSupportedException>()
            .WithMessage("^max is not supported as maxWidth, maxHeight or maxArea unknown");
    }
    
    [Theory]
    [InlineData(100, 100, "50,", 50, 50, "Square")]
    [InlineData(50, 100, "50,", 50, 100, "Portrait")]
    [InlineData(100, 50, "50,", 50, 25, "Landscape")]
    [InlineData(100, 100, "200,", 100, 100, "Square larger")]
    [InlineData(50, 100, "200,", 50, 100, "Portrait larger")]
    [InlineData(100, 50, "200,", 100, 50, "Landscape larger")]
    public void Resize_Width(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
    public void Resize_UpscaleWidth(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
    [InlineData(100, 100, ",200", 100, 100, "Square larger")]
    [InlineData(50, 100, ",200", 50, 100, "Portrait larger")]
    [InlineData(100, 50, ",200", 100, 50, "Landscape larger")]
    public void Resize_Height(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
    public void Resize_UpscaleHeight(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
    [InlineData(100, 100, "pct:200", 100, 100, "Square larger")]
    [InlineData(50, 100, "pct:200", 50, 100, "Portrait larger")]
    [InlineData(100, 50, "pct:200", 100, 50, "Landscape larger")]
    public void Resize_Percent(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
    public void Resize_UpscalePercent(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
    [InlineData(100, 100, "200,200", 100, 100, "Square larger")]
    [InlineData(50, 100, "200,200", 50, 100, "Portrait larger")]
    [InlineData(100, 50, "200,200", 100, 50, "Landscape larger")]
    public void Resize_WidthHeight(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
    public void Resize_UpscaleWidthHeight(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
    [InlineData(100, 100, "!200,200", 100, 100, "Square larger")]
    [InlineData(50, 100, "!200,200", 50, 100, "Portrait larger")]
    [InlineData(100, 50, "!200,200", 100, 50, "Landscape larger")]
    public void Resize_ConfinedWidthHeight(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
    public void Resize_UpscaleConfinedWidthHeight(int w, int h, string sizeParam, int expectedW, int expectedH, string test)
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
}