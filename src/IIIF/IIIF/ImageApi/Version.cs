using System.ComponentModel.DataAnnotations;
using IIIF.ImageApi.V2;
using IIIF.ImageApi.V3;

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
        [Display(Description = ImageService2.Image2Context)]
        V2,
        
        /// <summary>
        /// IIIF Image API version 3.
        /// </summary>
        [Display(Description = ImageService3.Image3Context)]
        V3
    }
}