using System;
using System.Collections.Generic;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Tests.Serialisation;

public class LanguageMapSerialiserTests
{
    private readonly LanguageMapSerialiser sut;

    public LanguageMapSerialiserTests()
    {
        sut = new LanguageMapSerialiser();
    }

    [Fact]
    public void WriteJson_WritesSingleLine_IfSingleLanguageWithSingleShortValue()
    {
        // Arrange
        var languageMap = new LanguageMap("en", "single and short");
        const string expected = """
        {"en":["single and short"]}
        """;

        // Act
        var result = JsonConvert.SerializeObject(languageMap, Formatting.Indented, sut);

        // Assert
        result.Should().Be(expected.Trim());
    }

    [Fact]
    public void ReadJson_SingleLanguageSingleValue()
    {
        // Arrange
        var expected = new LanguageMap("en", "auto rock");
        const string json = """
        {"en":["auto rock"]}
        """;

        // Act
        var result = JsonConvert.DeserializeObject<LanguageMap>(json.Trim(), sut);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ReadJson_SingleLanguageMultiValues()
    {
        // Arrange
        var expected = new LanguageMap("en", new[] { "we're no here", "hungry face" });
        const string json = """
        {"en":["we're no here", "hungry face"]}
        """;

        // Act
        var result = JsonConvert.DeserializeObject<LanguageMap>(json.Trim(), sut);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void ReadJson_MultiLanguageMultiValues()
    {
        // Arrange
        var expected = new LanguageMap("en", new[] { "we're no here", "hungry face" });
        expected.Add("fr", new List<string> { "le voyage est dangereux" });
        var json = "{\"en\":[\"we're no here\", \"hungry face\"], \"fr\":[\"le voyage est dangereux\"]}";

        // Act
        var result = JsonConvert.DeserializeObject<LanguageMap>(json, sut);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void WriteJson_WritesIndented_IfSingleLanguageWithSingleLongValue()
    {
        // Arrange
        var languageMap = new LanguageMap("en", "this string is slightly longer than the threshold");
        var expected = $"{{{Environment.NewLine}  \"en\": [{Environment.NewLine}    \"this string is slightly longer than the threshold\"{Environment.NewLine}  ]{Environment.NewLine}}}";

        // Act
        var result = JsonConvert.SerializeObject(languageMap, Formatting.Indented, sut);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void WriteJson_WritesIndented_IfSingleLanguageWithMultipleShortValues()
    {
        // Arrange
        var languageMap = new LanguageMap("en", new[] { "single and short", "also short" });
        var expected = $"{{{Environment.NewLine}  \"en\": [{Environment.NewLine}    \"single and short\",{Environment.NewLine}    \"also short\"{Environment.NewLine}  ]{Environment.NewLine}}}";

        // Act
        var result = JsonConvert.SerializeObject(languageMap, Formatting.Indented, sut);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void WriteJson_WritesIndented_IfMultiLanguageWithShortValues()
    {
        // Arrange
        var languageMap = new LanguageMap("en", "single and short");
        languageMap.Add("fr", new List<string> { "also short" });
        var expected = $"{{{Environment.NewLine}  \"en\": [{Environment.NewLine}    \"single and short\"{Environment.NewLine}  ],{Environment.NewLine}  \"fr\": [{Environment.NewLine}    \"also short\"{Environment.NewLine}  ]{Environment.NewLine}}}";

        // Act
        var result = JsonConvert.SerializeObject(languageMap, Formatting.Indented, sut);

        // Assert
        result.Should().Be(expected);
    }
}