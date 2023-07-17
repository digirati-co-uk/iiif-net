using System.Collections.Generic;
using IIIF.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2;

public class Resource : ResourceBase
{
    [JsonProperty(Order = 10, PropertyName = "format")]
    public string? Format { get; set; }

    [JsonProperty(Order = 99, PropertyName = "service")]
    [ObjectIfSingle]
    public List<IService>? Service { get; set; }

    public override string? Type { get; set; }
}