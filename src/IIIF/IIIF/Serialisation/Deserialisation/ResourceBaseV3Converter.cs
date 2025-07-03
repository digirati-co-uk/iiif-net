using System;
using System.Collections.Generic;
using IIIF.Auth.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Serialisation.Deserialisation.Helpers;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation;

/// <summary>
/// JsonConverter to handle reading <see cref="ResourceBase"/> objects to concrete type.
/// Falls through to <see cref="ExternalResource"/> if type cannot be identified.
/// </summary>
public class ResourceBaseV3Converter : ReadOnlyConverter<ResourceBase>
{
    public override ResourceBase? ReadJson(JsonReader reader, Type objectType, ResourceBase? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var converter = new ResourceDeserialiser<ResourceBase>(this);
        return converter.ReadJson(reader, serializer);
    }
}