using System.Collections.Generic;

namespace IIIF.Presentation.V3.Feature;

public class GeometryCollection : Geometry
{
    public override string Type { get; set; } = "GeometryCollection";

    [JsonProperty(Order = 2)]
    public List<Geometry> Geometries { get; set; } = null!;
}