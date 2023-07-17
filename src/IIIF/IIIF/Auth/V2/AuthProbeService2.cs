using IIIF.Presentation.V3;
using IIIF.Presentation.V3.Strings;

namespace IIIF.Auth.V2;

public class AuthProbeService2 : ResourceBase
{
    public override string Type => nameof(AuthProbeService2);

    [JsonProperty(Order = 102, PropertyName = "errorHeading")]
    public LanguageMap? ErrorHeading { get; set; }

    [JsonProperty(Order = 103, PropertyName = "errorNote")]
    public LanguageMap? ErrorNote { get; set; }
}