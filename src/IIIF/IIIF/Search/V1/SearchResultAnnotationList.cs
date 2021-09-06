using IIIF.Presentation.V2;
using IIIF.Presentation.V2.Annotation;
using Newtonsoft.Json;

namespace IIIF.Search.V1
{
    public class SearchResultAnnotationList : AnnotationList
    {
        [JsonProperty(Order = 10, PropertyName = "within")]
        public SearchResultsLayer Within { get; set; }

        [JsonProperty(Order = 12, PropertyName = "previous")]
        public string Previous { get; set; }

        [JsonProperty(Order = 13, PropertyName = "next")]
        public string Next { get; set; }

        [JsonProperty(Order = 16, PropertyName = "startIndex")]
        public int StartIndex { get; set; }

        [JsonProperty(Order = 30, PropertyName = "hits")]
        public Hit[] Hits { get; set; }
    }
}
