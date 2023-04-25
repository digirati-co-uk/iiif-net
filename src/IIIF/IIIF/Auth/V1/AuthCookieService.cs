using System.Collections.Generic;
using IIIF.Presentation.V2;
using IIIF.Presentation.V2.Strings;
using IIIF.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Auth.V1
{
    /// <summary>
    /// Used to obtain a cookie for interacting with content
    /// </summary>
    /// <remarks>https://iiif.io/api/auth/1.0/#access-cookie-service</remarks>
    public class AuthCookieService : ResourceBase, IService
    {
        public const string LoginProfile = "http://iiif.io/api/auth/1/login";
        public const string ClickthroughProfile = "http://iiif.io/api/auth/1/clickthrough";
        public const string KioskProfile = "http://iiif.io/api/auth/1/kiosk";
        public const string ExternalProfile = "http://iiif.io/api/auth/1/external";

        public AuthCookieService(string profile)
        {
            Profile = profile;
        }
        
        [JsonProperty(PropertyName = "@type", Order = 3)]
        public override string? Type { get; set; } = "AuthCookieService1";

        [JsonProperty(Order = 12, PropertyName = "description")]
        public MetaDataValue Description { get; set; }

        [JsonProperty(Order = 26, PropertyName = "service")]
        [ObjectIfSingle]
        public List<IService> Service { get; set; }
        
        [JsonProperty(Order = 103, PropertyName = "confirmLabel")]
        public MetaDataValue? ConfirmLabel { get; set; }

        [JsonProperty(Order = 111, PropertyName = "header")]
        public MetaDataValue? Header { get; set; }

        [JsonProperty(Order = 121, PropertyName = "failureHeader")]
        public MetaDataValue? FailureHeader { get; set; }

        [JsonProperty(Order = 122, PropertyName = "failureDescription")]
        public MetaDataValue? FailureDescription { get; set; }
        
        public static AuthCookieService NewLoginInstance() => new(LoginProfile);

        public static AuthCookieService NewClickthroughInstance() => new(ClickthroughProfile);

        public static AuthCookieService NewKioskInstance() => new(KioskProfile);

        public static AuthCookieService NewExternalInstance() => new(ExternalProfile);
    }
}