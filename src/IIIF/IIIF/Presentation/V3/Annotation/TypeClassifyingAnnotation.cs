using System;

namespace IIIF.Presentation.V3.Annotation;

[Obsolete("GeneralAnnotation should be used instead")]
public class TypeClassifyingAnnotation : Annotation
{
    public override string Motivation => Constants.Motivation.Classifying;

    public ResourceBase? Body { get; set; }
}
