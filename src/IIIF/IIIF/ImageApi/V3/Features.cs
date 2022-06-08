namespace IIIF.ImageApi.V3
{
    /// <summary>
    /// Available features defined for use in the "extraFeatures" property
    /// </summary>
    /// <remarks>
    /// See https://iiif.io/api/image/3.0/#57-extra-functionality
    /// </remarks>
    public static class Features
    {
        /// <summary>
        ///	The base URI of the service will redirect to the image information document.
        /// </summary>
        public const string BaseUriRedirect = "baseUriRedirect";

        /// <summary>
        ///	The canonical image URI HTTP link header is provided on image responses.
        /// </summary>
        public const string CanonicalLinkHeader = "canonicalLinkHeader";

        /// <summary>
        ///	The CORS HTTP headers are provided on all responses.
        /// </summary>
        public const string Cors = "cors";

        /// <summary>
        ///	The JSON-LD media type is provided when requested.
        /// </summary>
        public const string JsonldMediaType = "jsonldMediaType";

        /// <summary>
        ///	The image may be rotated around the vertical axis, resulting in a left-to-right mirroring of the content.
        /// </summary>
        public const string Mirroring = "mirroring";

        /// <summary>
        ///	The profile HTTP link header is provided on image responses.
        /// </summary>
        public const string ProfileLinkHeader = "profileLinkHeader";

        /// <summary>
        ///	Regions of the full image may be requested by percentage.
        /// </summary>
        public const string RegionByPct = "regionByPct";

        /// <summary>
        ///	Regions of the full image may be requested by pixel dimensions.
        /// </summary>
        public const string RegionByPx = "regionByPx";

        /// <summary>
        ///	A square region may be requested, where the width and height are equal to the shorter dimension of the full
        /// image.
        /// </summary>
        public const string RegionSquare = "regionSquare";

        /// <summary>
        ///	Image rotation may be requested using values other than multiples of 90 degrees.
        /// </summary>
        public const string RotationArbitrary = "rotationArbitrary";

        /// <summary>
        ///	Image rotation may be requested in multiples of 90 degrees.
        /// </summary>
        public const string RotationBy90s = "rotationBy90s";

        /// <summary>
        ///	Image size may be requested in the form !w,h.
        /// </summary>
        public const string SizeByConfinedWh = "sizeByConfinedWh";

        /// <summary>
        ///	Image size may be requested in the form ,h.
        /// </summary>
        public const string SizeByH = "sizeByH";

        /// <summary>
        ///	Images size may be requested in the form pct:n.
        /// </summary>
        public const string SizeByPct = "sizeByPct";

        /// <summary>
        ///	Image size may be requested in the form w,.
        /// </summary>
        public const string SizeByW = "sizeByW";

        /// <summary>
        ///	Image size may be requested in the form w,h.
        /// </summary>
        public const string SizeByWh = "sizeByWh";

        /// <summary>
        ///	Image sizes prefixed with ^ may be requested.
        /// </summary>
        public const string SizeUpscaling = "sizeUpscaling";
    }
}