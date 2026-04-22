using System.Linq;
using IIIF.Presentation.V3;
using IIIF.Serialisation;
using IIIF.Tests.Serialisation.Data;

namespace IIIF.Tests.Serialisation;

public class LanguageFilterTests
{
    [Fact]
    public void WithLanguages_RemovesExcludedLanguage_FromRootLabel()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        collection.WithLanguages("en");

        collection.Label.Should().ContainKey("en");
        collection.Label.Should().NotContainKey("nl");
    }

    [Fact]
    public void WithLanguages_RemovesExcludedLanguage_FromAllItemLabels()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        collection.WithLanguages("en");

        foreach (var item in collection.Items!.Cast<Manifest>())
            item.Label.Should().NotContainKey("nl",
                because: $"nl should have been removed from item '{item.Id}'");
    }

    [Fact]
    public void WithLanguages_PreservesIncludedLanguage_Values()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        collection.WithLanguages("en");

        collection.Label!["en"].Should().Contain("Sample Exhibitions");

        var exhibit10 = collection.Items!.Cast<Manifest>()
            .Single(m => m.Id!.Contains("exhibit-10"));
        exhibit10.Label!["en"].Should().Contain("Through the Lens of a Professor");
    }

    [Fact]
    public void WithLanguages_MultipleLanguages_KeepsBoth()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        collection.WithLanguages("en", "nl");

        collection.Label.Should().ContainKey("en");
        collection.Label.Should().ContainKey("nl");
    }

    [Fact]
    public void WithLanguages_IsCaseInsensitive()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        // "EN" should match the "en" key in the LanguageMap
        collection.WithLanguages("EN");

        collection.Label.Should().ContainKey("en");
        collection.Label.Should().NotContainKey("nl");
    }

    [Fact]
    public void WithLanguages_EmptyLanguages_IsNoOp()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        collection.WithLanguages(new string[0]);

        collection.Label.Should().ContainKey("en");
        collection.Label.Should().ContainKey("nl");
    }

    [Fact]
    public void WithLanguages_ReturnsOriginalInstance_ForChaining()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        var result = collection.WithLanguages("en");

        result.Should().BeSameAs(collection);
    }

    [Fact]
    public void WithLanguages_AsJson_ContainsOnlyRequestedLanguage()
    {
        var json = ManifestDataFileTwo.Json.FromJson<Collection>()
            .WithLanguages("en")
            .AsJson();

        json.Should().Contain("\"en\"");
        json.Should().NotContain("\"nl\"");
    }

    [Fact]
    public void WithLanguages_CanChain_WithWithoutAdditionalProperties()
    {
        var json = ManifestDataFileTwo.Json.FromJson<Collection>()
            .WithoutAdditionalProperties()
            .WithLanguages("en")
            .AsJson();

        json.Should().NotContain("hss:slug");
        json.Should().NotContain("\"nl\"");
        json.Should().Contain("\"en\"");
        json.Should().Contain(ManifestDataFileTwo.CollectionId);
    }
}