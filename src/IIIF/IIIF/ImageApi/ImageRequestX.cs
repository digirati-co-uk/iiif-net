namespace IIIF.ImageApi;

/// <summary>
/// Extension methods for dealing with ImageRequests
/// </summary>
public static class ImageRequestX
{
    /// <summary>
    /// Resize the original object in accordance with size parameters.
    /// This method supports upsizing and always allows upscaling.
    /// </summary>
    /// <param name="sizeParameter">Current <see cref="SizeParameter"/> object</param>
    /// <param name="requestSize">
    /// <see cref="Size"/> of requested resource - this can be original image for /full/ requests or size of tile
    /// for tile requests etc
    /// </param>
    /// <returns></returns>
    public static Size GetResultingSize(this SizeParameter sizeParameter, Size requestSize)
    {
        if (sizeParameter.Max) return requestSize;

        if (sizeParameter.PercentScale.HasValue)
            return Size.ResizePercent(requestSize, sizeParameter.PercentScale.Value);

        if (sizeParameter.Width.HasValue && sizeParameter.Height.HasValue && sizeParameter.Confined)
        {
            var targetSize = new Size(sizeParameter.Width.Value, sizeParameter.Height.Value);
            return Size.Confine(targetSize, requestSize);
        }

        return Size.Resize(requestSize, sizeParameter.Width, sizeParameter.Height);
    }
}