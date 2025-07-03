using System;
using IIIF.Presentation.V3;
using IIIF.Serialisation.Deserialisation.Helpers;
using IIIF.Utils;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation;

/// <summary>
/// <see cref="JsonConverter"/> for <see cref="IStructuralLocation"/> objects.
/// Serialises Id-only <see cref="Canvas"/> objects to string representation and deserialises back. 
/// </summary>
public class TargetConverter : JsonConverter<ResourceBase>
{
    public override ResourceBase? ReadJson(JsonReader reader, Type objectType,
        ResourceBase? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            return new Canvas { Id = reader.Value.ToString() };
        }
        else if (reader.TokenType == JsonToken.StartObject)
        {
            var converter = new ResourceDeserialiser<ResourceBase>(this);
            return converter.ReadJson(reader, serializer);
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, ResourceBase? value, JsonSerializer serializer)
    {
        if (value is Canvas canvas && (canvas.SerialiseTargetAsId || IsSimpleCanvas(canvas)))
        {
            writer.WriteValue(canvas.Id);
            return;
        }

        // Default, pass through behaviour:
        JObject.FromObject(value, serializer).WriteTo(writer);
    }

    private static bool IsSimpleCanvas(Canvas canvas)
    {
        return canvas.Width == null && canvas.Duration == null && canvas.Items.IsNullOrEmpty();
    }
}