using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation
{
    /// <summary>
    /// Serialises List as a single object if 1 element, else renders json array.
    /// Note - for deserialisation to work the targettype must be List{T}
    /// </summary>
    public class ObjectIfSingleConverter : JsonConverter
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

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            // [ {"foo": "bar"} ] or {"foo": "bar"}
            if (reader.TokenType is JsonToken.StartArray or JsonToken.StartObject)
            {
                var isArray = reader.TokenType is JsonToken.StartArray;
                return Deserialise(reader, objectType, serializer, isArray);
            }

            // "foo bar" - target type will be List<string>
            if (reader.TokenType == JsonToken.String)
            {
                return CreateListOfOne(objectType, reader.Value?.ToString());
            }
            
            throw new FormatException("Unable to convert provided object");
        }
        
        public override bool CanConvert(Type objectType)
            => objectType.IsAssignableTo(typeof(IEnumerable));

        private object? Deserialise(JsonReader reader, Type objectType, JsonSerializer serializer, bool isArray)
        {
            var targetType = Activator.CreateInstance(objectType);
            if (targetType == null) return null;
            
            // Remove current type to avoid circular loop
            var jsonSerializer = serializer.CreateCopy(converter => converter is not ObjectIfSingleConverter);

            // If this is already an array load it as-is, else load the JObject into an array
            var array = isArray ? JArray.Load(reader) : new JArray(JObject.Load(reader));
            jsonSerializer.Populate(array.CreateReader(), targetType);
            return targetType;
        }

        private object? CreateListOfOne(Type objectType, object? singleValue)
        {
            if (singleValue == null) return null;
            
            var targetType = Activator.CreateInstance(objectType);
            if (targetType == null) return null;

            var addMethod = objectType.GetMethod("Add");
            if (addMethod == null) return null;

            addMethod.Invoke(targetType, new[] { singleValue });
            return targetType;
        }
    }
}