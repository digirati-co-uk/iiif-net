using System;
using Newtonsoft.Json;

namespace IIIF.Serialisation;

public abstract class WriteOnlyConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override bool CanRead => false;

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public abstract class WriteOnlyConverter<T> : JsonConverter<T>
{
    public override bool CanRead => false;

    public override T ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}