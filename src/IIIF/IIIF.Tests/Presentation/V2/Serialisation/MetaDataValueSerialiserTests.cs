using System;
using System.Linq;
using IIIF.Presentation.V2.Serialisation;
using IIIF.Presentation.V2.Strings;
using Newtonsoft.Json;


namespace IIIF.Tests.Presentation.V2.Serialisation;

public class MetaDataValueSerialiserTests
{
    private readonly MetaDataValueSerialiser sut;

    public MetaDataValueSerialiserTests()
    {
        sut = new MetaDataValueSerialiser();
    }

    [Fact]
    public void WriteJson_Throws_IfNoLanguageValues()
    {
        // Arrange
        var metadata = new MetaDataValue(Enumerable.Empty<LanguageValue>());

        // Act
        Action action = () => JsonConvert.SerializeObject(metadata, sut);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void WriteJson_WritesSingleValue_IfNoLanguage_SingleValue()
    {
        // Arrange
        var metadata = new MetaDataValue("Foo bar");
        const string expected = """ "Foo bar" """;

        // Act
        var result = JsonConvert.SerializeObject(metadata, sut);

        // Assert
        result.Should().Be(expected.Trim());
    }

    [Fact]
    public void WriteJson_WritesMultipleValues_IfNoLanguage_MultipleValues()
    {
        // Arrange
        var metadata = new MetaDataValue("foo");
        metadata.LanguageValues.Add(new LanguageValue { Value = "bar" });
        const string expected = """["foo","bar"]""";

        // Act
        var result = JsonConvert.SerializeObject(metadata, sut);
        // Assert
        result.Should().Be(expected.Trim());
    }

    [Fact]
    public void WriteJson_WritesLanguageAndValue_IfLanguage_SingleValue()
    {
        // Arrange
        var metadata = new MetaDataValue("Foo bar", "en");
        const string expected = """{"@value":"Foo bar","@language":"en"}""";

        // Act
        var result = JsonConvert.SerializeObject(metadata, sut);

        // Assert
        result.Should().Be(expected.Trim());
    }

    [Fact]
    public void WriteJson_WritesLanguageAndValue_IfMultipleValues_SingleLanguage()
    {
        // Arrange
        var metadata = new MetaDataValue("foo", "en");
        metadata.LanguageValues.Add(new LanguageValue { Value = "bar", Language = "en" });
        const string expected = """[{"@value":"foo","@language":"en"},{"@value":"bar","@language":"en"}]""";

        // Act
        var result = JsonConvert.SerializeObject(metadata, sut);

        // Assert
        result.Should().Be(expected.Trim());
    }

    [Fact]
    public void WriteJson_WritesLanguageAndValue_IfMultipleLanguages()
    {
        // Arrange
        var metadata = new MetaDataValue("foo", "en");
        metadata.LanguageValues.Add(new LanguageValue { Value = "bar", Language = "fr" });
        const string expected = """[{"@value":"foo","@language":"en"},{"@value":"bar","@language":"fr"}]""";

        // Act
        var result = JsonConvert.SerializeObject(metadata, sut);

        // Assert
        result.Should().Be(expected.Trim());
    }

    [Fact]
    public void ReadJson_SingleValue_NoLanguage()
    {
        // Arrange
        const string metadata =""" "Foo bar" """;
        var expected = new MetaDataValue("Foo bar");

        // Act
        var result = JsonConvert.DeserializeObject<MetaDataValue>(metadata.Trim());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadJson_MultipleValues_NoLanguage()
    {
        // Arrange
        const string metadata = """ ["foo","bar"] """;
        var expected = new MetaDataValue("foo");
        expected.LanguageValues.Add(new LanguageValue { Value = "bar" });

        // Act
        var result = JsonConvert.DeserializeObject<MetaDataValue>(metadata.Trim());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadJson_SingleValue_SingleLanguage()
    {
        // Arrange
        const string metadata = """{"@value":"Foo bar","@language":"en"}""";
        var expected = new MetaDataValue("Foo bar", "en");

        // Act
        var result = JsonConvert.DeserializeObject<MetaDataValue>(metadata.Trim());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadJson_MultipleValues_SingleLanguage()
    {
        // Arrange
        const string metadata = """[{"@value":"foo","@language":"en"},{"@value":"bar","@language":"en"}]""";
        var expected = new MetaDataValue("foo", "en");
        expected.LanguageValues.Add(new LanguageValue { Value = "bar", Language = "en" });

        // Act
        var result = JsonConvert.DeserializeObject<MetaDataValue>(metadata.Trim());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadJson_MultipleLanguages()
    {
        // Arrange
        const string metadata = """[{"@value":"foo","@language":"en"},{"@value":"bar","@language":"fr"}]""";
        var expected = new MetaDataValue("foo", "en");
        expected.LanguageValues.Add(new LanguageValue { Value = "bar", Language = "fr" });

        // Act
        var result = JsonConvert.DeserializeObject<MetaDataValue>(metadata.Trim());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadJson_Throws_IfFormatUnknown()
    {
        // Arrange
        const string metadata = "true";

        // Act
        Action action = () => JsonConvert.DeserializeObject<MetaDataValue>(metadata);

        // Assert
        action.Should().Throw<FormatException>();
    }
}