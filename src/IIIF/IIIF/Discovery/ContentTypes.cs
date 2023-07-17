namespace IIIF.Discovery;

/// <summary>
/// Contains Content-Type/Accepts headers for IIIF Discovery API. 
/// </summary>
public static class ContentTypes
{
    /// <summary>
    /// Content-Type for change discovery v1.
    /// </summary>
    public const string V1 = "application/ld+json;profile=\"" + Context.ChangeDiscovery1Context + "\"";
}