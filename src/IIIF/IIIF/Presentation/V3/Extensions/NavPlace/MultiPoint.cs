using System.Collections.Generic;

namespace IIIF.Presentation.V3.Extensions.NavPlace;

public class MultiPoint : Geometry
{
    public override string Type => nameof(MultiPoint);

    /// <summary>
    /// This is an array of positions.
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<List<double>>? Coordinates { get; set; }
}