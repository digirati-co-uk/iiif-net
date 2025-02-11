using System.Collections.Generic;

namespace IIIF.Presentation.V3.NavPlace;

public class NavPlace
{
    [JsonProperty(Order = 1)]
    public string Id { get; set; } = null!;
    
    [JsonProperty(Order = 2)]
    public string Type => "FeatureCollection";
    
    /// <summary>
    /// Represents a spatially bounded area.
    /// </summary>
    [JsonProperty(Order = 3)]
    public List<Feature>? Features { get; set; }
}