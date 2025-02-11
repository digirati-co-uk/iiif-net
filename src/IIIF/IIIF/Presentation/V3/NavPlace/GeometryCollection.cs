using System.Collections.Generic;

namespace IIIF.Presentation.V3.NavPlace;

public class GeometryCollection : Geometry
{
    public override string Type => nameof(GeometryCollection);

    /// <summary>
    /// A list of objects that are geometries
    /// </summary>
    [JsonProperty(Order = 2)]
    public List<Geometry>? Geometries { get; set; }
}