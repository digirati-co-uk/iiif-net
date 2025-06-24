using System;
using IIIF.Auth.V2;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation.Helpers;

/// <summary>
/// Common class for deserlialising <see cref="IResource"/> implementations
/// </summary>
/// <typeparam name="T">Type of resource to deserialise to</typeparam>
internal class ResourceDeserialiser<T>
    where T : class, IResource
{
    private readonly Func<string, T?>? additionalTypeBasedConverter;
    private readonly Type callingConverterType;

    /// <summary>
    /// Instantiate new instance of <see cref="ResourceDeserialiser{T}"/>
    /// </summary>
    /// <param name="callingConverter">
    /// <see cref="JsonConverter"/> making initial conversion, required as some instances may result in circular
    /// dependency where initial converter is called repeatedly
    /// </param>
    /// <param name="additionalTypeBasedConverter">
    /// Additional "type" based converters, required for types that inherit from <see cref="IResource"/>
    /// </param>
    public ResourceDeserialiser(JsonConverter callingConverter, Func<string, T?>? additionalTypeBasedConverter = null)
    {
        this.additionalTypeBasedConverter = additionalTypeBasedConverter;
        callingConverterType =  callingConverter.GetType();
    }
    
    public T? ReadJson(JsonReader reader, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        
        var atTypeValue = jsonObject["@type"]?.Value<string>();

        var result = ToObjectReturn(serializer, atTypeValue, jsonObject);
        if (result != null) return result;

        var service = IdentifyConcreteType(jsonObject, atTypeValue);

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

    private T? IdentifyConcreteType(JObject jsonObject, string? atTypeValue)
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
                _ => null
            };
            if (service != null) return service;

            service = additionalTypeBasedConverter?.Invoke(typeValue);
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

        // TODO handle ResourceBase items

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

        return service;
    }
}