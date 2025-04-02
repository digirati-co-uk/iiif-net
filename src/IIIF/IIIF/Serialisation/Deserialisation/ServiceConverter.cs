using System;
using IIIF.Auth.V2;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation;

/// <summary>
/// JsonConverter to handle reading <see cref="IService"/> objects to concrete type.
/// </summary>
public class ServiceConverter : ReadOnlyConverter<IService>
{
    public override IService? ReadJson(JsonReader reader, Type objectType, IService? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);
        
        var atType = jsonObject["@type"];

        var result = ImageServiceTwoResult(serializer, atType, jsonObject);
        if (result != null) return result;

        var service = IdentifyConcreteType(jsonObject, atType);

        serializer.Populate(jsonObject.CreateReader(), service);
        return service;
    }

    private IService? ImageServiceTwoResult(JsonSerializer serializer, JToken? atType, JObject jsonObject)
    {
        var typeValue = atType?.Value<string>();
        if (typeValue is "iiif:Image" or nameof(ImageService2))
        {
            // avoids stack overflow exceptions from recursively calling the service converter
            var thisIndex = serializer.Converters.IndexOf(this);
            serializer.Converters.RemoveAt(thisIndex);
            
            var result = jsonObject.ToObject(typeof(ImageService2), serializer);
            return result as IService;
        }

        return null;
    }

    private static IService? IdentifyConcreteType(JObject jsonObject, JToken? atType)
    {
        IService? service = null;
        var type = jsonObject["type"];
        if (atType != null)
            service = atType.Value<string>() switch
            {
                "SearchService1" => new Search.V1.SearchService(),
                "AuthLogoutService1" => new Auth.V1.AuthLogoutService(),
                "AuthTokenService1" => new Auth.V1.AuthTokenService(),
                "AutoCompleteService1" => new Search.V1.AutoCompleteService(),
                _ => null,
            };
        if (service != null) return service;

        if (type != null)
            service = type.Value<string>() switch
            {
                nameof(ImageService3) => new ImageService3(),
                nameof(AuthAccessService2) => new AuthAccessService2(),
                nameof(AuthAccessTokenService2) => new AuthAccessTokenService2(),
                nameof(AuthLogoutService2) => new AuthLogoutService2(),
                nameof(AuthProbeService2) => new AuthProbeService2(),
                _ => null
            };
        if (service != null) return service;

        var profileToken = jsonObject["profile"];
        if (profileToken != null)
        {
            var profile = profileToken.Value<string>();
            service = profile switch
            {
                Auth.V1.AuthLogoutService.AuthLogout1Profile => new Auth.V1.AuthLogoutService(),
                Auth.V1.AuthTokenService.AuthToken1Profile => new Auth.V1.AuthTokenService(),
                Auth.V0.AuthLogoutService.AuthLogout0Profile => new Auth.V0.AuthLogoutService(),
                Auth.V0.AuthTokenService.AuthToken0Profile => new Auth.V0.AuthTokenService(),
                Search.V2.AutoCompleteService.AutoComplete2Profile => new Search.V2.AutoCompleteService(),
                Search.V1.AutoCompleteService.AutoCompleteService1Profile => new Search.V1.AutoCompleteService(),
                Search.V2.SearchService.Search2Profile => new Search.V2.SearchService(),
                _ => null
            };
            if (service != null) return service;

            const string auth0 = "http://iiif.io/api/auth/0/";
            const string auth1 = "http://iiif.io/api/auth/1/";

            if (profile.StartsWith(auth0)) return new Auth.V0.AuthCookieService(profile);
            if (profile.StartsWith(auth1)) return new Auth.V1.AuthCookieService(profile);
        }

        // TODO handle ResourceBase items

        if (atType != null)
        {
            // if there's @id and @type only, service reference
            if (jsonObject.Count == 2 && jsonObject["@id"] != null)
                return new V2ServiceReference();
            else
                return new Presentation.V2.ExternalService();
        }

        if (type != null)
        {
            return new Presentation.V3.ExternalService(type.Value<string>());
        }

        return service;
    }
}