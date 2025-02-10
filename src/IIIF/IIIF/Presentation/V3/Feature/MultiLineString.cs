using System.Collections.Generic;

namespace IIIF.Presentation.V3.Feature;

public class MultiLineString : Geometry
{
    public override string Type { get; set; } = "MultiLineString";

    [JsonProperty(Order = 2)]
    public List<LineString> Coordinates { get; set; } = null!;
}