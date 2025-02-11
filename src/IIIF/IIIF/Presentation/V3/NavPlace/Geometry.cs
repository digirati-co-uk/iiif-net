namespace IIIF.Presentation.V3.NavPlace;

/// <summary>
/// Base class for a Geometry 
/// </summary>
public abstract class Geometry
{
    [JsonProperty(Order = 1)] 
    public virtual string Type { get; init; } = null!;
}