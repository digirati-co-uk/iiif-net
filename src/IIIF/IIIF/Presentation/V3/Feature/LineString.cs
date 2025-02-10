using System.Collections.Generic;

namespace IIIF.Presentation.V3.Feature;

public class LineString : Geometry
{
    public override string Type { get; set; } = "LineString";
    
    [JsonProperty(Order = 2)]
    public List<double> Coordinates { get; set; } = null!;
}