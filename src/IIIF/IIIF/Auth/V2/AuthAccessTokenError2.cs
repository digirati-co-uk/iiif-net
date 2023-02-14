using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Strings;
using Newtonsoft.Json;

namespace IIIF.Auth.V2
{
    public class AuthAccessTokenError2 : ResourceBase
    {
        public const string InvalidRequest = "invalidRequest";
        public const string InvalidOrigin = "invalidOrigin";
        public const string MissingAspect = "missingAspect";
        public const string InvalidAspect = "invalidAspect";
        public const string ExpiredAspect = "expiredAspect";
        public const string Unavailable = "unavailable";
        
        public override string Type => nameof(AuthAccessTokenError2);
        
        [JsonProperty(Order = 101, PropertyName = "heading")]
        public LanguageMap? Heading { get; set; }
        
        [JsonProperty(Order = 102, PropertyName = "note")]
        public LanguageMap? Note { get; set; }

        public AuthAccessTokenError2(string profile, LanguageMap note)
        {
            Context = Constants.IIIFAuth2Context;
            Profile = profile;
            Note = note;
        }
        
        public AuthAccessTokenError2()
        { }
    }
}