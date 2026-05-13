using System.Linq;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Serialisation;
using IIIF.Tests.Serialisation.Data;
using Newtonsoft.Json.Linq;

namespace IIIF.Tests.Serialisation;

public class AdditionalPropertiesTests
{
    // -------------------------------------------------------------------------
    // Basic inline manifest (shallow smoke tests)
    // -------------------------------------------------------------------------

    private const string SimpleManifestWithExtras = """
        {
          "@context": "http://iiif.io/api/presentation/3/context.json",
          "id": "https://example.org/manifest/1",
          "type": "Manifest",
          "label": { "en": ["Test"] },
          "slug": "my-slug",
          "publicId": "abc123",
          "items": [
            {
              "id": "https://example.org/canvas/1",
              "type": "Canvas",
              "height": 100,
              "width": 100,
              "deepProp": "shouldBeGone",
              "items": []
            }
          ]
        }
        """;

    [Fact]
    public void FromJson_PopulatesAdditionalProperties_WithUnknownRootFields()
    {
        var manifest = SimpleManifestWithExtras.FromJson<Manifest>();

        manifest.AdditionalProperties.Should().ContainKey("slug");
        manifest.AdditionalProperties.Should().ContainKey("publicId");
    }

    [Fact]
    public void FromJson_PopulatesAdditionalProperties_WithUnknownDeepFields()
    {
        var manifest = SimpleManifestWithExtras.FromJson<Manifest>();

        manifest.Items![0].AdditionalProperties.Should().ContainKey("deepProp");
    }

    [Fact]
    public void WithoutAdditionalProperties_ClearsAll_OnRootAndDescendants()
    {
        var manifest = SimpleManifestWithExtras.FromJson<Manifest>();

        manifest.WithoutAdditionalProperties();

        manifest.AdditionalProperties.Should().BeEmpty();
        manifest.Items![0].AdditionalProperties.Should().BeEmpty();
    }

    [Fact]
    public void WithoutAdditionalProperties_WithKeys_RemovesOnlyNamedKeys_AtAllDepths()
    {
        var manifest = SimpleManifestWithExtras.FromJson<Manifest>();

        manifest.WithoutAdditionalProperties("slug", "deepProp");

        manifest.AdditionalProperties.Should().NotContainKey("slug");
        manifest.AdditionalProperties.Should().ContainKey("publicId"); // untouched
        manifest.Items![0].AdditionalProperties.Should().NotContainKey("deepProp");
    }

    [Fact]
    public void WithoutAdditionalProperties_ReturnsOriginalInstance_ForChaining()
    {
        var manifest = SimpleManifestWithExtras.FromJson<Manifest>();

        var result = manifest.WithoutAdditionalProperties();

        result.Should().BeSameAs(manifest);
    }

    [Fact]
    public void AsJson_AfterWithoutAdditionalProperties_ContainsNoExtraFields()
    {
        var json = SimpleManifestWithExtras.FromJson<Manifest>()
            .WithoutAdditionalProperties()
            .AsJson();

        json.Should().NotContain("slug");
        json.Should().NotContain("publicId");
        json.Should().NotContain("deepProp");
        json.Should().Contain("\"id\"");
        json.Should().Contain("\"label\"");
    }

    [Fact]
    public void WithoutAdditionalProperties_EmptyKeys_IsNoOp()
    {
        var manifest = SimpleManifestWithExtras.FromJson<Manifest>();

        manifest.WithoutAdditionalProperties(new string[0]);

        manifest.AdditionalProperties.Should().ContainKey("slug");
    }

    // -------------------------------------------------------------------------
    // ManifestDataFileOne — deep complex Manifest
    // -------------------------------------------------------------------------

    [Fact]
    public void ManifestDataFileOne_DeserializesWithout_AdditionalProperties()
    {
        // A well-formed IIIF manifest with no adjuncts should have empty AdditionalProperties everywhere
        var manifest = ManifestDataFileOne.Json.FromJson<Manifest>();

        manifest.AdditionalProperties.Should().BeEmpty();

        foreach (var canvas in manifest.Items!)
            canvas.AdditionalProperties.Should().BeEmpty();
    }

    [Fact]
    public void ManifestDataFileOne_RoundTrips_IdAndLabel()
    {
        var manifest = ManifestDataFileOne.Json.FromJson<Manifest>();

        manifest.Id.Should().Be(ManifestDataFileOne.ManifestId);
        manifest.Label!["en"].Should().ContainSingle()
            .Which.Should().Be("Welcome to Example Region: The art of Sample Artist");
    }

