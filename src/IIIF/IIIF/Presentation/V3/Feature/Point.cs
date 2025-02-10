using System.Collections.Generic;

namespace IIIF.Presentation.V3.Feature;

public class Point : Geometry
{
    public override string Type { get; set; } = "Point";

    [JsonProperty(Order = 2)]
    public List<double> Coordinates { get; set; } = null!;
}