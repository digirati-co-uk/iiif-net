using IIIF.Presentation.V3.Annotation;

namespace IIIF.Presentation.V3.Content;

public class Sound : ExternalResource, ITemporal, IPaintable
{
    public double? Duration { get; set; }

    public Sound() : base(nameof(Sound))
    {
    }
}