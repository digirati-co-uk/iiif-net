using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IIIF.Serialisation.Deserialisation
{
    /// <summary>
    /// JsonConverter to handle reading <see cref="IService"/> objects to concrete type.
    /// </summary>
    public class ServiceConverter : ReadOnlyConverter<IService>
    {
        public override IService? ReadJson(JsonReader reader, Type objectType, IService? existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            var service = IdentifyConcreteType(jsonObject);

            serializer.Populate(jsonObject.CreateReader(), service);
            return service;
        }

        private static IService? IdentifyConcreteType(JObject jsonObject)
        {
            var profile = jsonObject["profile"].Value<string>();
            IService? service = profile switch
            {
                Auth.V1.AuthLogoutService.AuthLogout1Profile => new Auth.V1.AuthLogoutService(),
                Auth.V1.AuthTokenService.AuthToken1Profile => new Auth.V1.AuthTokenService(),
                Search.V2.AutoCompleteService.AutoComplete2Profile => new Search.V2.AutoCompleteService(),
                Search.V1.AutoCompleteService.AutoCompleteService1Profile => new Search.V1.AutoCompleteService(),
                Auth.V1.AuthCookieService.LoginProfile => new Auth.V1.AuthCookieService(profile),
                Auth.V1.AuthCookieService.ClickthroughProfile => new Auth.V1.AuthCookieService(profile),
                Auth.V1.AuthCookieService.KioskProfile => new Auth.V1.AuthCookieService(profile),
                Auth.V1.AuthCookieService.ExternalProfile => new Auth.V1.AuthCookieService(profile),
                Search.V2.SearchService.Search2Profile => new Search.V2.SearchService(),
                _ => null
            };

            if (service == null)
            {
                var atType = jsonObject["@type"];
                if (atType != null)
                {
                    service = atType.Value<string>() switch
                    {
                        "SearchService1" => new Search.V1.SearchService(),
                        nameof(ImageApi.Service.ImageService2) => new ImageApi.Service.ImageService2(),
                        _ => null
                    };
                }
            }
            
            if (service == null)
            {
                var type = jsonObject["type"];
                if (type != null)
                {
                    service = type.Value<string>() switch
                    {
                        nameof(ImageApi.Service.ImageService3) => new ImageApi.Service.ImageService3(),
                        _ => null
                    };
                }
            }

            // TODO handle ResourceBase items?

            if (service == null)
            {
                service = new V2ServiceReference();
            }

            return service;
        }
    }
}