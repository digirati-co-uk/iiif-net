using System.Collections.Generic;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Strings;

namespace IIIF.Tests.Presentation.V3;

public class LabelValuePairXTests
{
    [Fact]
    public void TryGetValue_False_NullList()
    {
        List<LabelValuePair> metadata = null;

        var result = metadata.TryGetValue("en", "bar", out var languageMap);

        result.Should().BeFalse();
        languageMap.Should().BeNull();
    }
    
    [Fact]
    public void TryGetValue_False_EmptyList()
    {
        var metadata = new List<LabelValuePair>();

        var result = metadata.TryGetValue("en", "bar", out var languageMap);

        result.Should().BeFalse();
        languageMap.Should().BeNull();
    }
    
    [Fact]
    public void TryGetValue_True_ReturnsCorrect_IfFound()
    {
        var metadata = new List<LabelValuePair> { new("en", "foo", "bar", "baz") };
        var expected = new LanguageMap("en", new[] { "bar", "baz" });

        var result = metadata.TryGetValue("en", "foo", out var languageMap);

        result.Should().BeTrue();
        languageMap.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void TryGetValue_False_IfNotFoundForLanguage()
    {
        var metadata = new List<LabelValuePair> { new("en", "foo", "bar", "baz") };

        var result = metadata.TryGetValue("en", "bar", out var languageMap);

        result.Should().BeFalse();
        languageMap.Should().BeNull();
    }
    
    [Fact]
    public void TryGetValue_False_IfLabelForDifferentLanguage()
    {
        var metadata = new List<LabelValuePair> { new("en", "foo", "bar", "baz") };

        var result = metadata.TryGetValue("fr", "bar", out var languageMap);

        result.Should().BeFalse();
        languageMap.Should().BeNull();
    }
}