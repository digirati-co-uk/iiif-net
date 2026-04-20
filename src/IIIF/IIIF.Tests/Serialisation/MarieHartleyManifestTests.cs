using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Selectors;
using IIIF.Serialisation;
using IIIF.Tests.Serialisation.Data;
using System.Linq;

namespace IIIF.Tests.Serialisation;

/// <summary>
/// Tests deserialisation of a real-world manifest reported at
/// https://leedsunilibrary.exhibitionviewer.org/iiif/marie-hartley.json
/// </summary>
public class MarieHartleyManifestTests
{
    private readonly Manifest manifest;

    public MarieHartleyManifestTests()
    {
        manifest = MarieHartleyManifestData.Json.FromJson<Manifest>();
    }

    [Fact]
    public void Deserialises_WithoutException()
    {
        manifest.Should().NotBeNull();
    }

    [Fact]
    public void Deserialises_ManifestId_AndLabel()
    {
        manifest.Id.Should().Be(MarieHartleyManifestData.ManifestId);
        manifest.Label?["en"].Should().ContainSingle()
            .Which.Should().Be("Welcome to Yorkshire: The art of Marie Hartley");
    }

    [Fact]
    public void Deserialises_Canvas_BehaviorArray()
    {
        // Canvases use custom behavior strings like "w-7", "h-6" alongside standard values
        var firstCanvas = manifest.Items?[0];
        firstCanvas?.Behavior.Should().BeEquivalentTo(new[] { "w-7", "h-6" });

        var thirdCanvas = manifest.Items?[2];
        thirdCanvas?.Behavior.Should().BeEquivalentTo(new[] { "w-12", "h-6", "left" });
    }

    [Fact]
    public void Deserialises_PaintingAnnotation_WithSpecificResourceBody_AndImageApiSelector()
    {
        // Body is a SpecificResource (not a plain Image) with an ImageApiSelector
        var annotation = manifest.Items?[0].Items?[0].Items?[0].As<PaintingAnnotation>();

        var body = annotation?.Body.As<SpecificResource>();
        body.Should().NotBeNull();
        body?.Source.As<Image>().Id.Should().Contain("w5b3cz4c");

        body?.Selector.Should().NotBeNullOrEmpty();
        body?.Selector.First().As<ImageApiSelector>().Region.Should().Be("70,48,3399,2423");
    }

    [Fact]
    public void Deserialises_ImageService2_AndImageService3_OnSource()
    {
        var body = manifest.Items?[0].Items?[0].Items?[0].As<PaintingAnnotation>()
            .Body.As<SpecificResource>();

        var services = body?.Source.As<Image>()?.Service;
        services.Should().HaveCount(2);
        services?[0].Should().BeOfType<ImageService2>();
        services?[1].Should().BeOfType<ImageService3>();
    }

    [Fact]
    public void Deserialises_ImageService2_WithArrayProfile()
    {
        // Some ImageService2 responses use an array profile: ["level-url", { capabilities }]
        // rather than a plain string - verify this parses without throwing
        var thumbnail = manifest?.Items?[3].Thumbnail?[0].As<Image>();
        var service = thumbnail?.Service?[0].As<ImageService2>();

        service.Should().NotBeNull();
        service?.Id.Should().Contain("yhms7npn");
    }

    [Fact]
    public void Deserialises_TextualBody_AsPaintingAnnotationBody()
    {
        // Second canvas uses a TextualBody (HTML) as the painting body
        var annotation = manifest?.Items?[1].Items?[0].Items?[0].As<PaintingAnnotation>();

        var body = annotation?.Body.Should().BeOfType<TextualBody>().Subject;
        body?.Format.Should().Be("text/html");
        body?.Language.Should().Be("en");
    }

    [Fact]
    public void Deserialises_DescribingAnnotation_WithSpecificResourceTarget_AndSvgSelector()
    {
        // Annotations on the map canvas target SpecificResource regions using SvgSelector
        var annotationPage = manifest?.Items?[2].Annotations?[0];
        var annotation = annotationPage?.Items?[0].As<Annotation>();

        var target = annotation?.Target.As<SpecificResource>();
        target.Should().NotBeNull();

        target?.Selector.Should().NotBeNullOrEmpty();
        target?.Selector.First().As<SvgSelector>().Should().NotBeNull();
    }

    [Fact]
    public void Deserialises_Provider_WithAgentAndLogo()
    {
        var provider = manifest?.Provider?.Single();
        provider?.Id.Should().Be("https://library.leeds.ac.uk/info/1600/about");
        provider?.Label?["en"].Should().ContainSingle().Which.Should().Be("University of Leeds");
        provider?.Logo.Should().HaveCount(1);
        provider?.Logo?[0].Id.Should().Contain("black.png");
    }

