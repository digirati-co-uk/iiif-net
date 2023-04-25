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

            // List<T>
            if (reader.TokenType == JsonToken.StartArray)
            {
                return DeserialiseArray(reader, objectType, serializer);
            }

            // Single value of T but target type will be a List<T>
            if (reader.TokenType == JsonToken.StartObject)
            {
                return DeserialiseSingle(reader, objectType, serializer);
            }
            
            // Single string - target type will be List<string>
            if (reader.TokenType == JsonToken.String)
            {
                return CreateListOfOne(objectType, reader.Value?.ToString());
            }
            
            throw new FormatException("Unable to convert provided object");
        }
        
        public override bool CanConvert(Type objectType)
            => objectType.IsAssignableTo(typeof(IEnumerable));

        private object? DeserialiseArray(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            // e.g. List<IService>
            if (objectType.GenericTypeArguments[0].IsInterface)
            {
                return DeserialiseInterfaceList(JArray.Load(reader), objectType, serializer);
            }

            // e.g. List<Thumbnail> - we can deserialize directly
            var jo = JArray.Load(reader);
            var final = jo.ToObject(objectType);
            return final;
        }
        
        private object? DeserialiseSingle(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var genericType = objectType.GenericTypeArguments[0];

            if (genericType.IsInterface)
            {
                // If interface type then create instance manually.
                // Note: Calling serializer.Populate here goes into a ObjectIfSingleConverter.ReadJson loop
                return DeserialiseInterfaceList(new JArray(jo), objectType, serializer);
            }
            
            // Concrete type, deserialise single and create array
            var deserialised = jo.ToObject(genericType);
            return CreateListOfOne(objectType, deserialised);
        }

        private object? DeserialiseInterfaceList(JArray jArray, Type objectType, JsonSerializer serializer)
        {
            // This method is used when we have an interface type as generic arg for list (e.g. List<IService>)
            // This will use other JsonConverters to determine concrete type and deserialise
            // e.g. is it AuthTokenService, ImageService etc
            var targetType = Activator.CreateInstance(objectType);
            if (targetType == null) return null;
            
            serializer.Populate(jArray.CreateReader(), targetType);
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