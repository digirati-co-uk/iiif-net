using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Strings;

namespace IIIF.Auth.V2;

public class AuthAccessService2 : ResourceBase, IService
{
    public const string ActiveProfile = "active";
    public const string KioskProfile = "kiosk";
    public const string ExternalProfile = "external";

    public override string Type => nameof(AuthAccessService2);

    /// <summary>
    /// The label for the user interface element that opens the access service.
    /// </summary>
    [JsonProperty(Order = 101, PropertyName = "confirmLabel")]
    public LanguageMap? ConfirmLabel { get; set; }

    /// <summary>
    /// Heading text to be shown with the user interface element that opens the access service.
    /// </summary>
    [JsonProperty(Order = 102, PropertyName = "heading")]
    public LanguageMap? Heading { get; set; }

    /// <summary>
    /// Additional text to be shown with the user interface element that opens the access service.
    /// </summary>
    [JsonProperty(Order = 103, PropertyName = "note")]
    public LanguageMap? Note { get; set; }
}