    [Fact]
    public void ManifestDataFileOne_RoundTrips_CanvasStructure()
    {
        var manifest = ManifestDataFileOne.Json.FromJson<Manifest>();

        manifest.Items.Should().HaveCount(4);

        var firstCanvas = manifest.Items![0];
        firstCanvas.Behavior.Should().Contain("w-7").And.Contain("h-6");
        firstCanvas.Items.Should().HaveCount(1);
        firstCanvas.Items![0].Items.Should().HaveCount(1); // 1 painting annotation
    }

    [Fact]
    public void ManifestDataFileOne_WithAdjunctsAdded_StripsAll_Recursively()
    {
        // Simulate iiif-presentation injecting adjuncts after deserialization
        var manifest = ManifestDataFileOne.Json.FromJson<Manifest>();
        manifest.AdditionalProperties["slug"] = "injected-slug";
        manifest.AdditionalProperties["publicId"] = "injected-id";
        manifest.Items![0].AdditionalProperties["canvasAdjunct"] = "some-value";
        manifest.Items![0].Items![0].AdditionalProperties["pageAdjunct"] = "page-value";

        manifest.WithoutAdditionalProperties();

        manifest.AdditionalProperties.Should().BeEmpty();
        manifest.Items![0].AdditionalProperties.Should().BeEmpty();
        manifest.Items![0].Items![0].AdditionalProperties.Should().BeEmpty();
    }

    [Fact]
    public void ManifestDataFileOne_WithAdjunctsAdded_StripsNamed_LeavesOthers()
        {
            var manifest = ManifestDataFileOne.Json.FromJson<Manifest>();
        manifest.AdditionalProperties["slug"] = "injected-slug";
        manifest.AdditionalProperties["publicId"] = "injected-id";
        manifest.Items![0].AdditionalProperties["slug"] = "canvas-slug"; // same key, deeper level

        manifest.WithoutAdditionalProperties("slug");

        manifest.AdditionalProperties.Should().NotContainKey("slug");
        manifest.AdditionalProperties.Should().ContainKey("publicId"); // untouched
        manifest.Items![0].AdditionalProperties.Should().NotContainKey("slug"); // stripped at depth too
    }

    [Fact]
    public void ManifestDataFileOne_AsJson_AfterStrip_PreservesAllIIIFProperties()
    {
        var manifest = ManifestDataFileOne.Json.FromJson<Manifest>();
        manifest.AdditionalProperties["slug"] = "injected-slug";

        var json = manifest.WithoutAdditionalProperties().AsJson();

        json.Should().NotContain("slug");
        json.Should().Contain(ManifestDataFileOne.ManifestId);
        // All 4 canvases survive
        json.Should().Contain("https://example.org/iiif/sample-manifset.json/canvas/ooexgyxgavl-mb99kqia");
    }

    [Fact]
    public void ManifestDataFileOne_ServiceAdditionalProperties_ArePreserved_BeforeStrip()
    {
        // Verify service nodes (ImageService2 + ImageService3) in the graph survive deserialisation
        var manifest = ManifestDataFileOne.Json.FromJson<Manifest>();

        var paintingAnnotation = manifest.Items![0]   // first Canvas
            .Items![0]                                // AnnotationPage
            .Items![0]                                // Annotation
            .As<PaintingAnnotation>();

        var source = paintingAnnotation!.Body
            .As<SpecificResource>()!
            .Source
            .As<Image>();

        source.Should().NotBeNull();
        source!.Service.Should().HaveCount(2, because: "the first canvas painting body has both ImageService2 and ImageService3");
    }

    // -------------------------------------------------------------------------
    // ManifestDataFileTwo — Collection with hss:slug adjuncts at root + items
    // -------------------------------------------------------------------------

