using System.Collections.Generic;
using IIIF.Presentation.V3.Content;
using IIIF.Presentation.V3.Strings;

namespace IIIF.Presentation.V3;

/// <summary>
/// Base class for all IIIF presentation resources. 
/// </summary>
public abstract class ResourceBase : JsonLdBase, IResource
{
    /// <summary>
    /// The URI that identifies the resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#id">id</a>
    /// </summary>
    [JsonProperty(Order = 2)]
    public string? Id { get; set; }

    /// <summary>
    /// The type or class of the resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#type">type</a>
    /// </summary>
    [JsonProperty(Order = 3)]
    public abstract string Type { get; }

    /// <summary>
    /// A schema or named set of functionality available from the resource.
    /// The profile can further clarify the type and/or format of an external resource or service,
    /// allowing clients to customize their handling of the resource that has the profile property.
    /// See <a href="https://iiif.io/api/presentation/3.0/#profile">profile</a>
    /// </summary>
    [JsonProperty(Order = 4)]
    public string? Profile { get; set; }

    /// <summary>
    /// A human readable label, name or title.
    /// See <a href="https://iiif.io/api/presentation/3.0/#label">Label</a>
    /// </summary>
    [JsonProperty(Order = 10)]
    public LanguageMap? Label { get; set; }

    /// <summary>
    /// A short textual summary intended to be conveyed to the user when the metadata entries for the resource are
    /// not being displayed.
    /// See <a href="https://iiif.io/api/presentation/3.0/#summary">summary</a>
    /// </summary>
    [JsonProperty(Order = 15)]
    public LanguageMap? Summary { get; set; }

    /// <summary>
    /// A content resource that represents the resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#thumbnail">thumbnail</a>
    /// </summary>
    [JsonProperty(Order = 16)]
    public List<ExternalResource>? Thumbnail { get; set; }

    /// <summary>
    /// A web page that is about the object represented by this resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#homepage">homepage</a>
    /// </summary>
    [JsonProperty(Order = 17)]
    public List<ExternalResource>? Homepage { get; set; }

    /// <summary>
    /// An ordered list of descriptions to be displayed to the user when they interact with the resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#metadata">metadata</a>
    /// </summary>
    [JsonProperty(Order = 18)]
    public List<LabelValuePair>? Metadata { get; set; }

    /// <summary>
    /// A string that identifies a license or rights statement that applies to the content of the resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#rights">rights</a>
    /// </summary>
    [JsonProperty(Order = 19)]
    public string? Rights { get; set; }

    /// <summary>
    /// Text that must be displayed when the resource is displayed or used.
    /// See <a href="https://iiif.io/api/presentation/3.0/#requiredstatement">requiredstatement</a>
    /// </summary>
    [JsonProperty(Order = 20)]
    public LabelValuePair? RequiredStatement { get; set; }

    /// <summary>
    /// An organization or person that contributed to providing the content of the resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#provider">provider</a>
    /// </summary>
    [JsonProperty(Order = 21)]
    public List<Agent>? Provider { get; set; }

    /// <summary>
    /// A resource that is an alternative, non-IIIF representation of the resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#rendering">rendering</a>
    /// </summary>
    [JsonProperty(Order = 22)]
    public List<ExternalResource>? Rendering { get; set; }

    /// <summary>
    /// A machine-readable resource such as an XML or RDF description that is related to the current resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#seealso">seealso</a>
    /// </summary>
    [JsonProperty(Order = 24)]
    public List<ExternalResource>? SeeAlso { get; set; }

    [JsonProperty(Order = 28)] public List<IService>? Service { get; set; }

    /// <summary>
    /// A set of user experience features that the publisher of the content would prefer the client to use when
    /// presenting the resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#behavior">behavior</a>
    /// </summary>
    [JsonProperty(Order = 31)]
    public List<string>? Behavior { get; set; }
    
    /// <summary>
    /// A resource used to hold data on a geographic location.  This follows the GeoJSON spec.
    /// See <a href="https://iiif.io/api/extension/navplace/">navplace</a>
    /// </summary>
    [JsonProperty(Order = 31)]
    public NavPlace.NavPlace? NavPlace { get; set; }

    /// <summary>
    /// A containing resource that includes this resource.
    /// See <a href="https://iiif.io/api/presentation/3.0/#partof">partof</a>
    /// </summary>
    [JsonProperty(Order = 1000)]
    public List<ResourceBase>? PartOf { get; set; }
}
