using System;
using System.Globalization;
using IIIF.Exceptions;
using IIIF.ImageApi;

namespace IIIF.Tests.ImageApi;

public class RotationParameterTests
{
    [Fact]
    public void Parse_Correct_Angle()
    {
        const string rotation = "90";
        var expected = new RotationParameter
        {
            Angle = 90
        };

        // Act
        var rotationParameter = RotationParameter.Parse(rotation);

        // Assert
        rotationParameter.Should().BeEquivalentTo(expected);
        rotationParameter.ToString().Should().Be(rotation);
    }
    
    [Fact]
    public void Parse_Correct_Mirrored()
    {
        const string rotation = "!180";
        var expected = new RotationParameter
        {
            Angle = 180,
            Mirror = true
        };

        // Act
        var rotationParameter = RotationParameter.Parse(rotation);

        // Assert
        rotationParameter.Should().BeEquivalentTo(expected);
        rotationParameter.ToString().Should().Be(rotation);
    }

    [Theory]
    [InlineData("", "Empty")]
    [InlineData("!", "Mirror only")]
    [InlineData("abc", "Non-numeric")]
    [InlineData("!abc", "Mirrored non-numeric")]
    [InlineData("-90", "Negative")]
    [InlineData("!-90", "Mirrored negative")]
    [InlineData("360.5", "Greater than 360")]
    [InlineData("720", "Greater than 360")]
    [InlineData("NaN", "NaN")]
    [InlineData("Infinity", "Infinity")]
    [InlineData(" 90", "Whitespace")]
    [InlineData("1,000", "Thousands separator")]
    public void Parse_Throws_IfInvalid(string rotation, string reason)
    {
        // Act
        Action action = () => RotationParameter.Parse(rotation);

        // Assert
        action.Should().ThrowExactly<RotationException>(reason)
            .WithMessage($"Expected 'n' or '!n'. Found: {rotation}");
    }

    [Theory]
    [InlineData("0", 0f, false)]
    [InlineData("360", 360f, false)]
    [InlineData("22.5", 22.5f, false)]
    [InlineData("!22.5", 22.5f, true)]
    public void Parse_IsCultureInvariant(string rotation, float expectedAngle, bool expectedMirror)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            // Arrange - de-DE uses ',' as decimal separator and '.' as group separator
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

            // Act
            var rotationParameter = RotationParameter.Parse(rotation);

            // Assert
            rotationParameter.Angle.Should().Be(expectedAngle);
            rotationParameter.Mirror.Should().Be(expectedMirror);
            rotationParameter.ToString().Should().Be(rotation);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }
}