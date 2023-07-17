using System.Collections.Generic;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2;

public class Sequence : IIIFPresentationBase
{
    public override string? Type
    {
        get => "sc:Sequence";
        set => throw new System.NotImplementedException();
    }

    [JsonProperty(Order = 31, PropertyName = "startCanvas")]
    public string StartCanvas { get; set; }

    [JsonProperty(Order = 31, PropertyName = "viewingDirection")]
    public string ViewingDirection { get; set; }

    [JsonProperty(Order = 50, PropertyName = "canvases")]
    public List<Canvas> Canvases { get; set; }
}