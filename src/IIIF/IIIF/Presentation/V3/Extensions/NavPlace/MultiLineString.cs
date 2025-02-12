using System.Collections.Generic;

namespace IIIF.Presentation.V3.Extensions.NavPlace;

public class MultiLineString : Geometry
{
    public override string Type => nameof(MultiLineString);

    /// <summary>
    /// This is an array of LineString coordinate arrays.
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<List<double>>? Coordinates { get; set; }
}