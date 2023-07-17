namespace IIIF.Presentation.V3.Annotation;

public class SupplementingDocumentAnnotation : Annotation
{
    public override string Motivation => Constants.Motivation.Supplementing;

    public ResourceBase? Body { get; set; }
}