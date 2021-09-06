using Newtonsoft.Json;

namespace IIIF.Presentation.V2
{
    public class ImageResource : Resource
    {
        public override string? Type
        {
            get => "dctypes:Image";
            set => throw new System.NotImplementedException();
        }

        [JsonProperty(Order = 35, PropertyName = "height")]
        public int? Height { get; set; }

        [JsonProperty(Order = 36, PropertyName = "width")]
        public int? Width { get; set; }
    }
}