using System.Collections.Generic;
using FluentAssertions;
using IIIF.Serialisation;
using Newtonsoft.Json;
using Xunit;

namespace IIIF.Tests.Serialisation;

public class PrettyIIIFContractResolverTests
{
    private readonly JsonSerializerSettings jsonSerializerSettings;

    public PrettyIIIFContractResolverTests()
    {
        // NOTE: Using JsonSerializerSettings to facilitate testing as it makes it a LOT easier
        jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new PrettyIIIFContractResolver()
        };
    }

    [Theory]
    [InlineData(0, 123, "{\"height\":123}")]
    [InlineData(123, 0, "{\"width\":123}")]
    [InlineData(99, 123, "{\"width\":99,\"height\":123}")]
    [InlineData(0, 0, "{}")]
    public void WidthHeightIgnored_IfZero(int width, int height, string expected)
    {
        // Arrange
        var testClass = new TestClass { Width = width, Height = height };

        // Act
        var output = JsonConvert.SerializeObject(testClass, jsonSerializerSettings);

        // Assert
        output.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, 123, "{\"height\":123}")]
    [InlineData(null, 123, "{\"height\":123}")]
    [InlineData(123, 0, "{\"width\":123}")]
    [InlineData(123, null, "{\"width\":123}")]
    [InlineData(99, 123, "{\"width\":99,\"height\":123}")]
    [InlineData(0, 0, "{}")]
    [InlineData(null, null, "{}")]
    public void NullableWidthHeightIgnored_IfZeroOrNull(int? width, int? height, string expected)
    {
        // Arrange
        var testClass = new NullableTestClass { Width = width, Height = height };

        // Act
        var output = JsonConvert.SerializeObject(testClass, jsonSerializerSettings);

        // Assert
        output.Should().Be(expected);
    }

    [Fact]
    public void NullList_NotSerialised()
    {
        // Arrange
        var testClass = new TestClass { OptionalList = null };
        const string expected = "{}";

        // Act
        var output = JsonConvert.SerializeObject(testClass, jsonSerializerSettings);

        // Assert
        output.Should().Be(expected);
    }

    [Fact]
    public void EmptyList_NotSerialised()
    {
        // Arrange
        var testClass = new TestClass { OptionalList = new List<string>() };
        const string expected = "{}";

        // Act
        var output = JsonConvert.SerializeObject(testClass, jsonSerializerSettings);

        // Assert
        output.Should().Be(expected);
    }

    [Fact]
    public void PopulatedList_Serialised()
    {
        // Arrange
        var testClass = new TestClass { OptionalList = new List<string> { "foo" } };
        const string expected = "{\"optionalList\":[\"foo\"]}";

        // Act
        var output = JsonConvert.SerializeObject(testClass, jsonSerializerSettings);

        // Assert
        output.Should().Be(expected);
    }

    [Fact]
    public void NullList_NotSerialised_IfMarkedAsRequired()
    {
        // Arrange
        var testClass = new TestClass { RequiredList = null };
        const string expected = "{}";

        // Act
        var output = JsonConvert.SerializeObject(testClass, jsonSerializerSettings);

        // Assert
        output.Should().Be(expected);
    }

    [Fact]
    public void EmptyList_Serialised_IfMarkedAsRequired()
    {
        // Arrange
        var testClass = new TestClass { RequiredList = new List<string>() };
        const string expected = "{\"requiredList\":[]}";

        // Act
        var output = JsonConvert.SerializeObject(testClass, jsonSerializerSettings);

        // Assert
        output.Should().Be(expected);
    }

    [Fact]
    public void PopulatedList_Serialised_IfMarkedAsRequired()
    {
        // Arrange
        var testClass = new TestClass { RequiredList = new List<string> { "foo" } };
        const string expected = "{\"requiredList\":[\"foo\"]}";

        // Act
        var output = JsonConvert.SerializeObject(testClass, jsonSerializerSettings);

        // Assert
        output.Should().Be(expected);
    }

    [Fact]
    public void List_SerialisedAsObject_IfSingleElement_AndMarkedAsObjectIfSingle()
    {
        // Arrange
        var testClass = new TestClass { OneOrMore = new List<string> { "foo" } };
        const string expected = "{\"oneOrMore\":\"foo\"}";

        // Act
        var output = JsonConvert.SerializeObject(testClass, jsonSerializerSettings);

        // Assert
        output.Should().Be(expected);
    }

    [Fact]
    public void List_SerialisedAsArray_IfMultipleElements_AndMarkedAsObjectIfSingle()
    {
        // Arrange
        var testClass = new TestClass { OneOrMore = new List<string> { "foo", "bar" } };
        const string expected = "{\"oneOrMore\":[\"foo\",\"bar\"]}";

        // Act
        var output = JsonConvert.SerializeObject(testClass, jsonSerializerSettings);

        // Assert
        output.Should().Be(expected);
    }

    private class TestClass
    {
        public List<string> OptionalList { get; set; }

        [RequiredOutput] public List<string> RequiredList { get; set; }

        [ObjectIfSingle] public List<string> OneOrMore { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }

    private class NullableTestClass
    {
        public int? Width { get; set; }

        public int? Height { get; set; }
    }
}