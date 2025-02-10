using System.Collections.Generic;

namespace IIIF.Presentation.V3.Feature;

public class Polygon : Geometry
{
    public override string Type { get; set; } = "Polygon";

    [JsonProperty(Order = 2)]
    public List<LineString> Coordinates { get; set; } = null!;
}