using System.Collections.Generic;
using FluentAssertions;
using IIIF.Presentation.V3;
using Xunit;

namespace IIIF.Tests.Presentation.V3;

public class ManifestXTests
{
    [Fact]
    public void ContainsAV_False_IfItemsNull()
    {
        // Arrange
        var manifest = new Manifest();

        // Act
        var result = manifest.ContainsAV();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsAV_False_IfItemsEmpty()
    {
        // Arrange
        var manifest = new Manifest { Items = new List<Canvas>() };

        // Act
        var result = manifest.ContainsAV();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsAV_False_IfItemsNullDuration()
    {
        // Arrange
        var manifest = new Manifest
        {
            Items = new List<Canvas>
            {
                new() { Id = "test", Duration = null }
            }
        };

        // Act
        var result = manifest.ContainsAV();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsAV_False_IfItems0Duration()
    {
        // Arrange
        var manifest = new Manifest
        {
            Items = new List<Canvas>
            {
                new() { Id = "test", Duration = 0 }
            }
        };

        // Act
        var result = manifest.ContainsAV();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ContainsAV_True_IfItemsContainsAbove0Duration()
    {
        // Arrange
        var manifest = new Manifest
        {
            Items = new List<Canvas>
            {
                new() { Id = "test", Duration = null },
                new() { Id = "test", Duration = 0 },
                new() { Id = "test", Duration = 1 }
            }
        };

        // Act
        var result = manifest.ContainsAV();

        // Assert
        result.Should().BeTrue();
    }
}