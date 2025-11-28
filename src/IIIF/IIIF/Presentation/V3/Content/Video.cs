using IIIF.Presentation.V3.Annotation;

namespace IIIF.Presentation.V3.Content;

public class Video : ExternalResource, ISpatial, ITemporal, IPaintable
{
    [JsonProperty(Order = 11)] public int? Width { get; set; }
    [JsonProperty(Order = 12)] public int? Height { get; set; }
    [JsonProperty(Order = 13)] public double? Duration { get; set; }

    public Video() : base(nameof(Video))
    {
    }
}