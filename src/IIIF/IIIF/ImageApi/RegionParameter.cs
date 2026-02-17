using System;
using System.Diagnostics;
using IIIF.Exceptions;
using IIIF.Utils;

namespace IIIF.ImageApi;

/// <summary>
/// Represents the {region} parameter of a IIIF image request.
/// </summary>
/// <remarks>see https://iiif.io/api/image/3.0/#41-region </remarks>
public class RegionParameter
{
    private const string FullRegion = "full";
    private const string SquareRegion = "square";
    private const string PercentPrefix = "pct:";

    [JsonProperty(Order = 91, PropertyName = "x")]
    public float X { get; set; }

    [JsonProperty(Order = 92, PropertyName = "y")]
    public float Y { get; set; }

    [JsonProperty(Order = 93, PropertyName = "w")]
    public float W { get; set; }

    [JsonProperty(Order = 94, PropertyName = "h")]
    public float H { get; set; }

    public bool Full { get; set; }
    public bool Square { get; set; }
    public bool Percent { get; set; }

    public override string ToString()
    {
        if (Full) return FullRegion;
        if (Square) return SquareRegion;
        var xywh = $"{X},{Y},{W},{H}";
        if (Percent) return $"pct:{xywh}";
        return xywh;
    }

    public static RegionParameter Parse(string pathPart)
    {
        try
        {
            if (pathPart == FullRegion) return new RegionParameter { Full = true };
            if (pathPart == SquareRegion) return new RegionParameter { Square = true };

            var percent = pathPart.StartsWith(PercentPrefix);
            var stringParts = pathPart.Substring(percent ? 4 : 0).Split(',');
            var xywh = Array.ConvertAll(stringParts, float.Parse);
            return new RegionParameter
            {
                X = xywh[0], Y = xywh[1], W = xywh[2], H = xywh[3],
                Percent = percent
            };
        }
        catch (Exception ex)
        {
            throw new RegionException($"Expected 'full', 'square', 'pct:x,y,w,h' or 'x,y,w,h'. Found: {pathPart}",
                ex);
        }
    }
    
    /// <summary>
    /// Calculate the extracted region size from the full image size. 
    /// </summary>
    /// <param name="imageSize"><see cref="Size"/> parameter for source image</param>
    /// <returns>Extracted region <see cref="Size"/></returns>
    /// <exception cref="RegionException">
    /// Thrown if the requested region’s height or width is zero, or if the region is entirely outside the bounds of
    /// the reported dimensions
    /// </exception>
    public Size GetExtractedRegionSize(Size imageSize)
    {
        var extractedSize = ExtractedRegionSizeInternal(imageSize);

        ThrowIfZero(extractedSize.Width, "width");
        ThrowIfZero(extractedSize.Height, "height");

        return extractedSize;

        static void ThrowIfZero(int value, string dimensionName)
        {
            if (value == 0) throw new RegionException($"Region {dimensionName} cannot be zero");
        }
    }

    private Size ExtractedRegionSizeInternal(Size imageSize)
    {
        if (Full) return imageSize;
        if (Square) return Size.Square(Math.Min(imageSize.Width, imageSize.Height));
        
        // Get x,y,w,h from absolute or percentage values
        var location = GetRegionLocation(imageSize);
        
        // Ensure region is within image bounds
        EnsureRegionValid(imageSize, location);

        return new Size(location.W, location.H);
    }

    private static void EnsureRegionValid(Size imageSize, RegionLocation location)
    {
        if (location.X > imageSize.Width || location.Y > imageSize.Height)
        {
            throw new RegionException("Region is outside image bounds");
        }
        
        // if the region extends beyond the dimensions of the full image, crop at image's edge
        if (location.X + location.W > imageSize.Width)
        {
            location.W = imageSize.Width - location.X;
        }

        if (location.Y + location.H > imageSize.Height)
        {
            location.H = imageSize.Height - location.Y;
        }
    }

    private RegionLocation GetRegionLocation(Size imageSize) =>
        Percent
            ? new RegionLocation(
                X.PercentOf(imageSize.Width),
                Y.PercentOf(imageSize.Height),
                W.PercentOf(imageSize.Width),
                H.PercentOf(imageSize.Height)
            )
            : new RegionLocation((int)X, (int)Y, (int)W, (int)H);

    [DebuggerDisplay("{X},{Y},{W},{H}")]
    private class RegionLocation
    {
        public int X { get; }
        public int Y { get; }
        public int W { get; set; }
        public int H { get; set; }

        public RegionLocation(int x, int y, int w, int h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }
    }
}