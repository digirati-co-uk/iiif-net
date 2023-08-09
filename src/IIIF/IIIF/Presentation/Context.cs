namespace IIIF.Presentation;

/// <summary>
/// Contains JSON-LD Contexts for IIIF Presentation API.
/// </summary>
public static class Context
{
    /// <summary>
    /// JSON-LD context for IIIF presentation 2.
    /// </summary>
    public const string Presentation2Context = "http://iiif.io/api/presentation/2/context.json";

    /// <summary>
    /// JSON-LD context for IIIF presentation 3. 
    /// </summary>
    public const string Presentation3Context = "http://iiif.io/api/presentation/3/context.json";

    public static void EnsurePresentation3Context(this JsonLdBase resource)
    {
        resource.EnsureContext(Presentation3Context);
    }

    public static void EnsurePresentation2Context(this JsonLdBase resource)
    {
        resource.EnsureContext(Presentation2Context);
    }
}