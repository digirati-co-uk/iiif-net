using System.Collections.Generic;

namespace IIIF.Presentation.V3.Extensions.NavPlace;

public class MultiPolygon : Geometry
{
    public override string Type => nameof(MultiPolygon);

    /// <summary>
    /// This is an array of Polygon coordinate arrays. Each Polygon is an array of linear rings;
    /// each ring is an array of positions [lon, lat, alt?].
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<List<List<List<double>>>>? Coordinates { get; set; }
}