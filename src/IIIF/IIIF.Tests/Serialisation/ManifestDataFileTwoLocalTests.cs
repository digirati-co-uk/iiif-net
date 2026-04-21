#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IIIF.Presentation.V3;
using IIIF.Serialisation;
using IIIF.Tests.Serialisation.Data;

namespace IIIF.Tests.Serialisation;

/// <summary>
/// Offline unit tests for the anonymized sample collection using embedded JSON.
/// These run without any network access and are always safe to run in CI.
/// </summary>
public class ManifestDataFileTwoLocalTests
{
    private readonly Collection collection;

    public ManifestDataFileTwoLocalTests()
    {
        collection = ManifestDataFileTwo.Json.FromJson<Collection>()!;
    }

    [Fact]
    public void Local_Deserialises_WithoutException()
    {
        collection.Should().NotBeNull();
    }

    [Fact]
    public void Local_CollectionId_IsCorrect()
    {
        collection.Id.Should().Be(ManifestDataFileTwo.CollectionId);
    }

    [Fact]
    public void Local_Label_HasEnglishAndDutch()
    {
        collection.Label.Should().NotBeNull();
        collection.Label!["en"].Should().ContainSingle().Which.Should().Be("Sample Exhibitions");
        collection.Label["nl"].Should().ContainSingle().Which.Should().Be("Voorbeeldtentoonstellingen");
    }

    [Fact]
    public void Local_Items_HasExpectedCount()
    {
        collection.Items.Should().HaveCount(ManifestDataFileTwo.ExpectedItemCount);
    }

    [Fact]
    public void Local_AllItems_HaveIdAndLabel()
    {
        collection.Items.Should().AllSatisfy(item =>
        {
            var resource = item.Should().BeAssignableTo<ResourceBase>().Subject;
            resource.Id.Should().NotBeNullOrEmpty();
            resource.Label.Should().NotBeNull();
        });
    }

    [Fact]
    public void Local_SomeItems_HaveThumbnails()
    {
        collection.Items
            .OfType<ResourceBase>()
            .Where(r => r.Thumbnail?.Count > 0)
            .Should().NotBeEmpty("several manifests in the collection advertise a thumbnail");
    }

    [Fact]
    public void Local_Collection_HasThumbnail()
    {
        collection.Thumbnail.Should().NotBeNullOrEmpty();
        collection.Thumbnail![0].Id.Should().NotBeNullOrEmpty();
    }

    [Fact]  
    public void Local_NoResource_HasTypeInAdditionalProperties()
    {
        collection.AdditionalProperties.Should().NotContainKey("type");

        foreach (var item in collection.Items.OfType<JsonLdBase>())
        {
            item.AdditionalProperties.Should().NotContainKey("type",
                because: "item must not leak 'type' into extension data");
        }
    }

    [Fact]
    public void Local_ExtensionSlug_IsInAdditionalProperties()
    {
        collection.AdditionalProperties.Should().ContainKey("hss:slug");
        collection.AdditionalProperties["hss:slug"]!.ToString().Should().Be("collections/sample-exhibitions");
    }

    [Fact]
    public void Local_ItemExtensionSlugs_AreInAdditionalProperties()
    {
        foreach (var item in collection.Items.OfType<JsonLdBase>())
        {
            item.AdditionalProperties.Should().ContainKey("hss:slug",
                because: "every manifest stub in the collection has an hss:slug extension");
        }
    }

    [Fact]
    public void Local_BilingualItem_HasBothLanguages()
    {
        var collectionWall = collection.Items
            .OfType<ResourceBase>()
            .First(r => r.Id!.Contains("exhibit-02"));

        collectionWall.Label!["en"].Should().ContainSingle().Which.Should().Be("Exhibit Two");
        collectionWall.Label["nl"].Should().ContainSingle().Which.Should().Be("Tentoonstelling Twee");
    }

    [Fact]
    public void Local_NoJsonLdNode_HasTypeInAdditionalProperties_AtAnyDepth()
    {
        var nodes = DescendantsAndSelf(collection).ToList();

        nodes.Should().NotBeEmpty();
        nodes.Should().AllSatisfy(node =>
            node.AdditionalProperties.Should().NotContainKey("type",
                because: $"{node.GetType().Name} must not leak 'type' into extension data"));
    }

    [Fact]
    public void Local_ObjectGraph_ContainsNestedResources_AtDepth()
    {
        var nodes = DescendantsAndSelfWithDepth(collection).ToList();

        nodes.Any(x => ReferenceEquals(x.Node, collection) && x.Depth == 0).Should().BeTrue();
        nodes.Any(x => x.Node is ResourceBase resource && resource.Type == "Manifest" && x.Depth == 1)
            .Should().BeTrue();
        nodes.Any(x => x.Node is ResourceBase resource && resource.Type == "Image" && x.Depth >= 1)
            .Should().BeTrue();

        nodes.Max(x => x.Depth).Should().BeGreaterThanOrEqualTo(2);
    }

    [Fact]
    public void Local_ObjectGraph_HasExpectedKnownDepths()
    {
        collection.Items.Should().NotBeNullOrEmpty();
        collection.Thumbnail.Should().NotBeNullOrEmpty();

        var itemResources = collection.Items!
            .OfType<ResourceBase>()
            .ToList();

        itemResources.Should().HaveCount(ManifestDataFileTwo.ExpectedItemCount);
        itemResources.Should().OnlyContain(item => item.Id != null && item.Label != null);

        itemResources
            .Where(item => item.Thumbnail is not null && item.Thumbnail.Count > 0)
            .Should().NotBeEmpty(because: "some manifest stubs have thumbnails at depth 2");
    }

    private static IEnumerable<JsonLdBase> DescendantsAndSelf(JsonLdBase root)
    {
        yield return root;

        foreach (var child in GetChildren(root))
        {
            foreach (var descendant in DescendantsAndSelf(child))
            {
                yield return descendant;
            }
        }
    }

    private static IEnumerable<(JsonLdBase Node, int Depth)> DescendantsAndSelfWithDepth(
        JsonLdBase root,
        int depth = 0)
    {
        yield return (root, depth);

        foreach (var child in GetChildren(root))
        {
            foreach (var descendant in DescendantsAndSelfWithDepth(child, depth + 1))
            {
                yield return descendant;
            }
        }
    }

    private static IEnumerable<JsonLdBase> GetChildren(object instance)
    {
        foreach (var property in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.GetIndexParameters().Length > 0) continue;
            if (property.Name == nameof(JsonLdBase.AdditionalProperties)) continue;

            var value = property.GetValue(instance);
            if (value is null or string) continue;

            if (value is JsonLdBase child)
            {
                yield return child;
                continue;
            }

            if (value is IEnumerable sequence)
            {
                foreach (var item in sequence)
                {
                    if (item is JsonLdBase jsonLd)
                    {
                        yield return jsonLd;
                    }
                }
            }
        }
    }
}