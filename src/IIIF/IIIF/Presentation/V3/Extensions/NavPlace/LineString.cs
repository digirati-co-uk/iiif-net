using System.Collections.Generic;

namespace IIIF.Presentation.V3.Extensions.NavPlace;

public class LineString : Geometry
{
    public override string Type => nameof(LineString);
    
    /// <summary>
    /// This is an array of two or more positions. Each position is [lon, lat, alt?].
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<List<double>>? Coordinates { get; set; }
}