using System.Collections.Generic;

namespace IIIF.Presentation.V3.NavPlace;

public class MultiLineString : Geometry
{
    public override string Type => nameof(MultiLineString);

    /// <summary>
    /// This is an array of LineString coordinate arrays.
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<LineString>? Coordinates { get; set; }
}