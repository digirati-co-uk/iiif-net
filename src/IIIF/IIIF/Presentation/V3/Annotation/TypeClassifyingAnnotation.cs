using System;

namespace IIIF.Presentation.V3.Annotation;

[Obsolete("GeneralAnnotation should be used instead")]
public class TypeClassifyingAnnotation : Annotation
{
    public override string Motivation => Constants.Motivation.Classifying;

    public ResourceBase? Body { get; set; }
}

/// <summary>
/// Marker class for deserialising json object with an unknown motivation
/// </summary>
[Obsolete("GeneralAnnotation should be used instead")]
internal sealed class UnknownMotivation : Annotation
{
    public UnknownMotivation(string motivation)
    {
        Motivation = motivation;
    }
}