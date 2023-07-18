using System;
using System.Collections.Generic;
using System.Linq;
using IIIF.Presentation.V3.Strings;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation;

/// <summary>
/// Serialises <see cref="LanguageMap"/> as json, putting values on single line if single language with single value
/// of less than 40 chars.
/// </summary>
public class LanguageMapSerialiser : JsonConverter<LanguageMap>
{
    public override LanguageMap? ReadJson(JsonReader reader, Type objectType, LanguageMap? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var map = existingValue ?? new LanguageMap();
        
        var jsonObject = JObject.Load(reader);
        serializer.Populate(jsonObject.CreateReader(), map);
        return map;
    }

    public override void WriteJson(JsonWriter writer, LanguageMap? value, JsonSerializer serializer)
    {
        if (value == null)
            throw new ArgumentException(
                "LanguageMapSerialiser cannot serialise a null object", nameof(value));

        if (value.Count == 0)
            throw new ArgumentException(
                $"LanguageMapSerialiser cannot serialise an empty array {value.GetType().Name}", nameof(value));

        // if has a single language, with a single value, of length less than X, output without formatting
        if (value.Count == 1)
        {
            var (key, list) = value.Single();
            if (list.Count == 1 && list[0].Length <= StringArrayConverter.MaxChars)
            {
                var output = new Dictionary<string, List<string>>
                {
                    [key] = list
                };
                writer.WriteRawValue(JsonConvert.SerializeObject(output, Formatting.None));
                return;
            }
        }

        // output with 'normal' formatting. Can't just call serialiser with value or we end up in an infinite loop
        writer.WriteStartObject();
        foreach (var (key, values) in value)
        {
            writer.WritePropertyName(key);
            serializer.Serialize(writer, values);
        }

        writer.WriteEndObject();
    }
}