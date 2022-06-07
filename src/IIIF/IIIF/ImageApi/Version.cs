using System.ComponentModel.DataAnnotations;

namespace IIIF.ImageApi
{
    /// <summary>
    /// Available IIIF Image API Versions.
    /// </summary>
    public enum Version
    {
        /// <summary>
        /// Fallback value, unknown version.
        /// </summary>
        [Display(Description = "Unknown")]
        Unknown = 0,
        
        /// <summary>
        /// IIIF Image API version 2.
        /// </summary>
        [Display(Description = Service.ImageService2.Image2Context)]
        V2,
        
        /// <summary>
        /// IIIF Image API version 3.
        /// </summary>
        [Display(Description = Service.ImageService3.Image3Context)]
        V3
    }
}