using System.Collections.Generic;

namespace IIIF.Presentation.V3.Feature;

public class MultiPoint : Geometry
{
    public override string Type { get; set; } = "MultiPoint";

    [JsonProperty(Order = 2)]
    public List<Point> Coordinates { get; set; } = null!;
}