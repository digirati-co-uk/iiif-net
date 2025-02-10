using System.Collections.Generic;

namespace IIIF.Presentation.V3.Feature;

public class MultiPolygon : Geometry
{
    public override string Type { get; set; } = "MultiPolygon";

    [JsonProperty(Order = 2)]
    public List<Polygon> Coordinates { get; set; } = null!;
}