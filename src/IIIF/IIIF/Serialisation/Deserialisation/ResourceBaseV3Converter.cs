using System;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation
{
    /// <summary>
    /// JsonConverter to handle reading <see cref="ResourceBase"/> objects to concrete type.
    /// Falls through to <see cref="ExternalResource"/> if type cannot be identified.
    /// </summary>
    public class ResourceBaseV3Converter : ReadOnlyConverter<ResourceBase>
    {
        public override ResourceBase? ReadJson(JsonReader reader, Type objectType, ResourceBase? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            var resourceBase = IdentifyConcreteType(jsonObject);

            serializer.Populate(jsonObject.CreateReader(), resourceBase);
            return resourceBase;
        }

        private static ResourceBase? IdentifyConcreteType(JObject jsonObject)
        {
            ResourceBase? resourceBase = null;
            if (!jsonObject.ContainsKey("type"))
            {
                resourceBase = new ClassifyingBody(jsonObject["id"].Value<string>());
                return resourceBase;
            }

            var type = jsonObject["type"].Value<string>();
            resourceBase = type switch
            {
                nameof(Agent) => new Agent(),
                nameof(Annotation) => new Annotation(),
                nameof(AnnotationCollection) => new AnnotationCollection(),
                nameof(AnnotationPage) => new AnnotationPage(),
                nameof(Audio) => new Audio(),
                nameof(Canvas) => new Canvas(),
                nameof(Collection) => new Collection(),
                nameof(Image) => new Image(),
                nameof(Manifest) => new Manifest(),
                nameof(SpecificResource) => new SpecificResource(),
                nameof(TextualBody) => new TextualBody(jsonObject["value"].Value<string>()),
                _ => null
            };

            if (resourceBase != null) return resourceBase;

            if (jsonObject.ContainsKey("motivation"))
            {
                resourceBase = jsonObject["motivation"].Value<string>() switch
                {
                    IIIF.Presentation.V3.Constants.Motivation.Supplementing => new SupplementingDocumentAnnotation(),
                    IIIF.Presentation.V3.Constants.Motivation.Painting => new PaintingAnnotation(),
                    IIIF.Presentation.V3.Constants.Motivation.Classifying => new TypeClassifyingAnnotation(),
                    _ => null
                };
            }

            if (resourceBase == null)
            {
                return new ExternalResource(type);
            }

            return resourceBase;
        }
    }
}