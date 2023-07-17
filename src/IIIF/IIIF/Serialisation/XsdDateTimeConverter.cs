using System;
using Newtonsoft.Json;

namespace IIIF.Serialisation;

/// <summary>
/// Outputs DateTime as a valid xsd:dateTime format, see https://www.w3.org/TR/xmlschema11-2/#dateTime
/// </summary>
public class XsdDateTimeConverter : WriteOnlyConverter<DateTime?>
{
    public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
    {
        if (value == null) return;

        var xsdDate = value.Value.ToString("yyyy-MM-ddTHH:mm:ssK");
        writer.WriteValue(xsdDate);
    }
}