using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Strings;
using Newtonsoft.Json;

namespace IIIF.Auth.V2
{
    public class AuthTokenError2 : ResourceBase
    {
        public const string InvalidRequest = "invalidRequest";
        public const string MissingCredentials = "missingCredentials";
        public const string InvalidCredentials = "invalidCredentials";
        public const string ExpiredCredentials = "expiredCredentials";
        public const string InvalidOrigin = "invalidOrigin";
        public const string Unavailable = "unavailable";
        
        public override string Type => nameof(AuthTokenError2);
        
        [JsonProperty(Order = 101, PropertyName = "description")]
        public LanguageMap? Description { get; set; }

        public AuthTokenError2(string profile, LanguageMap description)
        {
            Context = Constants.IIIFAuth2Context;
            Profile = profile;
            Description = description;
        }
        
        public AuthTokenError2()
        { }
    }
}