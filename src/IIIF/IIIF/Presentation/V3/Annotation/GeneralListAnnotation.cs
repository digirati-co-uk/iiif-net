using System.Collections.Generic;

namespace IIIF.Presentation.V3.Annotation;

public sealed class GeneralListAnnotation : Annotation
{
    public GeneralListAnnotation(string motivation)
    {
        Motivation = motivation;
    }
    
    [JsonProperty(Order = 500)] public List<ResourceBase>? Body { get; set; }
}