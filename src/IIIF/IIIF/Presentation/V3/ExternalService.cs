namespace IIIF.Presentation.V3;

/// <summary>
/// Represents a generic, unknown <see cref="IService"/> reference
/// </summary>
public class ExternalService : ResourceBase, IService
{
    public override string Type { get; }

    public ExternalService(string type)
    {
        Type = type;
    }
}