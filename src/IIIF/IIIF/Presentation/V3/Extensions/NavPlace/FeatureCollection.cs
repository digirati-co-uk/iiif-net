using System.Collections.Generic;

namespace IIIF.Presentation.V3.Extensions.NavPlace;

public class FeatureCollection : JsonLdBase
{
    public const string NavPlaceContext = "http://iiif.io/api/extension/navplace/context.json";
    
    [JsonProperty(Order = 1)]
    public string Id { get; set; } = null!;
    
    [JsonProperty(Order = 2)]
    public string Type => nameof(FeatureCollection);
    
    /// <summary>
    /// Represents a spatially bounded area.
    /// </summary>
    [JsonProperty(Order = 3)]
    public List<Feature>? Features { get; set; }
}