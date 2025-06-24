using System;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Extensions.NavPlace;
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
        var converter = new ResourceDeserialiser<IResource>(this, type => type switch
        {
            nameof(Sound) => new Sound(),
            nameof(Video) => new Video(),
            nameof(Image) => new Image(),
            nameof(Feature) => new Feature(),
            _ => null
        });
        return converter.ReadJson(reader, serializer);
    }
}