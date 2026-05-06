using System.Collections.Generic;

namespace IIIF.Presentation.V3.Extensions.NavPlace;

public class Polygon : Geometry
{
    public override string Type => nameof(Polygon);

    /// <summary>
    /// Array of linear ring coordinate arrays. First ring is the exterior boundary;
    /// subsequent rings are interior rings (holes). Each ring is an array of positions [lon, lat, alt?].
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<List<List<double>>>? Coordinates { get; set; }
}