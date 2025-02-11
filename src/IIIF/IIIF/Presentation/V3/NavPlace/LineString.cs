using System.Collections.Generic;

namespace IIIF.Presentation.V3.NavPlace;

public class LineString : Geometry
{
    public override string Type => nameof(LineString);
    
    /// <summary>
    /// This is an array of two or more positions.
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<double>? Coordinates { get; set; }
}