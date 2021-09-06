using IIIF.Presentation.V2.Strings;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2.Annotation
{
    public class AnnotationListReference : ResourceBase, IAnnotationListReference
    {
        public override string? Type
        {
            get => "sc:AnnotationList";
            set => throw new System.NotImplementedException();
        }

        [JsonProperty(Order = 40, PropertyName = "label")]
        public MetaDataValue? Label { get; set; }
    }
}