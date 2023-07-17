namespace IIIF.Presentation.V3.Annotation;

public class TypeClassifyingAnnotation : Annotation
{
    public override string Motivation => Constants.Motivation.Classifying;

    public ResourceBase? Body { get; set; }
}

/// <summary>
/// Marker class for deserialising json object with an unknown motivation
/// </summary>
internal sealed class UnknownMotivation : Annotation
{
    public UnknownMotivation(string motivation)
    {
        Motivation = motivation;
    }
}