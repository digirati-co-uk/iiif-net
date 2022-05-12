using System;
using IIIF.Presentation.V3;
using IIIF.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Range = IIIF.Presentation.V3.Range;

namespace IIIF.Serialisation
{
    /// <summary>
    /// <see cref="JsonConverter"/> for <see cref="IStructuralLocation"/> objects.
    /// Serialises Id-only <see cref="Canvas"/> objects to string representation and deserialises back. 
    /// </summary>
    public class TargetConverter : JsonConverter<IStructuralLocation>
    {
        public override IStructuralLocation? ReadJson(JsonReader reader, Type objectType,
            IStructuralLocation? existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return new Canvas { Id = reader.Value.ToString() };
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                var obj = JObject.Load(reader);

                var type = obj["type"].Value<string>();
                return type switch
                {
                    nameof(Canvas) => obj.ToObject<Canvas>(),
                    nameof(Range) => obj.ToObject<Range>(),
                    nameof(SpecificResource) => obj.ToObject<SpecificResource>()
                };
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, IStructuralLocation? value, JsonSerializer serializer)
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
            => canvas.Width == null && canvas.Duration == null && canvas.Items.IsNullOrEmpty();
    }
}