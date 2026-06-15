using IIIF.Presentation.V3;

namespace IIIF.Search.V2;

public class TextQuoteSelector : ResourceBase
{
    public override string Type => nameof(TextQuoteSelector);
    
    [JsonProperty(Order = 51, PropertyName = "exact")]
    public string Exact { get; set; }

    [JsonProperty(Order = 52, PropertyName = "prefix")]
    public string? Prefix { get; set; }

    [JsonProperty(Order = 53, PropertyName = "suffix")]
    public string? Suffix { get; set; }
}