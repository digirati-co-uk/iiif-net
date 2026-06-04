namespace IIIF.Presentation.V3.Annotation;

/// <summary>
/// An annotation with "painting" motivation and a single body. This is a convenience class for constructing painting
/// annotations.
///
/// Single body is the most common use case for painting annotations, use <see cref="GeneralAnnotation"/> if
/// multiple bodies are required.
/// </summary>
public class PaintingAnnotation : Annotation
{
    public override string Motivation => Constants.Motivation.Painting;

    [JsonProperty(Order = 500)] public IPaintable? Body { get; set; }
}