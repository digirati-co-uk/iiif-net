﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Extensions.NavPlace;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;
using Newtonsoft.Json.Linq;
using ExternalResource = IIIF.Presentation.V3.Content.ExternalResource;

namespace IIIF.Tests.Serialisation;

public class ManifestSerialisationTests
{
    private Manifest sampleManifest;

    public ManifestSerialisationTests()
    {
        sampleManifest = new Manifest
        {
            Context = new JArray
            {
                "http://iiif.io/api/extension/navplace/context.json",
                "http://iiif.io/api/presentation/3/context.json"
            },
            Id = "https://test.example.com/manifest",
            Label = new LanguageMap("en", "Test string"),
            Thumbnail = new List<ExternalResource>
            {
                new Image
                {
                    Id = "https://test.image",
                    Format = "image/jpeg",
                    Service = new List<IService>
                    {
                        new ImageService2
                        {
                            Id = "https://test.example.com/canvas/1/image",
                            Profile = ImageService2.Level1Profile,
                            Context = ImageService2.Image2Context,
                            Width = 200,
                            Height = 200
                        }
                    }
                }
            },
            Items = new List<Canvas>
            {
                new()
                {
                    Id = "https://test.example.com/canvas/1",
                    Width = 1000,
                    Height = 1001,
                    Annotations = new List<AnnotationPage>()
                    {
                        new()
                        {
                            Id = "https://test.example.com/canvas/1/page",
                            Items = new List<IAnnotation>
                            {
                                new GeneralAnnotation("canvassing")
                                {
                                    Body = new List<ResourceBase>
                                    {
                                        new TextualBody("Hello World!")
                                        {
                                            Id = "https://test.example.com/canvas/1/page/textualBody",
                                            Format = "text/plain",
                                            Language = "en",
                                            Motivation = "supplementing",
                                            Purpose = "some purpose",
                                            Creator = "https://test.example.com/user",
                                            Created = DateTime.UtcNow,
                                            Modified = DateTime.UtcNow,
                                            Generator = "https://test.example.com/user",
                                            Generated = DateTime.UtcNow,
                                            Role = "https://test.example.com/user/role",
                                            Audience = "everyone",
                                            Accessibility = "public",
                                            Canonical = "true",
                                            Via = "somewhere",
                                            Rights = "some rights"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Items = new List<AnnotationPage>
                    {
                        new()
                        {
                            Id = "https://test.example.com/canvas/1/page",
                            Items = new List<IAnnotation>
                            {
                                new PaintingAnnotation
                                {
                                    Target = new Canvas { Id = "https://test.example.com/canvas/1" },
                                    Id = "https://test.example.com/canvas/1/page/image",
                                    Body = new Image
                                    {
                                        Id =
                                            "https://test.example.com/canvas/1/image/full/max/1000,1000/0/default.jpg",
                                        Format = "image/jpeg",
                                        Service = new List<IService>
                                        {
                                            new ImageService2
                                            {
                                                Id = "https://test.example.com/canvas/1/image",
                                                Profile = ImageService2.Level1Profile,
                                                Context = ImageService2.Image2Context,
                                                Width = 1000,
                                                Height = 1001
                                            },
                                            new ImageService3
                                            {
                                                Id = "https://test.example.com/canvas/1/image/3",
                                                Profile = ImageService3.Level2Profile,
                                                Context = ImageService3.Image3Context,
                                                Width = 1000,
                                                Height = 1001
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Thumbnail = new List<ExternalResource>
                    {
                        new Image
                        {
                            Id = "https://test.image",
                            Format = "image/jpeg",
                            Service = new List<IService>
                            {
                                new ImageService2
                                {
                                    Id = "https://test.example.com/canvas/1/image",
                                    Profile = ImageService2.Level1Profile,
                                    Context = ImageService2.Image2Context,
                                    Width = 200,
                                    Height = 200
                                }
                            }
                        }
                    }
                }
            },
            Homepage = new List<ExternalResource>
            {
                new("Text")
                {
                    Id = "https://test.example.com/homepage",
                    Label = new LanguageMap("en", "My Homepage"),
                    Format = "text/html",
                    Language = new List<string> { "en" }
                }
            },
            Metadata = new List<LabelValuePair>
            {
                new("en", "Gibberish", "foo", "bar"),
                new("en", "Published", "December 2021")
            },
            Rights = "https://en.wikipedia.org/wiki/All_rights_reserved",
            Provider = new List<Agent>
            {
                new()
                {
                    Id = "https://test.example.com",
                    Label = new LanguageMap("en", new[] { "one", "two" }),
                    Homepage = new List<ExternalResource>
                    {
                        new("Text")
                        {
                            Id = "https://test.example.com/homepage",
                            Label = new LanguageMap("en", "My Homepage"),
                            Format = "text/html",
                            Language = new List<string> { "en" }
                        }
                    },
                    Logo = new List<Image>
                    {
                        new()
                        {
                            Id = "https://test.example.com/logo",
                            Format = "image/jpeg"
                        }
                    }
                }
            },
            SeeAlso = new List<ExternalResource>
            {
                new("Dataset")
                {
                    Id = "https://test.example.com/other",
                    Profile = "https://api.test.example.com/context.json",
                    Label = new LanguageMap("en", "API Stuff"),
                    Format = "application/json"
                }
            },
            NavPlace = new FeatureCollection
            {
                Id = "https://test.example.com/nav-place",
                Features = new List<Feature>
                {
                    new ()
                    {
                        Id = "https://test.example.com/nav-place/feature",
                        Properties = new Dictionary<string, object>
                        {
                            { "label", JToken.Parse("{\"test\":\"test\"}") },
                        },
                        Geometry = new GeometryCollection
                        {
                            Geometries = new List<Geometry>
                            {
                                new MultiPolygon
                                {
                                    Coordinates = new List<List<List<double>>>
                                    {
                                        new ()
                                        {
                                            new List<double> { 100.0, 20.2, 10.1 }
                                        }
                                    }
                                },
                                new Point
                                {
                                    Coordinates = new List<double> { 100.0, 20.2, 10.1 }
                                }
                            }
                        }
                    }
                }
            }
        };
    }

    [Fact]
    public void CanDeserialiseSerialisedManifest()
    {
        var serialisedManifest = sampleManifest.AsJson();

        var deserialised = serialisedManifest.FromJson<Manifest>();

        deserialised.Should().BeEquivalentTo(sampleManifest);
    }
    
    [Fact]
    public void CanDeserialiseUnknownServices()
    {
        var serialisedManifest = "{\"@context\": [\"http://iiif.io/api/presentation/3/context.json\"],\"id\": \"https://iiif.example/12345\",\"type\": \"Manifest\",\"services\": [{\"id\": \"https://iiif.example.org/1234#tracking\",\"type\": \"Text\",\"profile\": \"http://universalviewer.io/tracking-extensions-profile\",\"label\": {\"en\": [\"Format: Monograph, Institution: n/a, foobarbaz\"]}}]}";
        var expectedServices = new List<ExternalService>
        {
            new ExternalService("Text")
            {
                Id = "https://iiif.example.org/1234#tracking",
                Profile = "http://universalviewer.io/tracking-extensions-profile",
                Label = new LanguageMap("en", "Format: Monograph, Institution: n/a, foobarbaz"),
            }
        };

        var deserialised = serialisedManifest.FromJson<Manifest>();
        deserialised.Services.Should().BeEquivalentTo(expectedServices);
    }

    [Fact]
    public void CanDeserialiseSerialisedManifest_Stream()
    {
        using var memoryStream = new MemoryStream();
        sampleManifest.AsJsonStream(memoryStream);

        memoryStream.Position = 0;
        var deserialised = memoryStream.FromJsonStream<Manifest>();

        deserialised.Should().BeEquivalentTo(sampleManifest);
    }

    [Fact]
    public void CanDeserialise_BodyWithAuth()
    {
        // Stripped back manifest from https://github.com/digirati-co-uk/iiif-net/issues/69
        var wellcomeWithAuth = @"{
    ""@context"": ""http://iiif.io/api/presentation/3/context.json"",
    ""id"": ""https://iiif.wellcomecollection.org/presentation/b18170821"",
    ""type"": ""Manifest"",
    ""items"": [
        {
            ""id"": ""https://iiif.wellcomecollection.org/presentation/b18170821/canvases/b18170821_pp_cri_j_11_2_0001.jp2"",
            ""type"": ""Canvas"",
            ""width"": 3965,
            ""height"": 2643,
            ""items"": [
                {
                    ""id"": ""https://iiif.wellcomecollection.org/presentation/b18170821/canvases/b18170821_pp_cri_j_11_2_0001.jp2/painting"",
                    ""type"": ""AnnotationPage"",
                    ""items"": [
                        {
                            ""id"": ""https://iiif.wellcomecollection.org/presentation/b18170821/canvases/b18170821_pp_cri_j_11_2_0001.jp2/painting/anno"",
                            ""type"": ""Annotation"",
                            ""motivation"": ""painting"",
                            ""body"": {
                                ""id"": ""https://iiif.wellcomecollection.org/image/b18170821_pp_cri_j_11_2_0001.jp2/full/200,133/0/default.jpg"",
                                ""type"": ""Image"",
                                ""width"": 200,
                                ""height"": 133,
                                ""format"": ""image/jpeg"",
                                ""service"": [
                                    {
                                        ""@id"": ""https://iiif.wellcomecollection.org/image/b18170821_pp_cri_j_11_2_0001.jp2"",
                                        ""@type"": ""ImageService2"",
                                        ""profile"": ""http://iiif.io/api/image/2/level1.json"",
                                        ""width"": 3965,
                                        ""height"": 2643,
                                        ""service"": {
                                            ""@id"": ""https://iiif.wellcomecollection.org/auth/clickthrough"",
                                            ""@type"": ""AuthCookieService1""
                                        }
                                    }
                                ]
                            },
                            ""target"": ""https://iiif.wellcomecollection.org/presentation/b18170821/canvases/b18170821_pp_cri_j_11_2_0001.jp2""
                        }
                    ]
                }
            ]
        }
    ]
}";

        var expectedService = new V2ServiceReference
        {
            Id = "https://iiif.wellcomecollection.org/auth/clickthrough",
            Type = "AuthCookieService1",
        };

        var mani = wellcomeWithAuth.FromJson<Manifest>();

        var services = mani.Items[0].Items[0].Items[0].As<PaintingAnnotation>()
            .Body.Service[0].As<ImageService2>().Service;
        services.Should().HaveCount(1);
        services.Single().Should().BeEquivalentTo(expectedService);
    }
    
    [Fact]
    public void CanDeserialise_AnnotationTargetingAnotherAnnotation()
    {
        // Stripped back manifest from https://github.com/digirati-co-uk/iiif-net/issues/74
        var annotationTargetingAnother = @"
{
    ""@context"": ""http://iiif.io/api/presentation/3/context.json"",
    ""id"": ""https://example.org/m3folwy0u7-mckceiru"",
    ""type"": ""Manifest"",
    ""items"": [
        {
            ""id"": ""https://example.org/m3folwy0u7-mckceiru/canvas/0sx2a5hbr6ga-mckcfbo6"",
            ""type"": ""Canvas"",
            ""annotations"": [
                {
                    ""id"": ""https://example.org/m3folwy0u7-mckceiru/canvas/0sx2a5hbr6ga-mckcfbo6/annotations/dsat3mfvzni-mckcffwo"",
                    ""type"": ""AnnotationPage"",
                    ""items"": [
                        {
                            ""id"": ""https://example.org/m3folwy0u7-mckceiru/canvas/0sx2a5hbr6ga-mckcfbo6/annotations/dsat3mfvzni-mckcffwo/annotation/aqibmu1dtrr-mckcfvrh"",
                            ""type"": ""Annotation"",
                            ""motivation"": ""describing"",
                            ""target"": {
                                ""id"": ""https://example.org/m3folwy0u7-mckceiru/annotation/qel8952t5f-mckcfbo5"",
                                ""type"": ""Annotation""
                            },
                            ""body"": []
                        }
                    ]
                }
            ]
        }
    ]
}
";

        var mani = annotationTargetingAnother.FromJson<Manifest>();
        
        var expectedTarget = new Annotation
        {
            Id = "https://example.org/m3folwy0u7-mckceiru/annotation/qel8952t5f-mckcfbo5"
        };

        var target = mani.Items[0].Annotations[0].Items[0].As<Annotation>().Target;
        target.Should().BeEquivalentTo(expectedTarget);
    }
}