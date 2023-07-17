using System;
using System.Collections.Generic;
using IIIF.Presentation.V2.Strings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Presentation.V2.Serialisation;

/// <summary>
/// JsonConverter for <see cref="MetaDataValue"/> objects.
/// </summary>
public class MetaDataValueSerialiser : JsonConverter<MetaDataValue>
{
    public override void WriteJson(JsonWriter writer, MetaDataValue? value, JsonSerializer serializer)
    {
        if (value == null)
            throw new ArgumentException(
                $"MetaDataValueSerialiser cannot serialise a {value.GetType().Name}", nameof(value));

        if (value.LanguageValues.Count == 0)
            throw new ArgumentException(
                $"MetaDataValueSerialiser cannot serialise an empty array {value.GetType().Name}", nameof(value));

        if (value.LanguageValues.Count > 1) writer.WriteStartArray();

        foreach (var lv in value.LanguageValues)
            if (string.IsNullOrWhiteSpace(lv.Language))
            {
                writer.WriteValue(lv.Value);
            }
            else
            {
                writer.WriteStartObject();
                writer.WritePropertyName("@value");
                writer.WriteValue(lv.Value);
                writer.WritePropertyName("@language");
                writer.WriteValue(lv.Language);
                writer.WriteEndObject();
            }

        if (value.LanguageValues.Count > 1) writer.WriteEndArray();
    }

    public override MetaDataValue? ReadJson(JsonReader reader, Type objectType, MetaDataValue? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;

        if (reader.TokenType == JsonToken.String)
            // Basic string, single value only
            return new MetaDataValue(reader.Value.ToString());

        if (reader.TokenType == JsonToken.StartObject)
        {
            // Single language value
            var jo = JObject.Load(reader);
            var languageValue = jo.ToObject<LanguageValue>();
            return new MetaDataValue(new List<LanguageValue> { languageValue });
        }

        if (reader.TokenType == JsonToken.StartArray)
        {
            var jo = JArray.Load(reader);
            if (jo.First.Type == JTokenType.String)
            {
                var values = jo.ToObject<string[]>();
                return ConvertFromStringArray(values);
            }
            else
            {
                var languageValues = jo.ToObject<LanguageValue[]>();
                return new MetaDataValue(languageValues);
            }
        }

        throw new FormatException("Unable to convert provided object to MetaDataValue");
    }

    private MetaDataValue ConvertFromStringArray(IReadOnlyList<string> values)
    {
        var mdv = new MetaDataValue(values[0]);
        for (var x = 1; x < values.Count; x++) mdv.LanguageValues.Add(new LanguageValue { Value = values[x] });

        return mdv;
    }
}