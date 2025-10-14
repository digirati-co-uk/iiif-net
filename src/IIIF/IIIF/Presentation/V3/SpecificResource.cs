using System.Collections.Generic;
using IIIF.Presentation.V3.Annotation;
using IIIF.Presentation.V3.Selectors;
using IIIF.Serialisation;

namespace IIIF.Presentation.V3;

public class SpecificResource : ResourceBase, IStructuralLocation, IPaintable
{
    public override string Type => nameof(SpecificResource);

    [JsonConverter(typeof(SourceConverter))]
    [JsonProperty(Order = 101)]
    public IPaintable Source { get; set; }

    [JsonProperty(Order = 102)] 
    [ObjectIfSingle]
    public List<ISelector> Selector { get; set; }
}