using System.Collections.Generic;
using System.Linq;

namespace IIIF.Presentation.V2
{
    /// <summary>
    /// A collection of extension methods for working with <see cref="Metadata"/> elements.
    /// </summary>
    public static class MetadataX
    {
        /// <summary>
        /// Get value of metadata with specified label. Expected that there is a Single matching element.
        /// </summary>
        /// <param name="metadata">Metadata items to get item from.</param>
        /// <param name="label">Metadata label.</param>
        /// <returns>Value of metadata with label, or null if not found.</returns>
        public static string? GetValueByLabel(this List<Metadata>? metadata, string label)
        {
            if (metadata == null || metadata.Count == 0) return null;

            var languageValue = metadata.SingleOrDefault(m => m.Label.LanguageValues.Exists(lv => lv.Value == label));
            return languageValue?.Value.LanguageValues.FirstOrDefault()?.Value;
        }
    }
}