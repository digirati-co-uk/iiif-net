﻿using System;

namespace IIIF;

/// <summary>
/// Represents the 2d size of an object.
/// </summary>
public class Size
{
    [JsonProperty(PropertyName = "width")] public int Width { get; private set; }

    [JsonProperty(PropertyName = "height")]
    public int Height { get; private set; }

    [JsonIgnore] public int MaxDimension => Width > Height ? Width : Height;

    public override string ToString()
    {
        return $"{Width},{Height}";
    }

    /// <summary>
    /// Create new Size object with specified width and height.
    /// </summary>
    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Get size object as w,h array
    /// </summary>
    /// <returns></returns>
    public int[] ToArray()
    {
        return new[] { Width, Height };
    }

    /// <summary>
    /// Checks if current Size is confined within specified size.
    /// </summary>
    /// <param name="confineSize">Size object to check if confined within.</param>
    /// <returns>true if current item would fit inside specified size; else false.</returns>
    public bool IsConfinedWithin(Size confineSize)
    {
        return Width <= confineSize.Width && Height <= confineSize.Height;
    }

    /// <summary>
    /// Create new Size object representing square.
    /// </summary>
    /// <param name="dimension">width and height of square</param>
    public static Size Square(int dimension)
    {
        return new(dimension, dimension);
    }

    /// <summary>
    /// Create new Size object from "w,h" array.
    /// </summary>
    /// <param name="size">w,h array</param>
    /// <returns>New Size object</returns>
    public static Size FromArray(int[] size)
    {
        return new(size[0], size[1]);
    }

    /// <summary>
    /// Create new Size object from "w,h" string.
    /// </summary>
    /// <param name="size">String representing size.</param>
    /// <returns>New Size object</returns>
    public static Size FromString(string size)
    {
        var parts = size.Split(",");
        return new Size(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    /// <summary>
    /// Confine specified Size object to bounding square of specified size.
    /// </summary>
    /// <param name="boundingSquare">Dimension of bounding square to confine object to.</param>
    /// <param name="imageSize">Size object to Confine dimensions to.</param>
    /// <returns>New <see cref="Size"/> object with dimensions bound to specified square.</returns>
    public static Size Confine(int boundingSquare, Size imageSize)
    {
        return Confine(Square(boundingSquare), imageSize);
    }

    /// <summary>
    /// Confine specified Size object to bounding square of specified size.
    /// This is similar to FitWithin() but will returned original size if already within confines.
    /// </summary>
    /// <param name="requiredSize">Dimension of bounding square to confine object to.</param>
    /// <param name="imageSize">Size object to Confine dimensions to.</param>
    /// <returns>New <see cref="Size"/> object with dimensions bound to specified square.</returns>
    public static Size Confine(Size requiredSize, Size imageSize)
    {
        if (imageSize.Width <= requiredSize.Width && imageSize.Height <= requiredSize.Height) return imageSize;

        return FitWithin(requiredSize, imageSize);
    }

    /// <summary>
    /// Fit specified Size object withing bounding square of specified size, allowing to grow if required.
    /// </summary>
    /// <param name="requiredSize">Dimension of bounding square to confine object to.</param>
    /// <param name="imageSize">Size object to Confine dimensions to.</param>
    /// <returns>New <see cref="Size"/> object with dimensions bound to specified square.</returns>
    public static Size FitWithin(Size requiredSize, Size imageSize)
    {
        var scaleW = requiredSize.Width / (double)imageSize.Width;
        var scaleH = requiredSize.Height / (double)imageSize.Height;
        var scale = Math.Min(scaleW, scaleH);
        return new Size(
            (int)Math.Round(imageSize.Width * scale),
            (int)Math.Round(imageSize.Height * scale)
        );
    }

    /// <summary>
    /// Resize specified Size to new Width and/or Height.
    /// Maintains aspect ratio unless both are specified.
    /// </summary>
    public static Size Resize(Size size, int? targetWidth = null, int? targetHeight = null)
    {
        if (!targetWidth.HasValue && !targetHeight.HasValue)
            throw new InvalidOperationException("Cannot confine size without dimensions");

        if (targetWidth.HasValue && targetHeight.HasValue) return new Size(targetWidth.Value, targetHeight.Value);

        if (size.GetShape() == ImageShape.Square)
            return Square(targetWidth ?? targetHeight ?? -1);

        return new Size(
            targetWidth ?? size.Width * targetHeight!.Value / size.Height,
            targetHeight ?? size.Height * targetWidth!.Value / size.Width);
    }

    /// <summary>
    /// Resize specified Size growing/shrinking by specified % value
    /// </summary>
    public static Size ResizePercent(Size size, float percentage)
    {
        var width = Convert.ToInt32(size.Width * (percentage / 100));
        var height = Convert.ToInt32(size.Height * (percentage / 100));
        return new Size(width, height);
    }

    /// <summary>
    /// Get % size difference between larger and smaller size, based on longest edge.
    /// </summary>
    public static double GetSizeIncreasePercent(Size largerSize, Size smallerSize)
    {
        var largeMax = largerSize.MaxDimension;
        var smallMax = smallerSize.MaxDimension;
        if (smallMax > largeMax) throw new InvalidOperationException("Larger size must be larger than smaller");

        return (largeMax / (double)smallMax - 1) * 100;
    }

    /// <summary>
    /// Get the shape of image based on it's dimensions.
    /// </summary>
    public ImageShape GetShape()
    {
        if (Width == Height) return ImageShape.Square;

        return Width > Height ? ImageShape.Landscape : ImageShape.Portrait;
    }
}

/// <summary>
/// Enum representing shape of an image
/// </summary>
public enum ImageShape
{
    /// <summary>
    /// Width == Height
    /// </summary>
    Square = 0,

    /// <summary>
    /// Width &gt; Height
    /// </summary>
    Landscape = 1,

    /// <summary>
    /// Width &lt; Height
    /// </summary>
    Portrait = 2
}