using System;
using System.Collections.Generic;

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

    /// <summary>
    /// Given an image service's width, height and tiles properties, is this image request for
    /// a valid computed tile (such as might be made by a IIIF Image API Client like OpenSeadragon)?
    /// </summary>
    /// <param name="imageRequest">The request to be analysed</param>
    /// <param name="serviceWidth">The full width declared by the image service</param>
    /// <param name="serviceHeight">The full height declared by the image service</param>
    /// <param name="tiles">The tiles property, if available</param>
    /// <returns>null if answer cannot be determined, otherwise true or false</returns>
    /// <remarks>
    /// A request is a tile if, for one of the supplied <see cref="Tile"/> definitions and one of its
    /// scaleFactors, its region aligns to the tile grid and its size downsamples that region by the same
    /// scale factor. See https://iiif.io/api/image/3.0/#54-tiles and the region/size formulae in the
    /// implementation notes (https://iiif.io/api/image/3.0/implementation/).
    /// </remarks>
    public static bool? IsTileRequest(this ImageRequest imageRequest, int serviceWidth, int serviceHeight, List<Tile>? tiles)
    {
        if (tiles == null || tiles.Count == 0)
        {
            return null;
        }

        // info.json/base requests carry no region or size so cannot be tiles
        if (imageRequest.IsBase || imageRequest.IsInformationRequest) return false;
        if (imageRequest.Region == null || imageRequest.Size == null) return false;
        if (serviceWidth <= 0 || serviceHeight <= 0) return false;

        if (!TryGetAbsoluteRegion(imageRequest.Region, serviceWidth, serviceHeight,
                out var regionX, out var regionY, out var regionW, out var regionH))
        {
            return false;
        }

        // The size the server would output for this region, given the request's size parameter
        Size requestedSize;
        try
        {
            requestedSize = imageRequest.Size.GetResultingSize(new Size(regionW, regionH));
        }
        catch
        {
            return false;
        }

        foreach (var tile in tiles)
        {
            var tileWidth = tile.Width;
            // height is optional in the spec and defaults to width, giving square tiles
            var tileHeight = tile.Height > 0 ? tile.Height : tile.Width;
            if (tileWidth <= 0 || tileHeight <= 0 || tile.ScaleFactors == null) continue;

            foreach (var scaleFactor in tile.ScaleFactors)
            {
                if (scaleFactor <= 0) continue;

                long tileFullWidth = (long)tileWidth * scaleFactor;
                long tileFullHeight = (long)tileHeight * scaleFactor;

                // region origin must sit on the tile grid for this scale factor
                if (regionX % tileFullWidth != 0 || regionY % tileFullHeight != 0) continue;

                // region is clamped to the image edge for the right column / bottom row
                var expectedW = (int)Math.Min(tileFullWidth, serviceWidth - regionX);
                var expectedH = (int)Math.Min(tileFullHeight, serviceHeight - regionY);
                if (regionW != expectedW || regionH != expectedH) continue;

                if (ScaledSizeMatches(requestedSize, expectedW, expectedH, scaleFactor)) return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Resolve a <see cref="RegionParameter"/> to integral x,y,w,h pixels within the image. Returns false for
    /// regions that can never describe a tile (square, percentage, fractional, or out of bounds).
    /// </summary>
    private static bool TryGetAbsoluteRegion(RegionParameter region, int serviceWidth, int serviceHeight,
        out long x, out long y, out int w, out int h)
    {
        x = y = 0;
        w = h = 0;

        if (region.Full)
        {
            w = serviceWidth;
            h = serviceHeight;
            return true;
        }

        // Tiles are always absolute x,y,w,h regions
        if (region.Square || region.Percent) return false;

        // Tile coordinates are whole pixels
        if (region.X % 1 != 0 || region.Y % 1 != 0 || region.W % 1 != 0 || region.H % 1 != 0) return false;

        x = (long)region.X;
        y = (long)region.Y;
        w = (int)region.W;
        h = (int)region.H;

        if (x < 0 || y < 0 || w <= 0 || h <= 0) return false;
        if (x >= serviceWidth || y >= serviceHeight) return false;

        return true;
    }

    /// <summary>
    /// Does <paramref name="requestedSize"/> match the region scaled down by <paramref name="scaleFactor"/>? The
    /// spec computes each output dimension as ceil(region / scaleFactor); a dimension a client leaves implicit is
    /// instead derived from the other via aspect ratio, so it is allowed to differ by a single pixel of rounding.
    /// </summary>
    private static bool ScaledSizeMatches(Size requestedSize, int regionW, int regionH, long scaleFactor)
    {
        var scaledW = (int)((regionW + scaleFactor - 1) / scaleFactor);
        var scaledH = (int)((regionH + scaleFactor - 1) / scaleFactor);

        var widthMatches = requestedSize.Width == scaledW;
        var heightMatches = requestedSize.Height == scaledH;

        if (widthMatches && heightMatches) return true;
        if (widthMatches && Math.Abs(requestedSize.Height - scaledH) <= 1) return true;
        if (heightMatches && Math.Abs(requestedSize.Width - scaledW) <= 1) return true;

        return false;
    }
}