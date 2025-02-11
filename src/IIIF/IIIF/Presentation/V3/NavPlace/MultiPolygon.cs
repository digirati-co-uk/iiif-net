using System.Collections.Generic;

namespace IIIF.Presentation.V3.NavPlace;

public class MultiPolygon : Geometry
{
    public override string Type => nameof(MultiPolygon);

    /// <summary>
    /// This is an array of Polygon coordinate arrays.
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<Polygon>? Coordinates { get; set; }
}