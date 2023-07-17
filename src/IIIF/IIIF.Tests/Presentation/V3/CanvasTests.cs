using System.Collections.Generic;
using FluentAssertions;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Strings;
using IIIF.Serialisation;
using Xunit;

namespace IIIF.Tests.Presentation.V3;

public class CanvasTests
{
    [Fact]
    public void SerialiseTargetAsId_True_RendersIdAsTarget()
    {
        var targetIsIdOnlyCanvas = new Canvas
        {
            Id = "https://test.example.com/canvas/target-id-only",
            Width = 1000,
            Height = 1001,
            SerialiseTargetAsId = true
        };

        var referencingCanvas = new Canvas
        {
            Id = "https://test.example.com/canvas/referencing",
            Items = new List<AnnotationPage>
            {
                new()
                {
                    Id = "https://test.example.com/canvas/referencing/page",
                    Items = new List<IAnnotation>
                    {
                        new PaintingAnnotation { Target = targetIsIdOnlyCanvas }
                    }
                }
            }
        };

        var manifest = new Manifest
        {
            Context = "http://iiif.io/api/presentation/3/context.json",
            Id = "https://test.example.com/manifest",
            Label = new LanguageMap("en", "Test string"),
            Items = new List<Canvas> { targetIsIdOnlyCanvas, referencingCanvas }
        };

        var serialisedManifest = manifest.AsJson().Replace("\r\n", "\n");

        const string expected = @"{
  ""@context"": ""http://iiif.io/api/presentation/3/context.json"",
  ""id"": ""https://test.example.com/manifest"",
  ""type"": ""Manifest"",
  ""label"": {""en"":[""Test string""]},
  ""items"": [
    {
      ""id"": ""https://test.example.com/canvas/target-id-only"",
      ""type"": ""Canvas"",
      ""width"": 1000,
      ""height"": 1001
    },
    {
      ""id"": ""https://test.example.com/canvas/referencing"",
      ""type"": ""Canvas"",
      ""items"": [
        {
          ""id"": ""https://test.example.com/canvas/referencing/page"",
          ""type"": ""AnnotationPage"",
          ""items"": [
            {
              ""type"": ""Annotation"",
              ""motivation"": ""painting"",
              ""target"": ""https://test.example.com/canvas/target-id-only""
            }
          ]
        }
      ]
    }
  ]
}";

        serialisedManifest.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void SerialiseTargetAsId_False_RendersFullCanvasAsTarget()
    {
        var targetIsFullCanvas = new Canvas
        {
            Id = "https://test.example.com/canvas/full",
            Width = 1000,
            Height = 1001
        };

        var referencingCanvas = new Canvas
        {
            Id = "https://test.example.com/canvas/referencing",
            Items = new List<AnnotationPage>
            {
                new()
                {
                    Id = "https://test.example.com/canvas/referencing/page",
                    Items = new List<IAnnotation>
                    {
                        new PaintingAnnotation { Target = targetIsFullCanvas }
                    }
                }
            }
        };

        var manifest = new Manifest
        {
            Context = "http://iiif.io/api/presentation/3/context.json",
            Id = "https://test.example.com/manifest",
            Label = new LanguageMap("en", "Test string"),
            Items = new List<Canvas> { targetIsFullCanvas, referencingCanvas }
        };

        var serialisedManifest = manifest.AsJson().Replace("\r\n", "\n");

        const string expected = @"{
  ""@context"": ""http://iiif.io/api/presentation/3/context.json"",
  ""id"": ""https://test.example.com/manifest"",
  ""type"": ""Manifest"",
  ""label"": {""en"":[""Test string""]},
  ""items"": [
    {
      ""id"": ""https://test.example.com/canvas/full"",
      ""type"": ""Canvas"",
      ""width"": 1000,
      ""height"": 1001
    },
    {
      ""id"": ""https://test.example.com/canvas/referencing"",
      ""type"": ""Canvas"",
      ""items"": [
        {
          ""id"": ""https://test.example.com/canvas/referencing/page"",
          ""type"": ""AnnotationPage"",
          ""items"": [
            {
              ""type"": ""Annotation"",
              ""motivation"": ""painting"",
              ""target"": {
                ""id"": ""https://test.example.com/canvas/full"",
                ""type"": ""Canvas"",
                ""width"": 1000,
                ""height"": 1001
              }
            }
          ]
        }
      ]
    }
  ]
}";

        serialisedManifest.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void CanSelfReferenceCanvas()
    {
        var canvas = new Canvas
        {
            Id = "https://test.example.com/canvas/target-id-only",
            SerialiseTargetAsId = true
        };

        canvas.Items = new List<AnnotationPage>
        {
            new()
            {
                Id = "https://test.example.com/canvas/referencing/page",
                Items = new List<IAnnotation>
                {
                    new PaintingAnnotation
                    {
                        Id = "https://test.example.com/canvas/referencing/page/anno",
                        Target = canvas
                    }
                }
            }
        };

        var manifest = new Manifest
        {
            Context = "http://iiif.io/api/presentation/3/context.json",
            Id = "https://test.example.com/manifest",
            Label = new LanguageMap("en", "Test string"),
            Items = new List<Canvas> { canvas }
        };

        var serialisedManifest = manifest.AsJson().Replace("\r\n", "\n");

        const string expected = @"{
  ""@context"": ""http://iiif.io/api/presentation/3/context.json"",
  ""id"": ""https://test.example.com/manifest"",
  ""type"": ""Manifest"",
  ""label"": {""en"":[""Test string""]},
  ""items"": [
    {
      ""id"": ""https://test.example.com/canvas/target-id-only"",
      ""type"": ""Canvas"",
      ""items"": [
        {
          ""id"": ""https://test.example.com/canvas/referencing/page"",
          ""type"": ""AnnotationPage"",
          ""items"": [
            {
              ""id"": ""https://test.example.com/canvas/referencing/page/anno"",
              ""type"": ""Annotation"",
              ""motivation"": ""painting"",
              ""target"": ""https://test.example.com/canvas/target-id-only""
            }
          ]
        }
      ]
    }
  ]
}";

        serialisedManifest.Should().BeEquivalentTo(expected);
    }
}