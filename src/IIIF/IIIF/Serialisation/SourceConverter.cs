using System;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Utils;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation;

public class SourceConverter : JsonConverter<IPaintable>
{
    public override IPaintable? ReadJson(JsonReader reader, Type objectType, IPaintable? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            // We do not know that this is a Canvas...
            // We would need knowledge of the rest of the IIIF Resource
            return new Canvas { Id = reader.Value.ToString() };
        }
        else if (reader.TokenType == JsonToken.StartObject)
        {
            var obj = JObject.Load(reader);
            var type = obj["type"].Value<string>();
            IPaintable paintable = type switch
            {
                nameof(Sound) => new Sound(),
                nameof(Video) => new Video(),
                nameof(Image) => new Image(),
                nameof(Canvas) => new Canvas(),
                nameof(SpecificResource) => new SpecificResource()
            };
            serializer.Populate(obj.CreateReader(), paintable);
            return paintable;
        }

        return null;
    }
    
    public override void WriteJson(JsonWriter writer, IPaintable? value, JsonSerializer serializer)
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