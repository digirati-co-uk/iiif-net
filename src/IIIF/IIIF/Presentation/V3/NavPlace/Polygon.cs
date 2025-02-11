using System.Collections.Generic;

namespace IIIF.Presentation.V3.NavPlace;

public class Polygon : Geometry
{
    public override string Type => nameof(Polygon);

    /// <summary>
    /// This MUST be an array of linear ring coordinate arrays.
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<List<double>>? Coordinates { get; set; }
}