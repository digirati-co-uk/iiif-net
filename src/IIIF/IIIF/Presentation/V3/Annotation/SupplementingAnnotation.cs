using System;

namespace IIIF.Presentation.V3.Annotation;

[Obsolete("GeneralAnnotation should be used instead")]
public class SupplementingDocumentAnnotation : Annotation
{
    public override string Motivation => Constants.Motivation.Supplementing;

    public ResourceBase? Body { get; set; }
}