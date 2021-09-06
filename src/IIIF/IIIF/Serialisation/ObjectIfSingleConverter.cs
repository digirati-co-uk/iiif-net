using System;
using System.Collections;
using IIIF.Presentation.V2.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Serialises List as a single object if 1 element, else renders json array.
    /// </summary>
    public class ObjectIfSingleConverter : WriteOnlyConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not IList list)
            {
                throw new ArgumentException(
                    $"ObjectIfSingleConverter expected IEnumerable but got {value.GetType().Name}", nameof(value));
            }

            if (list.Count > 1)
            {
                writer.WriteStartArray();
            }

            foreach (var element in list)
            {
                serializer.Serialize(writer, element);
            }
            
            if (list.Count > 1)
            {
                writer.WriteEndArray();
            }
        }
    }
}