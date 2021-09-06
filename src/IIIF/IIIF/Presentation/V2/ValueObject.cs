using Newtonsoft.Json;

namespace IIIF.Presentation.V2
{
    public abstract class ValueObject
    {
        [JsonProperty(Order = 1, PropertyName = "@context")]
        public string Context { get; set; }

        [JsonProperty(Order = 2, PropertyName = "@index")]
        public string Index { get; set; }

        [JsonProperty(Order = 3, PropertyName = "@value")]
        public string Value { get; set; }

    }
}