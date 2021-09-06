namespace IIIF.Presentation.V3.Annotation
{
    public class TypeClassifyingAnnotation : Annotation
    {
        public override string Motivation => Constants.Motivation.Classifying;
        
        public ResourceBase? Body { get; set; }
    }
}