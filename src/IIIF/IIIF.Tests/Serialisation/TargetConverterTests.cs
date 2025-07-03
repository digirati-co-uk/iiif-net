using System.Collections.Generic;
using FluentAssertions;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Serialisation;
using Newtonsoft.Json;
using Xunit;

namespace IIIF.Tests.Serialisation;

public class TargetConverterTests
{
    private readonly TargetConverter sut;

    public TargetConverterTests()
    {
        sut = new TargetConverter();
    }

    [Fact]
    public void ConvertCanvas_NoDimensions_OutputsIdOnly()
    {
        // Arrange
        const string testId = "https://test.example.com/canvas";
        var canvas = new Canvas { Id = testId };

        // Act
        var result = JsonConvert.SerializeObject(canvas, Formatting.None, sut);

        // Assert
        result.Should().Be($"\"{testId}\"");
    }

    [Fact]
    public void ConvertCanvas_EmptyItems_OutputsIdOnly()
    {
        // Arrange
        const string testId = "https://test.example.com/canvas";
        var canvas = new Canvas { Id = testId, Items = new List<AnnotationPage>(0) };

        // Act
        var result = JsonConvert.SerializeObject(canvas, Formatting.None, sut);

        // Assert
        result.Should().Be($"\"{testId}\"");
    }

    [Fact]
    public void ConvertCanvas_SerialiseTargetAsIdTrue_OnlyReturnsId()
    {
        // Arrange
        const string testId = "https://test.example.com/canvas";
        var canvas = new Canvas { Id = testId, Width = 300, Height = 200, SerialiseTargetAsId = true };

        // Act
        var result = JsonConvert.SerializeObject(canvas, Formatting.None, sut);

        // Assert
        result.Should().Be($"\"{testId}\"");
    }

    [Fact]
    public void CanDeserialise_SerialisedIdOnlyCanvas()
    {
        // Arrange
        var canvas = new Canvas { Id = "https://test.example.com/canvas" };
        var serialised = JsonConvert.SerializeObject(canvas, Formatting.None, sut);

        // Act
        var deserialised = JsonConvert.DeserializeObject<ResourceBase>(serialised, sut);

        // Assert
        deserialised.Should().BeEquivalentTo(canvas);
    }
}