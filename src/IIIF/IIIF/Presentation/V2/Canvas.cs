using System.Collections.Generic;
using IIIF.Presentation.V2.Annotation;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2;

public class Canvas : IIIFPresentationBase
{
    public override string? Type
    {
        get => "sc:Canvas";
        set => throw new System.NotImplementedException();
    }

    [JsonProperty(Order = 35, PropertyName = "height")]
    public int? Height { get; set; }

    [JsonProperty(Order = 36, PropertyName = "width")]
    public int? Width { get; set; }

    // Link to Image resources
    [JsonProperty(Order = 60, PropertyName = "images")]
    public List<ImageAnnotation> Images { get; set; }
}