namespace IIIF.Presentation.V3.Annotation
{
    public class TextualBody : ResourceBase
    {
        public TextualBody(string value)
        {
            Value = value;
        }
        
        public TextualBody(string value, string format)
        {
            Value = value;
            Format = format;
        }
        
        public override string Type => nameof(TextualBody);
        
        public string? Value { get; set; }
        public string? Format { get; set; }
    }
}