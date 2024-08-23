using System.Collections.Generic;
using System.IO;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;

namespace IIIF.Tests.Serialisation;

public class CollectionSerializationTests
{
    private Collection sampleCollection = new()
    {
        Id = $"someId",
        Context = "http://iiif.io/api/presentation/3/context.json",
        Label = new LanguageMap
        {
            {"en", new List<string>
            {
                "root"
            }}
        },
        Behavior = new List<string>
        {
            "some-behaviour"
        },
        Items = new List<ICollectionItem>
        {
            new Collection
            {
                Id = "child",
                Label = new LanguageMap
                {
                    {"en", new List<string>
                    {
                        "child"
                    }}
                }
            },
            new Manifest
            {
                Id = "child",
                Label = new LanguageMap
                {
                    {"en", new List<string>
                    {
                        "child manifest"
                    }}
                }
            }
        },
        PartOf = new List<ResourceBase>
        { 
            new ExternalResource("Collection")
            {
                Id = $"PartOf",
                Label = new LanguageMap
                {
                    {"en", new List<string>
                    {
                        "some collection"
                    }}
                }
            }
        },
        SeeAlso = new List<ExternalResource>
        {
            new ("SeeAlso")
            {
                Id = "see also",
                Label = new LanguageMap
                {
                    {"en", new List<string>
                    {
                        "child"
                    }}
                },
                Profile = "Public"
            }
        },
    };
    
    [Fact]
    public void CanDeserialiseSerialisedCollection()
    {
        var serialisedCollection = sampleCollection.AsJson();

        var deserialised = serialisedCollection.FromJson<Collection>();

        deserialised.Should().BeEquivalentTo(sampleCollection);
    }
    
    [Fact]
    public void CanDeserialiseSerialisedCollection_Stream()
    {
        using var memoryStream = new MemoryStream();
        sampleCollection.AsJsonStream(memoryStream);

        memoryStream.Position = 0;
        var deserialised = memoryStream.FromJsonStream<Collection>();

        deserialised.Should().BeEquivalentTo(sampleCollection);
    }
}