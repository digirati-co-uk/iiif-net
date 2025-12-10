using System;
using System.Runtime.Serialization;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation;

/// <summary>
/// JsonConverter to handle reading <see cref="IPaintable"/> objects to concrete type.
/// </summary>
public class PaintableConverter : ReadOnlyConverter<IPaintable>
{
    public override IPaintable? ReadJson(JsonReader reader, Type objectType, IPaintable? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);

        var type = jsonObject["type"].Value<string>();
        IPaintable? paintable = type switch
        {
            nameof(Sound) => new Sound(),
            "Audio" => new Sound(),
            nameof(Video) => new Video(),
            nameof(Image) => new Image(),
            nameof(Canvas) => new Canvas(),
            "Choice" => new PaintingChoice(),
            nameof(SpecificResource) => new SpecificResource(),
            _ => null
        };

        if (paintable == null && type == nameof(TextualBody))
        {
            // to construct TextualBody we need a value
            paintable = new TextualBody(jsonObject["value"].Value<string>());
        }

        if (paintable == null)
        {
            var idDetails = jsonObject.TryGetValue("id", out JToken? id) ? $"'id'={id}" : "'id' undefined";
            throw new SerializationException($"Unable to identify IPaintable, 'type'={type}: {idDetails}");
        }

        serializer.Populate(jsonObject.CreateReader(), paintable);
        return paintable;
    }
}