using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Strings;

namespace IIIF.Auth.V2;

public class AuthAccessService2 : ResourceBase, IService
{
    public const string InteractiveProfile = "interactive";
    public const string KioskProfile = "kiosk";
    public const string ExternalProfile = "external";

    public override string Type => nameof(AuthAccessService2);

    [JsonProperty(Order = 101, PropertyName = "confirmLabel")]
    public LanguageMap? ConfirmLabel { get; set; }

    [JsonProperty(Order = 102, PropertyName = "heading")]
    public LanguageMap? Heading { get; set; }

    [JsonProperty(Order = 103, PropertyName = "note")]
    public LanguageMap? Note { get; set; }
}