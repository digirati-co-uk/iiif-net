using System.Collections.Generic;

namespace IIIF.Presentation.V3.FeatureCollection;

public class Polygon : Geometry
{
    public override string Type => nameof(Polygon);

    /// <summary>
    /// This MUST be an array of linear ring coordinate arrays.
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<LineString>? Coordinates { get; set; }
}