namespace IIIF.ImageApi.V2;

/// <summary>
/// Specifies additional features that are supported for the image.
/// </summary>
/// <see cref="https://iiif.io/api/image/2.1/#profile-description"/>
public class ProfileDescription : JsonLdBase
{
    [JsonProperty(PropertyName = "@id", Order = 2)]
    public string? Id { get; set; }

    [JsonProperty(PropertyName = "@type", Order = 3)]
    public string? Type { get; set; }

    /// <summary>
    /// The set of image format parameter values available for the image.
    /// If not specified then clients should assume only formats declared in the compliance level document.
    /// </summary>
    [JsonProperty(Order = 4, PropertyName = "formats")]
    public string[]? Formats { get; set; }

    /// <summary>
    /// The maximum area in pixels supported for this image.
    /// Requests for images sizes with width*height greater than this may not be supported.
    /// </summary>
    [JsonProperty(Order = 5, PropertyName = "maxArea")]
    public int? MaxArea { get; set; }

    /// <summary>
    /// The maximum height in pixels supported for this image.
    /// Requests for images sizes with height greater than this may not be supported.
    /// If maxWidth is specified and maxHeight is not, then clients should infer that maxHeight = maxWidth.
    /// </summary>
    [JsonProperty(Order = 6, PropertyName = "maxHeight")]
    public int? MaxHeight { get; set; }

    /// <summary>
    /// The maximum width in pixels supported for this image.
    /// Requests for images sizes with width greater than this may not be supported.
    /// Must be specified if maxHeight is specified.
    /// </summary>
    [JsonProperty(Order = 7, PropertyName = "maxWidth")]
    public int? MaxWidth { get; set; }

    /// <summary>
    /// The set of image quality parameter values available for the image.
    /// If not specified then clients should assume only qualities declared in the compliance level document.
    /// </summary>
    [JsonProperty(Order = 8, PropertyName = "qualities")]
    public string[]? Qualities { get; set; }

    /// <summary>
    /// The set of features supported for the image.
    /// If not specified then clients should assume only features declared in the compliance level document.
    /// </summary>
    [JsonProperty(Order = 9, PropertyName = "supports")]
    public string[]? Supports { get; set; }
}