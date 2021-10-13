namespace IIIF.Presentation
{
    /// <summary>
    /// Contains Content-Type/Accepts headers for IIIF Presentation API. 
    /// </summary>
    public static class ContentTypes
    {
        /// <summary>
        /// Content-Type for IIIF presentation 2.
        /// </summary>
        public const string V2 = "application/ld+json;profile=\"" + Context.Presentation2Context + "\"";

        /// <summary>
        /// Content-Type for IIIF presentation 3. 
        /// </summary>
        public const string V3 = "application/ld+json;profile=\"" + Context.Presentation3Context + "\"";
    }
}