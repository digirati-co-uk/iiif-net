using System;
using IIIF.Presentation.V3.Annotation;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation;

/// <summary>
/// JsonConverter to handle reading <see cref="IAnnotation"/> objects to concrete type.
/// </summary>
public class AnnotationV3Converter : ReadOnlyConverter<IAnnotation>
{
    public override IAnnotation? ReadJson(JsonReader reader, Type objectType, IAnnotation? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);

        var motivation = jsonObject["motivation"]?.Value<string>();
        var annotation = MotivationConverter.TryGetAnnotation(jsonObject, motivation!);
        serializer.Populate(jsonObject.CreateReader(), annotation);
        return annotation;
    }
}