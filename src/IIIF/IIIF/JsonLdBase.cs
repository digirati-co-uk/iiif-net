namespace IIIF;

/// <summary>
/// Base class, serves as root for all IIIF models.
/// </summary>
public abstract class JsonLdBase
{
    [JsonProperty(Order = 1, PropertyName = "@context")]
    public object? Context { get; set; } // This one needs its serialisation name changing...
}