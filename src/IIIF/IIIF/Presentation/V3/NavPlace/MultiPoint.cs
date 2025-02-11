using System.Collections.Generic;

namespace IIIF.Presentation.V3.NavPlace;

public class MultiPoint : Geometry
{
    public override string Type => nameof(MultiPoint);

    /// <summary>
    /// This is an array of positions.
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<Point>? Coordinates { get; set; }
}