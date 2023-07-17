using System.Collections.Generic;
using IIIF.Presentation.V3.Annotation;
using IIIF.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2.Annotation;

public class AnnotationList : ResourceBase
{
    public override string? Type { get; set; } = "sc:AnnotationList";

    [JsonProperty(Order = 20, PropertyName = "resources")]
    [RequiredOutput]
    public List<IAnnotation> Resources { get; set; }
}