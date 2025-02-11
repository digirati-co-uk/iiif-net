using System;
using IIIF.Auth.V2;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.FeatureCollection;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation;

/// <summary>
/// JsonConverter to handle reading <see cref="IResource"/> objects to concrete type.
/// </summary>
public class ResourceConverter : ReadOnlyConverter<IResource>
{
    public override IResource? ReadJson(JsonReader reader, Type objectType, IResource? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var jsonObject = JObject.Load(reader);

        var service = IdentifyConcreteType(jsonObject);

        serializer.Populate(jsonObject.CreateReader(), service);
        return service;
    }

    private static IResource? IdentifyConcreteType(JObject jsonObject)
    {
        IResource? service = null;
        var atType = jsonObject["@type"];
        if (atType != null)
            service = atType.Value<string>() switch
            {
                "SearchService1" => new Search.V1.SearchService(),
                "AuthLogoutService1" => new Auth.V1.AuthLogoutService(),
                "AuthTokenService1" => new Auth.V1.AuthTokenService(),
                "AutoCompleteService1" => new Search.V1.AutoCompleteService(),
                nameof(ImageService2) => new ImageService2(),
                _ => null
            };

        if (service == null)
        {
            var type = jsonObject["type"];
            if (type != null)
                service = type.Value<string>() switch
                {
                    nameof(ImageService3) => new ImageService3(),
                    nameof(AuthAccessService2) => new AuthAccessService2(),
                    nameof(AuthAccessTokenError2) => new AuthAccessTokenError2(),
                    nameof(AuthAccessTokenService2) => new AuthAccessTokenService2(),
                    nameof(AuthLogoutService2) => new AuthLogoutService2(),
                    nameof(AuthProbeService2) => new AuthProbeService2(),
                    nameof(Sound) => new Sound(),
                    nameof(Video) => new Video(),
                    nameof(Image) => new Image(),
                    nameof(Feature) => new Feature(),
                    _ => null
                };
        }

        if (service == null)
        {
            var profile = jsonObject["profile"].Value<string>();
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

            if (service == null)
            {
                const string auth0 = "http://iiif.io/api/auth/0/";
                const string auth1 = "http://iiif.io/api/auth/1/";

                if (profile.StartsWith(auth0))
                    service = new Auth.V0.AuthCookieService(profile);
                else if (profile.StartsWith(auth1)) service = new Auth.V1.AuthCookieService(profile);
            }
        }

        // TODO handle ResourceBase items

        if (service == null) service = new V2ServiceReference();

        return service;
    }
}