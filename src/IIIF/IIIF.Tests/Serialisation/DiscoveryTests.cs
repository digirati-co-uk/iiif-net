using System;
using System.Collections.Generic;
using IIIF.Discovery.V1;
using IIIF.Presentation;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;
using Xunit;

namespace IIIF.Tests.Serialisation;

public class DiscoveryTests
{
    [Fact]
    public void OrderedCollection()
    {
        // Arrange
        // Example from https://iiif.io/api/discovery/1.0/#complete-ordered-collection-example
        var expected = @"{
  ""@context"": ""http://iiif.io/api/discovery/1/context.json"",
  ""id"": ""https://example.org/activity/all-changes"",
  ""type"": ""OrderedCollection"",
  ""totalItems"": 21456,
  ""rights"": ""http://creativecommons.org/licenses/by/4.0/"",
  ""seeAlso"": [
    {
      ""id"": ""https://example.org/dataset/all-dcat.jsonld"",
      ""type"": ""Dataset"",
      ""profile"": ""http://www.w3.org/ns/dcat#"",
      ""label"": {""en"":[""DCAT description of Collection""]},
      ""format"": ""application/ld+json""
    }
  ],
  ""partOf"": [
    {
      ""id"": ""https://example.org/aggregated-changes"",
      ""type"": ""OrderedCollection""
    }
  ],
  ""first"": {
    ""id"": ""https://example.org/activity/page-0"",
    ""type"": ""OrderedCollectionPage""
  },
  ""last"": {
    ""id"": ""https://example.org/activity/page-214"",
    ""type"": ""OrderedCollectionPage""
  }
}";
        var orderedCollection = new OrderedCollection
        {
            Id = "https://example.org/activity/all-changes",
            TotalItems = 21456,
            Rights = "http://creativecommons.org/licenses/by/4.0/",
            SeeAlso = new List<ExternalResource>
            {
                new("Dataset")
                {
                    Id = "https://example.org/dataset/all-dcat.jsonld",
                    Label = new LanguageMap("en", "DCAT description of Collection"),
                    Format = "application/ld+json",
                    Profile = "http://www.w3.org/ns/dcat#"
                }
            },
            PartOf = new List<OrderedCollection>
            {
                new() { Id = "https://example.org/aggregated-changes" }
            },
            First = new OrderedCollectionPage { Id = "https://example.org/activity/page-0" },
            Last = new OrderedCollectionPage { Id = "https://example.org/activity/page-214" }
        };
        orderedCollection.EnsureContext(Discovery.Context.ChangeDiscovery1Context);

        // Act
        var json = orderedCollection.AsJson();

        // Assert
        json.ShouldMatchJson(expected);
    }

    [Fact]
    public void OrderedCollectionPage()
    {
        // Arrange
        // Example from https://iiif.io/api/discovery/1.0/#complete-ordered-collection-page-example
        var expected = @"{
  ""@context"": ""http://iiif.io/api/discovery/1/context.json"",
  ""id"": ""https://example.org/activity/page-1"",
  ""type"": ""OrderedCollectionPage"",
  ""startIndex"": 20,
  ""partOf"": {
    ""id"": ""https://example.org/activity/all-changes"",
    ""type"": ""OrderedCollection""
  },
  ""prev"": {
    ""id"": ""https://example.org/activity/page-0"",
    ""type"": ""OrderedCollectionPage""
  },
  ""next"": {
    ""id"": ""https://example.org/activity/page-2"",
    ""type"": ""OrderedCollectionPage""
  },
  ""orderedItems"": [
    {
      ""type"": ""Update"",
      ""object"": {
        ""id"": ""https://example.org/iiif/1/manifest"",
        ""type"": ""Manifest""
      },
      ""endTime"": ""2018-03-10T10:00:00""
    }
  ]
}";
        var orderedCollectionPage = new OrderedCollectionPage
        {
            Id = "https://example.org/activity/page-1",
            StartIndex = 20,
            PartOf = new OrderedCollection { Id = "https://example.org/activity/all-changes" },
            Prev = new OrderedCollectionPage { Id = "https://example.org/activity/page-0" },
            Next = new OrderedCollectionPage { Id = "https://example.org/activity/page-2" },
            OrderedItems = new List<Activity>
            {
                new()
                {
                    Type = ActivityType.Update,
                    Object = new ActivityObject
                    {
                        Id = "https://example.org/iiif/1/manifest",
                        Type = "Manifest"
                    },
                    EndTime = new DateTime(2018, 3, 10, 10, 0, 0)
                }
            }
        };
        orderedCollectionPage.EnsureContext(Discovery.Context.ChangeDiscovery1Context);

        // Act
        var json = orderedCollectionPage.AsJson();

        // Assert
        json.ShouldMatchJson(expected);
    }

    [Fact]
    public void Activity()
    {
        // Arrange
        // Example from https://iiif.io/api/discovery/1.0/#complete-activity-example
        var expected = @"{
  ""@context"": ""http://iiif.io/api/discovery/1/context.json"",
  ""id"": ""https://example.org/activity/1"",
  ""type"": ""Update"",
  ""summary"": ""admin updated the manifest, fixing reported bug #15."",
  ""object"": {
    ""id"": ""https://example.org/iiif/1/manifest"",
    ""type"": ""Manifest"",
    ""canonical"": ""https://example.org/iiif/1"",
    ""seeAlso"": [
      {
        ""id"": ""https://example.org/dataset/single-item.jsonld"",
        ""type"": ""Dataset"",
        ""format"": ""application/ld+json""
      }
    ]
  },
  ""endTime"": ""2017-09-21T00:00:00"",
  ""startTime"": ""2017-09-20T23:58:00"",
  ""actor"": {
    ""id"": ""https://example.org/person/admin1"",
    ""type"": ""Person""
  }
}";
        var activity = new Activity
        {
            Id = "https://example.org/activity/1",
            Type = ActivityType.Update,
            Summary = "admin updated the manifest, fixing reported bug #15.",
            Object = new ActivityObject
            {
                Id = "https://example.org/iiif/1/manifest",
                Type = "Manifest",
                Canonical = "https://example.org/iiif/1",
                SeeAlso = new List<ExternalResource>
                {
                    new("Dataset")
                    {
                        Id = "https://example.org/dataset/single-item.jsonld",
                        Format = "application/ld+json"
                    }
                }
            },
            StartTime = new DateTime(2017, 9, 20, 23, 58, 0),
            EndTime = new DateTime(2017, 9, 21, 0, 0, 0),
            Actor = new Actor { Id = "https://example.org/person/admin1", Type = ActorType.Person }
        };

        activity.EnsureContext(Discovery.Context.ChangeDiscovery1Context);

        // Act
        var json = activity.AsJson();

        // Assert
        json.ShouldMatchJson(expected);
    }
}