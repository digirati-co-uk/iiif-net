using System.Collections.Generic;
using Newtonsoft.Json;

namespace IIIF.Presentation.V2;

/// <summary>
/// Collections are used to list the manifests available for viewing, and to describe the structures, hierarchies
/// or curated collections that the physical objects are part of.
/// </summary>
/// <remarks>See: https://iiif.io/api/presentation/2.1/#collection</remarks>
public class Collection : IIIFPresentationBase
{
    public override string? Type
    {
        get => "sc:Collection";
        set => throw new System.NotImplementedException();
    }

    [JsonProperty(Order = 100, PropertyName = "collections")]
    public List<Collection> Collections { get; set; }

    [JsonProperty(Order = 101, PropertyName = "manifests")]
    public List<Manifest> Manifests { get; set; }

    [JsonProperty(Order = 111, PropertyName = "members")]
    public List<IIIFPresentationBase> Members { get; set; }
}