using System;
using System.Globalization;
using IIIF.Exceptions;

namespace IIIF.ImageApi;

/// <summary>
/// Represents the {rotation} parameter of a IIIF image request.
/// </summary>
/// <remarks>see https://iiif.io/api/image/3.0/#43-rotation </remarks>
public class RotationParameter
{
    public bool Mirror { get; set; }
    public float Angle { get; set; }

    /// <summary>
    /// Parse provided {rotation} parameter to <see cref="RotationParameter"/> object
    /// </summary>
    /// <param name="pathPart">{rotation} parameter of IIIF image request (e.g. "0", "!90")</param>
    /// <returns>Parsed <see cref="RotationParameter"/></returns>
    /// <exception cref="RotationException">Thrown if provided value is not a valid {rotation} parameter</exception>
    public static RotationParameter Parse(string pathPart)
    {
        try
        {
            var rotation = new RotationParameter();
            var anglePart = pathPart;
            if (anglePart[0] == '!')
            {
                rotation.Mirror = true;
                anglePart = anglePart[1..];
            }

            // Only digits and decimal point are valid, angle must be 0-360
            rotation.Angle = float.Parse(anglePart, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            if (!float.IsFinite(rotation.Angle) || rotation.Angle > 360)
                throw new ArgumentOutOfRangeException(nameof(pathPart));

            return rotation;
        }
        catch (Exception ex)
        {
            throw new RotationException($"Expected 'n' or '!n'. Found: {pathPart}", ex);
        }
    }

    public override string ToString()
        => $"{(Mirror ? "!" : string.Empty)}{Angle.ToString(CultureInfo.InvariantCulture)}";
}
