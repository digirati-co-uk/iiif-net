using System.Collections.Generic;
using System.Threading.Tasks;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Selectors;
using IIIF.Serialisation;
using IIIF.Tests.Serialisation.Data;

namespace IIIF.Tests.Serialisation;

[Trait("Category", "Cookbook")]
public class CookbookDeserialization
{
    [Theory]
    [ClassData(typeof(CookbookManifestData))]
    public void Can_Deserialize_Cookbook_Manifest(string manifestId, Manifest manifest)
    {
        // perfunctory assertion
        manifest.Should().NotBeNull($"{manifestId} is a valid cookbook manifest");
        manifest.Id.Should().Be(manifestId);
    }

    [Fact]
    public void Can_Deserialize_AnnotationCookbook()
    {
        // This is from https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/manifest.json
        const string manifest = "{\n\"@context\": \"http://iiif.io/api/presentation/3/context.json\",\n\"id\": \"https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/manifest.json\",\n\"type\": \"Manifest\",\n\"label\": {\n\"en\": [\n\"Using a point selector for annotating a location on a map.\"\n]\n},\n\"summary\": {\n\"en\": [\n\"A map containing an point with an annotation of the location.\"\n]\n},\n\"items\": [\n{\n\"id\": \"https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/canvas.json\",\n\"type\": \"Canvas\",\n\"label\": {\n\"en\": [\n\"Chesapeake and Ohio Canal Pamphlet\"\n]\n},\n\"height\": 7072,\n\"width\": 5212,\n\"items\": [\n{\n\"id\": \"https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/contentPage.json\",\n\"type\": \"AnnotationPage\",\n\"items\": [\n{\n\"id\": \"https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/content.json\",\n\"type\": \"Annotation\",\n\"motivation\": \"painting\",\n\"body\": {\n\"id\": \"https://iiif.io/api/image/3.0/example/reference/43153e2ec7531f14dd1c9b2fc401678a-88695674/full/max/0/default.jpg\",\n\"type\": \"Image\",\n\"format\": \"image/jpeg\",\n\"height\": 7072,\n\"width\": 5212,\n\"service\": [\n{\n\"id\": \"https://iiif.io/api/image/3.0/example/reference/43153e2ec7531f14dd1c9b2fc401678a-88695674\",\n\"type\": \"ImageService3\",\n\"profile\": \"level1\"\n}\n]\n},\n\"target\": \"https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/canvas.json\"\n}\n]\n}\n],\n\"annotations\": [\n{\n\"id\": \"https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/page/p2/1\",\n\"type\": \"AnnotationPage\",\n\"items\": [\n{\n\"id\": \"https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/annotation/p0002-tag\",\n\"type\": \"Annotation\",\n\"motivation\": \"tagging\",\n\"body\": {\n\"type\": \"TextualBody\",\n\"value\": \"Town Creek Aqueduct\",\n\"language\": \"en\",\n\"format\": \"text/plain\"\n},\n\"target\": {\n\"type\": \"SpecificResource\",\n\"source\": \"https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/canvas.json\",\n\"selector\": {\n\"type\": \"PointSelector\",\n\"x\": 3385,\n\"y\": 1464\n}\n}\n}\n]\n}\n]\n}\n]\n}";
        var iiif = manifest.FromJson<Manifest>();

        var expectedAnnotation = new AnnotationPage
        {
            Id = "https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/page/p2/1",
            Items = new List<IAnnotation>
            {
                new GeneralAnnotation("tagging")
                {
                    Id = "https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/annotation/p0002-tag",
                    Body = new List<ResourceBase>
                    {
                        new TextualBody("Town Creek Aqueduct")
                        {
                            Language = "en",
                            Format = "text/plain",
                        },
                    },
                    Target = new SpecificResource
                    {
                        Source = new Canvas
                            { Id = "https://iiif.io/api/cookbook/recipe/0135-annotating-point-in-canvas/canvas.json" },
                        Selector = new List<ISelector>
                        {
                            new PointSelector
                            {
                                X = 3385, Y = 1464
                            }
                        }
                    }
                }
            }
        };

        iiif.Items[0].Annotations[0].Should().BeEquivalentTo(expectedAnnotation);
    }
}