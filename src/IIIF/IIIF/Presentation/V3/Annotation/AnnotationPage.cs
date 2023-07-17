using System.Collections.Generic;
using Newtonsoft.Json;

namespace IIIF.Presentation.V3.Annotation;

public class AnnotationPage : ResourceBase
{
    public override string Type => nameof(AnnotationPage);

    [JsonProperty(Order = 300)] public List<IAnnotation>? Items { get; set; }
}