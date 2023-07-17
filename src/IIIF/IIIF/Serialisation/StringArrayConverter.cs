using System.Collections.Generic;
using System.Linq;
using IIIF.Presentation.V2.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Serialisation;

/// <summary>
/// Converter for string arrays, will output on one line of single value and less than 50 chars.
/// </summary>
public class StringArrayConverter : WriteOnlyConverter<IEnumerable<string>>
{
    // Arrays of single strings below this length will be output on 1 line
    public const int MaxChars = 40;

    public override void WriteJson(JsonWriter writer, IEnumerable<string>? value, JsonSerializer serializer)
    {
        var stringList = value?.ToList();
        if (stringList == null || stringList.Count == 0) return;

        if (stringList.Count == 1 && stringList[0].Length <= MaxChars)
        {
            writer.WriteRawValue(JsonConvert.SerializeObject(value, Formatting.None));
        }
        else
        {
            writer.WriteStartArray();
            foreach (var val in stringList) writer.WriteValue(val);
            writer.WriteEndArray();
        }
    }
}