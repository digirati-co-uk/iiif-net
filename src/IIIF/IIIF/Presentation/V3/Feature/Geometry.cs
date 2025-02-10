using System.Collections.Generic;

namespace IIIF.Presentation.V3.Feature;

/// <summary>
/// Marker class for a Geometry 
/// </summary>
public abstract class Geometry
{
    [JsonProperty(Order = 1)]
    public virtual string Type { get; set; }
}