using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2;

public class Range : IIIFPresentationBase
{
    public override string? Type
    {
        get => "sc:Range";
        set => throw new NotImplementedException();
    }

    [JsonProperty(Order = 31, PropertyName = "viewingDirection")]
    public string? ViewingDirection { get; set; }

    [JsonProperty(Order = 31, PropertyName = "startCanvas")]
    public Uri? StartCanvas { get; set; }

    // URIs of ranges
    [JsonProperty(Order = 41, PropertyName = "ranges")]
    public List<string>? Ranges { get; set; }

    // URIs of canvases
    [JsonProperty(Order = 42, PropertyName = "canvases")]
    public List<string>? Canvases { get; set; }
}