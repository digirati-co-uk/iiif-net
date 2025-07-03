using System;
using IIIF.Serialisation.Deserialisation.Helpers;

namespace IIIF.Serialisation.Deserialisation;

/// <summary>
/// JsonConverter to handle reading <see cref="IResource"/> objects to concrete type.
/// </summary>
public class ResourceConverter : ReadOnlyConverter<IResource>
{
    public override IResource? ReadJson(JsonReader reader, Type objectType, IResource? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var converter = new ResourceDeserialiser<IResource>(this);
        return converter.ReadJson(reader, serializer);
    }
}