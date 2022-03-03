namespace IIIF.Presentation.V3.Content
{
    /// <summary>
    /// Represents a 2 dimensional resource.
    /// </summary>
    public interface ISpatial
    {
        int? Width { get; set; }
        int? Height { get; set; }
    }
}
