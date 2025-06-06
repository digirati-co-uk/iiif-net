using System.Collections.Generic;

namespace IIIF.Presentation.V3.Extensions.NavPlace;

public class Feature : IResource
{
    [JsonProperty(Order = 1)]
    public string? Id { get; set; }

    [JsonProperty(Order = 2)] 
    public string Type => nameof(Feature);
    
    /// <summary>
    /// Used to supply additional information associated with the geographic coordinates.
    /// <notes>Can be any JSON object</notes>
    /// </summary>
    [JsonProperty(Order = 3)]
    public Dictionary<string, object>? Properties { get; set; }
    
    /// <summary>
    /// This is a GeoJSON object
    /// </summary>
    [JsonProperty(Order = 4)]
    public Geometry? Geometry { get; set; }
}