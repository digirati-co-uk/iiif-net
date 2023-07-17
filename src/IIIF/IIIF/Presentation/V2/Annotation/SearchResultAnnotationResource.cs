using Newtonsoft.Json;

namespace IIIF.Presentation.V2.Annotation;

public class SearchResultAnnotationResource : ResourceBase
{
    public override string? Type
    {
        get => "cnt:ContentAsText";
        set => throw new System.NotImplementedException();
    }

    [JsonProperty(Order = 10, PropertyName = "chars")]
    public string? Chars { get; set; }
}