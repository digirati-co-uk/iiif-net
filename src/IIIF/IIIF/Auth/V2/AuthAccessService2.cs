using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Strings;
using Newtonsoft.Json;

namespace IIIF.Auth.V2
{
    public class AuthAccessService2 : ResourceBase, IService
    {
        public const string InteractiveProfile = "interactive";
        public const string KioskProfile = "kiosk";
        public const string ExternalProfile = "external";
        
        public override string Type => nameof(AuthAccessService2);

        [JsonProperty(Order = 101, PropertyName = "confirmLabel")]
        public LanguageMap? ConfirmLabel { get; set; }

        [JsonProperty(Order = 102, PropertyName = "header")]
        public LanguageMap? Header { get; set; }
        
        [JsonProperty(Order = 103, PropertyName = "description")]
        public LanguageMap? Description { get; set; }
        
        [JsonProperty(Order = 104, PropertyName = "failureHeader")]
        public LanguageMap? FailureHeader { get; set; }
        
        [JsonProperty(Order = 105, PropertyName = "failureDescription")]
        public LanguageMap? FailureDescription { get; set; }
    }
}