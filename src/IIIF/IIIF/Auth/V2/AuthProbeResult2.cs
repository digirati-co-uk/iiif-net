using System.Collections.Generic;
using IIIF.Presentation.V3.Strings;

namespace IIIF.Auth.V2;

public class AuthProbeResult2 : JsonLdBase
{
    [JsonProperty(Order = 1, PropertyName = "@context")]
    public new string Context = Constants.IIIFAuth2Context;

    [JsonProperty(Order = 1, PropertyName = "type")]
    public string Type => nameof(AuthProbeResult2);

    [JsonProperty(Order = 10, PropertyName = "status")]
    public int Status { get; set; }

    [JsonProperty(Order = 20, PropertyName = "substitute")]
    public List<IResource>? Substitute { get; set; }

    [JsonProperty(Order = 40, PropertyName = "location")]
    public IResource? Location { get; set; }

    [JsonProperty(Order = 50, PropertyName = "heading")]
    public LanguageMap? Heading { get; set; }

    [JsonProperty(Order = 60, PropertyName = "note")]
    public LanguageMap? Note { get; set; }
}