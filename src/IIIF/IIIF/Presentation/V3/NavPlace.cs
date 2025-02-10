using System.Collections.Generic;

namespace IIIF.Presentation.V3;

public class NavPlace : StructureBase
{
    public override string Type { get; } = "FeatureCollection";
    
    public List<Feature.Feature> Features { get; set; }
}