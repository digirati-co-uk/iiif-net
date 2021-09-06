using IIIF.Presentation.V2.Strings;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2
{
    /// <summary>
    /// A short, descriptive entry consisting of human readable label and value to be displayed to the user.
    /// </summary>
    /// <remarks>See https://iiif.io/api/presentation/2.1/#metadata</remarks>
    public class Metadata
    {
        [JsonProperty(Order = 1, PropertyName = "label")]
        public MetaDataValue Label { get; set; }

        [JsonProperty(Order = 2, PropertyName = "value")]
        public MetaDataValue Value { get; set; }
    }
}