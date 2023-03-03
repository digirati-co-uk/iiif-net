using System;
using Newtonsoft.Json;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Outputs DateTime as a valid xsd:dateTime format, see https://www.w3.org/TR/xmlschema11-2/#dateTime
    /// </summary>
    public class XsdDateTimeConverter : WriteOnlyConverter<DateTime?>
    {
        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            if (value == null) return;

            var xsdDate = value.Value.ToString(GetStringFormat(value.Value.Kind));
            writer.WriteValue(xsdDate);
        }

        // If date is Utc then "Z" would be output as timezone. In this case don't use the K specified
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings?redirectedfrom=MSDN#KSpecifier
        private string GetStringFormat(DateTimeKind kind)
            => kind == DateTimeKind.Utc ? "yyyy-MM-ddTHH:mm:ss" : "yyyy-MM-ddTHH:mm:ssK";
    }
}