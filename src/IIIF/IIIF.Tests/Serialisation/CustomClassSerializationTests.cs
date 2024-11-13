using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Collection = IIIF.Presentation.V3.Collection;
using ResourceBase = IIIF.Presentation.V3.ResourceBase;

namespace IIIF.Tests.Serialisation;

public class CustomClassSerializationTests
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
            new CustomCollectionItem
            {
                Id = "child",
                Label = new LanguageMap
                {
                    {"en", new List<string>
                    {
                        "child"
                    }}
                },
                CustomField = "some-custom-field"
            },
            new CustomItem
            {
                Id = "custom-item-child",
                Label = new LanguageMap
                {
                    {"en", new List<string>
                    {
                        "child"
                    }}
                },
                CustomField = "some-custom-field"
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
    
    private Collection sampleCollectionOnlyInheritance = new()
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
            new CustomCollectionItem
            {
                Id = "child",
                Label = new LanguageMap
                {
                    {"en", new List<string>
                    {
                        "child"
                    }}
                },
                CustomField = "some-custom-field"
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
    public async Task CanDeserialiseCustomSerialisedCollection()
    {
        var serialisedCollection = sampleCollection.AsJson();

        var deserialised = await serialisedCollection.AsJson<Collection>();
        var itemAsCustomCollection = deserialised.Items[0] as CustomCollectionItem;
        var itemAsCustomItem = deserialised.Items[1] as CustomItem;

        deserialised.Should().BeEquivalentTo(sampleCollection);
        itemAsCustomCollection.Should().NotBeNull();
        itemAsCustomItem.Should().NotBeNull();
    }
    
    [Fact]
    public void CanDeserialiseCustomSerialisedCollectionWithOnlyStandardLibrary()
    {
        var serialisedCollection = sampleCollectionOnlyInheritance.AsJson();

        var deserialised = serialisedCollection.FromJson<Collection>();
        
        var itemAsCustomCollection = deserialised.Items[0] as CustomCollectionItem;
        var itemAsCollection = deserialised.Items[0] as Collection;

        deserialised.Should().BeEquivalentTo(sampleCollectionOnlyInheritance);
        itemAsCustomCollection.Should().BeNull();
        itemAsCollection.Should().NotBeNull();
    }
}

public static class CustomSerializerX
{
    public static async Task<T?> AsJson<T>(this string content, JsonSerializerSettings? settings = null)
        where T : JsonLdBase, new()
    {
        using var streamReader = new StringReader(content);
        return await DeserializeStream<T>(settings, streamReader);
    }
    
    private static async Task<T?> DeserializeStream<T>(JsonSerializerSettings? settings, TextReader streamReader)
        where T : JsonLdBase, new()
    {
        await using var jsonReader = new JsonTextReader(streamReader);

        settings ??= new(IIIFSerialiserX.DeserializerSettings);
        settings.Context = new StreamingContext(StreamingContextStates.Other,
            new Dictionary<string, Func<JObject, ICollectionItem>>
            {
                { "Collection", p => new CustomCollectionItem() },
                { "CustomItem", p => new CustomItem() }
            });

        var serializer = JsonSerializer.Create(settings);

        try
        {
            var result = new T();
            serializer.Populate(jsonReader, result);
            return result;
        }
        catch (JsonException)
        {
            return default;
        }
    }
}

public class CustomCollectionItem : Collection
{
    public string CustomField { get; set; } = "custom";
}

public class CustomItem : StructureBase, ICollectionItem
{
    public string CustomField { get; set; } = "custom";
    
    public List<IService> Services { get; set; }
    public override string Type { get; } = nameof(CustomItem);
}