using System.Collections.Generic;
using System.Linq;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Traversal;

namespace IIIF.Tests.Presentation.V3.Traversal;

public class TraverseXTests
{
    /* Sample manifest with 3 canvases
     0 - contains single GeneralAnnotation
     1 - contains 2 paintingAnnos with Image bodies
     3 - contains 2 paintingAnno one Video body, one with no body
     */
    
    private Manifest sampleManifest = new()
    {
        Items = new List<Canvas>
        {
            new()
            {
                Items = new List<AnnotationPage>
                {
                    new()
                    {
                        Items = new List<IAnnotation>
                        {
                            new GeneralAnnotation("Describing") { Id = "0_0" },
                        }
                    }
                }
            },
            new()
            {
                Items = new List<AnnotationPage>
                {
                    new()
                    {
                        Items = new List<IAnnotation>
                        {
                            new PaintingAnnotation
                            {
                                Body = new Image { Id = "1_0" },
                            },
                            new PaintingAnnotation
                            {
                                Body = new Image { Id = "1_1" },
                            }
                        }
                    }
                }
            },
            new()
            {
                Items = new List<AnnotationPage>
                {
                    new()
                    {
                        Items = new List<IAnnotation>
                        {
                            new PaintingAnnotation
                            {
                                Body = new Video { Id = "2_0" },
                            },
                            new PaintingAnnotation { Id = "2_1" }
                        }
                    }
                }
            }
        }
    };

    private Manifest emptyManifest = new();
    private Canvas emptyCanvas = new();

    [Fact]
    public void AllAnnotations_Manifest_EmptyManifest()
    {
        emptyManifest.AllAnnotations().Should().BeEmpty();
    }
    
    [Fact]
    public void AllAnnotationsOfT_Manifest_EmptyManifest()
    {
        emptyManifest.AllAnnotations<PaintingAnnotation>().Should().BeEmpty();
    }
    
    [Fact]
    public void AllPaintingAnnoBodies_Manifest_EmptyManifest()
    {
        emptyManifest.AllPaintingAnnoBodies().Should().BeEmpty();
    }
    
    [Fact]
    public void AllPaintingAnnoBodiesOfT_Manifest_EmptyManifest()
    {
        emptyManifest.AllPaintingAnnoBodies<Image>().Should().BeEmpty();
    }

    [Fact]
    public void AllAnnotations_Manifest_Correct()
    {
        var expected = new List<IAnnotation>
        {
            new GeneralAnnotation("Describing") { Id = "0_0" },
            new PaintingAnnotation
            {
                Body = new Image { Id = "1_0" },
            },
            new PaintingAnnotation
            {
                Body = new Image { Id = "1_1" },
            },
            new PaintingAnnotation
            {
                Body = new Video { Id = "2_0" },
            },
            new PaintingAnnotation { Id = "2_1" }
        };

        var actual = sampleManifest.AllAnnotations().ToList();
        actual.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
    }
    
    [Fact]
    public void AllAnnotationsOfT_Manifest_Correct()
    {
        var expected = new List<IAnnotation>
        {
            new PaintingAnnotation
            {
                Body = new Image { Id = "1_0" },
            },
            new PaintingAnnotation
            {
                Body = new Image { Id = "1_1" },
            },
            new PaintingAnnotation
            {
                Body = new Video { Id = "2_0" },
            },
            new PaintingAnnotation { Id = "2_1" }
        };

        var actual = sampleManifest.AllAnnotations<PaintingAnnotation>().ToList();
        actual.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
    }
    
    [Fact]
    public void AllPaintingAnnoBodies_Manifest_Correct()
    {
        var expected = new List<IPaintable>
        {
            new Image { Id = "1_0" },
            new Image { Id = "1_1" },
            new Video { Id = "2_0" },
        };

        var actual = sampleManifest.AllPaintingAnnoBodies().ToList();
        actual.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
    }
    
    [Fact]
    public void AllPaintingAnnoBodiesOfT_Manifest_Correct()
    {
        var expected = new List<IPaintable>
        {
            new Image { Id = "1_0" },
            new Image { Id = "1_1" },
        };

        var actual = sampleManifest.AllPaintingAnnoBodies<Image>().ToList();
        actual.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
    }
    
    [Fact]
    public void AllAnnotations_Canvas_EmptyManifest()
    {
        emptyCanvas.AllAnnotations().Should().BeEmpty();
    }
    
    [Fact]
    public void AllAnnotationsOfT_Canvas_EmptyManifest()
    {
        emptyCanvas.AllAnnotations<PaintingAnnotation>().Should().BeEmpty();
    }
    
    [Fact]
    public void AllPaintingAnnoBodies_Canvas_EmptyManifest()
    {
        emptyCanvas.AllPaintingAnnoBodies().Should().BeEmpty();
    }
    
    [Fact]
    public void AllPaintingAnnoBodiesOfT_Canvas_EmptyManifest()
    {
        emptyCanvas.AllPaintingAnnoBodies<Image>().Should().BeEmpty();
    }

    [Fact]
    public void AllAnnotations_Canvas_Correct()
    {
        var expected = new List<IAnnotation>
        {
            new PaintingAnnotation
            {
                Body = new Image { Id = "1_0" },
            },
            new PaintingAnnotation
            {
                Body = new Image { Id = "1_1" },
            },
        };

        var actual = sampleManifest.Items[1].AllAnnotations().ToList();
        actual.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
    }
    
    [Fact]
    public void AllAnnotationsOfT_Canvas_Correct()
    {
        var expected = new List<IAnnotation>
        {
            new PaintingAnnotation
            {
                Body = new Video { Id = "2_0" },
            },
            new PaintingAnnotation { Id = "2_1" }
        };

        var actual = sampleManifest.Items[2].AllAnnotations<PaintingAnnotation>().ToList();
        actual.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
    }
    
    [Fact]
    public void AllPaintingAnnoBodies_Canvas_Correct()
    {
        var expected = new List<IPaintable>
        {
            new Video { Id = "2_0" },
        };

        var actual = sampleManifest.Items[2].AllPaintingAnnoBodies().ToList();
        actual.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
    }
    
    [Fact]
    public void AllPaintingAnnoBodiesOfT_Canvas_CorrectIfNone()
    {
        sampleManifest.Items[1].AllPaintingAnnoBodies<Video>().Should().BeEmpty();
    }
    
    [Fact]
    public void AllPaintingAnnoBodiesOfT_Canvas_Correct()
    {
        var expected = new List<IPaintable>
        {
            new Image { Id = "1_0" },
            new Image { Id = "1_1" },
        };

        var actual = sampleManifest.Items[1].AllPaintingAnnoBodies<Image>().ToList();
        actual.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
    }
}