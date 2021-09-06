using IIIF.Presentation.V2;
using Newtonsoft.Json;

namespace IIIF.Search.V1
{
    public class AutoCompleteService : ResourceBase, IService
    {
        public const string AutoCompleteService1Profile = "http://iiif.io/api/search/1/autocomplete";

        [JsonProperty(PropertyName = "@type", Order = 3)]
        public override string? Type { get; set; } = "AutoCompleteService1";
    }
}
