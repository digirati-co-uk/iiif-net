namespace IIIF.Presentation.V3.Annotation
{
    public class ClassifyingBody : ResourceBase
    {
        public ClassifyingBody(string classifyingType)
        {
            Id = classifyingType;
        }
        
        public override string Type => null;
    }
}