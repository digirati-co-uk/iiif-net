namespace IIIF.ImageApi.V2;

/// <summary>
/// Set of features supported by the image
/// </summary>
/// <remarks>
/// See https://iiif.io/api/image/2.1/#profile-description
/// </remarks>
public class Supports
{
    ///<summary>
    /// The base URI of the service will redirect to the image information document.
    ///</summary>
    public const string BaseUriRedirect = "baseUriRedirect";

    ///<summary>
    /// The canonical image URI HTTP link header is provided on image responses.
    ///</summary>
    public const string CanonicalLinkHeader = "canonicalLinkHeader";

    ///<summary>
    /// The CORS HTTP header is provided on all responses.
    ///</summary>
    public const string Cors = "cors";

    ///<summary>
    /// The JSON-LD media type is provided when JSON-LD is requested.
    ///</summary>
    public const string JsonldMediaType = "jsonldMediaType";

    ///<summary>
    /// The image may be rotated around the vertical axis, resulting in a left-to-right mirroring of the content.
    ///</summary>
    public const string Mirroring = "mirroring";

    ///<summary>
    /// The profile HTTP link header is provided on image responses.
    ///</summary>
    public const string ProfileLinkHeader = "profileLinkHeader";

    ///<summary>
    /// Regions of images may be requested by percentage.
    ///</summary>
    public const string RegionByPct = "regionByPct";

    ///<summary>
    /// Regions of images may be requested by pixel dimensions.
    ///</summary>
    public const string RegionByPx = "regionByPx";

    ///<summary>
    /// A square region where the width and height are equal to the shorter dimension of the complete image content.
    ///</summary>
    public const string RegionSquare = "regionSquare";

    ///<summary>
    /// Rotation of images may be requested by degrees other than multiples of 90.
    ///</summary>
    public const string RotationArbitrary = "rotationArbitrary";

    ///<summary>
    /// Rotation of images may be requested by degrees in multiples of 90.
    ///</summary>
    public const string RotationBy90s = "rotationBy90s";

    ///<summary>
    /// Size of images may be requested larger than the "full" size. See warning.
    ///</summary>
    public const string SizeAboveFull = "sizeAboveFull";

    ///<summary>
    /// Size of images may be requested in the form "!w,h".
    ///</summary>
    public const string SizeByConfinedWh = "sizeByConfinedWh";

    ///<summary>
    /// Size of images may be requested in the form "w,h", including sizes that would distort the image.
    ///</summary>
    public const string SizeByDistortedWh = "sizeByDistortedWh";

    ///<summary>
    /// Size of images may be requested in the form ",h".
    ///</summary>
    public const string SizeByH = "sizeByH";

    ///<summary>
    /// Size of images may be requested in the form "pct:n".
    ///</summary>
    public const string SizeByPct = "sizeByPct";

    /// <summary>
    /// Size of images may be requested in the form "w,".
    /// </summary>
    public const string SizeByW = "sizeByW";

    /// <summary>
    /// Size of images may be requested in the form "w,h" where the supplied w and h preserve the aspect ratio.
    /// </summary>
    public const string SizeByWh = "sizeByWh";
}