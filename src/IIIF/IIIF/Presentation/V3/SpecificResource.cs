using IIIF.Presentation.V3.Selectors;
using Newtonsoft.Json;

namespace IIIF.Presentation.V3;

public class SpecificResource : ResourceBase, IStructuralLocation
{
    public override string Type => nameof(SpecificResource);

    [JsonProperty(Order = 101)] public string Source { get; set; }

    [JsonProperty(Order = 102)] public ISelector Selector { get; set; }
}