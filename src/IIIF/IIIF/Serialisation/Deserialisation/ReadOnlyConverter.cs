using System;
using Newtonsoft.Json;

namespace IIIF.Serialisation.Deserialisation;

public abstract class ReadOnlyConverter<T> : JsonConverter<T>
{
    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}