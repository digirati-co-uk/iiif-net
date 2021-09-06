using System.ComponentModel.DataAnnotations;

namespace IIIF.Presentation
{
    /// <summary>
    /// Available IIIF presentation API Versions.
    /// </summary>
    public enum Version
    {
        /// <summary>
        /// Fallback value, unknown version.
        /// </summary>
        [Display(Description = "Unknown")]
        Unknown = 0,
        
        /// <summary>
        /// IIIF Presentation version 2.
        /// </summary>
        [Display(Description = Context.V2)]
        V2,
        
        /// <summary>
        /// IIIF Presentation version 3.
        /// </summary>
        [Display(Description = Context.V3)]
        V3
    }
}