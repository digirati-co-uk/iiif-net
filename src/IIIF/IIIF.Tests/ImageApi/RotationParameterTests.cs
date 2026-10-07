using System;
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
    public void Parse_Throws_IfInvalid(string rotation, string reason)
    {
        // Act
        Action action = () => RotationParameter.Parse(rotation);

        // Assert
        action.Should().ThrowExactly<RotationException>(reason)
            .WithMessage($"Expected 'n' or '!n'. Found: {rotation}");
    }
}