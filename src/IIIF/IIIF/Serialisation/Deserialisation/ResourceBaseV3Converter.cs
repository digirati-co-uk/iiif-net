using System;
using System.Collections.Generic;
using IIIF.Auth.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
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
        var jsonObject = JObject.Load(reader);

        var resourceBase = IdentifyConcreteType(jsonObject, serializer);

        serializer.Populate(jsonObject.CreateReader(), resourceBase);
        return resourceBase;
    }

    private static ResourceBase? IdentifyConcreteType(JObject jsonObject, JsonSerializer serializer)
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
            nameof(ImageService3) => new ImageService3(),
            nameof(Agent) => new Agent(),
            nameof(Annotation) => new Annotation(),
            nameof(AnnotationCollection) => new AnnotationCollection(),
            nameof(AnnotationPage) => new AnnotationPage(),
            nameof(Sound) => new Sound(),
            nameof(Canvas) => new Canvas(),
            nameof(Collection) => new Collection(),
            nameof(Image) => new Image(),
            nameof(Manifest) => new Manifest(),
            nameof(SpecificResource) => new SpecificResource(),
            nameof(AuthProbeService2) => new AuthProbeService2(),
            nameof(AuthAccessTokenError2) => new AuthAccessTokenError2(),
            nameof(TextualBody) => new TextualBody(jsonObject["value"].Value<string>()),
            _ => null
        };
        
        if (resourceBase != null) return resourceBase;

        if (jsonObject.ContainsKey("motivation"))
        {
            var motivation = jsonObject["motivation"].Value<string>();
            resourceBase = motivation switch
            {
                Presentation.V3.Constants.Motivation.Supplementing => new SupplementingDocumentAnnotation(),
                Presentation.V3.Constants.Motivation.Painting => new PaintingAnnotation(),
                Presentation.V3.Constants.Motivation.Classifying => new TypeClassifyingAnnotation(),
                _ => new GeneralAnnotation(motivation)
            };
        }
        
        // Look for consumer-provided mapping
        if (type != null
            && serializer.Context.Context is IDictionary<string, Func<JObject, ResourceBase>> customMappings
            && customMappings.TryGetValue(type, out var customMapping))
        {
            resourceBase = customMapping(jsonObject);
        }

        if (resourceBase == null) return new ExternalResource(type);

        return resourceBase;
    }
}