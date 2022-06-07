namespace IIIF.ImageApi
{
    /// <summary>
    /// Contains Content-Type/Accepts headers for IIIF Image API. 
    /// </summary>
    public static class ContentTypes
    {
        /// <summary>
        /// Content-Type for IIIF Image 2.
        /// </summary>
        public const string V2 = "application/ld+json;profile=\"" + Service.ImageService2.Image2Context + "\"";

        /// <summary>
        /// Content-Type for IIIF Image 3. 
        /// </summary>
        public const string V3 = "application/ld+json;profile=\"" + Service.ImageService3.Image3Context + "\"";
    }
}