using System.Collections.Generic;
using IIIF.Presentation.V2.Annotation;
using IIIF.Presentation.V2.Strings;
using IIIF.Serialisation;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2;

/// <summary>
/// Base class, used as root to all IIIF v2 Presentation models.
/// </summary>
public abstract class IIIFPresentationBase : ResourceBase
{
    [JsonProperty(Order = 12, PropertyName = "metadata")]
    public List<Metadata>? Metadata { get; set; }

    [JsonProperty(Order = 15, PropertyName = "thumbnail")]
    [ObjectIfSingle]
    public List<Thumbnail>? Thumbnail { get; set; }

    [JsonProperty(Order = 16, PropertyName = "attribution")]
    public MetaDataValue? Attribution { get; set; }

    [JsonProperty(Order = 17, PropertyName = "license")]
    public string? License { get; set; }

    [JsonProperty(Order = 18, PropertyName = "logo")]
    public string? Logo { get; set; }

    [JsonProperty(Order = 24, PropertyName = "rendering")]
    [ObjectIfSingle]
    public List<ExternalResource>? Rendering { get; set; }

    [JsonProperty(Order = 25, PropertyName = "related")]
    [ObjectIfSingle]
    public List<Resource>? Related { get; set; }

    [JsonProperty(Order = 26, PropertyName = "seeAlso")]
    [ObjectIfSingle]
    public List<Resource>? SeeAlso { get; set; }

    [JsonProperty(Order = 27, PropertyName = "service")]
    [ObjectIfSingle]
    public List<IService>? Service { get; set; }

    [JsonProperty(Order = 30, PropertyName = "viewingHint")]
    public string? ViewingHint { get; set; }

    [JsonProperty(Order = 32, PropertyName = "navDate")]
    public string? NavDate { get; set; }

    [JsonProperty(Order = 60, PropertyName = "otherContent")]
    public List<IAnnotationListReference>? OtherContent { get; set; }

    [JsonProperty(Order = 70, PropertyName = "within")]
    public string? Within { get; set; }
}