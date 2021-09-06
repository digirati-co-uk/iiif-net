namespace IIIF.Presentation.V3
{
    /// <summary>
    /// Extension methods for working with <see cref="Manifest"/> objects.
    /// </summary>
    public static class ManifestX
    {
        /// <summary>
        /// Check if this Manifest contains any AV items. 
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns>True if .Items contains a Canvas with a Duration of greater than 0.</returns>
        public static bool ContainsAV(this Manifest manifest) 
            => manifest.Items?.Exists(i => (i.Duration ?? 0) > 0) ?? false;
    }
}