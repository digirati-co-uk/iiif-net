using IIIF.Presentation.V3.Annotation;

namespace IIIF.Presentation.V3.Content
{
    public class Audio : ExternalResource, ITemporal, IPaintable
    {
        public double? Duration { get; set; }

        public Audio() : base("Sound") { }
    }
}