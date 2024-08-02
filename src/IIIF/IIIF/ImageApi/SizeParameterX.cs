using System;

namespace IIIF.ImageApi;

public static class SizeParameterX
{
    /// <summary>
    /// Resize given size in accordance with <see cref="SizeParameter"/>
    /// Note that /^max/ is not supported.
    /// If ^ is not specified and size larger than image the largest possible size is returned.
    /// </summary>  
    public static Size Resize(this SizeParameter sizeParameter, Size size)
    {
        // /max/
        if (sizeParameter.Max)
        {
            if (sizeParameter.Upscaled)
            {
                throw new NotSupportedException("^max is not supported as maxWidth, maxHeight or maxArea unknown");
            }

            return size;
        }

        // /pct:n/ /^pct:n/
        if (sizeParameter.PercentScale.HasValue)
        {
            var percent = sizeParameter.Upscaled
                ? sizeParameter.PercentScale.Value
                : Math.Min(sizeParameter.PercentScale.Value, 100);
            return Size.ResizePercent(size, percent);
        }
        
        // /!w,h/ /^!w,h/
        if (sizeParameter.Confined)
        {
            var requiredSize = new Size(sizeParameter.Width!.Value, sizeParameter.Height!.Value);
            return sizeParameter.Upscaled
                ? Size.FitWithin(requiredSize, size)
                : Size.Confine(requiredSize, size);
        }
        
        // /w,/ /^w,/ /,h/ /^,h/ /w,h/ /^w,h/ 
        var width = sizeParameter.Width;
        var height = sizeParameter.Height;

        if (!sizeParameter.Upscaled)
        {
            width = sizeParameter.Width.HasValue ? Math.Min(size.Width, sizeParameter.Width.Value) : null;
            height = sizeParameter.Height.HasValue ? Math.Min(size.Height, sizeParameter.Height.Value) : null;
        }
        return Size.Resize(size, width, height);
    }
}