    [Fact]
    public void ManifestDataFileTwo_DeserializesCollectionWithSlugAdjuncts()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        collection.Id.Should().Be(ManifestDataFileTwo.CollectionId);
        collection.AdditionalProperties.Should().ContainKey("hss:slug");
        collection.AdditionalProperties["hss:slug"].Value<string>().Should().Be("collections/sample-exhibitions");
    }

    [Fact]
    public void ManifestDataFileTwo_AllItems_HaveSlugAdjunct()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        collection.Items.Should().HaveCount(ManifestDataFileTwo.ExpectedItemCount);

        foreach (var item in collection.Items!.Cast<Manifest>())
            item.AdditionalProperties.Should().ContainKey("hss:slug",
                because: $"item '{item.Id}' should carry hss:slug adjunct");
    }

    [Fact]
    public void ManifestDataFileTwo_WithoutAdditionalProperties_ClearsRoot()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        collection.WithoutAdditionalProperties();

        collection.AdditionalProperties.Should().BeEmpty();
    }

    [Fact]
    public void ManifestDataFileTwo_WithoutAdditionalProperties_ClearsSlug_OnAllItems()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();

        collection.WithoutAdditionalProperties();

        foreach (var item in collection.Items!.Cast<Manifest>())
            item.AdditionalProperties.Should().BeEmpty(
                because: $"hss:slug on item '{item.Id}' should have been stripped");
    }

    [Fact]
    public void ManifestDataFileTwo_WithoutAdditionalProperties_NamedKey_ClearsSlugOnly()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>();
        // Add a second adjunct to root so we can verify selective removal
        collection.AdditionalProperties["otherProp"] = "should-remain";

        collection.WithoutAdditionalProperties("hss:slug");

        collection.AdditionalProperties.Should().NotContainKey("hss:slug");
        collection.AdditionalProperties.Should().ContainKey("otherProp");

        foreach (var item in collection.Items!.Cast<Manifest>())
            item.AdditionalProperties.Should().NotContainKey("hss:slug");
    }

    [Fact]
    public void ManifestDataFileTwo_AsJson_AfterStrip_ContainsNoSlug()
    {
        var json = ManifestDataFileTwo.Json.FromJson<Collection>()
            .WithoutAdditionalProperties()
            .AsJson();

        json.Should().NotContain("hss:slug");
        // Standard collection properties survive
        json.Should().Contain(ManifestDataFileTwo.CollectionId);
        json.Should().Contain("Sample Exhibitions");
    }

    [Fact]
    public void ManifestDataFileTwo_AsJson_AfterStrip_ItemCountUnchanged()
    {
        var original = ManifestDataFileTwo.Json.FromJson<Collection>();
        var stripped = ManifestDataFileTwo.Json.FromJson<Collection>()
            .WithoutAdditionalProperties();

        stripped.Items.Should().HaveCount(original.Items!.Count);
    }

    [Fact]
    public void ManifestDataFileTwo_Thumbnails_PreservedAfterStrip()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>()
            .WithoutAdditionalProperties();

        // Root thumbnail
        collection.Thumbnail.Should().HaveCount(1);
        collection.Thumbnail![0].Id.Should().Be(
            "https://assets.example.org/iiif-img/sample-root/full/1024,1024/0/default.jpg");

        // Items with thumbnails retain them
        var itemsWithThumbnail = collection.Items!.Cast<Manifest>()
            .Where(m => m.Thumbnail is { Count: > 0 })
            .ToList();
        itemsWithThumbnail.Should().NotBeEmpty();
    }
    
    [Fact]
    public void ManifestDataFileTwo_BilingualLabels_PreservedAfterStrip()
    {
        var collection = ManifestDataFileTwo.Json.FromJson<Collection>()
            .WithoutAdditionalProperties();

        collection.Label!["en"].Should().Contain("Sample Exhibitions");
        collection.Label["nl"].Should().Contain("Voorbeeldtentoonstellingen");

        var exhibit10 = collection.Items!.Cast<Manifest>()
            .Single(m => m.Id!.Contains("exhibit-10"));
        exhibit10.Label!["en"].Should().Contain("Through the Lens of a Professor");
        exhibit10.Label["nl"].Should().Contain("Door de lens van een professor");
    }
    
    // -------------------------------------------------------------------------
    // Known-property filtering — read-only overrides must not bleed into AdditionalProperties
    // -------------------------------------------------------------------------

    [Fact]
    public void FromJson_DoesNotStoreKnownProperties_InAdditionalProperties()
    {
        // PaintingAnnotation.Motivation is a getter-only computed property. Without the
        // contract-resolver guard the incoming "motivation" JSON key has nowhere to land
        // and falls back to AdditionalProperties, causing it to be emitted twice on
        // re-serialisation.
        const string json = """
                            {
                              "id": "https://example.org/anno/1",
                              "type": "Annotation",
                              "motivation": "painting",
                              "target": "https://example.org/canvas/1"
                            }
                            """;

        var anno = json.FromJson<PaintingAnnotation>();

        anno!.AdditionalProperties.Should().NotContainKey("motivation");
    }

    [Fact]
    public void AsJson_PaintingAnnotation_MotivationNotDuplicated()
    {
        const string json = """
                            {
                              "id": "https://example.org/anno/1",
                              "type": "Annotation",
                              "motivation": "painting",
                              "target": "https://example.org/canvas/1"
                            }
                            """;

        var roundTripped = json.FromJson<PaintingAnnotation>()!.AsJson();

        var motivationCount = System.Text.RegularExpressions.Regex
            .Matches(roundTripped, "\"motivation\"")
            .Count;
        motivationCount.Should().Be(1, because: "Motivation must not be emitted twice");
    }
}