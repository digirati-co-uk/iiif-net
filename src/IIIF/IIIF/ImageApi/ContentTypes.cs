using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;

namespace IIIF.ImageApi;

/// <summary>
/// Contains Content-Type/Accepts headers for IIIF Image API. 
/// </summary>
public static class ContentTypes
{
    /// <summary>
    /// Content-Type for IIIF Image 2.
    /// </summary>
    public const string V2 = "application/ld+json;profile=\"" + ImageService2.Image2Context + "\"";

    /// <summary>
    /// Content-Type for IIIF Image 3. 
    /// </summary>
    public const string V3 = "application/ld+json;profile=\"" + ImageService3.Image3Context + "\"";
}