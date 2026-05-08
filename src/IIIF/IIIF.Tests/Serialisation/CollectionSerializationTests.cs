using System.IO;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;

namespace IIIF.Tests.Serialisation;

public class CollectionSerializationTests
{
    private readonly Collection sampleCollection = new()
    {
        Id = "someId",
        Context = "http://iiif.io/api/presentation/3/context.json",
        Label = new LanguageMap
        {
            {"en", ["root"] }
        },
        Behavior = ["some-behaviour"],
        Items =
        [
            new Collection
            {
                Id = "child",
                Label = new LanguageMap
                {
                    {"en", ["child"] }
                }
            },
            new Manifest
            {
                Id = "child",
                Label = new LanguageMap
                {
                    {"en", ["child manifest"] }
                }
            }
        ],
        PartOf =
        [
            new ExternalResource("Collection")
            {
                Id = "PartOf",
                Label = new LanguageMap
                {
                    {"en", ["some collection"] }
                }
            }
        ],
        SeeAlso =
        [
            new("SeeAlso")
            {
                Id = "see also",
                Label = new LanguageMap
                {
                    { "en", ["child"] }
                },
                Profile = "Public"
            }
        ],
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