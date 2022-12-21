using IIIF.Presentation.V2;
using Newtonsoft.Json;

namespace IIIF.Search.V1
{
    public class TextQuoteSelector: ResourceBase
    {
        public override string? Type
        {
            get => "oa:TextQuoteSelector";
            set => throw new System.NotImplementedException();
        }

        [JsonProperty(Order = 51, PropertyName = "exact")]
        public string? Exact { get; set; }

        [JsonProperty(Order = 52, PropertyName = "prefix")]
        public string? Prefix { get; set; }

        [JsonProperty(Order = 53, PropertyName = "suffix")]
        public string? Suffix { get; set; }
    }
}
