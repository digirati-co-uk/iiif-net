using System.Collections.Generic;
using System.Linq;
using IIIF.Presentation.V3.Annotation;

namespace IIIF.Presentation.V3.Traversal;

/// <summary>
/// Collection of convenience methods to make traversing nested structure easier to work with 
/// </summary>
public static class TraverseX
{
    /// <summary>
    /// Get enumerator for all <see cref="IAnnotation"/> from every canvas in manifest, regardless of type.
    /// Iterates over all <see cref="AnnotationPage"/> Items.
    /// </summary>
    public static IEnumerable<IAnnotation> AllAnnotations(this Manifest manifest) =>
        manifest.AllAnnotations<IAnnotation>();
    
    /// <summary>
    /// Get enumerator for all <see cref="IAnnotation"/> from every canvas in manifest, of type {T}.
    /// Iterates over all <see cref="AnnotationPage"/> Items.
    /// </summary>
    public static IEnumerable<T> AllAnnotations<T>(this Manifest manifest)
        where T : IAnnotation
    {
        foreach (var canvas in manifest.Items ?? Enumerable.Empty<Canvas>())
        {
            foreach (var anno in canvas.AllAnnotations<T>()) yield return anno;
        }
    }
    
    /// <summary>
    /// Get enumerator for all <see cref="IAnnotation"/> from every canvas in manifest, regardless of type.
    /// This is manifest.Items[*].Items[*].Items[*].Body
    /// </summary>
    public static IEnumerable<IPaintable> AllPaintingAnnoBodies(this Manifest manifest)
    {
        foreach (var canvas in manifest.Items ?? Enumerable.Empty<Canvas>())
        {
            foreach (var paintingAnno in canvas.AllAnnotations<PaintingAnnotation>())
            {
                if (paintingAnno.Body != null) yield return paintingAnno.Body;
            }
        }
    }
    
    /// <summary>
    /// Get enumerator for all painting annotation bodies for every canvas in manifest, of type {T}.
    /// This is manifest.Items[*].Items[*].Items[*].Body where type is {T}
    /// </summary>
    public static IEnumerable<T> AllPaintingAnnoBodies<T>(this Manifest manifest)
        where T : IPaintable
    {
        foreach (var canvas in manifest.Items ?? Enumerable.Empty<Canvas>())
        {
            foreach (var paintingAnno in canvas.AllAnnotations<PaintingAnnotation>())
            {
                if (paintingAnno.Body is T typed) yield return typed;
            }
        }
    }

    /// <summary>
    /// Get enumerator for all <see cref="IAnnotation"/> from canvas, regardless of type.
    /// Iterates over all <see cref="AnnotationPage"/> Items.
    /// </summary>
    public static IEnumerable<IAnnotation> AllAnnotations(this Canvas canvas)
        => canvas.AllAnnotations<IAnnotation>();

    /// <summary>
    /// Get enumerator for all <see cref="IAnnotation"/> on canvas, of type {T}. Iterates over all
    /// <see cref="AnnotationPage"/> Items.
    /// </summary>
    public static IEnumerable<T> AllAnnotations<T>(this Canvas canvas)
        where T : IAnnotation
    {
        foreach (var annoPage in canvas.Items ?? Enumerable.Empty<AnnotationPage>())
        {
            foreach (var anno in annoPage.Items ?? Enumerable.Empty<IAnnotation>())
            {
                if (anno is T typed) yield return typed;
            }
        }
    }

    /// <summary>
    /// Get enumerator for all <see cref="IAnnotation"/> from every canvas in manifest, regardless of type.
    /// This is manifest.Items[*].Items[*].Items[*].Body
    /// </summary>
    public static IEnumerable<IPaintable> AllPaintingAnnoBodies(this Canvas canvas)
    {
        foreach (var paintingAnno in canvas.AllAnnotations<PaintingAnnotation>())
        {
            if (paintingAnno.Body != null) yield return paintingAnno.Body;
        }
    }

    /// <summary>
    /// Get enumerator for all painting annotation bodies for canvas in manifest, of type {T}.
    /// This is manifest.Items[*].Items[*].Items[*].Body where type is {T}
    /// </summary>
    public static IEnumerable<T> AllPaintingAnnoBodies<T>(this Canvas canvas)
        where T : IPaintable
    {
        foreach (var paintingAnno in canvas.AllAnnotations<PaintingAnnotation>())
        {
            if (paintingAnno.Body is T typed) yield return typed;
        }
    }
}