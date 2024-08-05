using System;
using System.Text;

namespace IIIF.ImageApi;

/// <summary>
/// Represents the {size} parameter of a IIIF image request.
/// </summary>
/// <remarks>see https://iiif.io/api/image/3.0/#42-size </remarks>
public class SizeParameter
{
    public int? Width { get; set; }

    public int? Height { get; set; }

    public bool Max { get; set; }

    public bool Upscaled { get; set; }

    public bool Confined { get; set; }

    public float? PercentScale { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        if (Upscaled) sb.Append('^');
        if (Max)
        {
            sb.Append("max");
            return sb.ToString();
        }

        if (Confined) sb.Append('!');
        if (PercentScale > 0)
        {
            sb.Append("pct:" + PercentScale);
            return sb.ToString();
        }

        if (Width > 0) sb.Append(Width);
        sb.Append(',');
        if (Height > 0) sb.Append(Height);

        return sb.ToString();
    }

    public static SizeParameter Parse(string pathPart)
    {
        var size = new SizeParameter();

        if (pathPart[0] == '^')
        {
            size.Upscaled = true;
            pathPart = pathPart[1..];
        }

        if (pathPart is "max" or "full")
        {
            size.Max = true;
            return size;
        }

        if (pathPart[0] == '!')
        {
            size.Confined = true;
            pathPart = pathPart[1..];
        }

        if (pathPart[0] == 'p')
        {
            size.PercentScale = float.Parse(pathPart[4..]);
            return size;
        }

        string[] wh = pathPart.Split(',');
        if (wh[0] != string.Empty) size.Width = int.Parse(wh[0]);
        if (wh[1] != string.Empty) size.Height = int.Parse(wh[1]);

        return size;
    }
    
    /// <summary>
    /// Resize given size in accordance with <see cref="SizeParameter"/>
    /// If ^ is not specified and size larger than image the behaviour is controlled by
    /// <see cref="InvalidUpscaleBehaviour"/> - will either throw an exception or return original size.
    /// Note that /^max/ is not supported.
    /// </summary>  
    /// <param name="extractedRegion"><see cref="Size"/> parameter to resize</param>
    /// <param name="upscaleBehaviour">
    /// Control how requests that would upscale extracted region but do not specify "^" are handled
    /// </param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">Thrown if "^max" parameter specified</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if size would be upscaled and upscaleBehaviour is <see cref="InvalidUpscaleBehaviour.Throw"/>
    /// </exception>
    public Size Resize(Size extractedRegion, InvalidUpscaleBehaviour upscaleBehaviour = InvalidUpscaleBehaviour.Throw)
    {
        // /max/
        if (Max)
        {
            if (Upscaled)
            {
                throw new NotSupportedException("^max is not supported as maxWidth, maxHeight or maxArea unknown");
            }

            return extractedRegion;
        }

        // /pct:n/ /^pct:n/
        if (PercentScale.HasValue)
        {
            return HandleResize(() => Size.ResizePercent(extractedRegion, PercentScale.Value),
                () => Size.ResizePercent(extractedRegion, Math.Min(PercentScale.Value, 100)),
                () => PercentScale.Value > 100);
        }
        
        // /!w,h/ /^!w,h/
        if (Confined)
        {
            var requiredSize = new Size(Width!.Value, Height!.Value);
            return HandleResize(() => Size.FitWithin(requiredSize, extractedRegion),
                () => Size.Confine(requiredSize, extractedRegion),
                () => Math.Max(Width ?? 0, Height ?? 0) > extractedRegion.MaxDimension);
        }
        
        // /w,/ /^w,/ /,h/ /^,h/ /w,h/ /^w,h/ 
        return HandleResize(() => Size.Resize(extractedRegion, Width, Height),
            () => Size.Resize(extractedRegion,
                Width.HasValue ? Math.Min(extractedRegion.Width, Width.Value) : null,
                Height.HasValue ? Math.Min(extractedRegion.Height, Height.Value) : null),
            () => (Width ?? 0) > extractedRegion.Width || (Height ?? 0) > extractedRegion.Height);

        Size HandleResize(Func<Size> upscaleSupported, Func<Size> nonUpscaling, Func<bool> wouldUpscale)
        {
            if (Upscaled) return upscaleSupported();
            if (!wouldUpscale()) return nonUpscaling();

            return upscaleBehaviour switch
            {
                InvalidUpscaleBehaviour.Throw => throw new InvalidOperationException(
                    $"SizeParameter /{ToString()}/ cannot upscale image size '{extractedRegion}'"),
                InvalidUpscaleBehaviour.ReturnOriginal => extractedRegion,
                _ => nonUpscaling()
            };
        }
    }
}

/// <summary>
/// Optional enum to control resize behaviour if it would result in extracted region being upscaled. Only applicable
/// when upscale (^) parameter not specified
/// </summary>
public enum InvalidUpscaleBehaviour
{
    /// <summary>
    /// Throw an exception if upscaling is required
    /// </summary>
    Throw,
    
    /// <summary>
    /// If upscaling is required, abort and return extracted region size instead
    /// </summary>
    ReturnOriginal,
}