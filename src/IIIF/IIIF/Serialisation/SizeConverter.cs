using IIIF.Presentation.V2.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Converter for <see cref="Size"/> type that writes single line
    /// e.g. {"width":100,"height":200}
    /// </summary>
    public class SizeConverter : WriteOnlyConverter<Size>
    {
        public override void WriteJson(JsonWriter writer, Size? value, JsonSerializer serializer) 
            => writer.WriteRawValue(JsonConvert.SerializeObject(value, Formatting.None));
    }
}