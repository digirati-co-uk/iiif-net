using IIIF.Presentation.V3.Annotation;

namespace IIIF.Presentation.V3.Content
{
    public class Video : ExternalResource, ISpatial, ITemporal, IPaintable
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public double? Duration { get; set; }

        public Video() : base(nameof(Video)) { }
    }
}