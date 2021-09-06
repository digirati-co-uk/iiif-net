using IIIF.Presentation.V2;
using IIIF.Presentation.V2.Serialisation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Converter for <see cref="Thumbnail"/>, will just Id as string if only Id populated
    /// </summary>
    public class ThumbnailConverter : WriteOnlyConverter<Thumbnail>
    {
        public override void WriteJson(JsonWriter writer, Thumbnail? value, JsonSerializer serializer)
        {
            // If we have Id and nothing else, write Id as string
            if (!string.IsNullOrEmpty(value?.Id) &&
                string.IsNullOrEmpty(value.Type) &&
                value.Service == null &&
                value.Context == null &&
                value.Description == null &&
                value.Label == null &&
                value.Profile == null)
            {
                writer.WriteValue(value!.Id);
            }
            else
            {
                writer.WriteRawValue(JsonConvert.SerializeObject(value,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new PrettyIIIFContractResolver(),
                        Formatting = Formatting.Indented,
                    }));
            }
        }
    }
}