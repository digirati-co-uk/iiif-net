using System;
using System.Collections.Generic;
using IIIF.Auth.V2;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Extensions.NavPlace;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation.Helpers;

/// <summary>
/// Common class for deserlialising <see cref="IResource"/> implementations
/// </summary>
/// <typeparam name="T">Type of resource to deserialise to</typeparam>
internal class ResourceDeserialiser<T>
    where T : class, IResource
{
    private readonly Type callingConverterType;

    /// <summary>
    /// Instantiate new instance of <see cref="ResourceDeserialiser{T}"/>
    /// </summary>
    /// <param name="callingConverter">
    /// <see cref="JsonConverter"/> making initial conversion, required as some instances may result in circular
    /// dependency where initial converter is called repeatedly
    /// </param>
    public ResourceDeserialiser(JsonConverter callingConverter)
    {
        callingConverterType =  callingConverter.GetType();
    }
    
    public T? ReadJson(JsonReader reader, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        
        var atTypeValue = jsonObject["@type"]?.Value<string>();

        var result = ToObjectReturn(serializer, atTypeValue, jsonObject);
        if (result != null) return result;

        var service = IdentifyConcreteType(jsonObject, serializer, atTypeValue);

        serializer.Populate(jsonObject.CreateReader(), service);
        return service;
    }
    
    /// <summary>
    /// Used when a serialized class requires using ToObject to use custom converters
    /// </summary>
    private T? ToObjectReturn(JsonSerializer serializer, string? atTypeValue, JObject jsonObject)
    {
        // ImageService2 has a specific ImageService2Converter to handle "profile" prop, if type is ImageService2,
        // copy serializers, minus current, to avoid 
        if (atTypeValue is "iiif:Image" or nameof(ImageService2))
        {
            var copiedSerializer = serializer.CreateCopy(converter => converter.GetType() != callingConverterType);
            var result = jsonObject.ToObject(typeof(ImageService2), copiedSerializer);
            return result as T;
        }

        return null;
    }

    private T? IdentifyConcreteType(JObject jsonObject, JsonSerializer serializer, string? atTypeValue)
    {
        T? service = null;
        var typeValue = jsonObject["type"]?.Value<string>();
        service = atTypeValue switch
        {
            "SearchService1" => new Search.V1.SearchService() as T,
            "AuthLogoutService1" => new Auth.V1.AuthLogoutService() as T,
            "AuthTokenService1" => new Auth.V1.AuthTokenService() as T,
            "AutoCompleteService1" => new Search.V1.AutoCompleteService() as T,
            _ => null,
        };
        if (service != null) return service;

        if (!string.IsNullOrEmpty(typeValue))
        {
            service = typeValue switch
            {
                nameof(ImageService3) => new ImageService3() as T,
                nameof(AuthAccessService2) => new AuthAccessService2() as T,
                nameof(AuthAccessTokenError2) => new AuthAccessTokenError2() as T,
                nameof(AuthAccessTokenService2) => new AuthAccessTokenService2() as T,
                nameof(AuthLogoutService2) => new AuthLogoutService2() as T,
                nameof(AuthProbeService2) => new AuthProbeService2() as T,
                nameof(AnnotationCollection) => new AnnotationCollection() as T,
                nameof(AnnotationPage) => new AnnotationPage() as T,
                nameof(Agent) => new Agent() as T,
                nameof(Annotation) => new Annotation() as T,
                nameof(Sound) => new Sound() as T,
                nameof(Video) => new Video() as T,
                nameof(Image) => new Image() as T,
                nameof(Feature) => new Feature() as T,
                nameof(Canvas) => new Canvas() as T,
                nameof(Collection) => new Collection() as T,
                nameof(Manifest) => new Manifest() as T,
                nameof(SpecificResource) => new SpecificResource() as T,
                nameof(TextualBody) =>  new TextualBody(jsonObject.ContainsKey("value") ? 
                    jsonObject["value"].Value<string>() : string.Empty) as T,
                _ => null
            };
            
            if (service != null) return service;
        }
        
        var profileToken = jsonObject["profile"];
        if (profileToken != null)
        {
            var profile = profileToken.Value<string>();
            service = profile switch
            {
                Auth.V1.AuthLogoutService.AuthLogout1Profile => new Auth.V1.AuthLogoutService() as T,
                Auth.V1.AuthTokenService.AuthToken1Profile => new Auth.V1.AuthTokenService() as T,
                Auth.V0.AuthLogoutService.AuthLogout0Profile => new Auth.V0.AuthLogoutService() as T,
                Auth.V0.AuthTokenService.AuthToken0Profile => new Auth.V0.AuthTokenService() as T,
                Search.V2.AutoCompleteService.AutoComplete2Profile => new Search.V2.AutoCompleteService() as T,
                Search.V1.AutoCompleteService.AutoCompleteService1Profile => new Search.V1.AutoCompleteService() as T,
                Search.V2.SearchService.Search2Profile => new Search.V2.SearchService() as T,
                _ => null
            };
            if (service != null) return service;

            const string auth0 = "http://iiif.io/api/auth/0/";
            const string auth1 = "http://iiif.io/api/auth/1/";

            if (profile.StartsWith(auth0)) return new Auth.V0.AuthCookieService(profile) as T;
            if (profile.StartsWith(auth1)) return new Auth.V1.AuthCookieService(profile) as T;
        }
        
        if (jsonObject.ContainsKey("motivation"))
        {
            var motivation = jsonObject["motivation"]?.Value<string>();
            service = motivation switch
            {
                Presentation.V3.Constants.Motivation.Supplementing => new SupplementingDocumentAnnotation() as T,
                Presentation.V3.Constants.Motivation.Painting => new PaintingAnnotation() as T,
                Presentation.V3.Constants.Motivation.Classifying => new TypeClassifyingAnnotation() as T,
                _ => new GeneralAnnotation(motivation) as T
            };
            
            if (service != null) return service;
        }
        
        // Look for consumer-provided mapping
        if (typeValue != null
            && serializer.Context.Context is IDictionary<string, Func<JObject, T>> customMappings
            && customMappings.TryGetValue(typeValue, out var customMapping))
        {
            service = customMapping(jsonObject) as T;
            if (service != null) return service;
        }

        if (!string.IsNullOrEmpty(atTypeValue))
        {
            // if there's @id and @type only, service reference
            if (jsonObject.Count == 2 && jsonObject["@id"] != null)
                return new V2ServiceReference() as T;
            else
                return new Presentation.V2.ExternalService() as T;
        }

        if (!string.IsNullOrEmpty(typeValue))
        {
            return new Presentation.V3.ExternalService(typeValue) as T;
        }
        
        if (jsonObject.ContainsKey("id"))
        {
            return new ClassifyingBody(jsonObject["id"].Value<string>()) as T;
        }

        return service;
    }
}