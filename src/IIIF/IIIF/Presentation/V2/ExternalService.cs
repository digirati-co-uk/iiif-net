namespace IIIF.Presentation.V2;

/// <summary>
/// Represents a generic, unknown <see cref="IService"/> reference
/// </summary>
public class ExternalService : ResourceBase, IService
{
    [JsonProperty(PropertyName = "@type", Order = 3)]
    public override string? Type { get; set; }
}