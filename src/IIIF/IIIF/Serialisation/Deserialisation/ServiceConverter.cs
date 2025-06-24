using System;
using IIIF.Serialisation.Deserialisation.Helpers;

namespace IIIF.Serialisation.Deserialisation;

/// <summary>
/// JsonConverter to handle reading <see cref="IService"/> objects to concrete type.
/// </summary>
public class ServiceConverter : ReadOnlyConverter<IService>
{
    public override IService? ReadJson(JsonReader reader, Type objectType, IService? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var converter = new ResourceDeserialiser<IService>(this);
        return converter.ReadJson(reader, serializer);
    }
}