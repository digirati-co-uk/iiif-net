using System.Collections.Generic;

namespace IIIF.Presentation.V3.NavPlace;

public class Point : Geometry
{
    public override string Type => nameof(Point);

    /// <summary>
    /// This is a single position.
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<double>? Coordinates { get; set; }
}