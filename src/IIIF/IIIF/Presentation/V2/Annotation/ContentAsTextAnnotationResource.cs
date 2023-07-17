using Newtonsoft.Json;

namespace IIIF.Presentation.V2.Annotation;

public class ContentAsTextAnnotationResource : ResourceBase
{
    public override string? Type
    {
        get => "cnt:ContentAsText";
        set => throw new System.NotImplementedException();
    }

    [JsonProperty(Order = 10, PropertyName = "format")]
    public string Format { get; set; }

    [JsonProperty(Order = 20, PropertyName = "chars")]
    public string Chars { get; set; }
}