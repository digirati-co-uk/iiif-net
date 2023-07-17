using IIIF.Presentation.V3.Annotation;

namespace IIIF.Presentation.V3.Content;

public class Image : ExternalResource, ISpatial, IPaintable
{
    [JsonProperty(Order = 11)] public int? Width { get; set; }

    [JsonProperty(Order = 12)] public int? Height { get; set; }

    public Image() : base(nameof(Image))
    {
    }
}