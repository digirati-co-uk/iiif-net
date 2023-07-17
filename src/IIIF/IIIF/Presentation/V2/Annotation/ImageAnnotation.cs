using Newtonsoft.Json;

namespace IIIF.Presentation.V2.Annotation;

public class ImageAnnotation : ResourceBase
{
    public override string? Type
    {
        get => "oa:Annotation";
        set => throw new System.NotImplementedException();
    }

    [JsonProperty(Order = 4, PropertyName = "motivation")]
    public string Motivation => "sc:painting";

    [JsonProperty(Order = 10, PropertyName = "resource")]
    public ImageResource Resource { get; set; }

    [JsonProperty(Order = 36, PropertyName = "on")]
    public string On { get; set; }
}