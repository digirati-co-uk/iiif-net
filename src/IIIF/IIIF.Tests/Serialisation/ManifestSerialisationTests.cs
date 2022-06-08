using System.Collections.Generic;
using FluentAssertions;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;
using Xunit;
using ExternalResource = IIIF.Presentation.V3.Content.ExternalResource;

namespace IIIF.Tests.Serialisation
{
    public class ManifestSerialisationTests
    {
        [Fact]
        public void CanDeserialiseSerialisedManifest()
        {
            var manifest = new Manifest
            {
                Context = "http://iiif.io/api/presentation/3/context.json",
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
                        },
                    }
                },
                Items = new List<Canvas>
                {
                    new()
                    {
                        Id = "https://test.example.com/canvas/1",
                        Width = 1000,
                        Height = 1001,
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
                                            },
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
                                },
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
                    },
                },
                Metadata = new List<LabelValuePair>
                {
                    new("en", "Gibberish", "foo", "bar"),
                    new("en", "Published", "December 2021"),
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
                            },
                        },
                        Logo = new List<Image>
                        {
                            new()
                            {
                                Id = "https://test.example.com/logo",
                                Format = "image/jpeg",
                            }
                        }
                    },
                },
                SeeAlso = new List<ExternalResource>
                {
                    new("Dataset")
                    {
                        Id = "https://test.example.com/other",
                        Profile = "https://api.test.example.com/context.json",
                        Label = new LanguageMap("en", "API Stuff"),
                        Format = "application/json",
                    },
                }
            };

            var serialisedManifest = manifest.AsJson();

            var deserialised = serialisedManifest.FromJson<Manifest>();

            deserialised.Should().BeEquivalentTo(manifest);
        }
    }
}