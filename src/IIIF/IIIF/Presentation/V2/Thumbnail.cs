using System.Collections.Generic;
using IIIF.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2;

public class Thumbnail : ResourceBase
{
    public override string? Type { get; set; } = "dctypes:Image";

    [JsonProperty(Order = 28)]
    [ObjectIfSingle]
    public List<IService>? Service { get; set; }
}