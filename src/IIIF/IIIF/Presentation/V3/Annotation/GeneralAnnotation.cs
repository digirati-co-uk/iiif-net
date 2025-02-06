namespace IIIF.Presentation.V3.Annotation;

public sealed class GeneralAnnotation : Annotation
{
    public GeneralAnnotation(string motivation)
    {
        Motivation = motivation;
    }
    
    [JsonProperty(Order = 500)] public ResourceBase? Body { get; set; }
}