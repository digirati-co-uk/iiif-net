using System.ComponentModel.DataAnnotations;

namespace IIIF.Presentation;

/// <summary>
/// Available IIIF presentation API Versions.
/// </summary>
public enum Version
{
    /// <summary>
    /// Fallback value, unknown version.
    /// </summary>
    [Display(Description = "Unknown")] Unknown = 0,

    /// <summary>
    /// IIIF Presentation version 2.
    /// </summary>
    [Display(Description = Context.Presentation2Context)]
    V2,

    /// <summary>
    /// IIIF Presentation version 3.
    /// </summary>
    [Display(Description = Context.Presentation3Context)]
    V3
}