using System;
using IIIF.Presentation.V3.Annotation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation
{
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

            IAnnotation annotation = jsonObject["motivation"].Value<string>() switch
            {
                IIIF.Presentation.V3.Constants.Motivation.Painting => new PaintingAnnotation(),
                IIIF.Presentation.V3.Constants.Motivation.Supplementing => new SupplementingDocumentAnnotation(),
                IIIF.Presentation.V3.Constants.Motivation.Classifying => new TypeClassifyingAnnotation(),
                _ => new Annotation()
            };

            serializer.Populate(jsonObject.CreateReader(), annotation);
            return annotation;
        }
    }
}