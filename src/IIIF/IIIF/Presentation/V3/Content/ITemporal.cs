namespace IIIF.Presentation.V3.Content;

/// <summary>
/// Represents a time-based resource.
/// </summary>
public interface ITemporal
{
    public double? Duration { get; set; }
}