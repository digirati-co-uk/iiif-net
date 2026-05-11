using System;
using FluentAssertions;
using IIIF.Serialisation;
using Newtonsoft.Json;
using Xunit;

namespace IIIF.Tests.Serialisation;

public class XsdDateTimeConverterTests
{
    private readonly XsdDateTimeConverter sut = new();

    [Fact]
    public void Convert_UtcDate_Success()
    {
        // Arrange
        var date = new DateTime(2023, 3, 3, 11, 08, 37);
        var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        const string expected = """
        "2023-03-03T11:08:37Z"
        """;

        // Act
        var result = JsonConvert.SerializeObject(utcDate, Formatting.None, sut);

        // Assert
        result.Should().Be(expected.Trim());
    }

    [Fact]
    public void Convert_LocalDate_Success()
    {
        // Arrange
        var date = new DateTime(2023, 3, 3, 11, 08, 37);
        var localDate = DateTime.SpecifyKind(date, DateTimeKind.Local);
        const string expected = """
        "2023-03-03T11:08:37+00:00"
        """;

        // Act
        var result = JsonConvert.SerializeObject(localDate, Formatting.None, sut);

        // Assert
        result.Should().Be(expected.Trim());
    }

    [Fact]
    public void Convert_UnspecifiedDate_Success()
    {
        // Arrange
        var date = new DateTime(2023, 3, 3, 11, 08, 37);
        var unspecifiedDate = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
        const string expected = """
        "2023-03-03T11:08:37"
        """;

        // Act
        var result = JsonConvert.SerializeObject(unspecifiedDate, Formatting.None, sut);

        // Assert
        result.Should().Be(expected.Trim());
    }
}