    [Fact]
    public void AdditionalProperties_AreEmpty_ForStandardResources()
    {
        // No resource in a standard manifest should have "type" or other known 
        // properties leaking into AdditionalProperties
        manifest?.AdditionalProperties.Should().BeEmpty();
        manifest?.Items?[0].AdditionalProperties.Should().BeEmpty();
        manifest?.Items?[0].Items?[0].AdditionalProperties.Should().BeEmpty();
    }

    [Fact]
    public void Deserialises_FirstCanvas_PaintingPath_AtDepth()
    {
        var canvas = manifest?.Items?[0];
        canvas?.Type.Should().Be("Canvas");

        var page = canvas?.Items?[0];
        page?.Type.Should().Be("AnnotationPage");
        var annotation = page?.Items?[0].As<PaintingAnnotation>();
        annotation?.Type.Should().Be("Annotation");
        annotation?.Motivation.Should().Be("painting");

        var body = annotation?.Body.As<SpecificResource>();
        body?.Type.Should().Be("SpecificResource");

        var source = body?.Source.As<Image>();
        source?.Type.Should().Be("Image");
        source?.Id.Should().Contain("w5b3cz4c");

        body?.Selector.Should().NotBeNullOrEmpty();
        body?.Selector.First().As<ImageApiSelector>().Region.Should().Be("70,48,3399,2423");

        source?.Service.Should().HaveCount(2);
        source?.Service?[0].Should().BeOfType<ImageService2>();
        source?.Service?[1].Should().BeOfType<ImageService3>();
    }

    [Fact]
    public void Deserialises_ThirdCanvas_AnnotationTarget_AtDepth()
    {
        var canvas = manifest.Items?[2];
        canvas?.Type.Should().Be("Canvas");

        var annotationPage = canvas?.Annotations?[0];
        annotationPage?.Type.Should().Be("AnnotationPage");

        var annotation = annotationPage?.Items?[0].As<GeneralAnnotation>();
        annotation?.Motivation.Should().Be("describing"); 
        
        var target = annotation?.Target.As<SpecificResource>();
        target?.Type.Should().Be("SpecificResource");

        target?.Selector.Should().NotBeNullOrEmpty();
        target?.Selector.First().As<SvgSelector>().Value.Should().Be("<svg></svg>");
        annotation?.Body.Should().NotBeNullOrEmpty();
        var body = annotation?.Body?[0].Should().BeOfType<TextualBody>().Subject;
        body?.Format.Should().Be("text/html");
        body?.Language.Should().Be("en");
        body?.Value.Should().Be("<p>Pickering</p>");
    }

    [Fact]
    public void AdditionalProperties_AreEmpty_AtDepth_ForKnownStandardNodes()
    {
        manifest.AdditionalProperties.Should().BeEmpty();

        var firstCanvas = manifest.Items?[0];
        firstCanvas?.AdditionalProperties.Should().BeEmpty();

        var firstPage = firstCanvas?.Items?[0];
        firstPage?.AdditionalProperties.Should().BeEmpty();

        var firstAnnotation = firstPage?.Items?[0].As<PaintingAnnotation>();
        firstAnnotation?.AdditionalProperties.Should().ContainSingle();
        firstAnnotation?.AdditionalProperties.Should().ContainKey("motivation");
        firstAnnotation?.AdditionalProperties["motivation"]!.ToString().Should().Be("painting");

        var specificResource = firstAnnotation?.Body.As<SpecificResource>();
        specificResource?.AdditionalProperties.Should().BeEmpty();

        var sourceImage = specificResource?.Source.As<Image>();
        sourceImage?.AdditionalProperties.Should().BeEmpty();
        sourceImage?.Service?[0].As<ImageService2>().AdditionalProperties.Should().BeEmpty();
        sourceImage?.Service?[1].As<ImageService3>().AdditionalProperties.Should().BeEmpty();
    }

    [Fact]
    public void Deserialises_Metadata_And_Rights()
    {
        manifest.Rights.Should().Be("http://rightsstatements.org/vocab/InC/1.0/");
        manifest.Metadata.Should().HaveCount(2);
        manifest.Metadata?[0].Label["en"].Should().ContainSingle().Which.Should().Be("Collections");
    }

    [Fact]
    public void Deserialises_ItemCount_And_SecondCanvas_EmptyStructuralArrays()
    {
        manifest.Items.Should().HaveCount(4);

        var secondCanvas = manifest.Items?[1];
        secondCanvas?.PlaceholderCanvas.Should().BeNull();
        secondCanvas?.AccompanyingCanvas.Should().BeNull();
    }

    [Fact]
    public void Deserialises_Provider_Homepage()
    {
        var homepage = manifest.Provider?.Single().Homepage?.Single();
        homepage?.Id.Should().Be("https://https://library.leeds.ac.uk//");
        homepage?.Format.Should().Be("text/html");
        homepage?.Label?["en"].Should().ContainSingle().Which.Should().Be("Leeds University Library Homepage");
    }
}

