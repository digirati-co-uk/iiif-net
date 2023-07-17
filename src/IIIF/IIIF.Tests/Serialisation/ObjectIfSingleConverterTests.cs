using System.Collections.Generic;
using FluentAssertions;
using IIIF.Serialisation;
using Newtonsoft.Json;
using Xunit;

namespace IIIF.Tests.Serialisation;

public class ObjectIfSingleConverterTests
{
    private readonly ObjectIfSingleConverter sut;

    public ObjectIfSingleConverterTests()
    {
        sut = new ObjectIfSingleConverter();
    }

    [Fact]
    public void ReadJson_Single_String()
    {
        // Arrange
        const string value = "\"Foo bar\"";
        var expected = new List<string> { "Foo bar" };

        // Act
        var result = JsonConvert.DeserializeObject<List<string>>(value, sut);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadJson_Array_String()
    {
        // Arrange
        const string value = "[\"Foo\",\"bar\"]";
        var expected = new List<string> { "Foo", "bar" };

        // Act
        var result = JsonConvert.DeserializeObject<List<string>>(value, sut);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadJson_Single_ComplexType()
    {
        // Arrange
        var value = new ComplexType { Age = 50, Name = "grogu" };
        var json = JsonConvert.SerializeObject(value);
        var expected = new List<ComplexType> { value };

        // Act
        // Act
        var result = JsonConvert.DeserializeObject<List<ComplexType>>(json, sut);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadJson_Array_ComplexType()
    {
        // Arrange
        var expected = new List<ComplexType>
        {
            new() { Age = 50, Name = "grogu" },
            new() { Age = 38, Name = "bo-katan" }
        };
        var json = JsonConvert.SerializeObject(expected);

        // Act
        // Act
        var result = JsonConvert.DeserializeObject<List<ComplexType>>(json, sut);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}

public class ComplexType
{
    public string Name { get; set; }
    public int Age { get; set; }
}